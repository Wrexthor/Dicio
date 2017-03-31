// Jack Swedjemark 2017-01-05
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DeusDicio
{
    public partial class MainForm : Form
    {
        // Store path where program runs in string for passing to methods
        private string path = (AppDomain.CurrentDomain.BaseDirectory);
        // initiate objects
        private Search search;
        private Hosts hostList;
        private Action actionList;

        public MainForm()
        {
            InitializeComponent();
            InitializeGUI();
        }

        /// <summary>
        /// Run initialization upon staring application
        /// </summary>
        private void InitializeGUI()
        {
            // Set default GUI values
            lblTip.Text = "Wildcard * permitted";            
            cboxSearch.Items.AddRange(Enum.GetNames(typeof(SearchType)));
            cboxSearch.SelectedIndex = (int)SearchType.AD;
            

            // clear all listboxes
            lboxHosts.Items.Clear();
            lboxResults.Items.Clear();

            // create new objects
            search = new Search();            
            hostList = new Hosts();
            actionList = new Action((path + "\\Scripts"));

            cboxAction.DataSource = actionList.List;
        }

        /// <summary>
        /// Update GUI values
        /// </summary>
        private void UpdateGUI()
        {
            // populate results list
            lboxResults.DataSource = null;            
            lboxResults.DataSource = search.Result;

            // populate hosts list
            lboxHosts.DataSource = null;
            if (hostList.List.Count >= 1)
            {                
                lboxHosts.DataSource = hostList.List;
            }
            cboxAction.DataSource = null;
            cboxAction.DataSource = actionList.List;
        }

        /// <summary>
        /// Find button event click, searches for host
        /// using input and searchtype from GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {                                
                // perform search based on searchtype
                if (cboxSearch.SelectedIndex == (int)SearchType.AD)
                {
                    if (!search.InDomain)
                    {
                        MessageBox.Show("This host does not seem to be joined to a Domain. Please run this script from a domain joined host.", "Domain Missing!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if(!search.SearchAD(txtSearch.Text))
                        MessageBox.Show("Nothing found, please try again :)", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                    
                if (cboxSearch.SelectedIndex == (int)SearchType.DNS)
                    if(!search.SearchDNS(txtSearch.Text))
                        MessageBox.Show("Nothing found, please try again :)", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // update GUI values
                UpdateGUI();
            }
            else
                MessageBox.Show("Search field is empty, please provide a keyword", "Input Missing!");
            
        }

        /// <summary>
        /// Save button click event, saves curent host list to txt file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHostsSave_Click(object sender, EventArgs e)
        {
            // create dialog object
            SaveFileDialog saveFile = new SaveFileDialog();
            // configure object
            saveFile.Filter = "txt files (*.txt)|*.txt";            
            saveFile.RestoreDirectory = true;
            
            // if pressed ok on dialog, save file to disk at chosen path
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                WriteFile(saveFile.FileName, hostList.List.ToArray());
            }

        }

        /// <summary>
        /// Load host button click event, loads hostlist from txt file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHostsLoad_Click(object sender, EventArgs e)
        {
            // create dialog object
            OpenFileDialog openFile = new OpenFileDialog();
            // configure object
            openFile.Filter = "txt files (*.txt)|*.txt";
            openFile.RestoreDirectory = true;

            // if ok on dialog, load hosts from file
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                hostList.AddRange(LoadFile(openFile.FileName));
                UpdateGUI();
                }
                catch { }
            }                
        }

        /// <summary>
        /// Clear All button click event, clears all hosts from list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHostsClearAll_Click(object sender, EventArgs e)
        {
            // prompt if sure
            if (lboxHosts.Items.Count > 0)
            {
                DialogResult choice = MessageBox.Show("Are you sure you wish to clear all Hosts from list?", "Warning!", MessageBoxButtons.YesNo);
                if (choice == DialogResult.Yes)
                {
                    // clear hostlist items
                    hostList.Clear();
                    UpdateGUI();
                }
            }
            
        }

        /// <summary>
        /// Remove button click event, removes selected host from list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHostsRemove_Click(object sender, EventArgs e)
        {
            if (lboxHosts.SelectedIndex != -1)
            {
                hostList.RemoveAt(lboxHosts.SelectedIndex);
                UpdateGUI();
            }
                

        }

        /// <summary>
        /// Run action button click event,
        /// runs selected script against selected hosts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActionRun_Click(object sender, EventArgs e)
        {
            // check that at least 1 host is selected
            if (lboxHosts.SelectedItems.Count > 0)
            {
                // get selected items and add to list of strings
                List<String> list = lboxHosts.SelectedItems.Cast<String>().ToList();
                // get path + script
                string scripPath = path + @"scripts\" + cboxAction.SelectedItem.ToString();
                // run action on script and list
                actionList.RunAction(scripPath, list);
            }
            else
            {
                MessageBox.Show("Nothing selected!\nPlease select at least 1 host in hosts list.", "Input Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        /// <summary>
        /// Load action button click event, loads a powershellscript to action list 
        /// by copying it to script directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnActionLoad_Click(object sender, EventArgs e)
        {
            // create dialog object
            OpenFileDialog openFile = new OpenFileDialog();
            // configure object
            openFile.Filter = "ps1 files (*.ps1)|*.ps1";
            openFile.RestoreDirectory = true;

            // if ok on dialog, load hosts from file
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //hostList.AddRange(LoadFile(openFile.FileName));
                    if (CopyFile(openFile.FileName, openFile.SafeFileName))
                    {
                        actionList.PopulateList((path + "\\Scripts"));
                        UpdateGUI();
                    }                        

                    else
                        MessageBox.Show("Something went wrong during file copy of selected script.", "Copy Error!");
                }
                catch { }
            }
        }

        /// <summary>
        /// Move button click event, moves selected items from result to host list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMove_Click(object sender, EventArgs e)
        {
            // check that something is selected
            if (lboxResults.SelectedIndex != -1)
            {
                // add each selected item to hostlist
                foreach (string str in lboxResults.SelectedItems)
                {
                    hostList.AddHost(str.ToString());
                }
                UpdateGUI();
            }
            else
                MessageBox.Show("Nothing selected!\nPlease select at least 1 item in results list.", "Input Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Listen for ctrl + right key press for activating
        /// move button
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Right))
            {
                btnMove.PerformClick();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Read file from disk, returning list of strings
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private List<String> LoadFile(string file)
        {
            return File.ReadAllLines(file).ToList();
        }

        /// <summary>
        /// Read file from disk, returning list of strings
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool CopyFile(string file, string name)
        {
            string dest = path + @"\scripts\" + name;
            try
            {
                File.Copy(file, dest);
                return true;

            }
            catch { return false; }
        }

        /// <summary>
        /// Write array of strings to file on disk
        /// </summary>
        /// <param name="path"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool WriteFile(string path, String[] array)
        {
            bool res = false;
            try { File.WriteAllLines(path, array);
                res = true;}
            catch { res = false; }
            return res;
        }      

        /// <summary>
        /// Search type combobox selected index change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxSearch.SelectedIndex == 0)
                lblTip.Text = "Wildcard * permitted";
            if (cboxSearch.SelectedIndex == 1)
                lblTip.Text = "No wildcard permitted";
        }

        /// <summary>
        /// Refresh button click event, repopulates action list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            actionList.PopulateList((path + "\\Scripts"));
            UpdateGUI();
        }
    }
}
