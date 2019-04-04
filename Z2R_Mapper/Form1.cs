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
        private Z2R_Mapper _z2rMapper;

        private Bitmap _westernHyruleImageBackup;
        private Bitmap _easternHyruleImageBackup;
        private Bitmap _deathMountainImageBackup;
        private Bitmap _mazeIslandImageBackup;

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
            _westernHyruleImageBackup = _z2rMapper.GetOverworldBitmap(OverworldArea.WesternHyrule);
            westernHyrulePictureBox.Image = _westernHyruleImageBackup;
            _easternHyruleImageBackup = _z2rMapper.GetOverworldBitmap(OverworldArea.EasternHyrule);
            easternHyrulePictureBox.Image = _easternHyruleImageBackup;
            _deathMountainImageBackup = _z2rMapper.GetOverworldBitmap(OverworldArea.DeathMountain);
            deathMountainPictureBox.Image = _deathMountainImageBackup;
            _mazeIslandImageBackup = _z2rMapper.GetOverworldBitmap(OverworldArea.MazeIsland);
            mazeIslandPictureBox.Image = _mazeIslandImageBackup;

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

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if(files.Length > 0)
            {
                OpenROMFile(files[0]);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private bool _panning = false;
        private Point _autoScrollStartPosition = Point.Empty;
        private Point _mouseStartingLocation = Point.Empty;

        private void westernHyruleTabPage_MouseDown(object sender, MouseEventArgs e)
        {
            _panning = true;
            _autoScrollStartPosition = westernHyruleTabPage.AutoScrollPosition;
            _autoScrollStartPosition.X = -_autoScrollStartPosition.X;
            _autoScrollStartPosition.Y = -_autoScrollStartPosition.Y;
            _mouseStartingLocation = e.Location;
        }
        private void westernHyruleTabPage_MouseUp(object sender, MouseEventArgs e)
        {
            _panning = false;
        }
        private void westernHyruleTabPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (_panning)
            {
                Point newAutoScrollPosition = new Point();
                newAutoScrollPosition.X = _autoScrollStartPosition.X + (_mouseStartingLocation.X - e.Location.X);
                newAutoScrollPosition.Y = _autoScrollStartPosition.Y + (_mouseStartingLocation.Y - e.Location.Y);
                westernHyruleTabPage.AutoScrollPosition = newAutoScrollPosition;

                westernHyruleTabPage.Invalidate();
                westernHyrulePictureBox.Invalidate();
            }
        }

        private void deathMountainTabPage_MouseDown(object sender, MouseEventArgs e)
        {
            _panning = true;
            _autoScrollStartPosition = deathMountainTabPage.AutoScrollPosition;
            _autoScrollStartPosition.X = -_autoScrollStartPosition.X;
            _autoScrollStartPosition.Y = -_autoScrollStartPosition.Y;
            _mouseStartingLocation = e.Location;
        }

        private void deathMountainTabPage_MouseUp(object sender, MouseEventArgs e)
        {
            _panning = false;
        }

        private void deathMountainTabPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (_panning)
            {
                Point newAutoScrollPosition = new Point();
                newAutoScrollPosition.X = _autoScrollStartPosition.X + (_mouseStartingLocation.X - e.Location.X);
                newAutoScrollPosition.Y = _autoScrollStartPosition.Y + (_mouseStartingLocation.Y - e.Location.Y);
                deathMountainTabPage.AutoScrollPosition = newAutoScrollPosition;

                deathMountainTabPage.Invalidate();
                deathMountainPictureBox.Invalidate();
            }
        }

        private void easternHyruleTabPage_MouseDown(object sender, MouseEventArgs e)
        {
            _panning = true;
            _autoScrollStartPosition = easternHyruleTabPage.AutoScrollPosition;
            _autoScrollStartPosition.X = -_autoScrollStartPosition.X;
            _autoScrollStartPosition.Y = -_autoScrollStartPosition.Y;
            _mouseStartingLocation = e.Location;
        }

        private void easternHyruleTabPage_MouseUp(object sender, MouseEventArgs e)
        {
            _panning = false;
        }

        private void easternHyruleTabPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (_panning)
            {
                Point newAutoScrollPosition = new Point();
                newAutoScrollPosition.X = _autoScrollStartPosition.X + (_mouseStartingLocation.X - e.Location.X);
                newAutoScrollPosition.Y = _autoScrollStartPosition.Y + (_mouseStartingLocation.Y - e.Location.Y);
                easternHyruleTabPage.AutoScrollPosition = newAutoScrollPosition;

                easternHyruleTabPage.Invalidate();
                easternHyrulePictureBox.Invalidate();
            }
        }

        private void mazeIslandTabPage_MouseDown(object sender, MouseEventArgs e)
        {
            _panning = true;
            _autoScrollStartPosition = mazeIslandTabPage.AutoScrollPosition;
            _autoScrollStartPosition.X = -_autoScrollStartPosition.X;
            _autoScrollStartPosition.Y = -_autoScrollStartPosition.Y;
            _mouseStartingLocation = e.Location;
        }

        private void mazeIslandTabPage_MouseUp(object sender, MouseEventArgs e)
        {
            _panning = false;
        }

        private void mazeIslandTabPage_MouseMove(object sender, MouseEventArgs e)
        {
            if (_panning)
            {
                Point newAutoScrollPosition = new Point();
                newAutoScrollPosition.X = _autoScrollStartPosition.X + (_mouseStartingLocation.X - e.Location.X);
                newAutoScrollPosition.Y = _autoScrollStartPosition.Y + (_mouseStartingLocation.Y - e.Location.Y);
                mazeIslandTabPage.AutoScrollPosition = newAutoScrollPosition;

                mazeIslandTabPage.Invalidate();
                mazeIslandPictureBox.Invalidate();
            }
        }
    }
}
