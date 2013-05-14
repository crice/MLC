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
        private const int HEADER_INDEX = 0;

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
                        soccerData.FullTimeHomeTeamGoals = split[4];
                        soccerData.FullTimeAwayTeamGoals = split[5];
                        soccerData.FullTimeResult = split[6];
                        soccerData.HalfTimeHomeTeamGoals = split[7];
                        soccerData.HalfTimeAwayTeamGoals = split[8];
                        soccerData.HalfTimeResult = split[9];
                        soccerData.HomeTeamShots = split[10];
                        soccerData.AwayTeamShots = split[11];
                        soccerData.HomeTeamShotsOnTarget = split[12];
                        soccerData.AwayTeamShotsOnTarget = split[13];
                        soccerData.HomeTeamFoulsCommitted = split[14];
                        soccerData.AwayTeamFoulsCommitted = split[15];
                        soccerData.HomeTeamCorners = split[16];
                        soccerData.AwayTeamCorners = split[17];
                        soccerData.HomeTeamYellowCards = split[18];
                        soccerData.AwayTeamYellowCards = split[19];
                        soccerData.HomeTeamRedCards = split[20];
                        soccerData.AwayTeamRedCards = split[21];
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

            //Extact all unique game fixtures like: Arsenal-Sunderland
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


    }
}
