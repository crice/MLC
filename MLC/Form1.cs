using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SVM;

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
                string outFileName = this.textBox5.Text; 

                if (checkBox1.Checked == true)
                {
                    string fileName = openFileDialog.FileName;
                    List<SoccerData> soccerData = Utility.ReadSoccerDataFromFile(fileName);
                    Utility.WriteOutSoccerDataToArffFormat(soccerData, outFileName);

                    richTextBox1.Text = "Text output to C:\\"; 
                }
                else if (checkBox3.Checked == true)
                {
                    string fileName = openFileDialog.FileName;
                    List<SoccerData> soccerData = Utility.ReadSoccerDataFromFilePre2012WithReferee(fileName);
                    Utility.WriteOutSoccerDataToArffFormat(soccerData, outFileName);

                    richTextBox1.Text = "Text output to C:\\"; 
                }
                else if (checkBox2.Checked == true)
                {
                    string fileName = openFileDialog.FileName;
                    List<SoccerData> soccerData = Utility.ReadSoccerDataFromFile(fileName);
                    Utility.WriteOutSoccerDataToLibsvmFormat(soccerData, outFileName);

                    richTextBox1.Text = "Text output to C:\\" + outFileName;
                }

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }


        //SVM - M. Johnson
        private void button2_Click(object sender, EventArgs e)
        {
            string trainingDataLocation = this.textBox1.Text;
            string testDataLocation = this.textBox2.Text;
            string paramsFileLocation = this.textBox3.Text;
            string resultsFileLocation = this.textBox4.Text; 

            Problem train = Problem.Read(@trainingDataLocation);
            Problem test = Problem.Read(@testDataLocation);

            //For this example (and indeed, many scenarios), the default
            //parameters will suffice.
            Parameter parameters = new Parameter();
            double C;
            double Gamma;


            //This will do a grid optimization to find the best parameters
            //and store them in C and Gamma, outputting the entire
            //search to params.txt.
            this.richTextBox2.Text = "Begin parameter selection...\n"; 
            ParameterSelection.Grid(train, parameters, @paramsFileLocation, out C, out Gamma);            
            parameters.C = C;
            parameters.Gamma = Gamma;


            //Train the model using the optimal parameters.
            this.richTextBox2.AppendText("Begin training...");
            Model model = Training.Train(train, parameters);


            //Perform classification on the test data, putting the
            //results in results.txt.
            this.richTextBox2.AppendText("Begin prediction...");
            Prediction.Predict(test, @resultsFileLocation, model, false);
        }






    }
}
