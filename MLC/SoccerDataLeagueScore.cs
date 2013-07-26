using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MLC
{
    public class SoccerDataLeagueScore : IEnumerable
    {

        

        private DateTime _date;                                 //Date
        private string _homeTeam;                               //HomeTeam
        private string _awayTeam;                               //AwayTeam
        private string _fullTimeResult;                          //Full time result
        private int _homeTeamLeagueScoreToThisPoint;            //Home team league score for that date
        private int _awayTeamLeagueScoreToThisPoint;            //Away team league score for that date


        #region Properties


        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

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

        public string FullTimeResult
        {
            get { return _fullTimeResult; }
            set { _fullTimeResult = value; }
        }

        public int HomeTeamLeagueScore
        {
            get { return _homeTeamLeagueScoreToThisPoint; }
            set { _homeTeamLeagueScoreToThisPoint = value; }
        }

        public int AwayTeamLeagueScore
        {
            get { return _awayTeamLeagueScoreToThisPoint; }
            set { _awayTeamLeagueScoreToThisPoint = value; }
        }


        #endregion


        #region Ignore
        public IEnumerator GetEnumerator() { return GetEnumerator(); }
        #endregion

    }
}
