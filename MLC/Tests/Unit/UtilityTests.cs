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


        [Test]
        public void Utility_GetHomeWinProbability_SuccessExpected()
        {
            //Assign
            int matchRating1 = 10;
            double expectedResult1 = 1.61d;

            int matchRating2 = -7;
            double expectedResult2 = 2.81d;

            int matchRating3 = 0;
            double expectedResult3 = 2.15d;

            int matchRating4 = 1;
            double expectedResult4 = 2.08d;

            //Act
            double actualResult1 = Utility.GetHomeWinFairOdds(matchRating1);
            double actualResult2 = Utility.GetHomeWinFairOdds(matchRating2);
            double actualResult3 = Utility.GetHomeWinFairOdds(matchRating3);
            double actualResult4 = Utility.GetHomeWinFairOdds(matchRating4); 
            
            //Assert
            Assert.AreEqual(expectedResult1, actualResult1);
            Assert.AreEqual(expectedResult2, actualResult2);
            Assert.AreEqual(expectedResult3, actualResult3);
            Assert.AreEqual(expectedResult4, actualResult4); 
        }


        [Test]
        public void Utility_GetDrawProbability()
        {
            //Assign
            int matchRating1 = 10;
            double expectedResult1 = 4.24d;

            int matchRating2 = -7;
            double expectedResult2 = 3.33d;

            int matchRating3 = 0;
            double expectedResult3 = 3.39d;

            int matchRating4 = 1;
            double expectedResult4 = 3.43d;

            //Act
            double actualResult1 = Utility.GetDrawFairOdds(matchRating1);
            double actualResult2 = Utility.GetDrawFairOdds(matchRating2);
            double actualResult3 = Utility.GetDrawFairOdds(matchRating3);
            double actualResult4 = Utility.GetDrawFairOdds(matchRating4);

            //Assert
            Assert.AreEqual(expectedResult1, actualResult1);
            Assert.AreEqual(expectedResult2, actualResult2);
            Assert.AreEqual(expectedResult3, actualResult3);
            Assert.AreEqual(expectedResult4, actualResult4); 

        }


        [Test]
        public void Utility_GetAwayWinProbability_SuccessExpected()
        {
            //Assign
            int matchRating1 = 10;
            double expectedResult1 = 7.17d;

            int matchRating2 = -8;
            double expectedResult2 = 2.80d;

            int matchRating3 = 0;
            double expectedResult3 = 4.23d;

            int matchRating4 = 1;
            double expectedResult4 = 4.46d;

            //Act
            double actualResult1 = Utility.GetAwayWinFairOdds(matchRating1);
            double actualResult2 = Utility.GetAwayWinFairOdds(matchRating2);
            double actualResult3 = Utility.GetAwayWinFairOdds(matchRating3);
            double actualResult4 = Utility.GetAwayWinFairOdds(matchRating4);

            //Assert
            Assert.AreEqual(expectedResult1, actualResult1);
            Assert.AreEqual(expectedResult2, actualResult2);
            Assert.AreEqual(expectedResult3, actualResult3);
            Assert.AreEqual(expectedResult4, actualResult4); 

        }

    }

}
