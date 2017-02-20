using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GedcomLib;
using Ancestry_Reporter.Reports;

namespace Ancestry_Reporter
{
	public partial class MainForm : Form
	{
		private GedcomParser parser = new GedcomParser();

		public MainForm()
		{
			InitializeComponent();
		}

		private void toggleUI(bool enabled)
		{
			butImport.Enabled = enabled;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			var formatItems = new[] {
				new { Value = ReportType.FullAncestry, Text = Extensions.GetDescription<ReportType>(ReportType.FullAncestry) },
				new { Value = ReportType.AbridgedAncestry, Text = Extensions.GetDescription<ReportType>(ReportType.AbridgedAncestry) },
				new { Value = ReportType.LeafAncestor, Text = Extensions.GetDescription<ReportType>(ReportType.LeafAncestor) },
				new { Value = ReportType.AbridgedLeafAncestor, Text = Extensions.GetDescription<ReportType>(ReportType.AbridgedLeafAncestor) },
				new { Value = ReportType.GenerationSummary, Text = Extensions.GetDescription<ReportType>(ReportType.GenerationSummary) },
				new { Value = ReportType.Descendant, Text = Extensions.GetDescription<ReportType>(ReportType.Descendant) },
				new { Value = ReportType.Place, Text = Extensions.GetDescription<ReportType>(ReportType.Place) },
                new { Value = ReportType.PlaceSummary, Text = Extensions.GetDescription<ReportType>(ReportType.PlaceSummary) },
                new { Value = ReportType.CountryOfOrigin, Text = Extensions.GetDescription<ReportType>(ReportType.CountryOfOrigin) },
                new { Value = ReportType.CountryOfOriginSummary, Text = Extensions.GetDescription<ReportType>(ReportType.CountryOfOriginSummary) },
                new { Value = ReportType.YDNA, Text = Extensions.GetDescription<ReportType>(ReportType.YDNA) },
				new { Value = ReportType.MtDNA, Text = Extensions.GetDescription<ReportType>(ReportType.MtDNA) },
                new { Value = ReportType.Lifespan, Text = Extensions.GetDescription<ReportType>(ReportType.Lifespan) }
            };

			cboReportType.DisplayMember = "Text";
			cboReportType.ValueMember = "Value";
			cboReportType.DataSource = formatItems;
		}

		private void butImport_Click(object sender, EventArgs e)
		{
			toggleUI(false);
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Gedcom Files|*.ged";
			openFileDialog.Title = "Select a Gedcom File";

			if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				lblStatus.Text = "Loading Gedcom...";
				Application.DoEvents();

				parser.Parse(openFileDialog.FileName);
				lblFamilies.Text = String.Format("{0}", parser.gedcomFamilies.Count);
				lblIndividuals.Text = String.Format("{0}", parser.gedcomIndividuals.Count);

				lblStatus.Text = "Finished loading Gedcom";
				Application.DoEvents();

			}
			toggleUI(true);
		}

		private void label6_Click(object sender, EventArgs e)
		{

		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void butOutput_Click(object sender, EventArgs e)
		{
			toggleUI(false);
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Text Files|*.txt";
			saveFileDialog.Title = "Save report as";

			if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				txtOutputPath.Text = saveFileDialog.FileName;
			}
			toggleUI(true);
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			toggleUI(false);
			lblStatus.Text = "Generating report....";
			Application.DoEvents();

			IBaseReport report;

			ReportType reportType = (ReportType)(cboReportType.SelectedValue);
			switch (reportType)
			{
				case ReportType.FullAncestry:
					report = new FullAncestryReport();
					break;
				case ReportType.AbridgedAncestry:
					report = new NameOnlyAncestryReport();
					break;
				case ReportType.LeafAncestor:
					report = new LeafAncestorReport();
					break;
				case ReportType.AbridgedLeafAncestor:
					report = new LeafNameOnlyAncestorReport();
					break;
				case ReportType.GenerationSummary:
					report = new GenerationCountReport();
					break;
				case ReportType.Descendant:
					report = new DescendantReport();
					break;
				case ReportType.Place:
					report = new PlaceReport();
					break;
                case ReportType.PlaceSummary:
                    report = new PlaceSummaryReport();
                    break;
                case ReportType.YDNA:
					report = new YDNAReport();
					break;
				case ReportType.MtDNA:
					report = new MtDNAReport();
					break;
                case ReportType.Lifespan:
                    report = new LifespanReport();
                    break;
                case ReportType.CountryOfOrigin:
                    report = new CountryOfOriginReport();
                    break;
                case ReportType.CountryOfOriginSummary:
                    report = new CountryOfOriginSummaryReport();
                    break;
                default:
					report = new FullAncestryReport();
					break;
			}

			report.GenerateReport(txtRootPerson.Text, Int32.Parse(txtMaxDepth.Text), txtOutputPath.Text, parser.gedcomIndividuals, parser.gedcomFamilies);

			lblStatus.Text = "Finished generating report.";
			Application.DoEvents();
			toggleUI(true);
		}
	}
}

