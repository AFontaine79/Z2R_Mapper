namespace Z2R_Mapper
{
    partial class PalaceRoutingAnalysis
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.routeToAnalyzeGroupBox = new System.Windows.Forms.GroupBox();
            this.entranceToBossRadioButton = new System.Windows.Forms.RadioButton();
            this.itemToBossRadioButton = new System.Windows.Forms.RadioButton();
            this.entranceToItemRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.romFilesFolderTextBox = new System.Windows.Forms.TextBox();
            this.romFilesFolderSelectButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.outputFileTextBox = new System.Windows.Forms.TextBox();
            this.outputFileBrowseButton = new System.Windows.Forms.Button();
            this.analyzeButton = new System.Windows.Forms.Button();
            this.numRomFilesLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.allRoomsRadioButton = new System.Windows.Forms.RadioButton();
            this.passthroughRoomsRadioButton = new System.Windows.Forms.RadioButton();
            this.decisionPointRoomsRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.routeRequirementsReportRadioButton = new System.Windows.Forms.RadioButton();
            this.correctPathFromExitReportRadioButton = new System.Windows.Forms.RadioButton();
            this.correctPathFromEntranceReportRadioButton = new System.Windows.Forms.RadioButton();
            this.whichWayReportRadioButton = new System.Windows.Forms.RadioButton();
            this.palacesToAnalyzeGroupBox = new System.Windows.Forms.GroupBox();
            this.includeGPCheckBox = new System.Windows.Forms.CheckBox();
            this.includeP6CheckBox = new System.Windows.Forms.CheckBox();
            this.includeP5CheckBox = new System.Windows.Forms.CheckBox();
            this.includeP4CheckBox = new System.Windows.Forms.CheckBox();
            this.includeP3CheckBox = new System.Windows.Forms.CheckBox();
            this.includeP2CheckBox = new System.Windows.Forms.CheckBox();
            this.includeP1CheckBox = new System.Windows.Forms.CheckBox();
            this.romAnalysisProgressBar = new System.Windows.Forms.ProgressBar();
            this.generateReportButton = new System.Windows.Forms.Button();
            this.dataMismatchWarningLabel = new System.Windows.Forms.Label();
            this.meaninglessResultsWarningLabel = new System.Windows.Forms.Label();
            this.romAnalysisBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.routeToAnalyzeGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.palacesToAnalyzeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // routeToAnalyzeGroupBox
            // 
            this.routeToAnalyzeGroupBox.Controls.Add(this.entranceToBossRadioButton);
            this.routeToAnalyzeGroupBox.Controls.Add(this.itemToBossRadioButton);
            this.routeToAnalyzeGroupBox.Controls.Add(this.entranceToItemRadioButton);
            this.routeToAnalyzeGroupBox.Location = new System.Drawing.Point(10, 13);
            this.routeToAnalyzeGroupBox.Name = "routeToAnalyzeGroupBox";
            this.routeToAnalyzeGroupBox.Size = new System.Drawing.Size(419, 56);
            this.routeToAnalyzeGroupBox.TabIndex = 0;
            this.routeToAnalyzeGroupBox.TabStop = false;
            this.routeToAnalyzeGroupBox.Text = "Route to Analyze";
            // 
            // entranceToBossRadioButton
            // 
            this.entranceToBossRadioButton.AutoSize = true;
            this.entranceToBossRadioButton.Checked = true;
            this.entranceToBossRadioButton.Location = new System.Drawing.Point(259, 22);
            this.entranceToBossRadioButton.Name = "entranceToBossRadioButton";
            this.entranceToBossRadioButton.Size = new System.Drawing.Size(137, 21);
            this.entranceToBossRadioButton.TabIndex = 2;
            this.entranceToBossRadioButton.TabStop = true;
            this.entranceToBossRadioButton.Text = "Entrance to Boss";
            this.entranceToBossRadioButton.UseVisualStyleBackColor = true;
            this.entranceToBossRadioButton.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // itemToBossRadioButton
            // 
            this.itemToBossRadioButton.AutoSize = true;
            this.itemToBossRadioButton.Location = new System.Drawing.Point(146, 22);
            this.itemToBossRadioButton.Name = "itemToBossRadioButton";
            this.itemToBossRadioButton.Size = new System.Drawing.Size(106, 21);
            this.itemToBossRadioButton.TabIndex = 1;
            this.itemToBossRadioButton.Text = "Item to Boss";
            this.itemToBossRadioButton.UseVisualStyleBackColor = true;
            this.itemToBossRadioButton.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // entranceToItemRadioButton
            // 
            this.entranceToItemRadioButton.AutoSize = true;
            this.entranceToItemRadioButton.Location = new System.Drawing.Point(7, 22);
            this.entranceToItemRadioButton.Name = "entranceToItemRadioButton";
            this.entranceToItemRadioButton.Size = new System.Drawing.Size(132, 21);
            this.entranceToItemRadioButton.TabIndex = 0;
            this.entranceToItemRadioButton.Text = "Entrance to Item";
            this.entranceToItemRadioButton.UseVisualStyleBackColor = true;
            this.entranceToItemRadioButton.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Folder of ROM Files";
            // 
            // romFilesFolderTextBox
            // 
            this.romFilesFolderTextBox.Location = new System.Drawing.Point(10, 112);
            this.romFilesFolderTextBox.Name = "romFilesFolderTextBox";
            this.romFilesFolderTextBox.Size = new System.Drawing.Size(530, 22);
            this.romFilesFolderTextBox.TabIndex = 4;
            this.romFilesFolderTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.romFilesFolderTextBox_Validating);
            // 
            // romFilesFolderSelectButton
            // 
            this.romFilesFolderSelectButton.Location = new System.Drawing.Point(546, 99);
            this.romFilesFolderSelectButton.Name = "romFilesFolderSelectButton";
            this.romFilesFolderSelectButton.Size = new System.Drawing.Size(109, 35);
            this.romFilesFolderSelectButton.TabIndex = 5;
            this.romFilesFolderSelectButton.Text = "Select Folder";
            this.romFilesFolderSelectButton.UseVisualStyleBackColor = true;
            this.romFilesFolderSelectButton.Click += new System.EventHandler(this.romFilesFolderSelectButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 336);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Output File";
            // 
            // outputFileTextBox
            // 
            this.outputFileTextBox.Location = new System.Drawing.Point(8, 357);
            this.outputFileTextBox.Name = "outputFileTextBox";
            this.outputFileTextBox.Size = new System.Drawing.Size(528, 22);
            this.outputFileTextBox.TabIndex = 11;
            // 
            // outputFileBrowseButton
            // 
            this.outputFileBrowseButton.Location = new System.Drawing.Point(538, 354);
            this.outputFileBrowseButton.Name = "outputFileBrowseButton";
            this.outputFileBrowseButton.Size = new System.Drawing.Size(117, 28);
            this.outputFileBrowseButton.TabIndex = 12;
            this.outputFileBrowseButton.Text = "Browse";
            this.outputFileBrowseButton.UseVisualStyleBackColor = true;
            this.outputFileBrowseButton.Click += new System.EventHandler(this.outputFileBrowseButton_Click);
            // 
            // analyzeButton
            // 
            this.analyzeButton.Location = new System.Drawing.Point(509, 140);
            this.analyzeButton.Name = "analyzeButton";
            this.analyzeButton.Size = new System.Drawing.Size(146, 37);
            this.analyzeButton.TabIndex = 7;
            this.analyzeButton.Text = "Analyze";
            this.analyzeButton.UseVisualStyleBackColor = true;
            this.analyzeButton.Click += new System.EventHandler(this.analyzeButton_Click);
            // 
            // numRomFilesLabel
            // 
            this.numRomFilesLabel.AutoSize = true;
            this.numRomFilesLabel.Location = new System.Drawing.Point(150, 87);
            this.numRomFilesLabel.Name = "numRomFilesLabel";
            this.numRomFilesLabel.Size = new System.Drawing.Size(87, 17);
            this.numRomFilesLabel.TabIndex = 3;
            this.numRomFilesLabel.Text = "(0 NES files)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.allRoomsRadioButton);
            this.groupBox2.Controls.Add(this.passthroughRoomsRadioButton);
            this.groupBox2.Controls.Add(this.decisionPointRoomsRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(10, 280);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(442, 53);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rooms to Include in Report";
            // 
            // allRoomsRadioButton
            // 
            this.allRoomsRadioButton.AutoSize = true;
            this.allRoomsRadioButton.Location = new System.Drawing.Point(343, 21);
            this.allRoomsRadioButton.Name = "allRoomsRadioButton";
            this.allRoomsRadioButton.Size = new System.Drawing.Size(92, 21);
            this.allRoomsRadioButton.TabIndex = 2;
            this.allRoomsRadioButton.Text = "All Rooms";
            this.allRoomsRadioButton.UseVisualStyleBackColor = true;
            this.allRoomsRadioButton.CheckedChanged += new System.EventHandler(this.reportSettings_CheckedChanged);
            // 
            // passthroughRoomsRadioButton
            // 
            this.passthroughRoomsRadioButton.AutoSize = true;
            this.passthroughRoomsRadioButton.Location = new System.Drawing.Point(180, 21);
            this.passthroughRoomsRadioButton.Name = "passthroughRoomsRadioButton";
            this.passthroughRoomsRadioButton.Size = new System.Drawing.Size(157, 21);
            this.passthroughRoomsRadioButton.TabIndex = 1;
            this.passthroughRoomsRadioButton.Text = "Passthrough Rooms";
            this.passthroughRoomsRadioButton.UseVisualStyleBackColor = true;
            this.passthroughRoomsRadioButton.CheckedChanged += new System.EventHandler(this.reportSettings_CheckedChanged);
            // 
            // decisionPointRoomsRadioButton
            // 
            this.decisionPointRoomsRadioButton.AutoSize = true;
            this.decisionPointRoomsRadioButton.Checked = true;
            this.decisionPointRoomsRadioButton.Location = new System.Drawing.Point(7, 21);
            this.decisionPointRoomsRadioButton.Name = "decisionPointRoomsRadioButton";
            this.decisionPointRoomsRadioButton.Size = new System.Drawing.Size(167, 21);
            this.decisionPointRoomsRadioButton.TabIndex = 0;
            this.decisionPointRoomsRadioButton.TabStop = true;
            this.decisionPointRoomsRadioButton.Text = "Decision Point Rooms";
            this.decisionPointRoomsRadioButton.UseVisualStyleBackColor = true;
            this.decisionPointRoomsRadioButton.CheckedChanged += new System.EventHandler(this.reportSettings_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.routeRequirementsReportRadioButton);
            this.groupBox3.Controls.Add(this.correctPathFromExitReportRadioButton);
            this.groupBox3.Controls.Add(this.correctPathFromEntranceReportRadioButton);
            this.groupBox3.Controls.Add(this.whichWayReportRadioButton);
            this.groupBox3.Location = new System.Drawing.Point(10, 191);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(434, 83);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Report Type";
            // 
            // routeRequirementsReportRadioButton
            // 
            this.routeRequirementsReportRadioButton.AutoSize = true;
            this.routeRequirementsReportRadioButton.Location = new System.Drawing.Point(252, 48);
            this.routeRequirementsReportRadioButton.Name = "routeRequirementsReportRadioButton";
            this.routeRequirementsReportRadioButton.Size = new System.Drawing.Size(159, 21);
            this.routeRequirementsReportRadioButton.TabIndex = 3;
            this.routeRequirementsReportRadioButton.Text = "Route Requirements";
            this.routeRequirementsReportRadioButton.UseVisualStyleBackColor = true;
            this.routeRequirementsReportRadioButton.CheckedChanged += new System.EventHandler(this.reportSettings_CheckedChanged);
            // 
            // correctPathFromExitReportRadioButton
            // 
            this.correctPathFromExitReportRadioButton.AutoSize = true;
            this.correctPathFromExitReportRadioButton.Location = new System.Drawing.Point(7, 48);
            this.correctPathFromExitReportRadioButton.Name = "correctPathFromExitReportRadioButton";
            this.correctPathFromExitReportRadioButton.Size = new System.Drawing.Size(239, 21);
            this.correctPathFromExitReportRadioButton.TabIndex = 2;
            this.correctPathFromExitReportRadioButton.Text = "Correct Path Based on Room Exit";
            this.correctPathFromExitReportRadioButton.UseVisualStyleBackColor = true;
            this.correctPathFromExitReportRadioButton.CheckedChanged += new System.EventHandler(this.reportSettings_CheckedChanged);
            // 
            // correctPathFromEntranceReportRadioButton
            // 
            this.correctPathFromEntranceReportRadioButton.AutoSize = true;
            this.correctPathFromEntranceReportRadioButton.Location = new System.Drawing.Point(152, 21);
            this.correctPathFromEntranceReportRadioButton.Name = "correctPathFromEntranceReportRadioButton";
            this.correctPathFromEntranceReportRadioButton.Size = new System.Drawing.Size(274, 21);
            this.correctPathFromEntranceReportRadioButton.TabIndex = 1;
            this.correctPathFromEntranceReportRadioButton.Text = "Correct Path Based on Room Entrance";
            this.correctPathFromEntranceReportRadioButton.UseVisualStyleBackColor = true;
            this.correctPathFromEntranceReportRadioButton.CheckedChanged += new System.EventHandler(this.reportSettings_CheckedChanged);
            // 
            // whichWayReportRadioButton
            // 
            this.whichWayReportRadioButton.AutoSize = true;
            this.whichWayReportRadioButton.Checked = true;
            this.whichWayReportRadioButton.Location = new System.Drawing.Point(7, 21);
            this.whichWayReportRadioButton.Name = "whichWayReportRadioButton";
            this.whichWayReportRadioButton.Size = new System.Drawing.Size(139, 21);
            this.whichWayReportRadioButton.TabIndex = 0;
            this.whichWayReportRadioButton.TabStop = true;
            this.whichWayReportRadioButton.Text = "Which Way to Go";
            this.whichWayReportRadioButton.UseVisualStyleBackColor = true;
            this.whichWayReportRadioButton.CheckedChanged += new System.EventHandler(this.reportSettings_CheckedChanged);
            // 
            // palacesToAnalyzeGroupBox
            // 
            this.palacesToAnalyzeGroupBox.Controls.Add(this.includeGPCheckBox);
            this.palacesToAnalyzeGroupBox.Controls.Add(this.includeP6CheckBox);
            this.palacesToAnalyzeGroupBox.Controls.Add(this.includeP5CheckBox);
            this.palacesToAnalyzeGroupBox.Controls.Add(this.includeP4CheckBox);
            this.palacesToAnalyzeGroupBox.Controls.Add(this.includeP3CheckBox);
            this.palacesToAnalyzeGroupBox.Controls.Add(this.includeP2CheckBox);
            this.palacesToAnalyzeGroupBox.Controls.Add(this.includeP1CheckBox);
            this.palacesToAnalyzeGroupBox.Location = new System.Drawing.Point(438, 13);
            this.palacesToAnalyzeGroupBox.Name = "palacesToAnalyzeGroupBox";
            this.palacesToAnalyzeGroupBox.Size = new System.Drawing.Size(217, 80);
            this.palacesToAnalyzeGroupBox.TabIndex = 1;
            this.palacesToAnalyzeGroupBox.TabStop = false;
            this.palacesToAnalyzeGroupBox.Text = "Palaces to Analyze";
            // 
            // includeGPCheckBox
            // 
            this.includeGPCheckBox.AutoSize = true;
            this.includeGPCheckBox.Checked = true;
            this.includeGPCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeGPCheckBox.Location = new System.Drawing.Point(113, 49);
            this.includeGPCheckBox.Name = "includeGPCheckBox";
            this.includeGPCheckBox.Size = new System.Drawing.Size(50, 21);
            this.includeGPCheckBox.TabIndex = 6;
            this.includeGPCheckBox.Text = "GP";
            this.includeGPCheckBox.UseVisualStyleBackColor = true;
            this.includeGPCheckBox.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // includeP6CheckBox
            // 
            this.includeP6CheckBox.AutoSize = true;
            this.includeP6CheckBox.Checked = true;
            this.includeP6CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeP6CheckBox.Location = new System.Drawing.Point(60, 49);
            this.includeP6CheckBox.Name = "includeP6CheckBox";
            this.includeP6CheckBox.Size = new System.Drawing.Size(47, 21);
            this.includeP6CheckBox.TabIndex = 5;
            this.includeP6CheckBox.Text = "P6";
            this.includeP6CheckBox.UseVisualStyleBackColor = true;
            this.includeP6CheckBox.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // includeP5CheckBox
            // 
            this.includeP5CheckBox.AutoSize = true;
            this.includeP5CheckBox.Checked = true;
            this.includeP5CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeP5CheckBox.Location = new System.Drawing.Point(6, 49);
            this.includeP5CheckBox.Name = "includeP5CheckBox";
            this.includeP5CheckBox.Size = new System.Drawing.Size(47, 21);
            this.includeP5CheckBox.TabIndex = 4;
            this.includeP5CheckBox.Text = "P5";
            this.includeP5CheckBox.UseVisualStyleBackColor = true;
            this.includeP5CheckBox.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // includeP4CheckBox
            // 
            this.includeP4CheckBox.AutoSize = true;
            this.includeP4CheckBox.Checked = true;
            this.includeP4CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeP4CheckBox.Location = new System.Drawing.Point(166, 21);
            this.includeP4CheckBox.Name = "includeP4CheckBox";
            this.includeP4CheckBox.Size = new System.Drawing.Size(47, 21);
            this.includeP4CheckBox.TabIndex = 3;
            this.includeP4CheckBox.Text = "P4";
            this.includeP4CheckBox.UseVisualStyleBackColor = true;
            this.includeP4CheckBox.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // includeP3CheckBox
            // 
            this.includeP3CheckBox.AutoSize = true;
            this.includeP3CheckBox.Checked = true;
            this.includeP3CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeP3CheckBox.Location = new System.Drawing.Point(113, 21);
            this.includeP3CheckBox.Name = "includeP3CheckBox";
            this.includeP3CheckBox.Size = new System.Drawing.Size(47, 21);
            this.includeP3CheckBox.TabIndex = 2;
            this.includeP3CheckBox.Text = "P3";
            this.includeP3CheckBox.UseVisualStyleBackColor = true;
            this.includeP3CheckBox.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // includeP2CheckBox
            // 
            this.includeP2CheckBox.AutoSize = true;
            this.includeP2CheckBox.Checked = true;
            this.includeP2CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeP2CheckBox.Location = new System.Drawing.Point(60, 21);
            this.includeP2CheckBox.Name = "includeP2CheckBox";
            this.includeP2CheckBox.Size = new System.Drawing.Size(47, 21);
            this.includeP2CheckBox.TabIndex = 1;
            this.includeP2CheckBox.Text = "P2";
            this.includeP2CheckBox.UseVisualStyleBackColor = true;
            this.includeP2CheckBox.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // includeP1CheckBox
            // 
            this.includeP1CheckBox.AutoSize = true;
            this.includeP1CheckBox.Checked = true;
            this.includeP1CheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includeP1CheckBox.Location = new System.Drawing.Point(6, 21);
            this.includeP1CheckBox.Name = "includeP1CheckBox";
            this.includeP1CheckBox.Size = new System.Drawing.Size(47, 21);
            this.includeP1CheckBox.TabIndex = 0;
            this.includeP1CheckBox.Text = "P1";
            this.includeP1CheckBox.UseVisualStyleBackColor = true;
            this.includeP1CheckBox.CheckedChanged += new System.EventHandler(this.analyzerSettings_CheckedChanged);
            // 
            // romAnalysisProgressBar
            // 
            this.romAnalysisProgressBar.Location = new System.Drawing.Point(10, 147);
            this.romAnalysisProgressBar.Name = "romAnalysisProgressBar";
            this.romAnalysisProgressBar.Size = new System.Drawing.Size(493, 23);
            this.romAnalysisProgressBar.TabIndex = 6;
            // 
            // generateReportButton
            // 
            this.generateReportButton.Enabled = false;
            this.generateReportButton.Location = new System.Drawing.Point(438, 388);
            this.generateReportButton.Name = "generateReportButton";
            this.generateReportButton.Size = new System.Drawing.Size(217, 49);
            this.generateReportButton.TabIndex = 13;
            this.generateReportButton.Text = "Generate Report";
            this.generateReportButton.UseVisualStyleBackColor = true;
            this.generateReportButton.Click += new System.EventHandler(this.generateReportButton_Click);
            // 
            // dataMismatchWarningLabel
            // 
            this.dataMismatchWarningLabel.AutoSize = true;
            this.dataMismatchWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.dataMismatchWarningLabel.Location = new System.Drawing.Point(479, 206);
            this.dataMismatchWarningLabel.Name = "dataMismatchWarningLabel";
            this.dataMismatchWarningLabel.Size = new System.Drawing.Size(157, 68);
            this.dataMismatchWarningLabel.TabIndex = 14;
            this.dataMismatchWarningLabel.Text = "Collected data does not\r\nmatch current settings.\r\nClick \"Analyze\" to\r\nrecalculate" +
    " statistics.";
            this.dataMismatchWarningLabel.Visible = false;
            // 
            // meaninglessResultsWarningLabel
            // 
            this.meaninglessResultsWarningLabel.AutoSize = true;
            this.meaninglessResultsWarningLabel.ForeColor = System.Drawing.Color.Olive;
            this.meaninglessResultsWarningLabel.Location = new System.Drawing.Point(28, 394);
            this.meaninglessResultsWarningLabel.Name = "meaninglessResultsWarningLabel";
            this.meaninglessResultsWarningLabel.Size = new System.Drawing.Size(267, 34);
            this.meaninglessResultsWarningLabel.TabIndex = 15;
            this.meaninglessResultsWarningLabel.Text = "Which Way report on passthrough rooms\r\nwill not return meaningful results.";
            this.meaninglessResultsWarningLabel.Visible = false;
            // 
            // romAnalysisBackgroundWorker
            // 
            this.romAnalysisBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.analyzingROMSBackgroundWorker_DoWork);
            this.romAnalysisBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.analyzingROMSBackgroundWorker_ProgressChanged);
            this.romAnalysisBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.analyzingROMSBackgroundWorker_RunWorkerCompleted);
            // 
            // PalaceRoutingAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 448);
            this.Controls.Add(this.meaninglessResultsWarningLabel);
            this.Controls.Add(this.dataMismatchWarningLabel);
            this.Controls.Add(this.generateReportButton);
            this.Controls.Add(this.romAnalysisProgressBar);
            this.Controls.Add(this.palacesToAnalyzeGroupBox);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.numRomFilesLabel);
            this.Controls.Add(this.analyzeButton);
            this.Controls.Add(this.outputFileBrowseButton);
            this.Controls.Add(this.outputFileTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.romFilesFolderSelectButton);
            this.Controls.Add(this.romFilesFolderTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.routeToAnalyzeGroupBox);
            this.MaximizeBox = false;
            this.Name = "PalaceRoutingAnalysis";
            this.Text = "Palace Analytics";
            this.routeToAnalyzeGroupBox.ResumeLayout(false);
            this.routeToAnalyzeGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.palacesToAnalyzeGroupBox.ResumeLayout(false);
            this.palacesToAnalyzeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox routeToAnalyzeGroupBox;
        private System.Windows.Forms.RadioButton entranceToBossRadioButton;
        private System.Windows.Forms.RadioButton itemToBossRadioButton;
        private System.Windows.Forms.RadioButton entranceToItemRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox romFilesFolderTextBox;
        private System.Windows.Forms.Button romFilesFolderSelectButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button outputFileBrowseButton;
        private System.Windows.Forms.Button analyzeButton;
        private System.Windows.Forms.Label numRomFilesLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton allRoomsRadioButton;
        private System.Windows.Forms.RadioButton passthroughRoomsRadioButton;
        private System.Windows.Forms.RadioButton decisionPointRoomsRadioButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton correctPathFromEntranceReportRadioButton;
        private System.Windows.Forms.RadioButton whichWayReportRadioButton;
        private System.Windows.Forms.GroupBox palacesToAnalyzeGroupBox;
        private System.Windows.Forms.CheckBox includeGPCheckBox;
        private System.Windows.Forms.CheckBox includeP6CheckBox;
        private System.Windows.Forms.CheckBox includeP5CheckBox;
        private System.Windows.Forms.CheckBox includeP4CheckBox;
        private System.Windows.Forms.CheckBox includeP3CheckBox;
        private System.Windows.Forms.CheckBox includeP2CheckBox;
        private System.Windows.Forms.CheckBox includeP1CheckBox;
        private System.Windows.Forms.ProgressBar romAnalysisProgressBar;
        private System.Windows.Forms.RadioButton routeRequirementsReportRadioButton;
        private System.Windows.Forms.RadioButton correctPathFromExitReportRadioButton;
        private System.Windows.Forms.Button generateReportButton;
        private System.Windows.Forms.Label dataMismatchWarningLabel;
        private System.Windows.Forms.Label meaninglessResultsWarningLabel;
        private System.Windows.Forms.TextBox outputFileTextBox;
        private System.ComponentModel.BackgroundWorker romAnalysisBackgroundWorker;
    }
}