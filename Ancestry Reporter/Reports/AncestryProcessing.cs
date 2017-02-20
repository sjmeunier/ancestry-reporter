using GedcomLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancestry_Reporter.Reports
{
	public class AncestryProcessing
	{
		public static string CalculateAncestorRelationship(int generations, bool isMale)
		{
			if (generations == 0)
				return String.Empty;

			string relationship = "";
			if (isMale)
			{
				if (generations == 1)
					relationship = "Father";
				else if (generations == 2)
					relationship = "Grandfather";
				else if (generations == 3)
					relationship = "Great-grandfather";
				else
					relationship = string.Format("Great({0})-grandfather", generations - 2);
			}
			else
			{
				if (generations == 1)
					relationship = "Mother";
				else if (generations == 2)
					relationship = "Grandmother";
				else if (generations == 3)
					relationship = "Great-grandmother";
				else
					relationship = string.Format("Great({0})-grandmother", generations - 2);
			}
			return relationship;
		}

		public static string ProcessDate(string date, bool onlyYear)
		{
			if (string.IsNullOrEmpty(date))
			{
				date = "?";
			}
			else
			{
				if (onlyYear)
				{
					string[] dateArr = date.Split(new char[] { ' ' });
					if (dateArr.Length > 1)
					{
						date = "";
						if (dateArr[0] == "ABT")
							date = "c";
						else if (dateArr[0] == "AFT")
							date = ">";
						else if (dateArr[0] == "BEF")
							date = "<";
						date += dateArr[dateArr.Length - 1];

						int year = 0;
						Int32.TryParse(dateArr[dateArr.Length - 1], out year);
					}
				}
				else
				{
					if (date.Contains("ABT"))
						date = date.Replace("ABT", "c");
					else if (date.Contains("AFT"))
						date = date.Replace("AFT", ">");
					else if (date.Contains("BEF"))
						date = date.Replace("BEF", "<");

					date = date.Replace("JAN", "Jan").Replace("FEB", "Feb").Replace("MAR", "Mar").Replace("APR", "Apr").Replace("MAY", "May").Replace("JUN", "Jun")
								.Replace("JUL", "Jul").Replace("AUG", "Aug").Replace("SEP", "Sep").Replace("OCT", "Oct").Replace("NOV", "Nov").Replace("DEC", "Dec");
				}
			}

			return date;
		}

        private static double GetYearFromDate(string date)
        {
            if (string.IsNullOrEmpty(date))
                return -1;

            if (date.Contains("AFT") || date.Contains("BEF"))
                return -1;

            string[] dateArr = date.Split(new char[] { ' ' });
            double year = 0;
            Double.TryParse(dateArr[dateArr.Length - 1], out year);
            if (year == 0)
                return -1;

            double month = 0;

            date = date.ToUpper();
            if (date.Contains("JAN"))
                month = 1;
            else if (date.Contains("FEB"))
                month = 2;
            else if (date.Contains("MAR"))
                month = 3;
            else if (date.Contains("APR"))
                month = 4;
            else if (date.Contains("MAY"))
                month = 5;
            else if (date.Contains("JUN"))
                month = 6;
            else if (date.Contains("JUL"))
                month = 7;
            else if (date.Contains("AUG"))
                month = 8;
            else if (date.Contains("SEP"))
                month = 9;
            else if (date.Contains("OCT"))
                month = 10;
            else if (date.Contains("NOV"))
                month = 11;
            else if (date.Contains("DEC"))
                month = 12;

            year = year + (month / 12.0);

            return year;
        }

        public static double CalculateAge(AncestorIndividual individual)
        {
            double birthYear = GetYearFromDate(individual.BirthDate);
            double deathYear = GetYearFromDate(individual.DiedDate);
            if (birthYear == -1 || deathYear == -1)
                return -1;
            return deathYear - birthYear;
        }

		public static string GenerateBirthDeathDate(AncestorIndividual individual, bool onlyYear)
		{
			string born = ProcessDate(individual.BirthDate, onlyYear);
			string died = ProcessDate(individual.DiedDate, onlyYear);
			if (born != "?" || died != "?")
			{
				if (born == "?")
					return string.Format("(d.{0})", died);
				else if (died == "?")
					return string.Format("(b.{0})", born);
				else
					return string.Format("(b.{0}, d.{1})", born, died);
			}
			return string.Empty;
		}

		public static string GenerateBirthDeathDate(GedcomIndividual individual, bool onlyYear)
		{
			string born = ProcessDate(individual.BirthDate, onlyYear);
			string died = ProcessDate(individual.DiedDate, onlyYear);
			if (born != "?" || died != "?")
			{
				if (born == "?")
					return string.Format("(d.{0})", died);
				else if (died == "?")
					return string.Format("(b.{0})", born);
				else
					return string.Format("(b.{0}, d.{1})", born, died);
			}
			return string.Empty;
		}

		public static string GenerateName(GedcomIndividual individual, bool includeDates)
		{
			string name = individual.GivenName;
			if (!string.IsNullOrEmpty(individual.Prefix))
				name += " " + individual.Prefix;
			if (!string.IsNullOrEmpty(individual.Surname))
				name += " " + individual.Surname;
			if (!string.IsNullOrEmpty(individual.Suffix))
				name += " (" + individual.Suffix + ")";

			if (includeDates)
			{
				string born = AncestryProcessing.ProcessDate(individual.BirthDate, false);
				string bornDate = "";
				if (born != "?" || !string.IsNullOrEmpty(individual.BirthPlace.Trim()))
					bornDate = string.Format("b. {0} {1}", born, individual.BirthPlace).Trim();
				string died = AncestryProcessing.ProcessDate(individual.DiedDate, false);
				string diedDate = "";
				if (died != "?" || !string.IsNullOrEmpty(individual.DiedPlace.Trim()))
					diedDate = string.Format("d. {0} {1}", died, individual.DiedPlace).Trim();
				if (!string.IsNullOrEmpty(bornDate) || !string.IsNullOrEmpty(diedDate))
				{
					name += " (";
					if (!string.IsNullOrEmpty(bornDate))
						name += bornDate;
					if (!string.IsNullOrEmpty(bornDate) && !string.IsNullOrEmpty(diedDate))
						name += " ";
					if (!string.IsNullOrEmpty(diedDate))
						name += diedDate;
					name += ")";
				}
			}
			return name;
		}
	}
}
