using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLC
{
    public class SoccerDataLeagueScore
    {

        private DateTime _date;                     //Date
        private string _homeTeam;                   //HomeTeam
        private string _awayTeam;                   //AwayTeam
        private int _homeTeamLeagueScore;           //Home team league score for that date
        private int _awayTeamLeagueScore;           //Away team league score for that date


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

        public int HomeTeamLeagueScore
        {
            get { return _homeTeamLeagueScore; }
            set { _homeTeamLeagueScore = value; }
        }

        public int AwayTeamLeagueScore
        {
            get { return _awayTeamLeagueScore; }
            set { _awayTeamLeagueScore = value; }
        }


        #endregion

    }
}
