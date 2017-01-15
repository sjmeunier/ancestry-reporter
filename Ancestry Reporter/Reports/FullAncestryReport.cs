using System.Collections.Generic;
using System.Linq;
using GedcomLib;
using System;
using System.Text;
using System.IO;

namespace Ancestry_Reporter.Reports
{
	public class FullAncestryReport : IBaseReport
	{
		public Dictionary<string, GedcomIndividual> gedcomIndividuals;
		public Dictionary<string, GedcomFamily> gedcomFamilies;

		public Dictionary<string, AncestorIndividual> ancestors = new Dictionary<string, AncestorIndividual>();

		private Dictionary<int, List<AncestorIndividual>> optimizedAncestors = new Dictionary<int, List<AncestorIndividual>>();

		private int highestDepth = 0;
		private int maxDepth = 0;

		public FullAncestryReport()
		{

		}

		public void GenerateReport(string rootIndividualId, int maxDepth, string outputPath, Dictionary<string, GedcomIndividual> gedcomIndividuals, Dictionary<string, GedcomFamily> gedcomFamilies)
		{
			this.maxDepth = maxDepth;
			this.gedcomFamilies = gedcomFamilies;
			this.gedcomIndividuals = gedcomIndividuals;
			ProcessAncestor("@" + rootIndividualId + "@", string.Empty, 1, 0);
			CalculateSummaryData();
			CalculateAncestorByGenerationDictionary();
			OutputReport("@" + rootIndividualId + "@", outputPath);
		}

		private void OutputReport(string rootIndividialId, string outputPath)
		{
			using (StreamWriter writer = new StreamWriter(outputPath))
			{

				writer.WriteLine(string.Format("Full Ancestry Report for {0}", ancestors[rootIndividialId].SummaryName));
				writer.WriteLine(string.Format("Generated on {0}", DateTime.Now.ToShortDateString()));
				writer.WriteLine(string.Format("Total ancestors in report {0}", ancestors.Count()));
				writer.WriteLine();
				for(int i = 0; i < this.highestDepth; i++)
				{
					writer.WriteLine("---------------------------------------------------");
					writer.WriteLine("---------------------------------------------------");
					writer.WriteLine(string.Format("Generation {0}", i + 1));
					writer.WriteLine("---------------------------------------------------");
					writer.WriteLine("");
					if (optimizedAncestors[i].Count > 0)
					{
						foreach (var individual in optimizedAncestors[i])
						{
							writer.WriteLine(individual.FullSummary);
							writer.WriteLine("----------");
							writer.WriteLine();
						}
					}
				}
			}
		}

		private void ProcessAncestor(string individualId, string childId, long ahnentafelNumber, int depth)
		{
			if (ancestors.ContainsKey(individualId))
			{
				IncrementAppearance(individualId, childId, ahnentafelNumber, depth);
			}
			else
			{
				highestDepth = Math.Max(depth, highestDepth);

				AncestorIndividual individual = new AncestorIndividual(individualId);
				GedcomIndividual gedcomIndividual = gedcomIndividuals[individualId];

				individual.GivenName = gedcomIndividual.GivenName.Trim();
				individual.Surname = gedcomIndividual.Surname.Trim();
				individual.Prefix = gedcomIndividual.Prefix.Trim();
				individual.Suffix = gedcomIndividual.Suffix.Trim();
				individual.Sex = gedcomIndividual.Sex.Trim();
				individual.BirthDate = gedcomIndividual.BirthDate.Trim();
				individual.BirthPlace = gedcomIndividual.BirthPlace.Trim();
				individual.DiedDate = gedcomIndividual.DiedDate.Trim();
				individual.DiedPlace = gedcomIndividual.DiedPlace.Trim();
				individual.AppearanceCount = 1;
				individual.LowestGeneration = depth;
				individual.HighestGeneration = depth;

				individual.AhnentafelNumber = ahnentafelNumber;

				foreach (GedcomFamily family in gedcomFamilies.Values)
				{
					if (family.Children.Contains(individualId))
					{
						individual.FatherId = family.HusbandId;
						individual.MotherId = family.WifeId;
						break;
					}
				}

				ancestors.Add(individualId, individual);
				if (depth < this.maxDepth)
				{
					if (!string.IsNullOrEmpty(individual.FatherId))
						ProcessAncestor(individual.FatherId, individualId, 2 * ahnentafelNumber, depth + 1);

					if (!string.IsNullOrEmpty(individual.MotherId))
						ProcessAncestor(individual.MotherId, individualId, 2 * ahnentafelNumber + 1, depth + 1);
				}
			}
		}

