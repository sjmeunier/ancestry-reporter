using System.Collections.Generic;
using System.Linq;
using GedcomLib;
using System;
using System.Text;
using System.IO;

namespace Ancestry_Reporter.Reports
{
	public class PlaceSummaryReport : IBaseReport
	{
		public Dictionary<string, GedcomIndividual> gedcomIndividuals;
		public Dictionary<string, GedcomFamily> gedcomFamilies;

		public Dictionary<string, AncestorIndividual> ancestors = new Dictionary<string, AncestorIndividual>();

		private Dictionary<int, List<AncestorIndividual>> optimizedAncestors = new Dictionary<int, List<AncestorIndividual>>();
		private List<string> places = new List<string>();

		private int highestDepth = 0;
		private int maxDepth = 0;

		public PlaceSummaryReport()
		{

		}

		public void GenerateReport(string rootIndividualId, int maxDepth, string outputPath, Dictionary<string, GedcomIndividual> gedcomIndividuals, Dictionary<string, GedcomFamily> gedcomFamilies)
		{
			this.maxDepth = maxDepth;
			this.gedcomFamilies = gedcomFamilies;
			this.gedcomIndividuals = gedcomIndividuals;
			ProcessAncestor("@" + rootIndividualId + "@", string.Empty, 1, 0);
			this.places.AddRange(this.ancestors.Values.Where(x => !string.IsNullOrEmpty(x.BirthPlace.Trim())).Select(x => x.BirthPlace).Distinct().ToList());
			this.places.AddRange(this.ancestors.Values.Where(x => !string.IsNullOrEmpty(x.DiedPlace.Trim())).Select(x => x.DiedPlace).Distinct().ToList());
			this.places = this.places.Distinct().ToList();

			OutputReport("@" + rootIndividualId + "@", outputPath);
		}

		private void OutputReport(string rootIndividialId, string outputPath)
		{
			using (StreamWriter writer = new StreamWriter(outputPath))
			{

				writer.WriteLine(string.Format("Place Name Report for {0}", ancestors[rootIndividialId].SummaryName));
				writer.WriteLine(string.Format("Generated on {0}", DateTime.Now.ToShortDateString()));
				writer.WriteLine(string.Format("Total ancestors in report {0}", ancestors.Count()));
				writer.WriteLine();
                writer.WriteLine("---------------------------------------------------");
                foreach (string place in this.places.OrderBy(x => x))
				{
					writer.WriteLine(string.Format("{0} ({1})", place, this.ancestors.Values.Where(x => x.BirthPlace == place || x.DiedPlace == place).Count()));
				}
			}
		}

		private void ProcessAncestor(string individualId, string childId, long ahnentafelNumber, int depth)
		{
			if (!ancestors.ContainsKey(individualId))
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

	}
}