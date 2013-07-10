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
        }

    }

}