		private void IncrementAppearance(string individualId, string childId, long ahnentafelNumber, int depth)
		{
			if (ancestors.ContainsKey(individualId))
			{
				highestDepth = Math.Max(depth, highestDepth);

				AncestorIndividual individual = ancestors[individualId];
				individual.LowestGeneration = Math.Min(individual.LowestGeneration, depth);
				if (depth > individual.HighestGeneration)
				{
					individual.AhnentafelNumber = ahnentafelNumber;
				}
				individual.AppearanceCount++;

				ancestors[individualId] = individual;

				if (!string.IsNullOrEmpty(individual.FatherId))
					IncrementAppearance(individual.FatherId, individualId, 2 * ahnentafelNumber, depth + 1);

				if (!string.IsNullOrEmpty(individual.MotherId))
					IncrementAppearance(individual.MotherId, individualId, 2 * ahnentafelNumber + 1, depth + 1);

			}
		}

		private void CalculateAncestorByGenerationDictionary()
		{
			for (int i = 0; i <= highestDepth; i++)
			{
				optimizedAncestors.Add(i, ancestors.Values.Where(x => x.HighestGeneration == i).ToList());
			}
		}


		private void CalculateSummaryData()
		{
			List<string> keys = new List<string>(ancestors.Keys);
			foreach (string individualId in keys)
			{
				AncestorIndividual individual = ancestors[individualId];

				individual.SummaryName = individual.GivenName;
				if (!string.IsNullOrEmpty(individual.Prefix))
					individual.SummaryName += " " + individual.Prefix;
				if (!string.IsNullOrEmpty(individual.Surname))
					individual.SummaryName += " " + individual.Surname;
				if (!string.IsNullOrEmpty(individual.Suffix))
					individual.SummaryName += " (" + individual.Suffix + ")";

				individual.SummaryRelationship = AncestryProcessing.CalculateAncestorRelationship(individual.LowestGeneration, individual.Sex.ToUpper() == "M");

				string born = AncestryProcessing.ProcessDate(individual.BirthDate, false);
				if (born != "?" || !string.IsNullOrEmpty(individual.BirthPlace.Trim()))
					individual.SummaryBirthDate = string.Format("b. {0} {1}", born, individual.BirthPlace).Trim();
				string died = AncestryProcessing.ProcessDate(individual.DiedDate, false);
				if (died != "?" || !string.IsNullOrEmpty(individual.DiedPlace.Trim()))
					individual.SummaryDeathDate = string.Format("d. {0} {1}", died, individual.DiedPlace).Trim();

				if (!string.IsNullOrEmpty(individual.FatherId) && gedcomIndividuals.ContainsKey(individual.FatherId))
				{
					GedcomIndividual father = gedcomIndividuals[individual.FatherId];
					individual.SummaryFatherName = father.GivenName;
					if (!string.IsNullOrEmpty(father.Prefix))
						individual.SummaryFatherName += " " + father.Prefix;
					if (!string.IsNullOrEmpty(father.Surname))
						individual.SummaryFatherName += " " + father.Surname;
					if (!string.IsNullOrEmpty(father.Suffix))
						individual.SummaryFatherName += " (" + father.Suffix + ")";
					individual.SummaryFatherName += " " + AncestryProcessing.GenerateBirthDeathDate(father, true);
				}
				else
				{
					individual.SummaryFatherName = "Unknown";
				}

				if (!string.IsNullOrEmpty(individual.MotherId) && gedcomIndividuals.ContainsKey(individual.MotherId))
				{
					GedcomIndividual mother = gedcomIndividuals[individual.MotherId];
					individual.SummaryMotherName = mother.GivenName;
					if (!string.IsNullOrEmpty(mother.Prefix))
						individual.SummaryMotherName += " " + mother.Prefix;
					if (!string.IsNullOrEmpty(mother.Surname))
						individual.SummaryMotherName += " " + mother.Surname;
					if (!string.IsNullOrEmpty(mother.Suffix))
						individual.SummaryMotherName += " (" + mother.Suffix + ")";
					individual.SummaryMotherName += " " + AncestryProcessing.GenerateBirthDeathDate(mother, true);
				}
				else
				{
					individual.SummaryMotherName = "Unknown";
				}

				foreach (GedcomFamily family in gedcomFamilies.Values.Where(x => x.WifeId == individual.Id || x.HusbandId == individual.Id))
				{
					string spouseId = (family.WifeId == individual.Id) ? family.HusbandId : family.WifeId;
					if (gedcomIndividuals.ContainsKey(spouseId))
					{
						GedcomIndividual spouse = gedcomIndividuals[spouseId];
						string summary = spouse.GivenName;
						if (!string.IsNullOrEmpty(spouse.Prefix))
							summary += " " + spouse.Prefix;
						if (!string.IsNullOrEmpty(spouse.Surname))
							summary += " " + spouse.Surname;
						if (!string.IsNullOrEmpty(spouse.Suffix))
							summary += " (" + spouse.Suffix + ")";
						summary += " " + AncestryProcessing.GenerateBirthDeathDate(spouse, true);
						individual.SummarySpouse.Add(family.Id, summary);
					}
					else
					{
						individual.SummarySpouse.Add(family.Id, "Unknown");
					}

					string married = AncestryProcessing.ProcessDate(family.MarriageDate, false);
					if (married != "?" || !string.IsNullOrEmpty(family.MarriagePlace.Trim()))
						individual.SummaryMarriage.Add(family.Id, string.Format("m. {0} {1}", married, family.MarriagePlace).Trim());

					HashSet<string> childSummaries = new HashSet<string>();
					foreach (string childId in family.Children)
					{
						if (!gedcomIndividuals.ContainsKey(childId))
							continue;

						GedcomIndividual child = gedcomIndividuals[childId];
						string summary = child.GivenName;
						if (!string.IsNullOrEmpty(child.Prefix))
							summary += " " + child.Prefix;
						if (!string.IsNullOrEmpty(child.Surname))
							summary += " " + child.Surname;
						if (!string.IsNullOrEmpty(child.Suffix))
							summary += " (" + child.Suffix + ")";
						summary += " " + AncestryProcessing.GenerateBirthDeathDate(child, true);
						childSummaries.Add(summary);
					}
					individual.SummaryChildren.Add(family.Id, childSummaries);
				}

				StringBuilder sb = new StringBuilder();
				sb.Append(individual.Id.Replace("@", "") + "\r\n");
				sb.Append("Ahnentafel Number: " + individual.AhnentafelNumber.ToString() + "\r\n");
				sb.Append(individual.SummaryName);
				sb.Append("\r\n" + individual.SummaryRelationship + "\r\n");
				if (!string.IsNullOrEmpty(individual.SummaryBirthDate))
					sb.Append("\r\n" + individual.SummaryBirthDate);
				if (!string.IsNullOrEmpty(individual.SummaryDeathDate))
					sb.Append("\r\n" + individual.SummaryDeathDate);

				sb.Append("\r\n\r\n" + "Father: " + individual.SummaryFatherName);
				sb.Append("\r\n" + "Mother: " + individual.SummaryMotherName);
				sb.Append("\r\n\r\n" + "Lines of Descent: " + individual.AppearanceCount.ToString());

				foreach (KeyValuePair<string, string> spouseSummary in individual.SummarySpouse)
				{
					sb.Append("\r\n\r\nSpouse: " + spouseSummary.Value);
					if (individual.SummaryMarriage.ContainsKey(spouseSummary.Key))
					{
						sb.Append("\r\n" + individual.SummaryMarriage[spouseSummary.Key]);
					}
					if (individual.SummaryChildren.ContainsKey(spouseSummary.Key))
					{
						sb.Append("\r\n  Children:");
						foreach (string childSummary in individual.SummaryChildren[spouseSummary.Key])
						{
							sb.Append("\r\n  - " + childSummary);
						}
					}
				}
				individual.FullSummary = sb.ToString();

				ancestors[individualId] = individual;
			}
		}

	}
}