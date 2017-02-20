using System.Collections.Generic;
using System.Linq;
using GedcomLib;
using System;
using System.Text;
using System.IO;

namespace Ancestry_Reporter.Reports
{
    public class DemographicLifespan
    {
        public string Name;
        public int Males;
        public int Females;
        public int Total;
        public int MinAge;
        public int MaxAge;
        public List<string> Ids;
        public DemographicLifespan(int minAge, int maxAge)
        {
            Name = string.Format("{0}-{1}", minAge, maxAge);
            MinAge = minAge;
            MaxAge = maxAge;
            Males = 0;
            Females = 0;
            Total = 0;
            Ids = new List<string>();
        }
    }

	public class LifespanReport : IBaseReport
	{
		public Dictionary<string, GedcomIndividual> gedcomIndividuals;
		public Dictionary<string, GedcomFamily> gedcomFamilies;

		public Dictionary<string, AncestorIndividual> ancestors = new Dictionary<string, AncestorIndividual>();

		private List<double> maleLifespans = new List<double>();
        private int maleUnknownLifespans = 0;
        private double averageMaleLifespan = 0;
        private List<double> femaleLifespans = new List<double>();
        private Dictionary<string, double> individualAges = new Dictionary<string, double>();

        private List<DemographicLifespan> demographicLifespans = new List<DemographicLifespan>();

        private int femaleUnknownLifespans = 0;
        private double averageFemaleLifespan = 0;
        private string oldestMaleId;
        private string oldestFemaleId;
        private double oldestMaleAge = 0;
        private double oldestFemaleAge = 0;


        private int highestDepth = 0;
		private int maxDepth = 0;

		public LifespanReport()
		{

		}

		public void GenerateReport(string rootIndividualId, int maxDepth, string outputPath, Dictionary<string, GedcomIndividual> gedcomIndividuals, Dictionary<string, GedcomFamily> gedcomFamilies)
		{
			this.maxDepth = maxDepth;
			this.gedcomFamilies = gedcomFamilies;
			this.gedcomIndividuals = gedcomIndividuals;
			ProcessAncestor("@" + rootIndividualId + "@", string.Empty, 1, 0);
            CalculateAges();
			OutputReport("@" + rootIndividualId + "@", outputPath);
		}

        private void CalculateAges()
        {
            demographicLifespans = new List<DemographicLifespan>();
            demographicLifespans.Add(new DemographicLifespan(0, 4));
            demographicLifespans.Add(new DemographicLifespan(5, 9));
            demographicLifespans.Add(new DemographicLifespan(10, 14));
            demographicLifespans.Add(new DemographicLifespan(15, 19));
            demographicLifespans.Add(new DemographicLifespan(20, 24));
            demographicLifespans.Add(new DemographicLifespan(25, 29));
            demographicLifespans.Add(new DemographicLifespan(30, 34));
            demographicLifespans.Add(new DemographicLifespan(35, 39));
            demographicLifespans.Add(new DemographicLifespan(40, 44));
            demographicLifespans.Add(new DemographicLifespan(45, 49));
            demographicLifespans.Add(new DemographicLifespan(50, 54));
            demographicLifespans.Add(new DemographicLifespan(55, 59));
            demographicLifespans.Add(new DemographicLifespan(60, 64));
            demographicLifespans.Add(new DemographicLifespan(65, 69));
            demographicLifespans.Add(new DemographicLifespan(70, 74));
            demographicLifespans.Add(new DemographicLifespan(75, 79));
            demographicLifespans.Add(new DemographicLifespan(80, 84));
            demographicLifespans.Add(new DemographicLifespan(85, 89));
            demographicLifespans.Add(new DemographicLifespan(90, 94));
            demographicLifespans.Add(new DemographicLifespan(95, 99));
            demographicLifespans.Add(new DemographicLifespan(100, 104));
            demographicLifespans.Add(new DemographicLifespan(105, 109));
            demographicLifespans.Add(new DemographicLifespan(110, 114));
            demographicLifespans.Add(new DemographicLifespan(115, 119));

            foreach (AncestorIndividual individual in ancestors.Values)
            {
                double age = AncestryProcessing.CalculateAge(individual);
                individualAges.Add(individual.Id, age);

                DemographicLifespan demographicLifespan = demographicLifespans.Where(d => d.MinAge <= age && d.MaxAge >= age).FirstOrDefault();

                if (individual.Sex.ToLower() == "m")
                {
                    if (age > oldestMaleAge)
                    {
                        oldestMaleAge = age;
                        oldestMaleId = individual.Id;
                    }
                    if (age == -1)
                        maleUnknownLifespans++;
                    else
                        maleLifespans.Add(age);
                    if (demographicLifespan != null)
                    {
                        demographicLifespan.Males++;
                        demographicLifespan.Total++;
                        demographicLifespan.Ids.Add(individual.Id);
                    }
                    
                }
                else
                {
                    if (age > oldestFemaleAge)
                    {
                        oldestFemaleAge = age;
                        oldestFemaleId = individual.Id;
                    }
                    if (age == -1)
                        femaleUnknownLifespans++;
                    else
                        femaleLifespans.Add(age);
                    if (demographicLifespan != null)
                    {
                        demographicLifespan.Females++;
                        demographicLifespan.Total++;
                        demographicLifespan.Ids.Add(individual.Id);
                    }
                }
            }
            averageMaleLifespan = maleLifespans.Average();
            averageFemaleLifespan = femaleLifespans.Average();
        }

