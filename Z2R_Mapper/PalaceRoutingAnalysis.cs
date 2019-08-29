using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Z2R_Mapper.Palace_Routing;

namespace Z2R_Mapper
{
    public partial class PalaceRoutingAnalysis : Form
    {
        private PalaceAnalyticsController _palaceAnalyticsController;

        public PalaceRoutingAnalysis()
        {
            InitializeComponent();
            _palaceAnalyticsController = new PalaceAnalyticsController(this);
            UpdateAnalyzerSettingsToController();
        }

        private void analyzerSettings_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAnalyzerSettingsToController();
        }

        private void UpdateAnalyzerSettingsToController()
        {
            AnalyzerSettings settings = new AnalyzerSettings(); ;
            settings.routeType = PalaceRouteType.EntranceToBoss;
            if (entranceToItemRadioButton.Checked)
            {
                settings.routeType = PalaceRouteType.EntranceToItem;
            }
            else if (itemToBossRadioButton.Checked)
            {
                settings.routeType = PalaceRouteType.ItemToBoss;
            }
            else if (entranceToBossRadioButton.Checked)
            {
                settings.routeType = PalaceRouteType.EntranceToBoss;
            }
            settings.includeP1 = includeP1CheckBox.Checked;
            settings.includeP2 = includeP2CheckBox.Checked;
            settings.includeP3 = includeP3CheckBox.Checked;
            settings.includeP4 = includeP4CheckBox.Checked;
            settings.includeP5 = includeP5CheckBox.Checked;
            settings.includeP6 = includeP6CheckBox.Checked;
            settings.includeGP = includeGPCheckBox.Checked;
            _palaceAnalyticsController.UpdateCurrentAnalyzerSettings(settings);
        }

        private void romFilesFolderSelectButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    romFilesFolderTextBox.Text = fbd.SelectedPath;
                    _palaceAnalyticsController.SetRomFilesFolder(romFilesFolderTextBox.Text);
                }
            }
        }

        private void romFilesFolderTextBox_Validating(object sender, CancelEventArgs e)
        {
            _palaceAnalyticsController.SetRomFilesFolder(romFilesFolderTextBox.Text);
        }

        public void SetNumNesFilesInROMFilesFolder(int numFiles)
        {
            numRomFilesLabel.Text = "(" + numFiles.ToString() + " NES files)";
        }

        private void analyzeButton_Click(object sender, EventArgs e)
        {
            if (!romAnalysisBackgroundWorker.IsBusy)
            {
                romAnalysisBackgroundWorker.RunWorkerAsync();
            } else
            {
                romAnalysisBackgroundWorker.CancelAsync();
            }
        }

        private void analyzingROMSBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _palaceAnalyticsController.AnalyzeFiles();
        }

        private void analyzingROMSBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void analyzingROMSBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        public void RomAnalysisStarted(int numRomFiles)
        {
            // Somewhat ugly pattern for switching to UI thread before updating component.
            if (routeToAnalyzeGroupBox.InvokeRequired)
            {
                routeToAnalyzeGroupBox.Invoke(new MethodInvoker(() => { RomAnalysisStarted(numRomFiles); }));
            } else
            {
                routeToAnalyzeGroupBox.Enabled = false;
                palacesToAnalyzeGroupBox.Enabled = false;
                romFilesFolderTextBox.Enabled = false;
                romFilesFolderSelectButton.Enabled = false;
                analyzeButton.Text = "Cancel";
                dataMismatchWarningLabel.Visible = false;
                generateReportButton.Enabled = false;
                romAnalysisProgressBar.Value = 0;
                romAnalysisProgressBar.Maximum = numRomFiles;
            }
        }

        public void RomAnalysisStatusUpdate(int numFilesAnalyzed)
        {
            // Not sure how to properly set up Model-View-Controller without
            // running into this ugly pattern.
            if (romAnalysisProgressBar.InvokeRequired)
            {
                romAnalysisProgressBar.Invoke(new MethodInvoker(() => { RomAnalysisStatusUpdate(numFilesAnalyzed); }));
            } else
            {
                romAnalysisProgressBar.Value = numFilesAnalyzed;
            }
        }

        public void RomAnalysisComplete(bool dataIsReady)
        {
            if (routeToAnalyzeGroupBox.InvokeRequired)
            {
                routeToAnalyzeGroupBox.Invoke(new MethodInvoker(() => { RomAnalysisComplete(dataIsReady); }));
            }
            else
            {
                routeToAnalyzeGroupBox.Enabled = true;
                palacesToAnalyzeGroupBox.Enabled = true;
                romFilesFolderTextBox.Enabled = true;
                romFilesFolderSelectButton.Enabled = true;
                analyzeButton.Text = "Analyze";
                generateReportButton.Enabled = dataIsReady;
                romAnalysisProgressBar.Value = dataIsReady ? romAnalysisProgressBar.Maximum : 0;
            }
        }

        public void ShowDataMismatchWarning()
        {
            dataMismatchWarningLabel.Visible = true;
        }

        public void HideDataMismatchWarning()
        {
            dataMismatchWarningLabel.Visible = false;
        }

        private void reportSettings_CheckedChanged(object sender, EventArgs e)
        {
            UpdateReportSettingsToController();
        }

        private void UpdateReportSettingsToController()
        {
            ReportType reportType = ReportType.WhichWayToGo;
            if (whichWayReportRadioButton.Checked)
            {
                reportType = ReportType.WhichWayToGo;
            }
            else if (correctPathFromEntranceReportRadioButton.Checked)
            {
                reportType = ReportType.CorrectPathEntering;
            }
            else if (correctPathFromExitReportRadioButton.Checked)
            {
                reportType = ReportType.CorrectPathExiting;
            }
            else if (routeRequirementsReportRadioButton.Checked)
            {
                reportType = ReportType.RouteRequirements;
            }
            RoomsToInclude roomsToInclude = RoomsToInclude.Decision;
            if (decisionPointRoomsRadioButton.Checked)
            {
                roomsToInclude = RoomsToInclude.Decision;
            }
            else if (passthroughRoomsRadioButton.Checked)
            {
                roomsToInclude = RoomsToInclude.Passthrough;
            }
            else if (allRoomsRadioButton.Checked)
            {
                roomsToInclude = RoomsToInclude.All;
            }
            _palaceAnalyticsController.UpdateReportSettings(reportType, roomsToInclude);
        }

        public void ShowMeaninglessResultsWarning()
        {
            meaninglessResultsWarningLabel.Visible = true;
        }

        public void HideMeaninglessResultsWarning()
        {
            meaninglessResultsWarningLabel.Visible = false;
        }

        private void outputFileBrowseButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    outputFileTextBox.Text = saveFileDialog.FileName;
                }
            }
        }

        private void generateReportButton_Click(object sender, EventArgs e)
        {
            _palaceAnalyticsController.GenerateReport(outputFileTextBox.Text);
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
