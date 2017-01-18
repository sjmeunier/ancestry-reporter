using System.Collections.Generic;
using System.Linq;
using GedcomLib;
using System;
using System.Text;
using System.IO;

namespace Ancestry_Reporter.Reports
{
	public class MtDNAReport : IBaseReport
	{
		public Dictionary<string, GedcomIndividual> gedcomIndividuals;
		public Dictionary<string, GedcomFamily> gedcomFamilies;
		private int maxDepth = 0;
		private int recordCount = 0;

		public MtDNAReport()
		{

		}

		public void GenerateReport(string rootIndividualId, int maxDepth, string outputPath, Dictionary<string, GedcomIndividual> gedcomIndividuals, Dictionary<string, GedcomFamily> gedcomFamilies)
		{
			this.maxDepth = maxDepth;
			this.gedcomFamilies = gedcomFamilies;
			this.gedcomIndividuals = gedcomIndividuals;
			OutputReport("@" + rootIndividualId + "@", outputPath);
		}

		private void OutputReport(string rootIndividualId, string outputPath)
		{
			using (StreamWriter writer = new StreamWriter(outputPath))
			{
				string rootAncestorId = FindRootAncestor(rootIndividualId);

				writer.WriteLine(string.Format("Shared mt-DNA Report for {0}", AncestryProcessing.GenerateName(gedcomIndividuals[rootIndividualId], false)));
				writer.WriteLine(string.Format("Most distant ancestor with this mt-DNA: {0}", AncestryProcessing.GenerateName(gedcomIndividuals[rootAncestorId], false)));
				writer.WriteLine(string.Format("Generated on {0}", DateTime.Now.ToShortDateString()));
				writer.WriteLine();
				this.recordCount = 0;
				ProcessIndividual(rootAncestorId, 0, 1, writer);

				writer.WriteLine(string.Format("Total descendants in report: {0}", this.recordCount));
			}
		}

		private string FindRootAncestor(string individualId)
		{
			GedcomIndividual individual = gedcomIndividuals[individualId];
			GedcomFamily family = gedcomFamilies.Values.FirstOrDefault(x => x.Children.Contains(individualId));
			if (family == null)
				return individualId;

			if (string.IsNullOrEmpty(family.WifeId))
				return individualId;

			return FindRootAncestor(family.WifeId);
		}
		private void ProcessIndividual(string individualId, int depth, int childPos, StreamWriter writer)
		{
			GedcomIndividual individual = gedcomIndividuals[individualId];

			if (individual.AlreadyIncluded)
			{
				writer.WriteLine(string.Format("{0}{1} - {2} (Already included)", "".PadLeft(depth * 3), "", AncestryProcessing.GenerateName(gedcomIndividuals[individualId], true)));
			}
			else
			{
				writer.WriteLine(string.Format("{0}{1} - {2}", "".PadLeft(depth * 3), "", AncestryProcessing.GenerateName(gedcomIndividuals[individualId], true)));
				this.recordCount++;
				if (individual.Sex.ToLower() != "m")
				{
					foreach (GedcomFamily family in gedcomFamilies.Values.Where(x => x.WifeId == individualId))
					{
						if (depth < this.maxDepth)
						{
							int i = 1;
							foreach (string childId in family.Children)
							{
								ProcessIndividual(childId, depth + 1, i, writer);
								i++;
							}
						}
					}
				}
			}
		}

	}
}