using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Collections; 

namespace MLC
{
    public static class Utility
    {
        private const string COMMA = ",";
        private const string HYPHEN = "-";
        private const string SPACE = " ";
        private const string NOTHING = "";
        private const string HASH = "#";
        private const int HEADER_INDEX = 0;
        private const string COMMASPACE = ", ";
        private const string QUESTION_MARK = "?";

        private const int RECENT_FORM = 6;      //the last 6 games
        private const int TWO = 2;
        private const int ZERO = 0;
        private const double ONE_HUNDRED = 100.0;

        private const string PATH_TO_TEAMS_DATA = @"..\..\Premiership.xml";
        private const string HISTORICAL_DATA_FILENAME = @"../../lib/Premiership_12_11_10_09_08_07_06_05_04_03_02_01.txt";
        private const string PREM_13_DATA = @"../../lib/Premiership13.txt";

        private const string HEADER1 = "Date, HomeTeam, AwayTeam, FTHomeTeamGoals, FTAwayTeamGoals, FixtureMatchRating, FTResult";

        //Arrives in an akward format.
        private const string nottingHamForestRawDataFormat = "Nott'mForest";
        private const string nottingHamForestPreferedFormat = "NottinghamForest";

        private const string middlesBroughRawDataFormat = "Middlesboro";
        private const string middlesBroughPreferredFormat = "Middlesbrough";

        public enum SoccerDataResultsWekaDistPos
        {
            D = 0,  //D=Draw
            H = 1,  //H=Home
            A = 2   //A=Away
        }

        private enum SoccerDataResults
        {
            D = 1,  //D=Draw
            H = 2,  //H=Home win
            A = 3   //A=Away win
        }

        public enum PremierLeagueScoring
        {
            LOSE = 0,
            DRAW = 1,
            WIN = 3
        }


        public static List<string> ReadXmlFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(PATH_TO_TEAMS_DATA);
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("team");

            List<string> teams = new List<String>();

            foreach (XmlNode xmlNode in nodeList)
            {
                teams.Add(xmlNode.InnerText);  
            }                       

            return teams;

        }


        public static List<SoccerData> ReadSoccerDataFromFile(string fileName, bool includeHeader)
        {
            List<SoccerData> soccerDataList = new List<SoccerData>();
            SoccerData soccerData = null;
            string line = string.Empty;
            int rowCount = 0;

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] split = line.Split(new char[] { Char.Parse(COMMA) });
                        soccerData = new SoccerData();

                        if (!includeHeader & rowCount > HEADER_INDEX)
                        {

                            soccerData.Division = split[(int)SoccerData.SoccerDataPosition.Division];
                            string dateTime = split[(int)SoccerData.SoccerDataPosition.Date];
                            soccerData.Date = Utility.ConvertStringToDateTime(dateTime);
                            //soccerData.Date = split[(int)SoccerData.SoccerDataPosition.Date];
                            soccerData.HomeTeam = split[(int)SoccerData.SoccerDataPosition.HomeTeam].Replace(SPACE, NOTHING);
                            soccerData.AwayTeam = split[(int)SoccerData.SoccerDataPosition.AwayTeam].Replace(SPACE, NOTHING); ;
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

                            //***** BETTING *****

                            //Bet365
                            soccerData.Bet365HomeWinOdds = split[(int)SoccerData.SoccerDataPosition.Bet365HomeWinOdds];
                            soccerData.Bet365DrawOdds = split[(int)SoccerData.SoccerDataPosition.Bet365DrawOdds];
                            soccerData.Bet365AwayWinOdds = split[(int)SoccerData.SoccerDataPosition.Bet365AwayWinOdds];

                            //Ladbrooks
                            soccerData.LadbrooksHomeWinOdds = split[(int)SoccerData.SoccerDataPosition.LadbrooksHomeWinOdds];
                            soccerData.LadbrooksDrawOdds = split[(int)SoccerData.SoccerDataPosition.LadbrooksDrawOdds];
                            soccerData.LadbrooksAwayWinOdds = split[(int)SoccerData.SoccerDataPosition.LadbrooksAwayWinOdds];

                            //William Hill
                            soccerData.WilliamHillHomeWinOdds = split[(int)SoccerData.SoccerDataPosition.WilliamHillHomeWinOdds];
                            soccerData.WilliamHillDrawOdds = split[(int)SoccerData.SoccerDataPosition.WilliamHillDrawOdds];
                            soccerData.WilliamHillAwayWinOdds = split[(int)SoccerData.SoccerDataPosition.WilliamHillAwayWinOdds];

                            soccerDataList.Add(soccerData);
                        }

                        rowCount++;
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

        public static List<SoccerData> ReadSoccerDataFromFilePre2012WithReferee(string fileName, bool includeHeader)
        {
            List<SoccerData> soccerDataList = new List<SoccerData>();
            SoccerData soccerData = null;
            string line = string.Empty;
            int rowCount = 0;

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] split = line.Split(new char[] { Char.Parse(COMMA) });
                        soccerData = new SoccerData();


