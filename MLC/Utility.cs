using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; 

namespace MLC
{
    public static class Utility
    {
        private const char COMMA = ',';
        private const string HYPHEN = "-";
        private const string SPACE = " ";
        private const string NOTHING = "";
        private const string HASH = "#";
        private const int HEADER_INDEX = 0;


        private enum SoccerDataResults
        {
            D = 1,  //Draw
            H = 2,  //Home win
            A = 3   //Away win
        }



        public static List<SoccerData> ReadSoccerDataFromFile(string fileName)
        {
            List<SoccerData> soccerDataList = new List<SoccerData>();
            SoccerData soccerData = null;
            string line = string.Empty;

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] split = line.Split(new char[] { COMMA });
                        soccerData = new SoccerData();

                        soccerData.Division = split[(int)SoccerData.SoccerDataPosition.Division];
                        soccerData.Date = split[(int)SoccerData.SoccerDataPosition.Date];
                        soccerData.HomeTeam = split[(int)SoccerData.SoccerDataPosition.HomeTeam];
                        soccerData.AwayTeam = split[(int)SoccerData.SoccerDataPosition.AwayTeam];
                        soccerData.FullTimeHomeTeamGoals = split[(int)SoccerData.SoccerDataPosition.FullTimeHomeTeamGoals];
                        soccerData.FullTimeAwayTeamGoals = split[(int)SoccerData.SoccerDataPosition.FullTimeAwayTeamGoals];
                        soccerData.FullTimeResult = split[(int)SoccerData.SoccerDataPosition.FullTimeResult];
                        soccerData.HalfTimeHomeTeamGoals = split[(int)SoccerData.SoccerDataPosition.HalfTimeHomeTeamGoals];
                        soccerData.HalfTimeAwayTeamGoals = split[(int)SoccerData.SoccerDataPosition.HalfTimeAwayTeamGoals];
                        soccerData.HalfTimeResult = split[(int)SoccerData.SoccerDataPosition.HalfTimeResult];
                        soccerData.HomeTeamShots = split[(int)SoccerData.SoccerDataPosition.HomeTeamShots];
                        soccerData.AwayTeamShots = split[(int)SoccerData.SoccerDataPosition.AwayTeamShots];
                        soccerData.HomeTeamShotsOnTarget = split[(int)SoccerData.SoccerDataPosition.HomeTeamShotsOnTarget];
                        soccerData.AwayTeamShotsOnTarget = split[(int)SoccerData.SoccerDataPosition.AwayTeamShotsOnTarget];
                        soccerData.HomeTeamFoulsCommitted = split[(int)SoccerData.SoccerDataPosition.HomeTeamFoulsCommitted];
                        soccerData.AwayTeamFoulsCommitted = split[(int)SoccerData.SoccerDataPosition.AwayTeamFoulsCommitted];
                        soccerData.HomeTeamCorners = split[(int)SoccerData.SoccerDataPosition.HomeTeamCorners];
                        soccerData.AwayTeamCorners = split[(int)SoccerData.SoccerDataPosition.AwayTeamCorners];
                        soccerData.HomeTeamYellowCards = split[(int)SoccerData.SoccerDataPosition.HomeTeamYellowCards];
                        soccerData.AwayTeamYellowCards = split[(int)SoccerData.SoccerDataPosition.AwayTeamYellowCards];
                        soccerData.HomeTeamRedCards = split[(int)SoccerData.SoccerDataPosition.HomeTeamRedCards];
                        soccerData.AwayTeamRedCards = split[(int)SoccerData.SoccerDataPosition.AwayTeamRedCards];
                        soccerDataList.Add(soccerData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(ex.Message);
            }

            return soccerDataList;
        }


