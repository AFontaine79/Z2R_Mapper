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

namespace Z2R_Mapper
{
    public partial class Form1 : Form
    {
        Z2R_Mapper _z2rMapper;

        public Form1()
        {
            InitializeComponent();
        }

        private void OpenZeldaIIROMFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "iNES ROM files (*.nes)|*.nes";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Load this ROM file
                    OpenROMFile(openFileDialog.FileName);
                }
            }
        }

        private void OpenROMFile(String filename)
        {
            //try
            //{
                _z2rMapper = new Z2R_Mapper(filename);
                LoadDataIntoForms();
                this.Text = string.Format("{0} - Z2R Mapper", System.IO.Path.GetFileNameWithoutExtension(filename));
            //}
            //catch (System.Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //}
        }

        private void LoadDataIntoForms()
        {
            westernHyrulePictureBox.Image = _z2rMapper.GetOverworldBitmap(OverworldArea.WesternHyrule);
            easternHyrulePictureBox.Image = _z2rMapper.GetOverworldBitmap(OverworldArea.EasternHyrule);
            deathMountainPictureBox.Image = _z2rMapper.GetOverworldBitmap(OverworldArea.DeathMountain);
            mazeIslandPictureBox.Image = _z2rMapper.GetOverworldBitmap(OverworldArea.MazeIsland);

            itemSummaryTextBox.Text = _z2rMapper.GetItemSummary();
            spellSummaryTextBox.Text = _z2rMapper.GetSpellSummary();
            spellCostsTextBox.Text = _z2rMapper.GetSpellCostsSummary();
            RedrawStatsSummary();
            RedrawPalaceRoutingSummary();
        }

        private void RedrawStatsSummary()
        {
            startingStatsTextBox.Text = _z2rMapper.GetStartingStatsSummary(
                showMaxHeartContainersCheckBox.Checked, showCombinedSpellCheckBox.Checked);
        }

        private void RedrawPalaceRoutingSummary()
        {
            palaceRoutingTextBox.Text = _z2rMapper.GetPalaceRoutingSummary(
                showDirectionsCheckBox.Checked, 
                showRequirementsCheckBox.Checked,
                showItemToBossCheckBox.Checked);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();

            // Allow auto-opening of a ROM file by simply dragging the file on top of the executable.
            if (args.Length > 1)
            {
                OpenROMFile(args[1]);
            }
        }

        private void showMaxHeartContainersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RedrawStatsSummary();
        }

        private void showCombinedSpellCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RedrawStatsSummary();
        }

        private void showMagicCostsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(showMagicCostsCheckBox.Checked)
            {
                spellCostsTextBox.Visible = true;
            } else
            {
                spellCostsTextBox.Visible = false;
            }
        }

        private void showDirectionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RedrawPalaceRoutingSummary();
        }

        private void showRequirementsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RedrawPalaceRoutingSummary();
        }

        private void showItemToBossCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RedrawPalaceRoutingSummary();
        }
    }
}
