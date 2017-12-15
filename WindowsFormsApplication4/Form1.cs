using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {       

        public Form1()
        {
            InitializeComponent();            
        }

        String folderPath = null;
        String altiumID = null;

        String warningFolderMissing = "Please select correct Altium folder";

        private void button1_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    folderPath = folderDialog.SelectedPath;
                    //MessageBox.Show("You've selected: " + folderPath);
                    richTextBox1.Text += "You've selected: " + folderPath + "\n";
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderPath == null)
            {
                MessageBox.Show(warningFolderMissing);
            }
            else
            {
                altiumID = findAltiumID(folderPath);
                richTextBox1.Text += "Altium ID: " + altiumID + "\n";
            }            
        }

        private string findAltiumID (string path)
        {
            string id_path = path + @"\System\PrefFolder.ini";
            string res = null;

            if (File.Exists(id_path))
            {
                string id = File.ReadAllText(id_path);
                string pattern = @"(?<=UniqueID={)(.*)(?=})";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(id);
                if (match.Success)
                {
                    res = match.Value;
                    //MessageBox.Show("ID: " + res);
                    textBox1.Text = res;
                }
                return res;
            }
            else
            {
                MessageBox.Show(warningFolderMissing);
                return res;
            }
        }

        private string findProductType (string path)
        {
            string id_path = path + @"\System\PrefFolder.ini";
            string res = null;
            
            if (File.Exists(id_path))
            {
                string id = File.ReadAllText(id_path);
                string pattern = @"Name=(.*)";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(id);
                if (match.Success)
                {
                    res = match.Groups[1].ToString();
                }
                //MessageBox.Show(res);
                string final_res = res.Trim();
                richTextBox1.Text += "Product type: " + final_res + "\n";
                return final_res;
            }
            else
            {
                return res;
            }
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (folderPath == null)
            {
                MessageBox.Show(warningFolderMissing);
            }             
            else
            {
                openFolder(@"C:\ProgramData\Altium\"+ findProductType(folderPath) + " {" + findAltiumID(folderPath) + @"}\Extensions");
            }                    
        }

        private void openFolder(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                richTextBox1.Text += "Opening file browser at location: " + filePath + "\n";
                Process.Start("explorer.exe", filePath);
            }
            else
            {
                MessageBox.Show("There is no such folder");
            }                         
        }

        private void deleteInFolder(string filePath)
        {            
            if (Directory.Exists(filePath))
            {
                richTextBox1.Text += "Starting the deletion of files at " + filePath + "\n";
                System.IO.DirectoryInfo di = new DirectoryInfo(filePath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                richTextBox1.Text += "Files were successfully deleted.\n";
            }
            else
                MessageBox.Show("There is no Mesh folder");            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            string userName = user.Name.Substring(user.Name.LastIndexOf("\\") + 1);

            if (folderPath == null)
            {
                MessageBox.Show(warningFolderMissing);
            }
            else
            {
                deleteInFolder(@"C:\Users\" + userName + @"\AppData\Local\Altium\" + findProductType(folderPath) + " {" + findAltiumID(folderPath) + @"}\Mesh");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            runAltium(folderPath);
        }

        private void runAltium (string filePath)
        {
            string product = null;
            if (Directory.Exists(filePath) && File.Exists(filePath + @"\System\PrefFolder.ini"))
            {
                product = (File.Exists(filePath + "\\X2.exe")) ? "X2.exe" : "DXP.exe";
                richTextBox1.Text += "Running " + product + " from " + filePath + "\n";
                filePath += "\\" + product;
                if (File.Exists(filePath))
                {
                    Process.Start(filePath);
                }                    
                else
                {
                    MessageBox.Show("Can't find .exe file to launch");
                }                    
            }
            else
            {
                MessageBox.Show(warningFolderMissing);
            }
        }
    }
}