        public static void WriteOutSoccerDataToArffFormat(List<SoccerData> soccerDataList)
        {
            soccerDataList.RemoveAt(HEADER_INDEX);  

            List<string> teamGames = new List<string>();
            string allTeamGames = string.Empty;
            string newHomeTeam = string.Empty;
            string newAwayTeam = string.Empty;

            //Extact all unique game fixtures like: Arsenal-Sunderland (also remove any spaces like Man United -> ManUnited)
            foreach (SoccerData sd in soccerDataList)
            {
                if (sd.HomeTeam.Contains(SPACE))
                    newHomeTeam = sd.HomeTeam.Replace(SPACE, NOTHING);
                else
                    newHomeTeam = sd.HomeTeam;

                if (sd.AwayTeam.Contains(SPACE))
                    newAwayTeam = sd.AwayTeam.Replace(SPACE, NOTHING);
                else
                    newAwayTeam = sd.AwayTeam; 

                string fixture = newHomeTeam + HYPHEN + newAwayTeam; 
                if (!teamGames.Contains(fixture))
                {
                    teamGames.Add(fixture);
                    allTeamGames += fixture + ",";
                }
            }          

            //Remove comma at end & add the brackets
            allTeamGames = allTeamGames.TrimEnd(new char[] { COMMA });   
            allTeamGames = "{ " + allTeamGames + " }"; 

            System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\FACup2013.arff");

            file.WriteLine("% 1. Title: FA Cup Data 2013 Season\n");
            file.WriteLine("% 2. Source: http://www.football-data.co.uk/data.php");
            file.WriteLine("\n\n"); 
            file.WriteLine("@relation FACup2013");
                        
            file.WriteLine("@attribute " + " \'HomeTeam-AwayTeam\' " + allTeamGames);
            file.WriteLine("@attribute \'FullTimeHomeTeamGoals\' numeric");
            file.WriteLine("@attribute \'FullTimeAwayTeamGoals\' numeric");            
            file.WriteLine("@attribute \'HalfTimeHomeTeamGoals\' numeric");
            file.WriteLine("@attribute \'HalfTimeAwayTeamGoals\' numeric");
            //file.WriteLine("@attribute \'HalfTimeResult\' numeric");
            file.WriteLine("@attribute \'HomeTeamShots\' numeric");
            file.WriteLine("@attribute \'AwayTeamShots\' numeric");
            file.WriteLine("@attribute \'HomeTeamShotsOnTarget\' numeric");
            file.WriteLine("@attribute \'AwayTeamShotsOnTarget\' numeric");
            
            file.WriteLine("@attribute \'HomeTeamFoulsCommitted\' numeric");
            file.WriteLine("@attribute \'AwayTeamFoulsCommitted\' numeric");
            
            file.WriteLine("@attribute \'HomeTeamCorners\' numeric");
            file.WriteLine("@attribute \'AwayTeamCorners\' numeric");
            
            file.WriteLine("@attribute \'HomeTeamYellowCards\' numeric");
            file.WriteLine("@attribute \'AwayTeamYellowCards\' numeric");
            
            file.WriteLine("@attribute \'HomeTeamRedCards\' numeric");
            file.WriteLine("@attribute \'AwayTeamRedCards\' numeric");
            file.WriteLine("@attribute \'class\' { D,H,A }");
            file.WriteLine("@data");

            foreach (SoccerData soccerData in soccerDataList)
            {
                file.Write(soccerData.HomeTeam.Replace(SPACE, NOTHING) + HYPHEN + soccerData.AwayTeam.Replace(SPACE, NOTHING) + COMMA);
                file.Write(soccerData.FullTimeHomeTeamGoals + COMMA);
                file.Write(soccerData.FullTimeAwayTeamGoals + COMMA);
                file.Write(soccerData.HalfTimeHomeTeamGoals + COMMA);
                file.Write(soccerData.HalfTimeAwayTeamGoals + COMMA);
                
                //file.Write(soccerData.HalfTimeResult + COMMA);
                file.Write(soccerData.HomeTeamShots + COMMA);
                file.Write(soccerData.AwayTeamShots + COMMA);
                
                file.Write(soccerData.HomeTeamShotsOnTarget + COMMA);
                file.Write(soccerData.AwayTeamShotsOnTarget + COMMA);
                
                file.Write(soccerData.HomeTeamFoulsCommitted + COMMA);
                file.Write(soccerData.AwayTeamFoulsCommitted + COMMA);

                file.Write(soccerData.HomeTeamCorners + COMMA);
                file.Write(soccerData.AwayTeamCorners + COMMA);

                file.Write(soccerData.HomeTeamYellowCards + COMMA);
                file.Write(soccerData.AwayTeamYellowCards + COMMA);
                
                file.Write(soccerData.HomeTeamRedCards + COMMA);
                file.Write(soccerData.AwayTeamRedCards + COMMA);
                file.WriteLine(soccerData.FullTimeResult);
            }

            file.Close();

        }


