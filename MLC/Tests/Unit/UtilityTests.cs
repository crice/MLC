using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace MLC.Tests.Unit
{
    [TestFixture]
    public class UtilityTests
    {
        private const string COMMA = ", ";

        [SetUp]
        public void Init()
        {

        }


        [Test]
        public void Utility_GetFixtureMatchRatingForAll()
        {            
            //Assign
            string PREM_13_DATA = @"../../lib/Premiership13.txt";

            //Act
            List<SoccerData> soccerDataList = Utility.GetFixtureMatchRatingForAll(PREM_13_DATA);  

            //Assert
            foreach(SoccerData soccerData in soccerDataList)
            {
                Assert.IsNotNull(soccerData.MatchRating);  
            }

            //Write-out
            PrintOutToFile(soccerDataList); 
        }


        private void PrintOutToFile(List<SoccerData> soccerDataList)
        {
            string fileOut = @"../../lib/SoccerDataMinusMatchRating.txt";

            System.IO.StreamWriter file = new System.IO.StreamWriter(fileOut);

            file.WriteLine("Date, HomeTeam, AwayTeam, FTHomeTeamGoals, FTAwayTeamGoals, FTResult, FixtureMatchRating");

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
                file.WriteLine(soccerData.FullTimeResult);
                //file.Write(COMMA);
                //file.WriteLine(soccerData.MatchRating.ToString());                 
            }

            file.Close();

        }

    }

}
