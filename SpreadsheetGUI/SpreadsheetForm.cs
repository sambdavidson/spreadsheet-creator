using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SS
{
    public partial class spreadsheetWinForm : Form
    {
        //Private member variables
        private AbstractSpreadsheet ourSpreadsheet; //Container for the internal spreadsheet
        private String previousContents; //Used for detecting content change
        private int previousCellX; // Used for saving position when shift-clicking
        private int previousCellY; // 

        private String filepath;
        private String documentName = "untitled";
        private String applicationName = "Spreadsheet Creator";
        private const String Version = "ps6";

        private bool documentChanged = false;
        private bool shiftPressed = false;

        //HERO MODE
        private bool heroMode = false;
        private bool heroModeSaved = false;

        //Jukebox
        private WMPLib.WindowsMediaPlayer jukebox;
        private bool isPlaying = false;
        
        /// <summary>
        /// Creates a new instance of Form1 with its own spreadsheet window and internal data structures.
        /// </summary>
        public spreadsheetWinForm()
        {
            InitializeComponent();

            // This could also be done graphically in the designer, as has been
            // demonstrated in class.
            spreadsheetPanel.SelectionChanged += cellSelected;
            spreadsheetPanel.SetSelection(2, 3);
            cellName.Text = ((char)('A' + 2)) + @":" + (4);
            previousCellX = 2;
            previousCellY = 3;
            contentButton.Enabled = false;

            //Initialize our internal spreadsheet
            ourSpreadsheet = new Spreadsheet(isValid, Normalize, Version);
            
            //Jukebox
            jukebox = new WMPLib.WindowsMediaPlayer();
            jukeboxPlay.Enabled = false;
            
            //Colorized Cells
            //coloredPoints = new HashSet<Point>();

        }
        /// <summary>
        /// Normalize function for constructing spreadsheets.
        /// Basically converts all functions to uppercase where a1 = A1
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private String Normalize(String str)
        {
            if (str.Length > 1)
            {
                if (str[0] == '=')
                {
                    return str.ToUpper();
                }
            }
            return str;
        }
        /// <summary>
        /// isValid function for constructing spreadsheets.
        /// Checks if it follows the A1 - Z99 format.
        /// Checks if it contains itself.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool isValid(String str)
        {
            return Regex.IsMatch(str, @"^[A-Z][1-9][0-9]*$");
        }
        /// <summary>
        /// Called when a cell on the spreadsheet is selected. 
        /// </summary>
        /// <param name="ss">Spreadsheet that called the method.</param>
        private void cellSelected(SpreadsheetPanel ss)
        {
            int row, col;
            ss.GetSelection(out col, out row);
            String _cellName = "" + ((char) ('A' + col)) + (row + 1);

            if (shiftPressed) //If shift is pressed we are only adding the cell name to the contents box.
            {
                int selectionStart = contentsBox.SelectionStart;
                contentsBox.Text = contentsBox.Text.Insert(selectionStart, _cellName);
                contentsBox.Select(selectionStart + _cellName.Length, 0);
                ss.SetSelection(previousCellX, previousCellY);
                return;
            }
            if (contentButton.Enabled) // If we click away submit what we've got if there has been a change.
            {
                contentButton_Click(null, null);
            }
            previousCellX = col;
            previousCellY = row;
            cellName.Text = _cellName;
            String contents = ourSpreadsheet.GetCellContents(_cellName).ToString();
            String value = ourSpreadsheet.GetCellValue(_cellName).ToString();

            contentsBox.Text = contents;
            previousContents = contents;
            if (value == "SpreadsheetUtilities.FormulaError")
            {
                value = "!FORMULA_ERROR"; //Change the name to something more readable
                if (heroMode) // HERO MODE Stuff
                {
                    HeroModeReset();
                }
            }
            toolTip.SetToolTip(valueBox, value); //Set the tooltip to be the full value
            if (value.Length > 16) // Trim it down to fit in the contents box
            {
                value = value.Substring(0, 15);
            }
            valueBox.Text = value;

            //Pretty Colorized stuff
            //colorizeDependencies(_cellName);

            contentButton.Enabled = false;//Keep the contents button disabled


        }

        /// <summary>
        /// Event called when the "enter" button is clicked. Also called any time contents are to be submitted. 
        /// </summary>
        private void contentButton_Click(object sender, EventArgs e)
        {
            String localCellName = "" + ((char)('A' + previousCellX)) + (previousCellY + 1); // Convert the cell name to something that can be read.

            try
            {
                ISet<String> cellsToUpdate = ourSpreadsheet.SetContentsOfCell(localCellName, contentsBox.Text);
                String value = ourSpreadsheet.GetCellValue(localCellName).ToString();
                UpdateCells(cellsToUpdate); // Update the actual cells that we see to match ourSpreadsheet
                if (filepath != null && !heroModeSaved) //Check if we've already got a path setup for quick saving
                {
                    saveToolStripMenuItem1.Enabled = true;
                }


                //The doc has been changed update its name and status
                this.Text = documentName + @"*-  " + applicationName;
                documentChanged = true;

                if (value == "SpreadsheetUtilities.FormulaError")
                {
                    value = "!FORMULA_ERROR";
                    if (heroMode) // HERO MODE Stuff
                    {
                        HeroModeReset();
                    }
                }
                valueBox.Text = value;
            }
            catch (Exception ex) // Usually catches when invalid tokens are used in the formula
            {
                MessageBox.Show(ex.Message);
                if (heroMode) // HERO MODE Stuff
                {
                    HeroModeReset();
                }
            }
            previousContents = contentsBox.Text; //Reset previous contents
            contentButton.Enabled = false;

            //Colorize stuff
            //colorizeDependencies(_cellName);
        }

        /// <summary>
        /// Event when the content text box gets changed.
        /// Used for enabling the Enter button.
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            contentButton.Enabled = contentsBox.Text != previousContents; //Enable Entering values if the 
        }

        /// <summary>
        /// Private helper function that updates all form cells to the correct values of cellsToUpdate.
        /// </summary>
        private void UpdateCells(IEnumerable<String> cellsToUpdate)
        {
            foreach (String cell in cellsToUpdate)
            {
                int col = cell[0] - 'A';
                int row;
                String value = ourSpreadsheet.GetCellValue(cell).ToString();
                if (value == "SpreadsheetUtilities.FormulaError")
                {
                    value = "!FORMULA_ERROR";
                    if (heroMode) // HERO MODE Stuff
                    {
                        HeroModeReset();
                    }
                }
                if (Int32.TryParse(cell.Remove(0, 1), out row))
                    spreadsheetPanel.SetValue(col, row - 1, value);
            }
        }
        /// <summary>
        /// Event for when a key is pressed while typing in the contents box. Checks if enter/return was pressed and triggers the "enter" button next to the box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contentsBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char) Keys.Enter:
                    if (!contentButton.Enabled)
                        return;
                    contentButton_Click(null,null);
                    e.Handled = true; // Removes "ding" sound
                    break;

                    
            }
        }
        /// <summary>
        /// Event for detecting when Shift is pressed.
        /// </summary>
        private void contentsBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                shiftPressed = true;
                e.Handled = true; // Removes "ding" sound
            }
        }

        /// <summary>
        /// Event for detecting when Shift is released.
        /// </summary>
        private void contentsBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Shift)
            {
                shiftPressed = false;
                e.Handled = true; // Removes "ding" sound
            }
        }

        /// <summary>
        /// Event for the Exit button within the File menu. If changes have been made, checks to save before exiting.
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (documentChanged)
            {
                var result = MessageBox.Show(@"Do you want to save changes to " + documentName + @"?", @"Spreadsheet Closing", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (filepath == null)
                        saveAsMenuItem_Click(null, null);       
                    else
                        saveToolStripMenuItem1_Click(null, null);
                    Close();
                }
                else if (result == DialogResult.No)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// Event for the Save As button on the File menu. Opens a SaveFileDialog and saves to the chosen destination.
        /// </summary>
        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            if (heroMode) // HERO MODE silliness
            {
                var result =
                    MessageBox.Show(
                        @"WARNING:\nHERO MODE is enabled. Are you sure you want to use your ONE save on the spreadsheet right now?",
                        @"THE GREAT HERO MODE SAVE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = @"Spreadsheet Files (*.sprd)|*.sprd|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String filename = saveFileDialog1.FileName;
                try
                {
                    ourSpreadsheet.Save(filename);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                filepath = filename;
                documentName = Path.GetFileNameWithoutExtension(filename);
                saveToolStripMenuItem1.Enabled = false;
                this.Text = documentName + @"- " + applicationName;
                documentChanged = false;
                if (heroMode) //More HERO MODE ridiculousness.
                {
                    heroModeSaved = true;
                    saveAsMenuItem.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Event that opens a new Form1. Taken from the skeleton.
        /// </summary>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            DemoApplicationContext.getAppContext().RunForm(new spreadsheetWinForm());
        }

        /// <summary>
        /// Event for the Save button on the File menu. Only enabled if a filepath is chosen and a change has been made.
        /// </summary>
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem1.Enabled = false;
            this.Text = documentName + @"- " + applicationName;
            documentChanged = false;
            try
            {
                ourSpreadsheet.Save(filepath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// Event for Open button on the File menu. Checks to save if a change has been made. Then opens a .sprd file.
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (documentChanged)
            {
                var result = MessageBox.Show(@"Do you want to save changes to " + documentName + @"?", @"Opening New Spreadsheet", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (filepath == null)
                        saveAsMenuItem_Click(null, null);
                    else
                        saveToolStripMenuItem1_Click(null, null);
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = @"Spreadsheet Files (*.sprd)|*.sprd|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String filename = openFileDialog1.FileName;
                try
                {
                    ourSpreadsheet = new Spreadsheet(filename, isValid, Normalize, Version);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.GetType() + @":\n" + exception.Message);
                }
                filepath = filename;
                documentName = Path.GetFileNameWithoutExtension(filename);
                saveToolStripMenuItem1.Enabled = false;
                this.Text = documentName + @"- " + applicationName;
                documentChanged = false;
                spreadsheetPanel.Clear();
                UpdateCells(ourSpreadsheet.GetNamesOfAllNonemptyCells());
                
            }
        }
        /// <summary>
        /// Event for the HERO MODE button. Presents the user with an "are you sure" dialog then enables the most dangerous of all modes. Hero Mode.
        /// </summary>
        private void hEROMODEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(@"Warning! Hero Mode is a high risk spreadsheet creation experience. " +
                                         @"Please read about it in the About page before entering.\nWould you like to enter Hero Mode?",@"ENTERING HERO MODE", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                heroMode = true;
                hEROMODEToolStripMenuItem.Enabled = false;
                spreadsheetPanel.BackColor = Color.Crimson;
                applicationName = "Spreadsheet HERO MODE";
                if(!documentChanged)
                    this.Text = documentName + @"- " + applicationName;
                else
                    this.Text = documentName + @"*- " + applicationName;
            }
        }

        /// <summary>
        /// Private method that is called when a mistake is made in hero mode. It deletes the save and resets the data.
        /// </summary>
        private void HeroModeReset()
        {
            if (filepath != null)
            {
                System.IO.File.Delete(filepath);
            }
            MessageBox.Show(
                @"You've made a mistake in HERO MODE. Your save has been deleted and the spreadsheet will now reset. Better luck next time!");
            ourSpreadsheet = new Spreadsheet(isValid, Normalize, Version);
            spreadsheetPanel.Clear();
            contentsBox.Text = "";
            valueBox.Text = "";
            heroMode = false;
            heroModeSaved = false;
            spreadsheetPanel.BackColor = System.Drawing.SystemColors.Control;
            documentName = "untitled";
            applicationName = "Spreadsheet";
            this.Text = documentName + @"- " + applicationName;
        }
        /// <summary>
        /// Event for the Help menu item. Displays how to use instructions to the user.
        /// </summary>
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show a the about box containing the instructions of our instructions.
            AboutBox box = new AboutBox();
            box.ShowDialog();

        }
        /// <summary>
        ///  Event for when the Jukebox Play button is pressed. Plays/Pauses the current song.
        /// </summary>
        private void jukeboxPlay_Click(object sender, EventArgs e)
        {
            if (isPlaying == false)
            {
                jukebox.controls.play();
                jukeboxPlay.Text = @"Pause";
                isPlaying = true;
            }
            else
            {
                jukebox.controls.pause();
                jukeboxPlay.Text = @"Play";
                isPlaying = false;
            }
        }

        /// <summary>
        /// Event for the jukebox open. Opens a file picker dialog and allows the user to load an audio file.
        /// </summary>
        private void jukeboxOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = @"(mp3,wav,mp4,mov,wmv,mpg)|*.mp3;*.wav;*.mp4;*.mov;*.wmv;*.mpg|all files|*.*";
                //All supported WMP file types
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                jukebox.URL = openFileDialog1.FileName;
                jukeboxPlay.Enabled = true;
                String songTitle = jukebox.currentMedia.name.Substring(0,
                    Math.Min(10, jukebox.currentMedia.name.Length - 1)); //Drawn name trimmed
                if (jukebox.currentMedia.name.Length - 1 > 10) // append a "..." if the name has been cut.
                {
                    songTitle = songTitle + "...";
                }
                jukeboxTitle.Text = songTitle;
                toolTip.SetToolTip(jukeboxTitle, jukebox.currentMedia.name);
                    //Set the toopTip so you can mouse over for full name
                jukeboxPlay.Text = @"Pause";
                isPlaying = true;
            }
        }
    }
}
