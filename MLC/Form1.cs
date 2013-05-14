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


        //SVM - M. Johnson
        private void button2_Click(object sender, EventArgs e)
        {
            Problem train = Problem.Read(@"C:\\libsvm\\diabetes.train");
            Problem test = Problem.Read(@"C:\\libsvm\\diabetes.test");

            //For this example (and indeed, many scenarios), the default
            //parameters will suffice.
            Parameter parameters = new Parameter();
            double C;
            double Gamma;


            //This will do a grid optimization to find the best parameters
            //and store them in C and Gamma, outputting the entire
            //search to params.txt.

            ParameterSelection.Grid(train, parameters, @"C:\\libsvm\\params.txt", out C, out Gamma);
            parameters.C = C;
            parameters.Gamma = Gamma;


            //Train the model using the optimal parameters.

            Model model = Training.Train(train, parameters);


            //Perform classification on the test data, putting the
            //results in results.txt.

            Prediction.Predict(test, @"C:\\libsvm\\results.txt", model, false);
        }


    }
}