        public static void WriteOutSoccerDataToLibsvmFormat(List<SoccerData> soccerDataList, string outFileName)
        {
            soccerDataList.RemoveAt(HEADER_INDEX);  

            System.IO.StreamWriter outFile = new System.IO.StreamWriter(outFileName);

            List<string> teamGames = new List<string>();
            string newHomeTeam = string.Empty;
            string newAwayTeam = string.Empty;

            //Extact all unique game fixtures like: Arsenal-Sunderland (also remove any spaces like Man United -> ManUnited)
            foreach (SoccerData sd in soccerDataList)
            {
                if (sd.HomeTeam.Contains(SPACE))
                    newHomeTeam = sd.HomeTeam.Replace(SPACE, NOTHING);
                else
                    newHomeTeam = sd.HomeTeam;

                if (sd.AwayTeam.Contains(SPACE))
                    newAwayTeam = sd.AwayTeam.Replace(SPACE, NOTHING);
                else
                    newAwayTeam = sd.AwayTeam;

                string fixture = newHomeTeam + HYPHEN + newAwayTeam;
                if (!teamGames.Contains(fixture))
                {
                    teamGames.Add(fixture);
                }
            }

            List<string> sortedTeamGames = teamGames.OrderBy(mc => mc, StringComparer.CurrentCultureIgnoreCase).ToList();   

            foreach (SoccerData soccerData in soccerDataList)
            {
                //Convert the result letter to a consistent number
                string fullTimeResultNumber = ConvertFullTimeResultToConsistentNumber(soccerData.FullTimeResult);
                outFile.Write(fullTimeResultNumber + SPACE);

                string fixture = soccerData.HomeTeam.Replace(SPACE, NOTHING) + HYPHEN + soccerData.AwayTeam.Replace(SPACE, NOTHING);
                string fixtureNumber = AnnexHomeTeamAndAwayTeamAndReturnIndexNumber(sortedTeamGames, soccerData);

                outFile.Write("1:" + fixtureNumber + SPACE);                
                outFile.Write("2:" +soccerData.FullTimeHomeTeamGoals + SPACE);
                outFile.Write("3:" +soccerData.FullTimeAwayTeamGoals + SPACE);
                outFile.Write("4:" +soccerData.HalfTimeHomeTeamGoals + SPACE);
                outFile.Write("5:" +soccerData.HalfTimeAwayTeamGoals + SPACE);

                //file.Write(soccerData.HalfTimeResult + SPACE);
                outFile.Write("6:" +soccerData.HomeTeamShots + SPACE);
                outFile.Write("7:" +soccerData.AwayTeamShots + SPACE);

                outFile.Write("8:" +soccerData.HomeTeamShotsOnTarget + SPACE);
                outFile.Write("9:" +soccerData.AwayTeamShotsOnTarget + SPACE);

                outFile.Write("10:" +soccerData.HomeTeamFoulsCommitted + SPACE);
                outFile.Write("11:" +soccerData.AwayTeamFoulsCommitted + SPACE);

                outFile.Write("12:" +soccerData.HomeTeamCorners + SPACE);
                outFile.Write("13:" +soccerData.AwayTeamCorners + SPACE);

                outFile.Write("14:" +soccerData.HomeTeamYellowCards + SPACE);
                outFile.Write("15:" +soccerData.AwayTeamYellowCards + SPACE);

                outFile.Write("16:" +soccerData.HomeTeamRedCards + SPACE);
                outFile.Write("17:" +soccerData.AwayTeamRedCards + SPACE);
                outFile.WriteLine("# " + fixture + " (" +sortedTeamGames.IndexOf(fixture).ToString() +")");                    
            }

            outFile.Close();

        }


        public static string ConvertFullTimeResultToConsistentNumber(string fullTimeResult)
        {
            if (fullTimeResult.Equals(SoccerDataResults.D.ToString()))
                return SoccerDataResults.D.ToString();
            else if (fullTimeResult.Equals(SoccerDataResults.H.ToString()))
                return SoccerDataResults.D.ToString();
            else if (fullTimeResult.Equals(SoccerDataResults.A.ToString()))
                return SoccerDataResults.D.ToString();
            else
                return "ERROR";

        }

        public static string AnnexHomeTeamAndAwayTeamAndReturnIndexNumber(List<string> sortedTeamGames, SoccerData soccerData)
        {
            string homeTeamAwayTeam = soccerData.HomeTeam.Replace(SPACE, NOTHING) + HYPHEN + soccerData.AwayTeam.Replace(SPACE, NOTHING);

            string fixtureNumber = "";
            if (sortedTeamGames.Contains(homeTeamAwayTeam))
                fixtureNumber = sortedTeamGames.IndexOf(homeTeamAwayTeam).ToString(); 
            else
                fixtureNumber = "-1";

            return fixtureNumber;
        }

    }
}
