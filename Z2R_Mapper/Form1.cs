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

        private bool _westernHyruleAutoScrollNeedsAdjustment = false;
        private Point _westernHyruleMapAutoScrollPosition;

        private bool _easternHyruleAutoScrollNeedsAdjustment = false;
        private Point _easternHyruleMapAutoScrollPosition;

        private bool _panning = false;
        private Point _autoScrollStartPosition = Point.Empty;
        private Point _mouseStartingLocation = Point.Empty;

        private bool _ctrlKeyHeld = false;

        public Form1()
        {
            InitializeComponent();

            westernHyruleTabPage.Tag = 1.0;
            deathMountainTabPage.Tag = 1.0;
            easternHyruleTabPage.Tag = 1.0;
            mazeIslandTabPage.Tag = 1.0;
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
            DisableZoomControls();

            startingStatsTextBox.MouseWheel += StartingStatsTextBox_MouseWheel;
            itemSummaryTextBox.MouseWheel += ItemSummaryTextBox_MouseWheel;
            spellSummaryTextBox.MouseWheel += SpellSummaryTextBox_MouseWheel;
            spellCostsTextBox.MouseWheel += SpellSummaryTextBox_MouseWheel;
            palaceRoutingTextBox.MouseWheel += PalaceRoutingTextBox_MouseWheel;

            westernHyrulePictureBox.MouseWheel += MapPage_MouseWheel;
            deathMountainPictureBox.MouseWheel += MapPage_MouseWheel;
            easternHyrulePictureBox.MouseWheel += MapPage_MouseWheel;
            mazeIslandPictureBox.MouseWheel += MapPage_MouseWheel;

            // Allow opening of a ROM file by dragging the file on top of the executable or its shortcut.
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                OpenROMFile(args[1]);
            }
        }

        private void StartingStatsTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            Point newScrollPosition = startingStatsPanel.AutoScrollPosition;
            newScrollPosition.X = -newScrollPosition.X;
            newScrollPosition.Y = -newScrollPosition.Y;
            newScrollPosition.Y -= e.Delta;
            startingStatsPanel.AutoScrollPosition = newScrollPosition;
        }

        private void ItemSummaryTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            Point newScrollPosition = itemSummaryTabPage.AutoScrollPosition;
            newScrollPosition.X = -newScrollPosition.X;
            newScrollPosition.Y = -newScrollPosition.Y;
            newScrollPosition.Y -= e.Delta;
            itemSummaryTabPage.AutoScrollPosition = newScrollPosition;
        }

        private void SpellSummaryTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            Point newScrollPosition = spellSummaryPanel.AutoScrollPosition;
            newScrollPosition.X = -newScrollPosition.X;
            newScrollPosition.Y = -newScrollPosition.Y;
            newScrollPosition.Y -= e.Delta;
            spellSummaryPanel.AutoScrollPosition = newScrollPosition;
        }

        private void PalaceRoutingTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            Point newScrollPosition = palaceRoutingPanel.AutoScrollPosition;
            newScrollPosition.X = -newScrollPosition.X;
            newScrollPosition.Y = -newScrollPosition.Y;
            newScrollPosition.Y -= e.Delta;
            palaceRoutingPanel.AutoScrollPosition = newScrollPosition;
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
            westernHyrulePictureBox.Tag = _z2rMapper.GetOverworldBitmap(OverworldArea.WesternHyrule);
            easternHyrulePictureBox.Tag = _z2rMapper.GetOverworldBitmap(OverworldArea.EasternHyrule);
            deathMountainPictureBox.Tag = _z2rMapper.GetOverworldBitmap(OverworldArea.DeathMountain);
            mazeIslandPictureBox.Tag = _z2rMapper.GetOverworldBitmap(OverworldArea.MazeIsland);

            _romLoaded = true;

            // This function creates the scaled bitmaps and attaches them to the PictureBoxes.
            ResizeMap(westernHyruleTabPage, westernHyrulePictureBox, new Point(0,0), 1.0);
            ResizeMap(deathMountainTabPage, deathMountainPictureBox, new Point(0, 0), 1.0);
            ResizeMap(easternHyruleTabPage, easternHyrulePictureBox, new Point(0, 0), 1.0);
            ResizeMap(mazeIslandTabPage, mazeIslandPictureBox, new Point(0, 0), 1.0);

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
            ZoomOutMap(GetBitmapXYAtCenterOfVisibleArea());
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomInMap(GetBitmapXYAtCenterOfVisibleArea());
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Add:
                case Keys.Oemplus:
                    ZoomInMap(GetBitmapXYAtCenterOfVisibleArea());
                    e.Handled = true;
                    break;

                case Keys.Subtract:
                case Keys.OemMinus:
                    ZoomOutMap(GetBitmapXYAtCenterOfVisibleArea());
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

                case Keys.ControlKey:
                    _ctrlKeyHeld = true;
                    e.Handled = false;
                    break;

                default:
                    e.Handled = false;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ControlKey:
                    _ctrlKeyHeld = false;
                    e.Handled = false;
                    break;

                default:
                    e.Handled = false;
                    break;
            }
        }

        private void MapPage_MouseWheel(object sender, MouseEventArgs e)
        {
            if(_ctrlKeyHeld)
            {
                if(e.Delta < 0)
                {
                    ZoomOutMap(new Point(e.X, e.Y));
                }
                else
                {
                    ZoomInMap(new Point(e.X, e.Y));
                }
                ((HandledMouseEventArgs)e).Handled = true;
            } else
            {
                ((HandledMouseEventArgs)e).Handled = false;
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

        private Point GetBitmapXYAtCenterOfVisibleArea()
        {
            TabPage selectedMapTabPage = mainTabControl.SelectedTab;
            Point centerOfPictureBox = new Point(selectedMapTabPage.Width / 2, selectedMapTabPage.Height / 2);
            centerOfPictureBox.X += (-selectedMapTabPage.AutoScrollPosition.X);
            centerOfPictureBox.Y += (-selectedMapTabPage.AutoScrollPosition.Y);
            return centerOfPictureBox;
        }

        private void ZoomOutMap(Point pictureBoxXY)
        {
            TabPage selectedMapTabPage = mainTabControl.SelectedTab;
            PictureBox selectedMapPictureBox = (PictureBox)selectedMapTabPage.GetChildAtPoint(new Point(10, 10));
            double zoomFactor = (double)selectedMapTabPage.Tag;
            if (zoomFactor > 0.25)
            {
                double oldZoomFactor = zoomFactor;
                zoomFactor -= 0.25;
                selectedMapTabPage.Tag = zoomFactor;
                ResizeMap(selectedMapTabPage, selectedMapPictureBox, pictureBoxXY, oldZoomFactor);
            }
        }

        private void ZoomInMap(Point pictureBoxXY)
        {
            TabPage selectedMapTabPage = mainTabControl.SelectedTab;
            PictureBox selectedMapPictureBox = (PictureBox)selectedMapTabPage.GetChildAtPoint(new Point(10, 10));
            double zoomFactor = (double)selectedMapTabPage.Tag;
            if (zoomFactor < 1.5)
            {
                double oldZoomFactor = zoomFactor;
                zoomFactor += 0.25;
                selectedMapTabPage.Tag = zoomFactor;
                ResizeMap(selectedMapTabPage, selectedMapPictureBox, pictureBoxXY, oldZoomFactor);
            }
        }

        private void ResizeMap(TabPage selectedMapTabPage, PictureBox selectedMapPictureBox, Point pictureBoxXY, double oldZoomFactor)
        {
            Bitmap unscaledImage = (Bitmap)selectedMapPictureBox.Tag;
            double zoomFactor = (double)selectedMapTabPage.Tag;
            if (unscaledImage != null)
            {
                // This math can probably be reduced, but I like having it clearly laid out
                // and understandable.
                Point oldScrollPosition = selectedMapTabPage.AutoScrollPosition;

                // If MouseWheel event is coming from TabPage.
                //Point oldBitmapFocalPoint = new Point(pictureBoxXY.X + (-oldScrollPosition.X),
                //    pictureBoxXY.Y + (-oldScrollPosition.Y));

                // If MouseWheel event is coming from PictureBox.
                Point oldBitmapFocalPoint = new Point(pictureBoxXY.X, pictureBoxXY.Y);

                double focalPointRescalingFactor = (zoomFactor / oldZoomFactor);
                Point newBitmapFocalPoint = new Point((int)(oldBitmapFocalPoint.X * focalPointRescalingFactor),
                    (int)(oldBitmapFocalPoint.Y * focalPointRescalingFactor));

                // If MouseWheel event is coming from TabPage.
                //Point newAutoScrollPosition = new Point(newBitmapFocalPoint.X - pictureBoxXY.X,
                //    newBitmapFocalPoint.Y - pictureBoxXY.Y);

                // If MouseWheel event is coming from PictureBox.
                Point newAutoScrollPosition = new Point(newBitmapFocalPoint.X - pictureBoxXY.X + (-oldScrollPosition.X),
                    newBitmapFocalPoint.Y - pictureBoxXY.Y + (-oldScrollPosition.Y));

                Size newSize = new Size((int)(unscaledImage.Width * zoomFactor), (int)(unscaledImage.Height * zoomFactor));
                Bitmap bmp = new Bitmap(unscaledImage, newSize);
                selectedMapPictureBox.Image = bmp;
                selectedMapTabPage.AutoScrollPosition = newAutoScrollPosition;
            }
            EnableZoomControlsAndShowZoomFactor(zoomFactor);
        }

        private Point CenterMapOnXY(TabPage pictureBoxContainer, Point xyLoc)
        {
            // We have been given the focal point for the unscaled bitmap.
            // We need to scale by the zoom factor if we want the focal point to be
            // centered for a map that is zoomed in or zoomed out.
            double zoomFactor = (double)pictureBoxContainer.Tag;
            xyLoc.X = (int)(xyLoc.X * zoomFactor);
            xyLoc.Y = (int)(xyLoc.Y * zoomFactor);

            Point newAutoScrollPosition = new Point()
            {
                X = xyLoc.X - (pictureBoxContainer.Width / 2),
                Y = xyLoc.Y - (pictureBoxContainer.Height / 2),
            };
            return newAutoScrollPosition;
        }

        private void EnableZoomControlsAndShowZoomFactor(double zoomFactor)
        {
            zoomInToolStripMenuItem.Enabled = true;
            zoomOutToolStripMenuItem.Enabled = true;
            zoomFactorTextBox.Enabled = true;
            zoomFactorTextBox.Text = string.Format("Zoom Factor: {0}", zoomFactor.ToString("0.00"));
        }

        private void DisableZoomControls()
        {
            zoomInToolStripMenuItem.Enabled = false;
            zoomOutToolStripMenuItem.Enabled = false;
            zoomFactorTextBox.Enabled = false;
            zoomFactorTextBox.Text = string.Format("");
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

        private void mapImagePictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            TabPage selectedTabPage = (TabPage)((PictureBox)sender).Parent;
            _panning = true;
            _autoScrollStartPosition = selectedTabPage.AutoScrollPosition;
            _autoScrollStartPosition.X = -_autoScrollStartPosition.X;
            _autoScrollStartPosition.Y = -_autoScrollStartPosition.Y;
            _mouseStartingLocation = e.Location;
        }

        private void mapImagePictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _panning = false;
        }

        private void mapImagePictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_panning)
            {
                PictureBox selectedPictureBox = (PictureBox)sender;
                TabPage selectedTabPage = (TabPage)((PictureBox)sender).Parent;
                Point newAutoScrollPosition = new Point();
                newAutoScrollPosition.X = _autoScrollStartPosition.X + (_mouseStartingLocation.X - e.Location.X);
                newAutoScrollPosition.Y = _autoScrollStartPosition.Y + (_mouseStartingLocation.Y - e.Location.Y);
                selectedTabPage.AutoScrollPosition = newAutoScrollPosition;

                // When clicking and dragging on the PictureBox, the mouse stays in the same location
                // on the PictureBox (after adjusting the scroll postion that is).
                // That means we need to reset our scroll starting position so as to not have the
                // image jumping around all herky-jerkily.
                _autoScrollStartPosition = selectedTabPage.AutoScrollPosition;
                _autoScrollStartPosition.X = -_autoScrollStartPosition.X;
                _autoScrollStartPosition.Y = -_autoScrollStartPosition.Y;

                selectedPictureBox.Invalidate();
            }
        }

        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = ((TabControl)sender);
            TabPage selectedTabPage = tabControl.SelectedTab;

            if(selectedTabPage == westernHyruleTabPage && _westernHyruleAutoScrollNeedsAdjustment)
            {
                westernHyruleTabPage.AutoScrollPosition = _westernHyruleMapAutoScrollPosition;
                _westernHyruleAutoScrollNeedsAdjustment = false;
            }

            if(selectedTabPage == easternHyruleTabPage && _easternHyruleAutoScrollNeedsAdjustment)
            {
                easternHyruleTabPage.AutoScrollPosition = _easternHyruleMapAutoScrollPosition;
                _easternHyruleAutoScrollNeedsAdjustment = false;
            }

            if(selectedTabPage.Tag != null)
            {
                double zoomFactor = (double)selectedTabPage.Tag;
                EnableZoomControlsAndShowZoomFactor(zoomFactor);
            }else
            {
                DisableZoomControls();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
