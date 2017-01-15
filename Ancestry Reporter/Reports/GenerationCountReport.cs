using System.Collections.Generic;
using System.Linq;
using GedcomLib;
using System;
using System.Text;
using System.IO;

namespace Ancestry_Reporter.Reports
{
	public class GenerationCountReport : IBaseReport
	{
		public Dictionary<string, GedcomIndividual> gedcomIndividuals;
		public Dictionary<string, GedcomFamily> gedcomFamilies;

		public Dictionary<string, AncestorIndividual> ancestors = new Dictionary<string, AncestorIndividual>();

		private Dictionary<int, int> ancestorGenerationCount = new Dictionary<int, int>();

		private int highestDepth = 0;
		private int maxDepth = 0;

		public GenerationCountReport()
		{

		}

		public void GenerateReport(string rootIndividualId, int maxDepth, string outputPath, Dictionary<string, GedcomIndividual> gedcomIndividuals, Dictionary<string, GedcomFamily> gedcomFamilies)
		{
			this.maxDepth = maxDepth;
			this.gedcomFamilies = gedcomFamilies;
			this.gedcomIndividuals = gedcomIndividuals;
			ProcessAncestor("@" + rootIndividualId + "@", string.Empty, 1, 0);
			CalculateAncestorCountPerGenerationDictionary();
			OutputReport("@" + rootIndividualId + "@", outputPath);
		}

		private void OutputReport(string rootIndividialId, string outputPath)
		{
			using (StreamWriter writer = new StreamWriter(outputPath))
			{

				writer.WriteLine(string.Format("Generation Count Report for {0}", ancestors[rootIndividialId].SummaryName));
				writer.WriteLine(string.Format("Generated on {0}", DateTime.Now.ToShortDateString()));
				writer.WriteLine(string.Format("Total ancestors in report {0}", ancestors.Count()));
				writer.WriteLine();
				writer.WriteLine("---------------------------------------------------");
				for (int i = 0; i < this.highestDepth; i++)
				{
					if (ancestorGenerationCount[i] > 0)
					{
						writer.WriteLine(string.Format("Generation {0}: {1}", i + 1, ancestorGenerationCount[i]));
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

		private void CalculateAncestorCountPerGenerationDictionary()
		{
			for (int i = 0; i <= highestDepth; i++)
			{
				ancestorGenerationCount.Add(i, ancestors.Values.Where(x => x.HighestGeneration == i).Count());
			}

		}

	}
}