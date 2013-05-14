using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MLC
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }


        //Browse button
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (checkBox1.Checked == true)
                {
                    string fileName = openFileDialog.FileName;
                    List<SoccerData> soccerData = Utility.ReadSoccerDataFromFile(fileName);
                    Utility.WriteOutSoccerDataToArffFormat(soccerData);

                    richTextBox1.Text = "Text output to C:\\"; 
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }


    }
}
