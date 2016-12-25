using System.Collections.Generic;
using System.Linq;
using GedcomLib;
using System;
using System.Text;
using System.IO;

namespace Ancestry_Reporter.Reports
{
	public interface IBaseReport
	{
		void GenerateAncestryReport(string rootIndividualId, int maxDepth, string outputPath, Dictionary<string, GedcomIndividual> gedcomIndividuals, Dictionary<string, GedcomFamily> gedcomFamilies);

	}
}