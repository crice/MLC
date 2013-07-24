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
using weka.core.converters;  


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
            bool includeHeader = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string outFileName = this.textBox5.Text; 

                if (checkBox1.Checked == true)
                {
                    string fileName = openFileDialog.FileName;
                    List<SoccerData> soccerData = Utility.ReadSoccerDataFromFile(fileName, includeHeader);
                    Utility.WriteOutSoccerDataToArffFormat_SIMPLEFORMAT(soccerData, outFileName);   

                    richTextBox1.Text = "Text output to C:\\"; 
                }
                else if (checkBox3.Checked == true)
                {
                    string fileName = openFileDialog.FileName;
                    List<SoccerData> soccerData = Utility.ReadSoccerDataFromFile(fileName, includeHeader);
                    Utility.WriteOutSoccerDataToArffFormat_SIMPLEFORMAT_WithQuestionsMarks(soccerData, outFileName);  
                    richTextBox1.Text = "Text output to C:\\"; 
                }
                else if (checkBox2.Checked == true)
                {
                    string fileName = openFileDialog.FileName;
                    List<SoccerData> soccerData = Utility.ReadSoccerDataFromFile(fileName, includeHeader);
                    Utility.WriteOutSoccerDataToLibsvmFormat(soccerData, outFileName);

                    richTextBox1.Text = "Text output to C:\\" + outFileName;
                }
                else if (checkBox4.Checked == true)
                {
                    string fileName = openFileDialog.FileName;
                    List<SoccerData> soccerData = Utility.ReadSoccerDataFromFile(fileName, includeHeader);
                    List<SoccerData> soccerDataPlusGoalSuperiority = Utility.AddGoalSuperiority(soccerData);
                    Utility.WriteMatchRatingOutToFile(soccerDataPlusGoalSuperiority, outFileName);  

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

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
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
            //string trainPath = "C:/WekaModelSaves/PreimiershipRawData/CSV/Prem12_11_10_09_08.arff";
            //string testPath = "C:/WekaModelSaves/PreimiershipRawData/CSV/Prem13ReqPred.arff";

            //string trainPath = @"../../lib/Premiership_12to01_Train_3BookiesOnly.arff";
            //string trainPath = @"../../lib/Prem_Pre12_With3Bookies.arff";
            //string trainPath = @"../../lib/Prem_12_11_10_With3Bookies.arff";
            string trainPath = @"../../lib/Prem_12to08_Train_3Bookies.arff";
            //string trainPath = @"../../lib/Prem_12_11_10_09_08_07_With3Bookies.arff";
            //string trainPath = @"../../lib/Prem12_11_10_09_With3Bookies.arff";
            string testPath = @"../../lib/Premiership_13_ReqPred_3Bookies.arff";

            try
            {
                Instances train = new Instances(new BufferedReader(new FileReader(trainPath)));
                train.setClassIndex(train.numAttributes() - 1);  
                Instances test = new Instances(new BufferedReader(new FileReader(testPath)));
                test.setClassIndex(test.numAttributes() - 1);  

                //Train classifier
                Classifier classifier = new NaiveBayes();
                //classifier.setOptions(new string[] { "-D" });         //use supervised descritization
                classifier.setOptions(new string[] { "-K" });           //use kernel estimator
                classifier.buildClassifier(train);

                Evaluation eval = new Evaluation(train);
                java.util.Random rand = new java.util.Random(1); 
                 
                //eval.crossValidateModel(classifier, train, 10, rand, new Object[] { } ); 
                eval.evaluateModel(classifier, test);               

                this.richTextBox3.Text = eval.toSummaryString("\nResults\n======\n", true);                
                this.richTextBox3.Text += eval.toClassDetailsString(); 
                this.richTextBox3.Text += eval.toMatrixString();

                this.richTextBox3.Text += "\n\n\n"; 


                Instances unlabeled = new Instances(
                                    new BufferedReader(
                                        new FileReader(testPath)));
                unlabeled.setClassIndex(unlabeled.numAttributes() - 1);

                string outcome = "RIGHT";
                for (int i = 0; i < test.numInstances(); i++)
                {
                    //Extract the predicted class result
                    double dblPredictedClass = classifier.classifyInstance(test.instance(i));
                    string strPredictedClass = unlabeled.classAttribute().value((int) dblPredictedClass);   
                    double[] distribution = classifier.distributionForInstance(test.instance(i));

                    //Extract actual class result
                    double dblActualClass = unlabeled.instance(i).classValue();
                    string strActualClass = unlabeled.classAttribute().value((int)dblActualClass);

                    double difference = 0.0;
                    if (!dblActualClass.Equals(dblPredictedClass))
                    {
                        outcome = "WRONG";
                        double dblActualResult = GetActualValue(distribution, strActualClass);
                        double dblPredictedResult = GetActualValue(distribution, strPredictedClass);

                        difference = dblActualResult - dblPredictedResult;
                        difference = Math.Abs(difference);
                        difference = Math.Round(difference, 3);
                    }
                    else
                        outcome = "RIGHT";

                    //Extract home/away team combo...
                    string homeTeamStr = unlabeled.instance(i).stringValue(0);
                    string awayTeamStr = unlabeled.instance(i).stringValue(1);
                    string fixtures = ExtractInfoOnPreviousEncountersFromTrainingData(homeTeamStr, awayTeamStr);  

                    this.richTextBox3.Text += i.ToString() + "\t" +"ACTUAL: " +strActualClass +"\t" + "PREDICTED: " + strPredictedClass +"\t"; 
                    for (int j = 0; j < distribution.Count(); j++)
                    {
                        double rounded = Math.Round(distribution[j], 3);
                        this.richTextBox3.Text += rounded.ToString() + "\t";
                    }

                    this.richTextBox3.Text += outcome + "\t";

                    if (difference <= 0.1)
                        this.richTextBox3.Text += "VERY CLOSE" + "\t";

                    this.richTextBox3.Text += difference.ToString() + "\t";
                    this.richTextBox3.Text += fixtures;
 
                    this.richTextBox3.Text += "\n"; 
                }                
            }
            catch (java.lang.Exception ex)
            {
                ex.printStackTrace(); 
            }
        }



        public string ExtractInfoOnPreviousEncountersFromTrainingData(string homeTeam, string awayTeam)
        {
            //string fileName = @"../../lib/Premiership_12_11_10_09_08_07_06_05_04_03_02_01.txt";
            string fileName = @"../../lib/Premiership_12_11_10_09_08.txt";

            bool includeHeader = false;
            List<SoccerData> allSoccerData = Utility.ReadSoccerDataFromFile(fileName, includeHeader);

            //Extact the home-team subset from all the data
            List<SoccerData> homeTeamSubset = allSoccerData.FindAll(mc => mc.HomeTeam == homeTeam);

            //Extract away-team subset from the home-team subset
            List<SoccerData> awayTeamSubset = homeTeamSubset.FindAll(mc => mc.AwayTeam == awayTeam);

            string fixtureInfo = string.Empty;
            int count = 0;
            foreach (SoccerData soccerData in awayTeamSubset)
            {
                count++;
                fixtureInfo += soccerData.FullTimeResult; 
            }

            return count.ToString() + fixtureInfo;
        }






        public double GetActualValue(double[] distribution, string result)
        {
            if (result.Equals(Utility.SoccerDataResultsWekaDistPos.D.ToString()))
            {
                return distribution[(int)Utility.SoccerDataResultsWekaDistPos.D];
            }
            else if (result.Equals(Utility.SoccerDataResultsWekaDistPos.H.ToString()))
            {
                return distribution[(int)Utility.SoccerDataResultsWekaDistPos.H];
            }
            else if (result.Equals(Utility.SoccerDataResultsWekaDistPos.A.ToString()))
            {
                return distribution[(int)Utility.SoccerDataResultsWekaDistPos.A];
            }
            else
                return 0.0;
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


            GetRandomCommittee(trainPath, testPath, folds);
        
        }

        private void button5_Click(object sender, EventArgs e)
        {            
            //String filePath = @"../../lib/Premiership13.txt";
            String filePath = @"../../lib/SoccerDataPlusMatchRating.txt";
            java.io.File file = new java.io.File(filePath);  

            //Load CSV
            CSVLoader loader = new CSVLoader();
            loader.setSource(file);           
            Instances data = loader.getDataSet();
            data.setClassIndex(5);

            Classifier classifier = new NaiveBayes();
            //classifier.setOptions(new string[] { "-D" });         //use supervised descritization
            classifier.setOptions(new string[] { "-K" });           //use kernel estimator
            classifier.buildClassifier(data);

            Evaluation eval = new Evaluation(data);
            java.util.Random rand = new java.util.Random(1);
            
            eval.crossValidateModel(classifier, data, 10, rand, new Object[] { } ); 
            //eval.evaluateModel(classifier, test);
            eval.evaluateModel(classifier, data); 

            this.richTextBox3.Text = eval.toSummaryString("\nResults\n======\n", true);
            this.richTextBox3.Text += eval.toClassDetailsString();
            this.richTextBox3.Text += eval.toMatrixString();
            

            //Save CSV
            //String fileOut = @"../../lib/PREM13.arff";
            //ArffSaver saver = new ArffSaver();
            //saver.setInstances(data);
            //saver.setFile(new java.io.File(fileOut));
            //saver.writeBatch();  

        }

        private void button6_Click(object sender, EventArgs e)
        {
           
            //Training data
            string[] trainingFiles = new string[] { @"../../lib/Premiership2012.txt", @"../../lib/Premiership2011.txt", 
                @"../../lib/Premiership2010.txt", @"../../lib/Premiership2009.txt", @"../../lib/Premiership2009.txt" };

            //Test data
            string[] testFiles = new string[] { @"../../lib/Premiership2013.txt" };


            List<SoccerData> allSoccerData = new List<SoccerData>();

            foreach (string fileName in testFiles)
            {
                List<SoccerData> seasonSoccerData = Utility.GetFixtureMatchRatingForAll(fileName);
                allSoccerData.AddRange(seasonSoccerData);

                List<SoccerDataLeagueScore> leagueScores = Utility.GetLeagueScoreForAll(allSoccerData); 
            }

            //Convert to arff format
            //Utility.WriteOutSoccerDataToArffFormat(allSoccerData, @"../../lib/Prem12to08With3BookiesPlusMatchRating_Training.arff"); 
            Utility.WriteOutSoccerDataToArffFormat_SIMPLEFORMAT_WithQuestionsMarks(allSoccerData, @"../../lib/Prem13WithNoWithStrictMatchRatingPlusFairOdds_Test.arff"); 
            //Utility.WriteOutSoccerDataToArffFormat_SIMPLEFORMAT_WithQuestionsMarks(allSoccerData, @"../../lib/Prem12to08WithQuestionsNoBookiesStrictMatchRatingPlusFairOdds_Training.arff"); 
        }






    }
}
