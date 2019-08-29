using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z2R_Mapper.Palace_Routing;

namespace Z2R_Mapper
{
    public class PalaceAnalyticsController
    {
        private PalaceRoutingAnalysis _viewReference;
        private RoutingAnalytics _model;

        private bool _folderSelectionValid = false;
        private string _romFilesFolder = "";
        private int _nesFileCount = 0;

        private AnalyzerSettings _currentAnalyzerSettings;

        private bool _dataIsReady = false;
        private AnalyzerSettings _analyzerSettingsForCurrentDataSet;

        private ReportType _reportType;
        private RoomsToInclude _roomsToInclude;

        public PalaceAnalyticsController(PalaceRoutingAnalysis viewReference)
        {
            _viewReference = viewReference;
            _model = new RoutingAnalytics();
        }

        public void UpdateCurrentAnalyzerSettings(AnalyzerSettings settings)
        {
            _currentAnalyzerSettings = settings;
            if (_dataIsReady && !AnalyzerSettingsMatch())
            {
                _viewReference.ShowDataMismatchWarning();
            } else
            {
                _viewReference.HideDataMismatchWarning();
            }
        }

        public void SetRomFilesFolder(string folderPath)
        {
            _romFilesFolder = folderPath;
            string[] files;
            try
            {
                files = Directory.GetFiles(folderPath);
            }
            catch
            {
                _folderSelectionValid = false;
                return;
            }

            _nesFileCount = 0;
            foreach (string fileName in files)
            {
                string extension = Path.GetExtension(fileName);
                if ((extension == ".nes") || (extension == ".NES"))
                {
                    _nesFileCount++;
                }
            }
            if (_nesFileCount > 0)
            {
                _folderSelectionValid = true;
            }
            else
            {
                _folderSelectionValid = false;
            }
            _viewReference.SetNumNesFilesInROMFilesFolder(_nesFileCount);
        }

        public void AnalyzeFiles()
        {
            if (!_folderSelectionValid || (_nesFileCount == 0))
                return;

            _viewReference.RomAnalysisStarted(_nesFileCount);

            _model.ChangeSettings(_currentAnalyzerSettings);
            _model.ResetStatistics();

            string[] files = Directory.GetFiles(_romFilesFolder);
            int numFilesAnalyzed = 0;
            foreach (string fileName in files)
            {
                string extension = Path.GetExtension(fileName);
                if ((extension == ".nes") || (extension == ".NES"))
                {
                    _model.AnalyzeNextROMFile(fileName);
                    numFilesAnalyzed++;
                    _viewReference.RomAnalysisStatusUpdate(numFilesAnalyzed);
                }
            }

            UpdateDataReady(true);
            _viewReference.RomAnalysisComplete(true);
        }

        private void UpdateDataReady(bool dataIsReady)
        {
            if(dataIsReady)
            {
                _analyzerSettingsForCurrentDataSet = new AnalyzerSettings();
                _analyzerSettingsForCurrentDataSet.routeType = _currentAnalyzerSettings.routeType;
                _analyzerSettingsForCurrentDataSet.includeP1 = _currentAnalyzerSettings.includeP1;
                _analyzerSettingsForCurrentDataSet.includeP2 = _currentAnalyzerSettings.includeP2;
                _analyzerSettingsForCurrentDataSet.includeP3 = _currentAnalyzerSettings.includeP3;
                _analyzerSettingsForCurrentDataSet.includeP4 = _currentAnalyzerSettings.includeP4;
                _analyzerSettingsForCurrentDataSet.includeP5 = _currentAnalyzerSettings.includeP5;
                _analyzerSettingsForCurrentDataSet.includeP6 = _currentAnalyzerSettings.includeP6;
                _analyzerSettingsForCurrentDataSet.includeGP = _currentAnalyzerSettings.includeGP;
                _dataIsReady = true;
            } else
            {
                _analyzerSettingsForCurrentDataSet = null;
                _dataIsReady = false;
            }
        }

        private bool AnalyzerSettingsMatch()
        {
            if (_currentAnalyzerSettings == null || _analyzerSettingsForCurrentDataSet == null)
                return false;

            if (_currentAnalyzerSettings.routeType != _analyzerSettingsForCurrentDataSet.routeType)
                return false;
            if (_currentAnalyzerSettings.includeP1 != _analyzerSettingsForCurrentDataSet.includeP1)
                return false;
            if (_currentAnalyzerSettings.includeP2 != _analyzerSettingsForCurrentDataSet.includeP2)
                return false;
            if (_currentAnalyzerSettings.includeP3 != _analyzerSettingsForCurrentDataSet.includeP3)
                return false;
            if (_currentAnalyzerSettings.includeP4 != _analyzerSettingsForCurrentDataSet.includeP4)
                return false;
            if (_currentAnalyzerSettings.includeP5 != _analyzerSettingsForCurrentDataSet.includeP5)
                return false;
            if (_currentAnalyzerSettings.includeP6 != _analyzerSettingsForCurrentDataSet.includeP6)
                return false;
            if (_currentAnalyzerSettings.includeGP != _analyzerSettingsForCurrentDataSet.includeGP)
                return false;

            return true;
        }

        public void UpdateReportSettings(ReportType reportType, RoomsToInclude roomsToInclude)
        {
            _reportType = reportType;
            _roomsToInclude = roomsToInclude;

            if ((_reportType == ReportType.WhichWayToGo) && (roomsToInclude == RoomsToInclude.Passthrough))
            {
                _viewReference.ShowMeaninglessResultsWarning();
            } else
            {
                _viewReference.HideMeaninglessResultsWarning();
            }
        }

        public void GenerateReport(string outputFileName)
        {
            if (!IsOutputFileValid(outputFileName))
            {
                _viewReference.ShowMessage("Bad filename: " + outputFileName);
                return;
            }

            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                string csvOutput = _model.GenerateReport(_reportType, _roomsToInclude);
                sw.Write(csvOutput);
            }

            _viewReference.ShowMessage(outputFileName + " created!");
        }

        private bool IsOutputFileValid(string fileName)
        {
            System.IO.FileInfo fi = null;
            try
            {
                fi = new System.IO.FileInfo(fileName);
            }
            catch (ArgumentException) { }
            catch (System.IO.PathTooLongException) { }
            catch (NotSupportedException) { }
            if (ReferenceEquals(fi, null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