		private void OutputReport(string rootIndividialId, string outputPath)
		{
			using (StreamWriter writer = new StreamWriter(outputPath))
			{

				writer.WriteLine(string.Format("Average Lifespan Report for {0}", AncestryProcessing.GenerateName(this.gedcomIndividuals[rootIndividialId], false)));
				writer.WriteLine(string.Format("Generated on {0}", DateTime.Now.ToShortDateString()));
				writer.WriteLine(string.Format("Total ancestors in report {0}", ancestors.Count()));
				writer.WriteLine();
                writer.WriteLine(string.Format("Oldest male ancestor ({0:0.00} years old): {1}", oldestMaleAge, AncestryProcessing.GenerateName(this.gedcomIndividuals[oldestMaleId], true)));
                writer.WriteLine(string.Format("Oldest female ancestor ({0:0.00} years old): {1}", oldestFemaleAge, AncestryProcessing.GenerateName(this.gedcomIndividuals[oldestFemaleId], true)));

                writer.WriteLine("------------------------------------------------------------");
                writer.WriteLine("|                      |   Male    |  Female   |   Total   |");
                writer.WriteLine("------------------------------------------------------------");
                writer.WriteLine(string.Format("|{0,-22}|{1, 10} |{2,10} |{3,10} |", "Individuals with ages", maleLifespans.Count, femaleLifespans.Count, maleLifespans.Count + femaleLifespans.Count));
                writer.WriteLine(string.Format("|{0,-22}|{1, 10} |{2,10} |{3,10} |", "With unknown ages", maleUnknownLifespans, femaleUnknownLifespans, maleUnknownLifespans + femaleUnknownLifespans));
                writer.WriteLine("------------------------------------------------------------");
                writer.WriteLine(string.Format("|{0,-22}|{1, 10} |{2,10} |{3,10} |", "Average Lifespan", Math.Round(averageMaleLifespan, 1), Math.Round(averageFemaleLifespan, 1), Math.Round((averageMaleLifespan + averageFemaleLifespan) / 2.0, 1)));
                writer.WriteLine("------------------------------------------------------------");
                foreach(DemographicLifespan demographicLifespan in demographicLifespans)
                    writer.WriteLine(string.Format("|{0,-22}|{1, 10} |{2,10} |{3,10} |", demographicLifespan.Name, demographicLifespan.Males, demographicLifespan.Females, demographicLifespan.Total));
                writer.WriteLine("------------------------------------------------------------");
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("Individuals per Demographic");
                writer.WriteLine("------------------------------------------------------------");
                foreach (DemographicLifespan demographicLifespan in demographicLifespans)
                {
                    writer.WriteLine(string.Format("{0}", demographicLifespan.Name));
                    writer.WriteLine("------------------------------------------------------------");
                    foreach(string individualId in demographicLifespan.Ids)
                        writer.WriteLine(string.Format("{0} ({1:0.0} years old)", AncestryProcessing.GenerateName(this.gedcomIndividuals[individualId], true), individualAges[individualId]));
                    writer.WriteLine("------------------------------------------------------------");
                    writer.WriteLine();
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