                        if (!includeHeader & rowCount > HEADER_INDEX)
                        {

                            soccerData.Division = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.Division];
                            string dateTime = split[(int)SoccerData.SoccerDataPosition.Date];
                            soccerData.Date = Utility.ConvertStringToDateTime(dateTime);
                            soccerData.HomeTeam = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HomeTeam].Replace(SPACE, NOTHING);
                            soccerData.AwayTeam = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.AwayTeam].Replace(SPACE, NOTHING);
                            soccerData.FullTimeHomeTeamGoals = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.FullTimeHomeTeamGoals];
                            soccerData.FullTimeAwayTeamGoals = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.FullTimeAwayTeamGoals];
                            soccerData.FullTimeResult = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.FullTimeResult];
                            soccerData.HalfTimeHomeTeamGoals = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HalfTimeHomeTeamGoals];
                            soccerData.HalfTimeAwayTeamGoals = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HalfTimeAwayTeamGoals];
                            soccerData.HalfTimeResult = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HalfTimeResult];
                            soccerData.Referee = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.Referee];
                            soccerData.HomeTeamShots = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HomeTeamShots];
                            soccerData.AwayTeamShots = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.AwayTeamShots];
                            soccerData.HomeTeamShotsOnTarget = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HomeTeamShotsOnTarget];
                            soccerData.AwayTeamShotsOnTarget = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.AwayTeamShotsOnTarget];
                            soccerData.HomeTeamFoulsCommitted = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HomeTeamFoulsCommitted];
                            soccerData.AwayTeamFoulsCommitted = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.AwayTeamFoulsCommitted];
                            soccerData.HomeTeamCorners = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HomeTeamCorners];
                            soccerData.AwayTeamCorners = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.AwayTeamCorners];
                            soccerData.HomeTeamYellowCards = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HomeTeamYellowCards];
                            soccerData.AwayTeamYellowCards = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.AwayTeamYellowCards];
                            soccerData.HomeTeamRedCards = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.HomeTeamRedCards];
                            soccerData.AwayTeamRedCards = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.AwayTeamRedCards];

                            //***** BETTING *****

                            //Bet365
                            soccerData.Bet365HomeWinOdds = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.Bet365HomeWinOdds];
                            soccerData.Bet365DrawOdds = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.Bet365DrawOdds];
                            soccerData.Bet365AwayWinOdds = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.Bet365AwayWinOdds];

                            //Ladbrooks
                            soccerData.LadbrooksHomeWinOdds = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.LadbrooksHomeWinOdds];
                            soccerData.LadbrooksDrawOdds = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.LadbrooksDrawOdds];
                            soccerData.LadbrooksAwayWinOdds = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.LadbrooksAwayWinOdds];

                            //William Hill
                            soccerData.WilliamHillHomeWinOdds = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.WilliamHillHomeWinOdds];
                            soccerData.WilliamHillDrawOdds = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.WilliamHillDrawOdds];
                            soccerData.WilliamHillAwayWinOdds = split[(int)SoccerData.SoccerDataPositionPre2012WithReferee.WilliamHillAwayWinOdds];

                            soccerDataList.Add(soccerData);
                        }

                        rowCount++;
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


        public static void WriteOutSoccerDataToArffFormat(List<SoccerData> soccerDataList, string outFileName)
        {
            ReadXmlFile();

            //soccerDataList.RemoveAt(HEADER_INDEX);  
            List<string> homeTeams = new List<string>();
            List<string> awayTeams = new List<string>();
            string allHomeTeams = string.Empty;
            string allAwayTeams = string.Empty;
            string newHomeTeam = string.Empty;
            string newAwayTeam = string.Empty;

            homeTeams = ReadXmlFile();
            allHomeTeams = SortAnnexAddBrackets(homeTeams);
            allAwayTeams = allHomeTeams;

            List<string> teamGames = new List<string>();
            string allTeamGames = string.Empty;
            //string newHomeTeam = string.Empty;
            //string newAwayTeam = string.Empty;

            //Extact all unique game fixtures like: Arsenal-Sunderland (also remove any spaces like Man United -> ManUnited)
            /*
            foreach (SoccerData sd in soccerDataList)
            {

                if (sd.HomeTeam.Contains(SPACE))
                    newHomeTeam = sd.HomeTeam.Replace(SPACE, NOTHING);
                else
                    newHomeTeam = sd.HomeTeam;

                if (newHomeTeam.Equals(nottingHamForestRawDataFormat))
                    newHomeTeam = nottingHamForestPreferedFormat;

                if (sd.AwayTeam.Contains(SPACE))
                    newAwayTeam = sd.AwayTeam.Replace(SPACE, NOTHING);
                else
                    newAwayTeam = sd.AwayTeam;

                if (newAwayTeam.Equals(nottingHamForestRawDataFormat))
                    newAwayTeam = nottingHamForestPreferedFormat;

                string fixture = newHomeTeam + HYPHEN + newAwayTeam; 
                if (!teamGames.Contains(fixture))
                {
                    teamGames.Add(fixture);
                    allTeamGames += fixture + ",";
                }
            }  
             * */

            //Remove comma at end & add the brackets
            //allTeamGames = allTeamGames.TrimEnd(new char[] { Char.Parse(COMMA) });   
            //allTeamGames = "{ " + allTeamGames + " }";

            System.IO.StreamWriter file = new System.IO.StreamWriter(outFileName);

            file.WriteLine("% 1. Title: FA Cup Data 2012 to 2008 Season Plus Match Rating\n");
            file.WriteLine("% 2. Source: http://www.football-data.co.uk/data.php");
            file.WriteLine("\n\n"); 
            file.WriteLine("@relation FACup2012to08PlusMatchRating");

            file.WriteLine("@attribute \'HomeTeam\' " + allHomeTeams);
            file.WriteLine("@attribute \'AwayTeam\' " + allAwayTeams);
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

            //Betting
            file.WriteLine("@attribute \'Bet365HomeWinOdds\' numeric");
            file.WriteLine("@attribute \'Bet365DrawOdds\' numeric");
            file.WriteLine("@attribute \'Bet365AwayWinOdds\' numeric");

            file.WriteLine("@attribute \'LadbrooksHomeWinOdds\' numeric");
            file.WriteLine("@attribute \'LadbrooksDrawOdds\' numeric");
            file.WriteLine("@attribute \'LadbrooksAwayWinOdds\' numeric");

            file.WriteLine("@attribute \'WilliamHillHomeWinOdds\' numeric");
            file.WriteLine("@attribute \'WilliamHillDrawOdds\' numeric");
            file.WriteLine("@attribute \'WilliamHillAwayWinOdds\' numeric"); 

            file.WriteLine("@attribute \'MatchRating\' numeric");

            file.WriteLine("@attribute \'class\' { D,H,A }");
            file.WriteLine("@data");

            foreach (SoccerData soccerData in soccerDataList)
            {
                //string fixture = soccerData.HomeTeam.Replace(SPACE, NOTHING) + HYPHEN + soccerData.AwayTeam.Replace(SPACE, NOTHING) + COMMA;

                //if (fixture.Contains(nottingHamForestRawDataFormat))
                    //fixture = fixture.Replace(nottingHamForestRawDataFormat, nottingHamForestPreferedFormat);

                string homeTeam = soccerData.HomeTeam.Replace(SPACE, NOTHING);
                string awayTeam = soccerData.AwayTeam.Replace(SPACE, NOTHING);

                if (homeTeam.Contains(nottingHamForestRawDataFormat))
                    homeTeam = homeTeam.Replace(nottingHamForestRawDataFormat, nottingHamForestPreferedFormat);
                if (awayTeam.Contains(nottingHamForestRawDataFormat))
                    awayTeam = awayTeam.Replace(nottingHamForestRawDataFormat, nottingHamForestPreferedFormat);


                if (homeTeam.Contains(middlesBroughRawDataFormat))
                    homeTeam = homeTeam.Replace(middlesBroughRawDataFormat, middlesBroughPreferredFormat);
                if (awayTeam.Contains(middlesBroughRawDataFormat))
                    awayTeam = awayTeam.Replace(middlesBroughRawDataFormat, middlesBroughPreferredFormat);

                file.Write(homeTeam + COMMA);
                file.Write(awayTeam + COMMA);
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

                //***** BETTING *****

                //Bet365
                file.Write(soccerData.Bet365HomeWinOdds + COMMA);
                file.Write(soccerData.Bet365DrawOdds + COMMA);
                file.Write(soccerData.Bet365AwayWinOdds + COMMA);

                //Ladbrooks
                if (!soccerData.LadbrooksHomeWinOdds.Equals(NOTHING))
                    file.Write(soccerData.LadbrooksHomeWinOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.LadbrooksDrawOdds.Equals(NOTHING))
                    file.Write(soccerData.LadbrooksDrawOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.LadbrooksAwayWinOdds.Equals(NOTHING))
                    file.Write(soccerData.LadbrooksAwayWinOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                //William Hill
                if (!soccerData.WilliamHillHomeWinOdds.Equals(NOTHING))
                    file.Write(soccerData.WilliamHillHomeWinOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.WilliamHillDrawOdds.Equals(NOTHING))
                    file.Write(soccerData.WilliamHillDrawOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.WilliamHillAwayWinOdds.Equals(NOTHING))
                    file.Write(soccerData.WilliamHillAwayWinOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                file.Write(soccerData.MatchRating + COMMA);  

                file.WriteLine(soccerData.FullTimeResult);
            }

            file.Close();

        }


        public static void WriteOutSoccerDataToArffFormat_SIMPLEFORMAT(List<SoccerData> soccerDataList, string outFileName)
        {

            ReadXmlFile();

            soccerDataList.RemoveAt(HEADER_INDEX);

            List<string> homeTeams = new List<string>();
            List<string> awayTeams = new List<string>();
            string allHomeTeams = string.Empty;
            string allAwayTeams = string.Empty;
            string newHomeTeam = string.Empty;
            string newAwayTeam = string.Empty;


            homeTeams = ReadXmlFile(); 
            allHomeTeams = SortAnnexAddBrackets(homeTeams);
            allAwayTeams = allHomeTeams;
            //Remove comma at end & add the brackets
            //allHomeTeams = SortAnnexAddBrackets(homeTeams);
            //allAwayTeams = SortAnnexAddBrackets(awayTeams);

            System.IO.StreamWriter file = new System.IO.StreamWriter(outFileName);

            file.WriteLine("% 1. Title: Premiership Data 2013 Season\n");
            file.WriteLine("% 2. Source: http://www.football-data.co.uk/data.php");
            file.WriteLine("\n\n");
            file.WriteLine("@relation Premiership2013");

            file.WriteLine("@attribute \'HomeTeam\' " + allHomeTeams);
            file.WriteLine("@attribute \'AwayTeam\' " + allAwayTeams); 
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

            file.WriteLine("@attribute \'Bet365HomeWinOdds\' numeric");
            file.WriteLine("@attribute \'Bet365DrawOdds\' numeric");
            file.WriteLine("@attribute \'Bet365AwayWinOdds\' numeric");

            file.WriteLine("@attribute \'LadbrooksHomeWinOdds\' numeric");
            file.WriteLine("@attribute \'LadbrooksDrawOdds\' numeric");
            file.WriteLine("@attribute \'LadbrooksAwayWinOdds\' numeric");                 

            file.WriteLine("@attribute \'WilliamHillHomeWinOdds\' numeric");
            file.WriteLine("@attribute \'WilliamHillDrawOdds\' numeric");
            file.WriteLine("@attribute \'WilliamHillAwayWinOdds\' numeric"); 

            file.WriteLine("@attribute \'class\' { D,H,A }");
            file.WriteLine("@data");

            foreach (SoccerData soccerData in soccerDataList)
            {
                string homeTeam = soccerData.HomeTeam.Replace(SPACE, NOTHING);
                string awayTeam = soccerData.AwayTeam.Replace(SPACE, NOTHING);

                if (homeTeam.Contains(nottingHamForestRawDataFormat))
                    homeTeam = homeTeam.Replace(nottingHamForestRawDataFormat, nottingHamForestPreferedFormat);
                if (awayTeam.Contains(nottingHamForestRawDataFormat))
                    awayTeam = awayTeam.Replace(nottingHamForestRawDataFormat, nottingHamForestPreferedFormat);


                if (homeTeam.Contains(middlesBroughRawDataFormat))
                    homeTeam = homeTeam.Replace(middlesBroughRawDataFormat, middlesBroughPreferredFormat);
                if (awayTeam.Contains(middlesBroughRawDataFormat))
                    awayTeam = awayTeam.Replace(middlesBroughRawDataFormat, middlesBroughPreferredFormat);

                file.Write(homeTeam + COMMA);
                file.Write(awayTeam + COMMA);
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

                //***** BETTING *****
                
                //Bet365
                file.Write(soccerData.Bet365HomeWinOdds + COMMA);
                file.Write(soccerData.Bet365DrawOdds + COMMA);
                file.Write(soccerData.Bet365AwayWinOdds + COMMA);

                //Ladbrooks
                if (!soccerData.LadbrooksHomeWinOdds.Equals(NOTHING))
                    file.Write(soccerData.LadbrooksHomeWinOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.LadbrooksDrawOdds.Equals(NOTHING))
                    file.Write(soccerData.LadbrooksDrawOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.LadbrooksAwayWinOdds.Equals(NOTHING))
                    file.Write(soccerData.LadbrooksAwayWinOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                //William Hill
                if(!soccerData.WilliamHillHomeWinOdds.Equals(NOTHING))
                    file.Write(soccerData.WilliamHillHomeWinOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.WilliamHillDrawOdds.Equals(NOTHING))
                    file.Write(soccerData.WilliamHillDrawOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.WilliamHillAwayWinOdds.Equals(NOTHING))
                    file.Write(soccerData.WilliamHillAwayWinOdds + COMMA);
                else
                    file.Write(QUESTION_MARK + COMMA);

                file.WriteLine(soccerData.FullTimeResult);
            }

            file.Close();

        }


        public static void WriteOutSoccerDataToArffFormat_SIMPLEFORMAT_WithQuestionsMarks(List<SoccerData> soccerDataList, List<SoccerDataLeagueScore> soccerDataLeagueScore,
            string outFileName)
        {

            ReadXmlFile();

            //soccerDataList.RemoveAt(HEADER_INDEX);
            if (soccerDataList.Count != soccerDataLeagueScore.Count)
            {
                string stop = "HERE";
            }

            List<string> homeTeams = new List<string>();
            List<string> awayTeams = new List<string>();
            string allHomeTeams = string.Empty;
            string allAwayTeams = string.Empty;
            string newHomeTeam = string.Empty;
            string newAwayTeam = string.Empty;

            homeTeams = ReadXmlFile();
            allHomeTeams = SortAnnexAddBrackets(homeTeams);
            allAwayTeams = allHomeTeams;

            int counter = 0;

            System.IO.StreamWriter file = new System.IO.StreamWriter(outFileName);

            file.WriteLine("% 1. Title: Premiership Data 2013 Season With 3 Bokies And Match Rating\n");
            file.WriteLine("% 2. Source: http://www.football-data.co.uk/data.php");
            file.WriteLine("\n\n");
            file.WriteLine("@relation Premiership2013");

            file.WriteLine("@attribute \'HomeTeam\' " + allHomeTeams);
            file.WriteLine("@attribute \'AwayTeam\' " + allAwayTeams);
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
            
            file.WriteLine("@attribute \'Bet365HomeWinOdds\' numeric");
            file.WriteLine("@attribute \'Bet365DrawOdds\' numeric");
            file.WriteLine("@attribute \'Bet365AwayWinOdds\' numeric");

            file.WriteLine("@attribute \'LadbrooksHomeWinOdds\' numeric");
            file.WriteLine("@attribute \'LadbrooksDrawOdds\' numeric");
            file.WriteLine("@attribute \'LadbrooksAwayWinOdds\' numeric");  

            file.WriteLine("@attribute \'WilliamHillHomeWinOdds\' numeric");
            file.WriteLine("@attribute \'WilliamHillDrawOdds\' numeric");
            file.WriteLine("@attribute \'WilliamHillAwayWinOdds\' numeric");            

            file.WriteLine("@attribute \'MatchRating\' numeric");

            file.WriteLine("@attribute \'HomeWinFairOdds\' numeric");
            file.WriteLine("@attribute \'DrawFairOdds\' numeric");
            file.WriteLine("@attribute \'AwayWinFairOdds\' numeric"); 

            //file.WriteLine("@attribute \'HomeTeamLeagueScoreToThisPoint\' numeric");
            //file.WriteLine("@attribute \'AwayTeamLeagueScoreToThisPoint\' numeric"); 
            //file.WriteLine("@attribute \'LeagueScoreDelta\' numeric"); 
 
            file.WriteLine("@attribute \'class\' { D,H,A }");
            file.WriteLine("@data");

            foreach (SoccerData soccerData in soccerDataList)
            {
                string homeTeam = soccerData.HomeTeam.Replace(SPACE, NOTHING);
                string awayTeam = soccerData.AwayTeam.Replace(SPACE, NOTHING);

                if (homeTeam.Contains(nottingHamForestRawDataFormat))
                    homeTeam = homeTeam.Replace(nottingHamForestRawDataFormat, nottingHamForestPreferedFormat);
                if (awayTeam.Contains(nottingHamForestRawDataFormat))
                    awayTeam = awayTeam.Replace(nottingHamForestRawDataFormat, nottingHamForestPreferedFormat);


                if (homeTeam.Contains(middlesBroughRawDataFormat))
                    homeTeam = homeTeam.Replace(middlesBroughRawDataFormat, middlesBroughPreferredFormat);
                if (awayTeam.Contains(middlesBroughRawDataFormat))
                    awayTeam = awayTeam.Replace(middlesBroughRawDataFormat, middlesBroughPreferredFormat);

                file.Write(homeTeam + COMMA);
                file.Write(awayTeam + COMMA);
                file.Write(QUESTION_MARK + COMMA);
                file.Write(QUESTION_MARK + COMMA);
                file.Write(QUESTION_MARK + COMMA);
                file.Write(QUESTION_MARK + COMMA);

                //file.Write(soccerData.HalfTimeResult + COMMA);
                file.Write(QUESTION_MARK + COMMA);
                file.Write(QUESTION_MARK + COMMA);

                file.Write(QUESTION_MARK + COMMA);
                file.Write(QUESTION_MARK + COMMA);

                file.Write(QUESTION_MARK + COMMA);
                file.Write(QUESTION_MARK + COMMA);

                file.Write(QUESTION_MARK + COMMA);
                file.Write(QUESTION_MARK + COMMA);

                file.Write(QUESTION_MARK + COMMA);
                file.Write(QUESTION_MARK + COMMA);

                file.Write(QUESTION_MARK + COMMA);
                file.Write(QUESTION_MARK + COMMA);

                //***** BETTING *****

                //Bet365
                //file.Write(soccerData.Bet365HomeWinOdds + COMMA);
                file.Write(QUESTION_MARK + COMMA);
                //file.Write(soccerData.Bet365DrawOdds + COMMA);
                file.Write(QUESTION_MARK + COMMA);
                //file.Write(soccerData.Bet365AwayWinOdds + COMMA);
                file.Write(QUESTION_MARK + COMMA);

                //Ladbrooks
                if (!soccerData.LadbrooksHomeWinOdds.Equals(NOTHING))
                {
                    //file.Write(soccerData.LadbrooksHomeWinOdds + COMMA);
                    file.Write(QUESTION_MARK + COMMA);
                }
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.LadbrooksDrawOdds.Equals(NOTHING))
                {
                    //file.Write(soccerData.LadbrooksDrawOdds + COMMA);
                    file.Write(QUESTION_MARK + COMMA);
                }
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.LadbrooksAwayWinOdds.Equals(NOTHING))
                {
                    //file.Write(soccerData.LadbrooksAwayWinOdds + COMMA);
                    file.Write(QUESTION_MARK + COMMA);
                }
                else
                    file.Write(QUESTION_MARK + COMMA);


                //WilliamHill
                if (!soccerData.WilliamHillHomeWinOdds.Equals(NOTHING))
                {
                    //file.Write(soccerData.WilliamHillHomeWinOdds + COMMA);
                    file.Write(QUESTION_MARK + COMMA);
                }
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.WilliamHillDrawOdds.Equals(NOTHING))
                {
                    //file.Write(soccerData.WilliamHillDrawOdds + COMMA);
                    file.Write(QUESTION_MARK + COMMA);
                }
                else
                    file.Write(QUESTION_MARK + COMMA);

                if (!soccerData.WilliamHillAwayWinOdds.Equals(NOTHING))
                {
                    //file.Write(soccerData.WilliamHillAwayWinOdds + COMMA);
                    file.Write(QUESTION_MARK + COMMA);
                }
                else
                    file.Write(QUESTION_MARK + COMMA);

                file.Write(soccerData.MatchRating + COMMA);

                if (!soccerData.MatchRating.Equals(QUESTION_MARK))
                {
                    int matchRating = Int32.Parse(soccerData.MatchRating);

                    soccerData.HomeWinFairOdds  = Utility.GetHomeWinFairOdds(matchRating);
                    soccerData.DrawFairOdds = Utility.GetDrawFairOdds(matchRating);
                    soccerData.AwayWinFairOdds = Utility.GetAwayWinFairOdds(matchRating);

                    file.Write(soccerData.HomeWinFairOdds.ToString() + COMMA);
                    file.Write(soccerData.DrawFairOdds.ToString() + COMMA);
                    file.Write(soccerData.AwayWinFairOdds.ToString() + COMMA); 
                }
                else
                {
                    file.Write(QUESTION_MARK + COMMA);
                    file.Write(QUESTION_MARK + COMMA);
                    file.Write(QUESTION_MARK + COMMA); 
                }

                //QA check
                if (soccerData.HomeTeam != soccerDataLeagueScore[counter].HomeTeam || soccerData.AwayTeam != soccerDataLeagueScore[counter].AwayTeam)
                {
                    string stop1 = "HERE";
                }

                //file.Write(soccerDataLeagueScore[counter].HomeTeamLeagueScore + COMMA);
                //file.Write(soccerDataLeagueScore[counter].AwayTeamLeagueScore + COMMA);
                //file.Write(soccerDataLeagueScore[counter].LeagueScoreDelta + COMMA); 

                file.WriteLine(soccerData.FullTimeResult);
                counter++;
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

                //Betting
                outFile.Write("18:" + soccerData.Bet365HomeWinOdds + SPACE);
                outFile.Write("19:" + soccerData.Bet365DrawOdds + SPACE);
                outFile.Write("20:" + soccerData.Bet365AwayWinOdds + SPACE);
                outFile.Write("21:" + soccerData.WilliamHillHomeWinOdds + SPACE);
                outFile.Write("22:" + soccerData.WilliamHillDrawOdds + SPACE);
                outFile.WriteLine("23:" + soccerData.WilliamHillAwayWinOdds + SPACE);

                //outFile.WriteLine("# " + fixture + " (" +sortedTeamGames.IndexOf(fixture).ToString() +")");                    
            }

            outFile.Close();

        }


        public static string ConvertFullTimeResultToConsistentNumber(string fullTimeResult)
        {
            if (fullTimeResult.Equals(SoccerDataResults.D.ToString()))            
                return ((int)SoccerDataResults.D).ToString();             
            else if (fullTimeResult.Equals(SoccerDataResults.H.ToString()))
                return ((int)SoccerDataResults.H).ToString();
            else if (fullTimeResult.Equals(SoccerDataResults.A.ToString()))
                return ((int)SoccerDataResults.A).ToString();
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

        /// <summary>
        /// Sorts & concatonates an unsorted list of teams and then adds brackets to form arff format
        /// </summary>
        /// <param name="strList">Unsorted list of teams like: ManUnited, Wigan, AstonVilla</param>
        /// <returns>Comma seperated string like: { AstonVilla, ManUnited, Wigan, etc.. }</returns>
        public static string SortAnnexAddBrackets(List<string> strList)
        {
            List<string> sortedList = strList.OrderBy(mc => mc).ToList();       //Sort alpha first
            string concat = String.Join<string>(COMMASPACE, sortedList);        //CSV format            
            return "{ " + concat + " }";                                        //Arff format
        }


        #region League Score Stuff


        /// <summary>
        /// Populates a suitable container with league scores.
        /// Also calulates the league score delta (which is home team league score - away team). The underlying assumption is:
        /// Big delta => high probability of home team win.
        /// 0 delta => high probability of a draw.
        /// Negative delta => high probability of away win.
        /// </summary>
        /// <param name="allSoccerData"></param>
        /// <returns></returns>
        public static List<SoccerDataLeagueScore> GetLeagueScoreForAll(List<SoccerData> allSoccerData)
        {
            List<SoccerData> allSoccerDataOrdered = allSoccerData.OrderBy(mc => mc.Date).ToList();    //Earliest fixture at the top
            List<SoccerDataLeagueScore> soccerDataLeagueScoresList = new List<SoccerDataLeagueScore>();

            List<string> teams = ReadXmlFile(); 

            foreach (SoccerData soccerData in allSoccerDataOrdered)
            {
                string homeTeam = soccerData.HomeTeam;
                string awayTeam = soccerData.AwayTeam;
                DateTime dtFixture = soccerData.Date;  
                string fullTimeResult = soccerData.FullTimeResult;

                SoccerDataLeagueScore soccerDataLeagueScore = new SoccerDataLeagueScore();
                soccerDataLeagueScore.Date = dtFixture;
                soccerDataLeagueScore.HomeTeam = homeTeam;
                soccerDataLeagueScore.AwayTeam = awayTeam;
                soccerDataLeagueScore.FullTimeResult = fullTimeResult;

                if (homeTeam == "ManUnited")
                {
                    string stop = "here";
                }

                //Extract all the fixtures this team was in before this point
                List<SoccerData> homeTeamPreviousFixtures = Utility.GetHomeOrAwayFixturesBeforeSpecifiedPoint(allSoccerDataOrdered, dtFixture, homeTeam, false);
                List<SoccerData> awayTeamPreviousFixtures = Utility.GetHomeOrAwayFixturesBeforeSpecifiedPoint(allSoccerDataOrdered, dtFixture, awayTeam, false);                  

                soccerDataLeagueScore.HomeTeamLeagueScore = GetAccumulatedLeagueScore(homeTeamPreviousFixtures, homeTeam);
                soccerDataLeagueScore.AwayTeamLeagueScore = GetAccumulatedLeagueScore(awayTeamPreviousFixtures, awayTeam);
                soccerDataLeagueScore.LeagueScoreDelta = soccerDataLeagueScore.HomeTeamLeagueScore - soccerDataLeagueScore.AwayTeamLeagueScore;     

                soccerDataLeagueScoresList.Add(soccerDataLeagueScore); 
            }

            return soccerDataLeagueScoresList;

        }


        public static int GetAccumulatedLeagueScore(List<SoccerData> soccerDataList, string teamNameUnderInvestigation)
        {
            int leagueScore = 0;

            if (soccerDataList.Count == ZERO)
                return ZERO;

            foreach (SoccerData soccerData in soccerDataList)
            {
                //Determine if the team was playing home or away...
                string homeTeam = soccerData.HomeTeam;
                string awayTeam = soccerData.AwayTeam;
                string fullTimeResult = soccerData.FullTimeResult; 

                if (fullTimeResult.Equals(SoccerDataResults.D.ToString()))
                {
                    leagueScore += (int)PremierLeagueScoring.DRAW;
                    continue;
                }

                //For that fixture - determine if the team under investiation is playing at home
                bool isPlayingHome = IsTeamPlayingHome(soccerData, teamNameUnderInvestigation);

                if (isPlayingHome & fullTimeResult.Equals(SoccerDataResults.H.ToString()))
                {
                    leagueScore += (int)PremierLeagueScoring.WIN;
                    continue;
                }

                if(!isPlayingHome & fullTimeResult.Equals(SoccerDataResults.A.ToString()))
                    leagueScore += (int)PremierLeagueScoring.WIN;

            }

            return leagueScore;
        }

        /// <summary>
        /// For a given fixture and a specified team determines if the team is playing at home or not.
        /// </summary>
        /// <param name="soccerData"></param>
        /// <param name="teamNameUnderInvestigation"></param>
        /// <returns></returns>
        private static bool IsTeamPlayingHome(SoccerData soccerData, string teamNameUnderInvestigation)
        {
            string homeTeam = soccerData.HomeTeam;
  
            if(homeTeam.Equals(teamNameUnderInvestigation))
                return true;
            else
                return false;
        }


        #endregion



        #region Goal Superiority Stuff


        /// <summary>
        /// For an entire ensemble calculates the "recent-form" goal superiorty rating (goals scored - goals conceded)
        /// Recent form of a team is considered to be the last 6 games (home/away) assuming the data is available of course...
        /// Both teams *must* have played the "recent-form" amount of fixtures. Otherwise, it cannot be calculated... 
        /// The underlying assumption is: Teams that score more goals than they concede over several previous matches 
        /// are more likely to win their next game... 
        /// </summary>
        /// <param name="fileName">Full path to the file</param>
        /// <returns></returns>
        public static List<SoccerData> GetFixtureMatchRatingForAll(string fileName)
        {
            bool includeHeader = false;
            List<SoccerData> allSoccerData = Utility.ReadSoccerDataFromFile(fileName, includeHeader);
            List<SoccerData> allSoccerDataOrdered = allSoccerData.OrderBy(mc => mc.Date).ToList();    //Earliest fixture at the top


            foreach (SoccerData soccerData in allSoccerDataOrdered)
            {
                string homeTeam = soccerData.HomeTeam;
                string awayTeam = soccerData.AwayTeam; 
                DateTime dtFixture = soccerData.Date;
 
                //For the home team obtain all the fixtures before this point (max of 6 returned)
                List<SoccerData> allHomeTeamFixturesBeforePoint = GetHomeOrAwayFixturesBeforeSpecifiedPoint(allSoccerData, dtFixture, homeTeam, true);
                List<SoccerData> allAwayTeamFixturesBeforePoint = GetHomeOrAwayFixturesBeforeSpecifiedPoint(allSoccerData, dtFixture, awayTeam, true);

                string homeTeamGoalSuperiority = string.Empty;
                string awayTeamGoalSuperiority = string.Empty;

                if(allHomeTeamFixturesBeforePoint.Count < RECENT_FORM)
                {
                    //Cannot be calculated as its less that 6 matches ie "recent form" is not yet reached
                    homeTeamGoalSuperiority = QUESTION_MARK; 
                }
                else
                    homeTeamGoalSuperiority = GetGoalSuperiorityRating(allHomeTeamFixturesBeforePoint, homeTeam).ToString();

                

                if (allAwayTeamFixturesBeforePoint.Count < RECENT_FORM)
                {
                    //Cannot be calculated as its less that 6 matches ie "recent form" is not yet reached
                    awayTeamGoalSuperiority = QUESTION_MARK;
                }
                else
                    awayTeamGoalSuperiority = GetGoalSuperiorityRating(allAwayTeamFixturesBeforePoint, awayTeam).ToString();

                if (homeTeamGoalSuperiority.Equals(QUESTION_MARK) || awayTeamGoalSuperiority.Equals(QUESTION_MARK))
                    soccerData.MatchRating = QUESTION_MARK;
                else
                {
                    int temp = Int32.Parse(homeTeamGoalSuperiority) - Int32.Parse(awayTeamGoalSuperiority);
                    soccerData.MatchRating = temp.ToString();
                }
            }

            return allSoccerDataOrdered;
        }

        /// <summary>
        /// For an entire ensemble calculates the "recent-form" goal superiorty rating (goals scored - goals conceded)
        /// Recent form of a team is considered to be the last 6 games (home/away) assuming the data is available of course...
        /// The underlying assumption is: Teams that score more goals than they concede over several previous matches 
        /// are more likely to win their next game... 
        /// </summary>
        /// <param name="fileName">List soccer data</param>
        /// <returns></returns>
        public static List<SoccerData> GetFixtureMatchRatingForAll(List<SoccerData> soccerDataList)
        {
            bool includeHeader = false;
            List<SoccerData> allSoccerDataOrdered = soccerDataList.OrderBy(mc => mc.Date).ToList();    //Earliest fixture at the top

            foreach (SoccerData soccerData in allSoccerDataOrdered)
            {
                string homeTeam = soccerData.HomeTeam;
                string awayTeam = soccerData.AwayTeam;
                DateTime dtFixture = soccerData.Date;

                //For the home team obtain all the fixtures before this point
                List<SoccerData> allHomeTeamFixturesBeforePoint = GetHomeOrAwayFixturesBeforeSpecifiedPoint(soccerDataList, dtFixture, homeTeam, true);
                int homeTeamGoalSuperiority = GetGoalSuperiorityRating(allHomeTeamFixturesBeforePoint, homeTeam);

                List<SoccerData> allAwayTeamFixturesBeforePoint = GetHomeOrAwayFixturesBeforeSpecifiedPoint(soccerDataList, dtFixture, awayTeam, true);
                int awayTeamGoalSuperiority = GetGoalSuperiorityRating(allAwayTeamFixturesBeforePoint, awayTeam);

                soccerData.MatchRating = (homeTeamGoalSuperiority - awayTeamGoalSuperiority).ToString();
            }

            return allSoccerDataOrdered;
        }


        /// <summary>
        /// Returns a maximum of recent-form amount of the home/away fixtures before or at a certain time period for a particular team.
        /// </summary>
        /// <param name="soccerDataOrderedList"></param>
        /// <param name="dt">The datetime before or at which you want the fixtures</param>
        /// <param name="teamName">The team name (like Man United) whos fixtures you want (home or away)</param>
        /// <returns></returns>
        private static List<SoccerData> GetHomeOrAwayFixturesBeforeSpecifiedPoint(List<SoccerData> soccerDataOrderedList, DateTime dt, string teamName, bool returnRecentFormAmount)
        {
            List<SoccerData> earlierSoccerData = new List<SoccerData>();

            //Get all fixture before the specified time period
            foreach (SoccerData soccerData in soccerDataOrderedList)
            {
                DateTime dateTime = soccerData.Date;

                if (Compare2Dates(dateTime, dt))
                {
                    if(soccerData.HomeTeam == teamName || soccerData.AwayTeam == teamName)  
                        earlierSoccerData.Add(soccerData);
                }
            }

            
            if (returnRecentFormAmount & earlierSoccerData.Count > RECENT_FORM)            
                return SortAndTakeRecentFormAmount(earlierSoccerData);            
            else
                return earlierSoccerData; 
        }

        /// <summary>
        /// Sorts by most recent at the top & least recent at the bottom & takes the top recent-form amount (which is usually 6)
        /// </summary>
        /// <param name="soccerData"></param>
        /// <returns></returns>
        private static List<SoccerData> SortAndTakeRecentFormAmount(List<SoccerData> soccerData)
        {
            List<SoccerData> sortedSocccerData = soccerData.OrderByDescending(mc => mc.Date).ToList();
            return sortedSocccerData.Take(RECENT_FORM).ToList();  
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="soccerDataList"></param>
        /// <param name="teamName"></param>
        /// <returns></returns>
        private static int GetGoalSuperiorityRating(List<SoccerData> soccerDataList, string teamName)
        {
            int goalsConceded = 0;
            int goalsScored = 0;

            foreach (SoccerData soccerData in soccerDataList)
            {
                if (soccerData.HomeTeam == teamName)
                {
                    goalsScored += Convert.ToInt16(soccerData.FullTimeHomeTeamGoals);
                    goalsConceded += Convert.ToInt16(soccerData.FullTimeAwayTeamGoals);  

                }
                else if (soccerData.AwayTeam == teamName)
                {
                    goalsScored += Convert.ToInt16(soccerData.FullTimeAwayTeamGoals);
                    goalsConceded += Convert.ToInt16(soccerData.FullTimeHomeTeamGoals);  
                }
                else
                {
                    //Logger error
                }
            }

            return goalsScored - goalsConceded;
        }

        /// <summary>
        /// Compare 2 time-stamps and return true if the second time stamp is before the first
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        private static bool Compare2Dates(DateTime dateTime1, DateTime dateTime2)
        {
            int result = DateTime.Compare(dateTime1, dateTime2);

            if (result < 0)
                return true;
            else
                return false;
        }



        /// <summary>
        /// Converts a string timestamp of the form: DD/MM/YYYY to the DateTime equivalent
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string strDateTime)
        {
            DateTime dt = new DateTime();

            try
            {
                dt = Convert.ToDateTime(strDateTime);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);  
            }

            return dt;

        }

        #endregion


        internal static List<SoccerData> AddGoalSuperiority(List<SoccerData> soccerData)
        {
            List<SoccerData> soccerDataPlusGoalSuperiority = new List<SoccerData>(); 

            //Extract the unique years contains in the data
            List<int> distinctYearList = soccerData.Select(mc => mc.Date.Year).Distinct().ToList();
            var orderedDistinctYearList = distinctYearList.OrderByDescending(mc => mc).ToList();        //Most recent at list-top

            //Cycle thru these seasons (years) and extract all fixtures for that year
            //Then append the goals superiority for that particular season (year)
            //Then add this data to the return object.
            foreach (int year in orderedDistinctYearList)
            {
                List<SoccerData> yearSoccerData = soccerData.Where(mc => mc.Date.Year == year).ToList();
                List<SoccerData> soccerDataPlusGS = GetFixtureMatchRatingForAll(yearSoccerData);
                soccerDataPlusGoalSuperiority.AddRange(soccerDataPlusGS);   
            }

            return soccerDataPlusGoalSuperiority;             
        }


        //Tuned for minimal output and match rating statistic
        public static void WriteMatchRatingOutToFile(List<SoccerData> soccerDataList, string outFilePathAndName)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(outFilePathAndName);
            
            file.WriteLine(HEADER1);

            foreach (SoccerData soccerData in soccerDataList)
            {
                file.Write(soccerData.Date.ToShortDateString());
                file.Write(COMMA);
                file.Write(soccerData.HomeTeam);
                file.Write(COMMA);
                file.Write(soccerData.AwayTeam);
                file.Write(COMMA);
                file.Write(soccerData.FullTimeHomeTeamGoals);
                file.Write(COMMA);
                file.Write(soccerData.FullTimeAwayTeamGoals);
                file.Write(COMMA);
                file.Write(soccerData.MatchRating.ToString());  
                file.Write(COMMA);
                file.WriteLine(soccerData.FullTimeResult);                             
            }

            file.Close();

        }


        public static double GetHomeWinFairOdds(int matchRating)
        {
            double dblMatchRating = (double)matchRating; 
            double ans = (1.56 * matchRating) + 46.47;

            return Math.Round((ONE_HUNDRED / ans), TWO);
        }


        public static double GetDrawFairOdds(int matchRating)
        {
            double dblMatchRating = (double) matchRating;
            double ans =  -(0.03 * (dblMatchRating * dblMatchRating)) - (0.29 * dblMatchRating) + 29.48;

            return Math.Round((ONE_HUNDRED / ans), TWO);
        }


        public static double GetAwayWinFairOdds(int matchRating)
        {
            double dblMatchRating = (double) matchRating;
            double ans =  (0.03 * (dblMatchRating * dblMatchRating)) - (1.27 * matchRating) + 23.65;

            return Math.Round((ONE_HUNDRED / ans), TWO);
        }




        /// <summary>
        /// For a given home & away teams - obtains how many times that they encountered & the result of that encounter
        /// </summary>
        /// <param name="pathToRawData"></param>
        /// <param name="homeTeam"></param>
        /// <param name="awayTeam"></param>
        /// <returns>Dictionary of fixture encounter number & fixture result</returns>
        public static Dictionary<int, string> ExtractInfoOnPreviousExactEncountersFromTrainingData(string pathToRawData, string homeTeam, string awayTeam)
        {
            Dictionary<int, string> previousFixtureResults = new Dictionary<int, string>();

            bool includeHeader = false;
            List<SoccerData> allSoccerData = Utility.ReadSoccerDataFromFile(pathToRawData, includeHeader);

            //Extact the home-team subset from all the data
            List<SoccerData> homeTeamSubset = allSoccerData.FindAll(mc => mc.HomeTeam.Replace(SPACE, NOTHING) == homeTeam);

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

    }
}
