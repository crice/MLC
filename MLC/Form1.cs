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

using weka.core;
using java.io;
using weka.classifiers;
using weka.classifiers.bayes;  


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
                    Utility.WriteOutSoccerDataToArffFormat_SIMPLEFORMAT_WithQuestionsMarks(soccerData, outFileName);

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


        //WEKA
        private void button3_Click(object sender, EventArgs e)
        {               
            string trainPath = "C:/WekaModelSaves/PreimiershipRawData/CSV/Prem12_11_10_09_08.arff";
            string testPath = "C:/WekaModelSaves/PreimiershipRawData/CSV/Prem13ReqPred.arff";

            try
            {
                Instances train = new Instances(new BufferedReader(new FileReader(trainPath)));
                train.setClassIndex(train.numAttributes() - 1);  
                Instances test = new Instances(new BufferedReader(new FileReader(testPath)));
                test.setClassIndex(test.numAttributes() - 1);  

                //Train classifier
                Classifier classifier = new NaiveBayes();
                classifier.setOptions(new string[] { "-D" });   //use supervised descritization
                classifier.buildClassifier(train);

                Evaluation eval = new Evaluation(train);
                java.util.Random rand = new java.util.Random(1); 
                 
                eval.crossValidateModel(classifier, train, 9, rand, new Object[] { } ); 
 
                this.richTextBox3.Text = eval.toSummaryString("\nResults\n======\n", true);                
                this.richTextBox3.Text += eval.toClassDetailsString(); 
                this.richTextBox3.Text += eval.toMatrixString();
  
            }
            catch (java.lang.Exception ex)
            {
                ex.printStackTrace(); 
            }
        }



        public void GetBayesNet(string trainPath, string testPath, int folds)
        {

            try
            {
                //Load training & test datasets
                Instances train = new Instances(new BufferedReader(new FileReader(trainPath)));
                train.setClassIndex(train.numAttributes() - 1);
                Instances test = new Instances(new BufferedReader(new FileReader(testPath)));
                test.setClassIndex(test.numAttributes() - 1);

                Classifier classifier = new BayesNet();                   
                
                String[] options = new String[20];
                options[0] = "-D";
                options[1] = "-Q";
                options[2] = "weka.classifiers.bayes.net.search.local.K2";
                options[3] = "--";
                options[4] = "-P";
                options[5] = "1";
                options[6] = "-S";
                options[7] = "BAYES";
                options[8] = "";
                options[9] = "";
                options[10] = "";
                options[11] = "-E";
                //options[12] = "weka.classifiers.bayes.net.estimate.SimpleEstimator";
                options[12] = "weka.classifiers.bayes.net.estimate.BMAEstimator";
                options[13] = "--";
                options[14] = "-A";
                options[15] = "0.5";
                options[16] = "";
                options[17] = "";
                options[18] = "";
                options[19] = "";
                classifier.setOptions(options);

                Evaluation eval = new Evaluation(train);
                java.util.Random rand = new java.util.Random(1);

                eval.crossValidateModel(classifier, train, 30, rand, new Object[] { }); 

                this.richTextBox3.Text = eval.toSummaryString("\nResults\n======\n", true);
                this.richTextBox3.Text += eval.toClassDetailsString();
                this.richTextBox3.Text += eval.toMatrixString();

            }
            catch (java.lang.Exception ex)
            {

            }
        }


        public void GetRandomCommittee(string trainPath, string testPath, int folds)
        {
            //Load training & test datasets
            Instances train = new Instances(new BufferedReader(new FileReader(trainPath)));
            train.setClassIndex(train.numAttributes() - 1);
            Instances test = new Instances(new BufferedReader(new FileReader(testPath)));
            test.setClassIndex(test.numAttributes() - 1);

            Classifier classifier = new weka.classifiers.meta.RandomCommittee();

            String[] options = new String[13];
            options[0] = "-S";
            options[1] = "1";
            options[2] = "-I";
            options[3] = "30";
            options[4] = "-W";
            options[5] = "weka.classifiers.trees.RandomTree";
            options[6] = "--";
            options[7] = "-K";
            options[8] = "0";
            options[9] = "-M";
            options[10] = "1.0";
            options[11] = "-S";
            options[12] = "1";


            Evaluation eval = new Evaluation(train);
            java.util.Random rand = new java.util.Random(1);

            eval.crossValidateModel(classifier, test, folds, rand, new Object[] { });

            this.richTextBox3.Text = eval.toSummaryString("\nResults\n======\n", true);
            this.richTextBox3.Text += eval.toClassDetailsString();
            this.richTextBox3.Text += eval.toMatrixString();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            string trainPath = "C:/WekaModelSaves/PreimiershipRawData/CSV/Prem12_11_10_09_08.arff";
            string testPath = "C:/WekaModelSaves/PreimiershipRawData/CSV/Prem13ReqPred.arff";
            int folds = 10;

            //GetBayesNet(trainPath, testPath, folds);

            GetRandomCommittee(trainPath, testPath, folds);
        
        }







    }
}
