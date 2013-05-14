using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLC
{
    /// <summary>
    /// Models the data from here:
    /// http://www.football-data.co.uk/data.php
    /// </summary>
    public class SoccerData
    {

        public enum SoccerDataPosition
        {
            Division = 0,
            Date= 1,
            HomeTeam = 2,
            AwayTeam = 3,
            FullTimeHomeTeamGoals = 4,
            FullTimeAwayTeamGoals = 5,
            FullTimeResult = 6,
            HalfTimeHomeTeamGoals = 7,
            HalfTimeAwayTeamGoals = 8,
            HalfTimeResult = 9,
            homeTeamShots = 10,              
            awayTeamShots = 11,
            homeTeamShotsOnTarget = 12,
            awayTeamShotsOnTarget = 13,
            homeTeamFoulsCommitted = 14,
            awayTeamFoulsCommitted = 15,
            homeTeamCorners = 16,
            awayTeamCorners = 17,
            homeTeamYellowCards = 18,
            awayTeamYellowCards = 19,
            homeTeamRedCards = 20,
            awayTeamRedCards = 21
        }

        //HomeTeam,AwayTeam,FTHomeTeamGoals,FTAwayTeamGoals,HomeTimeHomeTeamGoals,HalfTimeAwayTeamGoals,HalfTimeTResult,HomeTeamShots,AwayTeamShots,HomeTeamShotsonTarget,AwayTeamShotsonTarget,HomeTeamFoulsCommitted,AwayTeamFoulsCommitted,HomeTeamCorners,AwayTeamCorners,HometeamYellowCards,AwayTeamYellowCards,HomeTeamRedCards,AwayTeamRedCards,FTResult

        private string _division;                   //Div
        private string _date;                       //Date
        private string _homeTeam;                   //HomeTeam
        private string _awayTeam;                   //AwayTeam
        private string _fullTimeHomeTeamGoals;      //FTHG
        private string _fullTimeAwayTeamGoals;      //FTAG
        private string _fullTimeResult;             //FTR
        private string _halfTimeHomeTeamGoals;      //HTHG
        private string _halfTimeAwayTeamGoals;      //HTAG
        private string _halfTimeResult;             //HTR
        private string _homeTeamShots;              //HS
        private string _awayTeamShots;              //AS
        private string _homeTeamShotsOnTarget;      //HST
        private string _awayTeamShotsOnTarget;      //AST
        private string _homeTeamFoulsCommitted;     //HF
        private string _awayTeamFoulsCommitted;     //AF
        private string _homeTeamCorners;            //HC
        private string _awayTeamCorners;            //AC
        private string _homeTeamYellowCards;        //HY
        private string _awayTeamYellowCards;        //AY
        private string _homeTeamRedCards;           //HR
        private string _awayTeamRedCards;           //AR


        #region Non-Statistical Properties

        public string Division
        {
            get { return _division; }
            set { _division = value; }
        }
        
        public string Date
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
        

        public string FullTimeHomeTeamGoals
        {
            get { return _fullTimeHomeTeamGoals; }
            set { _fullTimeHomeTeamGoals = value; }
        }
        

        public string FullTimeAwayTeamGoals
        {
            get { return _fullTimeAwayTeamGoals; }
            set { _fullTimeAwayTeamGoals = value; }
        }
        

        public string FullTimeResult
        {
            get { return _fullTimeResult; }
            set { _fullTimeResult = value; }
        }
       

        public string HalfTimeHomeTeamGoals
        {
            get { return _halfTimeHomeTeamGoals; }
            set { _halfTimeHomeTeamGoals = value; }
        }
        

        public string HalfTimeAwayTeamGoals
        {
            get { return _halfTimeAwayTeamGoals; }
            set { _halfTimeAwayTeamGoals = value; }
        }
        

        public string HalfTimeResult
        {
            get { return _halfTimeResult; }
            set { _halfTimeResult = value; }
        }
        

        public string HomeTeamShots
        {
            get { return _homeTeamShots; }
            set { _homeTeamShots = value; }
        }
        

        public string AwayTeamShots
        {
            get { return _awayTeamShots; }
            set { _awayTeamShots = value; }
        }
        

        public string HomeTeamShotsOnTarget
        {
            get { return _homeTeamShotsOnTarget; }
            set { _homeTeamShotsOnTarget = value; }
        }
        

        public string AwayTeamShotsOnTarget
        {
            get { return _awayTeamShotsOnTarget; }
            set { _awayTeamShotsOnTarget = value; }
        }
        

        public string HomeTeamFoulsCommitted
        {
            get { return _homeTeamFoulsCommitted; }
            set { _homeTeamFoulsCommitted = value; }
        }
        

        public string AwayTeamFoulsCommitted
        {
            get { return _awayTeamFoulsCommitted; }
            set { _awayTeamFoulsCommitted = value; }
        }
        

        public string HomeTeamCorners
        {
            get { return _homeTeamCorners; }
            set { _homeTeamCorners = value; }
        }
        

        public string AwayTeamCorners
        {
            get { return _awayTeamCorners; }
            set { _awayTeamCorners = value; }
        }
       

        public string HomeTeamYellowCards
        {
            get { return _homeTeamYellowCards; }
            set { _homeTeamYellowCards = value; }
        }
        

        public string AwayTeamYellowCards
        {
            get { return _awayTeamYellowCards; }
            set { _awayTeamYellowCards = value; }
        }
        

        public string HomeTeamRedCards
        {
            get { return _homeTeamRedCards; }
            set { _homeTeamRedCards = value; }
        }
        

        public string AwayTeamRedCards
        {
            get { return _awayTeamRedCards; }
            set { _awayTeamRedCards = value; }
        }

        #endregion

    }
}
