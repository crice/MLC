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
        
        public const string SPACE = " ";
        public const string NOTHING = "";

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
                    //Utility.WriteOutSoccerDataToArffFormat_SIMPLEFORMAT_WithQuestionsMarks(soccerData, outFileName);  
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

        public class WekaResult
        {
            private string _homeTeam;
            private string _awayTeam;
            private string _actualResult;
            private string _predictedResult;
            private string _correct;
            private string _interpretation;
            Dictionary<int, string> previousFixtureData = new Dictionary<int,string>();
            double[] distribution = new double[3];
            private string _drawProbability;
            private string _homeWinProbability;
            private string _awayWinProbability;


            private string _differenceBetweenActualAndPredicted;


            #region Properties

            public string HomeTeam
            {
              get { return _homeTeam; }
              set { _homeTeam = value; }
            }

            public string AwayTeam
            {
              get { return _awayTeam; }
              set { _awayTeam = value; }
            }

            public string ActualResult
            {
              get { return _actualResult; }
              set { _actualResult = value; }
            }


            public string PredictedResult
            {
              get { return _predictedResult; }
              set { _predictedResult = value; }
            }

            public string Correct
            {
              get { return _correct; }
              set { _correct = value; }
            }

            public string Interpretation
            {
              get { return _interpretation; }
              set { _interpretation = value; }
            }

            public Dictionary<int, string> PreviousFixtureData
            {
                get { return previousFixtureData; }
                set { previousFixtureData = value; }
            } 

            public double[] Distribution
            {
              get { return distribution; }
              set { distribution = value; }
            }

            public string DrawProbability
            {
              get { return _drawProbability; }
              set { _drawProbability = value; }
            }

            public string HomeWinProbability
            {
                get { return _homeWinProbability; }
                set { _homeWinProbability = value; }
            }

            public string AwayWinProbability
            {
              get { return _awayWinProbability; }
              set { _awayWinProbability = value; }
            }

            public string DifferenceBetweenActualAndPredicted
            {
              get { return _differenceBetweenActualAndPredicted; }
              set { _differenceBetweenActualAndPredicted = value; }
            }

            #endregion


        }

        //WEKA
        private void button3_Click(object sender, EventArgs e)
        {               
            //string trainPath = "C:/WekaModelSaves/PreimiershipRawData/CSV/Prem12_11_10_09_08.arff";
            //string testPath = "C:/WekaModelSaves/PreimiershipRawData/CSV/Prem13ReqPred.arff";

            //string trainPath = @"../../lib/Premiership_12to01_Train_3BookiesOnly.arff";
            //string trainPath = @"../../lib/Prem_Pre12_With3Bookies.arff";
            //string trainPath = @"../../lib/Prem_12_11_10_With3Bookies.arff";
            //string trainPath = @"../../lib/Prem_12to08_Train_3Bookies.arff";
            //string trainPath = @"../../lib/Prem_12_11_10_09_08_07_With3Bookies.arff";
            //string trainPath = @"../../lib/Prem12_11_10_09_With3Bookies.arff";
            //string testPath = @"../../lib/Premiership_13_ReqPred_3Bookies.arff";

            string trainPath = @"../../lib/SeasonEnding2013/Prem12to08WithQuestionsNoBookiesStrictMatchRatingPlusFairOddsPlusLeagueScores_Training.arff";
            string testPath = @"../../lib/SeasonEnding2013/Prem13NoBookiesWithStrictMatchRatingPlusFairOddsPlusLeagueScores_Test.arff";
            string pathToRawData = @"../../lib/Premiership_12_11_10_09_08.txt";
            List<WekaResult> wekaResultsList = new List<WekaResult>();  

            try
            {
                Instances train = new Instances(new BufferedReader(new FileReader(trainPath)));
                train.setClassIndex(train.numAttributes() - 1);  
                Instances test = new Instances(new BufferedReader(new FileReader(testPath)));
                test.setClassIndex(test.numAttributes() - 1);  

                //Train classifier
                //Classifier classifier = new NaiveBayes();
                //classifier.setOptions(new string[] { "-D" });         //use supervised descritization
                //classifier.setOptions(new string[] { "-K" });           //use kernel estimator
                //classifier.buildClassifier(train);

                Classifier classifier = new weka.classifiers.trees.J48();
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

                for (int i = 0; i < test.numInstances(); i++)
                {
                    WekaResult wekaResult = new WekaResult();

                    //Extract home/away team combo...
                    string homeTeamStr = unlabeled.instance(i).stringValue(0);
                    wekaResult.HomeTeam = unlabeled.instance(i).stringValue(0);
                    string awayTeamStr = unlabeled.instance(i).stringValue(1);
                    wekaResult.AwayTeam = unlabeled.instance(i).stringValue(1);
                    Dictionary<int, string> previousFixtureInfo = Utility.ExtractInfoOnPreviousExactEncountersFromTrainingData(pathToRawData, homeTeamStr, awayTeamStr);  
                    wekaResult.PreviousFixtureData = Utility.ExtractInfoOnPreviousExactEncountersFromTrainingData(pathToRawData, homeTeamStr, awayTeamStr);  


                    //Extract the predicted class result
                    double dblPredictedClass = classifier.classifyInstance(test.instance(i));
                    string strPredictedClass = unlabeled.classAttribute().value((int) dblPredictedClass); 
                    wekaResult.PredictedResult = "Predicted: " + unlabeled.classAttribute().value((int) dblPredictedClass); 
  
                    //Extract the distribution
                    double[] distribution = classifier.distributionForInstance(test.instance(i));
                    wekaResult.Distribution = classifier.distributionForInstance(test.instance(i));
                    wekaResult.DrawProbability = Math.Round(distribution[(int)Utility.SoccerDataResultsWekaDistPos.D], 3).ToString();
                    wekaResult.HomeWinProbability = Math.Round(distribution[(int)Utility.SoccerDataResultsWekaDistPos.H], 3).ToString();
                    wekaResult.AwayWinProbability = Math.Round(distribution[(int)Utility.SoccerDataResultsWekaDistPos.A], 3).ToString();

                    //Extract actual class result
                    double dblActualClass = unlabeled.instance(i).classValue();
                    string strActualClass = unlabeled.classAttribute().value((int)dblActualClass);
                    wekaResult.ActualResult = "Actual: " + unlabeled.classAttribute().value((int)dblActualClass);

                    if (!dblActualClass.Equals(dblPredictedClass))
                    {
                        wekaResult.Correct = "WRONG";
                        wekaResult.DifferenceBetweenActualAndPredicted = GetDifferenceBetweenActualAndPredicted(distribution, strActualClass, strPredictedClass);
                    }
                    else
                    {
                        wekaResult.Correct = "RIGHT";
                        wekaResult.DifferenceBetweenActualAndPredicted = "0";
                    }


                    wekaResultsList.Add(wekaResult);

                } 
               
                int j = 0;                
                //Pump out to screen
                foreach (WekaResult wekaResult in wekaResultsList)
                {
                    Dictionary<int, string> previousFixtureScores = wekaResult.PreviousFixtureData;
                    int count = previousFixtureScores.Keys.FirstOrDefault();
                    string fixturesPattern = previousFixtureScores.Values.FirstOrDefault();  


                    this.richTextBox3.Text += wekaResult.HomeTeam + "\t" + wekaResult.AwayTeam + "\t" + wekaResult.ActualResult + "\t" + wekaResult.PredictedResult +
                        "\t" + wekaResult.Correct + "\t" + count.ToString()  + "\t" + fixturesPattern +
                        "\t" + wekaResult.DrawProbability + "\t" + wekaResult.HomeWinProbability + "\t" + wekaResult.AwayWinProbability + "\t" + wekaResult.DifferenceBetweenActualAndPredicted + "\n";       

                    j++;
                }

            }
            catch (java.lang.Exception ex)
            {
                ex.printStackTrace(); 
            }
        }


        public string GetDifferenceBetweenActualAndPredicted(double[] distribution, string strActualClassNumber, string strPredictedClassNumber)
        {

            double dblActualResult = GetActualValue(distribution, strActualClassNumber);
            double dblPredictedResult = GetActualValue(distribution, strPredictedClassNumber);

            double difference = 0.0;
            difference = dblActualResult - dblPredictedResult;
            difference = Math.Abs(difference);
            difference = Math.Round(difference, 3);

            return difference.ToString();

        }

        public Dictionary<int, string> ExtractInfoOnPreviousEncountersFromTrainingData(string pathToRawData, string homeTeam, string awayTeam)
        {
            Dictionary<int, string> previousFixtureResults = new Dictionary<int, string>();

            bool includeHeader = false;
            List<SoccerData> allSoccerData = Utility.ReadSoccerDataFromFile(pathToRawData, includeHeader);

            //Extact the home-team subset from all the data
            List<SoccerData> homeTeamSubset = allSoccerData.FindAll(mc => mc.HomeTeam.Replace(SPACE, NOTHING)  == homeTeam);

            //Extract away-team subset from the home-team subset
            List<SoccerData> awayTeamSubset = homeTeamSubset.FindAll(mc => mc.AwayTeam.Replace(SPACE, NOTHING) == awayTeam);

            //Order the fixturesd where both teams met.
            List<SoccerData> orderedPreviousFixture = awayTeamSubset.OrderByDescending(mc => mc.Date).ToList();  //Most recent at the top of the list  
            
            string peviousEncountersResult = string.Empty;
            int previousEncountersCount = 0;
            foreach (SoccerData soccerData in orderedPreviousFixture)
            {
                previousEncountersCount++;
                peviousEncountersResult += soccerData.FullTimeResult; 
            }

            previousFixtureResults.Add(previousEncountersCount, peviousEncountersResult);  
            return previousFixtureResults; 
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
                @"../../lib/Premiership2010.txt", @"../../lib/Premiership2009.txt", @"../../lib/Premiership2009.txt", @"../../lib/Premiership2008.txt" };

            //string[] trainingFiles = new string[] { @"../../lib/Premiership2011.txt", @"../../lib/Premiership2010.txt", 
            //    @"../../lib/Premiership2009.txt", @"../../lib/Premiership2008.txt", @"../../lib/Premiership2007.txt" };

            //string[] trainingFiles = new string[] { @"../../lib/Premiership2010.txt", @"../../lib/Premiership2009.txt", 
            //    @"../../lib/Premiership2008.txt", @"../../lib/Premiership2007.txt", @"../../lib/Premiership2006.txt" };

            //Test data
            string[] testFiles = new string[] { @"../../lib/Premiership2013.txt" };
            //string[] testFiles = new string[] { @"../../lib/Premiership2012.txt" }; 
            //string[] testFiles = new string[] { @"../../lib/Premiership2011.txt" }; 


            List<SoccerData> allSoccerData = new List<SoccerData>();
            List<SoccerDataLeagueScore> allSoccerDataLeagueScore = new List<SoccerDataLeagueScore>();  

            foreach (string fileName in testFiles)
            {
                List<SoccerData> seasonSoccerData = Utility.GetFixtureMatchRatingForAll(fileName);
                List<SoccerDataLeagueScore> seasonSoccerDataLeagueScore = Utility.GetLeagueScoreForAll(seasonSoccerData);  

                allSoccerData.AddRange(seasonSoccerData);
                allSoccerDataLeagueScore.AddRange(seasonSoccerDataLeagueScore);  
            }

            //Get the league score...
            //List<SoccerDataLeagueScore> leagueScores = Utility.GetLeagueScoreForAll(allSoccerData); 


            //Convert to arff format
            //Utility.WriteOutSoccerDataToArffFormat(allSoccerData, @"../../lib/Prem12to08With3BookiesPlusMatchRating_Training.arff"); 
            Utility.WriteOutSoccerDataToArffFormat_SIMPLEFORMAT_WithQuestionsMarks(allSoccerData, allSoccerDataLeagueScore, @"../../lib/Prem13NoMetaNoBookiesWithStrictMatchRatingPlusFairOdds_Test.arff"); 
            //Utility.WriteOutSoccerDataToArffFormat_SIMPLEFORMAT_WithQuestionsMarks(allSoccerData, allSoccerDataLeagueScore, @"../../lib/Prem12to07NoMetaNoBookiesWithStrictMatchRatingPlusFair_Training.arff"); 
        }






    }
}
