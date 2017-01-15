using System.Collections.Generic;
using System.Linq;
using GedcomLib;
using System;
using System.Text;
using System.IO;

namespace Ancestry_Reporter.Reports
{
	public class DescendantReport : IBaseReport
	{
		public Dictionary<string, GedcomIndividual> gedcomIndividuals;
		public Dictionary<string, GedcomFamily> gedcomFamilies;
		private int maxDepth = 0;
		private int recordCount = 0;

		public DescendantReport()
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

				writer.WriteLine(string.Format("Descendant Report for {0}", AncestryProcessing.GenerateName(gedcomIndividuals[rootIndividualId], false)));
				writer.WriteLine(string.Format("Generated on {0}", DateTime.Now.ToShortDateString()));
				writer.WriteLine();
				this.recordCount = 0;
				ProcessIndividual(rootIndividualId, 0, 1, "", writer);

				writer.WriteLine(string.Format("Total descendants in report: {0}", this.recordCount));
			}
		}

		private void ProcessIndividual(string individualId, int depth, int childPos, string parentPath, StreamWriter writer)
		{
			GedcomIndividual individual = gedcomIndividuals[individualId];
			string path = string.Format("{0}{1}{2}", parentPath, (char)(97 + depth), childPos);
			if (individual.AlreadyIncluded)
			{
				writer.WriteLine(string.Format("{0}{1} - {2} (Already included)", "".PadLeft(depth * 3), path, AncestryProcessing.GenerateName(gedcomIndividuals[individualId], true)));
			}
			else
			{
				writer.WriteLine(string.Format("{0}{1} - {2}", "".PadLeft(depth * 3), path, AncestryProcessing.GenerateName(gedcomIndividuals[individualId], true)));
				this.recordCount++;
				foreach (GedcomFamily family in gedcomFamilies.Values.Where(x => x.HusbandId == individualId))
				{
					if (string.IsNullOrEmpty(family.WifeId))
						writer.WriteLine(string.Format("{0}  x <Unknown>", "".PadLeft(depth * 3)));
					else
						writer.WriteLine(string.Format("{0}  x {1}", "".PadLeft(depth * 3), AncestryProcessing.GenerateName(gedcomIndividuals[family.WifeId], true)));
					if (depth < this.maxDepth)
					{
						int i = 1;
						foreach (string childId in family.Children)
						{
							ProcessIndividual(childId, depth + 1, i, path, writer);
							i++;
						}
					}
				}
				foreach (GedcomFamily family in gedcomFamilies.Values.Where(x => x.WifeId == individualId))
				{
					if (string.IsNullOrEmpty(family.HusbandId))
						writer.WriteLine(string.Format("{0}  x <Unknown>", "".PadLeft(depth * 3)));
					else
						writer.WriteLine(string.Format("{0}  x {1}", "".PadLeft(depth * 3), AncestryProcessing.GenerateName(gedcomIndividuals[family.HusbandId], true)));
					if (depth < this.maxDepth)
					{
						int i = 1;
						foreach (string childId in family.Children)
						{
							ProcessIndividual(childId, depth + 1, i, path, writer);
							i++;
						}
					}
				}
			}
		}

	}
}