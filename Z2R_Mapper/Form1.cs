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
        private bool _romLoaded = false;

        private double _zoomFactor = 1.0;
        private Bitmap _westernHyruleImageBackup;
        private Bitmap _easternHyruleImageBackup;
        private Bitmap _deathMountainImageBackup;
        private Bitmap _mazeIslandImageBackup;

        private bool _westernHyruleAutoScrollNeedsAdjustment = false;
        private Point _westernHyruleMapAutoScrollPosition;

        private bool _easternHyruleAutoScrollNeedsAdjustment = false;
        private Point _easternHyruleMapAutoScrollPosition;

        private bool _panning = false;
        private Point _autoScrollStartPosition = Point.Empty;
        private Point _mouseStartingLocation = Point.Empty;

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

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                OpenROMFile(files[0]);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            zoomFactorTextBox.Text = _zoomFactor.ToString("0.00");

            // Allow opening of a ROM file by dragging the file on top of the executable or its shortcut.
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                OpenROMFile(args[1]);
            }
        }

        private void OpenROMFile(String filename)
        {
            try
            {
                _z2rMapper = new Z2R_Mapper(filename);
                ResetFormSettings();
                LoadDataIntoForms();
                this.Text = string.Format("{0} - Z2R Mapper", System.IO.Path.GetFileNameWithoutExtension(filename));
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void LoadDataIntoForms()
        {
            _westernHyruleImageBackup = _z2rMapper.GetOverworldBitmap(OverworldArea.WesternHyrule);
            _easternHyruleImageBackup = _z2rMapper.GetOverworldBitmap(OverworldArea.EasternHyrule);
            _deathMountainImageBackup = _z2rMapper.GetOverworldBitmap(OverworldArea.DeathMountain);
            _mazeIslandImageBackup = _z2rMapper.GetOverworldBitmap(OverworldArea.MazeIsland);

            _romLoaded = true;

            // This function creates the scaled bitmaps and attaches them to the PictureBoxes.
            ResizeMaps();

            _westernHyruleMapAutoScrollPosition = CenterMapOnXY(westernHyruleTabPage, _z2rMapper.GetBitmapXYAtCenterOfNorthPalace());
            westernHyruleTabPage.AutoScrollPosition = _westernHyruleMapAutoScrollPosition;
            if(westernHyruleTabPage.AutoScrollPosition == Point.Empty)
            {
                // This tab page has not been shown yet.  Adjust the image location when the page is shown.
                // This must be something to do with Windows forms and tab pages.  It seems the user must
                // have clicked over to this tab page before we can actively maniuplate it.  If the value
                // we just set reads back as zero, then we have to wait and assign the value again when the
                // tab page becomes visible.
                _westernHyruleAutoScrollNeedsAdjustment = true;
            }

            _easternHyruleMapAutoScrollPosition = CenterMapOnXY(easternHyruleTabPage, _z2rMapper.GetBitmapXYAtCenterOfEasternHyruleRaftLocation());
            easternHyruleTabPage.AutoScrollPosition = _easternHyruleMapAutoScrollPosition;
            if(easternHyruleTabPage.AutoScrollPosition == Point.Empty)
            {
                // This tab page has not been shown yet.  Adjust the image location when the page is shown.
                _easternHyruleAutoScrollNeedsAdjustment = true;
            }

            deathMountainTabPage.AutoScrollPosition = Point.Empty;
            mazeIslandTabPage.AutoScrollPosition = Point.Empty;

            itemSummaryTextBox.Text = _z2rMapper.GetItemSummary();
            spellSummaryTextBox.Text = _z2rMapper.GetSpellSummary();
            spellCostsTextBox.Text = _z2rMapper.GetSpellCostsSummary();
            RedrawStatsSummary();
            RedrawPalaceRoutingSummary();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomOutMaps();
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomInMaps();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Add:
                case Keys.Oemplus:
                    ZoomInMaps();
                    e.Handled = true;
                    break;

                case Keys.Subtract:
                case Keys.OemMinus:
                    ZoomOutMaps();
                    e.Handled = true;
                    break;

                case Keys.Left:
                case Keys.NumPad4:
                    PanSelectedMap(-50, 0);
                    e.Handled = true;
                    break;

                case Keys.Right:
                case Keys.NumPad6:
                    PanSelectedMap(50, 0);
                    e.Handled = true;
                    break;

                case Keys.Up:
                case Keys.NumPad8:
                    PanSelectedMap(0, -50);
                    e.Handled = true;
                    break;

                case Keys.Down:
                case Keys.NumPad2:
                    PanSelectedMap(0, 50);
                    e.Handled = true;
                    break;

                default:
                    e.Handled = false;
                    break;
            }
        }

        private void PanSelectedMap(int deltaX, int deltaY)
        {
            TabPage tabPage;

            tabPage = mainTabControl.SelectedTab;
            if ((tabPage != westernHyruleTabPage) && (tabPage != easternHyruleTabPage) &&
                (tabPage != deathMountainTabPage) && (tabPage != mazeIslandTabPage))
            {
                return;
            }

            Point scrollPosition = tabPage.AutoScrollPosition;

            // AutoScrollPosition gives us negative offsets, but we are required to feed it positive offsets.
            scrollPosition.X = -scrollPosition.X;
            scrollPosition.Y = -scrollPosition.Y;

            scrollPosition.X += deltaX;
            scrollPosition.Y += deltaY;
            tabPage.AutoScrollPosition = scrollPosition;
        }

        private void ZoomOutMaps()
        {
            if (_zoomFactor > 0.25)
            {
                _zoomFactor -= 0.25;
            }
            ResizeMaps();
        }

        private void ZoomInMaps()
        {
            if (_zoomFactor < 1.5)
            {
                _zoomFactor += 0.25;
            }
            ResizeMaps();
        }

        private void ResizeMaps()
        {
            zoomFactorTextBox.Text = _zoomFactor.ToString("0.00");
            if (_westernHyruleImageBackup != null)
            {
                Size newSize = new Size((int)(_westernHyruleImageBackup.Width * _zoomFactor), (int)(_westernHyruleImageBackup.Height * _zoomFactor));
                Bitmap bmp = new Bitmap(_westernHyruleImageBackup, newSize);
                westernHyrulePictureBox.Image = bmp;
            }
            if (_easternHyruleImageBackup != null)
            {
                Size newSize = new Size((int)(_easternHyruleImageBackup.Width * _zoomFactor), (int)(_easternHyruleImageBackup.Height * _zoomFactor));
                Bitmap bmp = new Bitmap(_easternHyruleImageBackup, newSize);
                easternHyrulePictureBox.Image = bmp;
            }
            if (_deathMountainImageBackup != null)
            {
                Size newSize = new Size((int)(_deathMountainImageBackup.Width * _zoomFactor), (int)(_deathMountainImageBackup.Height * _zoomFactor));
                Bitmap bmp = new Bitmap(_deathMountainImageBackup, newSize);
                deathMountainPictureBox.Image = bmp;
            }
            if (_mazeIslandImageBackup != null)
            {
                Size newSize = new Size((int)(_mazeIslandImageBackup.Width * _zoomFactor), (int)(_mazeIslandImageBackup.Height * _zoomFactor));
                Bitmap bmp = new Bitmap(_mazeIslandImageBackup, newSize);
                mazeIslandPictureBox.Image = bmp;
            }
        }

        private Point CenterMapOnXY(TabPage pictureBoxContainer, Point xyLoc)
        {
            // We have been given the focal point for the unscaled bitmap.
            // We need to scale by the zoom factor if we want the focal point to be
            // centered for a map that is zoomed in or zoomed out.
            xyLoc.X = (int)(xyLoc.X * _zoomFactor);
            xyLoc.Y = (int)(xyLoc.Y * _zoomFactor);

            Point newAutoScrollPosition = new Point()
            {
                X = xyLoc.X - (pictureBoxContainer.Width / 2),
                Y = xyLoc.Y - (pictureBoxContainer.Height / 2),
            };
            return newAutoScrollPosition;
        }

        private void ResetFormSettings()
        {
            //mainTabControl.SelectedIndex = 0;
            showMaxHeartContainersCheckBox.Checked = false;
            showCombinedSpellCheckBox.Checked = false;
            showMagicCostsCheckBox.Checked = false;
            showDirectionsCheckBox.Checked = false;
            showRequirementsCheckBox.Checked = true;
            showItemToBossCheckBox.Checked = false;
        }

        private void RedrawStatsSummary()
        {
            if(_romLoaded)
            {
                startingStatsTextBox.Text = _z2rMapper.GetStartingStatsSummary(
                    showMaxHeartContainersCheckBox.Checked, showCombinedSpellCheckBox.Checked);
            }
        }

        private void RedrawPalaceRoutingSummary()
        {
            if(_romLoaded)
            {
                palaceRoutingTextBox.Text = _z2rMapper.GetPalaceRoutingSummary(
                    showDirectionsCheckBox.Checked,
                    showRequirementsCheckBox.Checked,
                    showItemToBossCheckBox.Checked);
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

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = ((TabControl)sender);

            if(tabControl.SelectedTab == westernHyruleTabPage && _westernHyruleAutoScrollNeedsAdjustment)
            {
                westernHyruleTabPage.AutoScrollPosition = _westernHyruleMapAutoScrollPosition;
                _westernHyruleAutoScrollNeedsAdjustment = false;
            }

            if(tabControl.SelectedTab == easternHyruleTabPage && _easternHyruleAutoScrollNeedsAdjustment)
            {
                easternHyruleTabPage.AutoScrollPosition = _easternHyruleMapAutoScrollPosition;
                _easternHyruleAutoScrollNeedsAdjustment = false;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }
    }
}
