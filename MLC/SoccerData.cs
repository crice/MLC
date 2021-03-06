﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MLC
{
    /// <summary>
    /// Models the data from here:
    /// http://www.football-data.co.uk/data.php
    /// </summary>
    public class SoccerData : IEnumerable
    {
        public IEnumerator GetEnumerator() { return GetEnumerator(); }

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
            HomeTeamShots = 10,              
            AwayTeamShots = 11,
            HomeTeamShotsOnTarget = 12,
            AwayTeamShotsOnTarget = 13,
            HomeTeamFoulsCommitted = 14,
            AwayTeamFoulsCommitted = 15,
            HomeTeamCorners = 16,
            AwayTeamCorners = 17,
            HomeTeamYellowCards = 18,
            AwayTeamYellowCards = 19,
            HomeTeamRedCards = 20,
            AwayTeamRedCards = 21,
            Bet365HomeWinOdds = 22,
            Bet365DrawOdds = 23,
            Bet365AwayWinOdds = 24,
            LadbrooksHomeWinOdds = 34,
            LadbrooksDrawOdds = 35,
            LadbrooksAwayWinOdds = 36,
            WilliamHillHomeWinOdds = 40,
            WilliamHillDrawOdds = 41,
            WilliamHillAwayWinOdds = 42
        }

        public enum SoccerDataPositionPre2012WithReferee
        {
            Division = 0,
            Date = 1,
            HomeTeam = 2,
            AwayTeam = 3,
            FullTimeHomeTeamGoals = 4,
            FullTimeAwayTeamGoals = 5,
            FullTimeResult = 6,
            HalfTimeHomeTeamGoals = 7,
            HalfTimeAwayTeamGoals = 8,
            HalfTimeResult = 9,
            Referee = 10,
            HomeTeamShots = 11,
            AwayTeamShots = 12,
            HomeTeamShotsOnTarget = 13,
            AwayTeamShotsOnTarget = 14,
            HomeTeamFoulsCommitted = 15,
            AwayTeamFoulsCommitted = 16,
            HomeTeamCorners = 17,
            AwayTeamCorners = 18,
            HomeTeamYellowCards = 19,
            AwayTeamYellowCards = 20,
            HomeTeamRedCards = 21,
            AwayTeamRedCards = 22,
            Bet365HomeWinOdds = 23,
            Bet365DrawOdds = 24,
            Bet365AwayWinOdds = 25,
            LadbrooksHomeWinOdds = 35,
            LadbrooksDrawOdds = 36,
            LadbrooksAwayWinOdds = 37,
            WilliamHillHomeWinOdds = 41,
            WilliamHillDrawOdds = 42,
            WilliamHillAwayWinOdds = 43
        }

        //HomeTeam,AwayTeam,FTHomeTeamGoals,FTAwayTeamGoals,HomeTimeHomeTeamGoals,HalfTimeAwayTeamGoals,HalfTimeTResult,HomeTeamShots,AwayTeamShots,HomeTeamShotsonTarget,AwayTeamShotsonTarget,HomeTeamFoulsCommitted,AwayTeamFoulsCommitted,HomeTeamCorners,AwayTeamCorners,HometeamYellowCards,AwayTeamYellowCards,HomeTeamRedCards,AwayTeamRedCards,FTResult
        //Div,Date,HomeTeam,AwayTeam,FTHG,FTAG,FTR,HTHG,HTAG,HTR,Referee,HS,AS,HST,AST,HF,AF,HC,AC,HY,AY,HR,AR

        private string _division;                   //Div
        private DateTime _date;                     //Date
        private string _homeTeam;                   //HomeTeam
        private string _awayTeam;                   //AwayTeam
        private string _fullTimeHomeTeamGoals;      //FTHG
        private string _fullTimeAwayTeamGoals;      //FTAG
        private string _fullTimeResult;             //FTR
        private string _halfTimeHomeTeamGoals;      //HTHG
        private string _halfTimeAwayTeamGoals;      //HTAG
        private string _halfTimeResult;             //HTR
        private string _referee;                    //Referee  <-- only present in pre-2012 seasons
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

        private string _bet365HomeWinOdds;          //B365H
        private string _bet365DrawOdds;             //B365D
        private string _bet365AwayWinOdds;          //B365A

        private string _ladbrooksHomeWinOdds;       //LBH
        private string _ladbrooksDrawOdds;          //LBD
        private string _ladbrooksAwayWinOdds;       //LBA

        private string _williamHillHomeWinOdds;     //WHH
        private string _williamHillDrawOdds;        //WHO
        private string _williamHillAwayWinOdds;     //WHA

        //Derived metrics
        private int _homeTeamRecentFormGoalsConceded;       //Recent form defined as the last 6 matches 
        private int _awayTeamRecentFormGoalsScored;         //Recent form defined as the last 6 matches 
        private string _matchRating;                        //Fixture goal superiority match rating

        private double _homeWinFairOdds;                    //From match rating & a paper
        private double _drawFairOdds;                       //From match rating & a paper
        private double _awayWinFairOdds;                    //From match rating & a paper

        Dictionary<string, int> _leagueScores = new Dictionary<string, int>();

        //Home team stats
        private int _homeTeamPeviousMatchesPlayed;
        private int _homeTeamPreviousHomeWins;
        private int _homeTeamPreviousHomeDraws;
        private int _homeTeamPreviousHomeLosses;
        private int _homeTeamPreviousHomeGoalsScored;
        private int _homeTeamPreviousHomeGoalsConceded;
        private int _homeTeamPreviousAwayWins;
        private int _homeTeamPreviousAwayDraws;
        private int _homeTeamPreviousAwayLosses;
        private int _homeTeamPreviousAwayGoalsScored;
        private int _homeTeamPreviousAwayGoalsConceded;
        private int _homeTeamRecentFormPoints;

        //Away team stats
        private int _awayTeamPreviousMatchesPlayed;
        private int _awayTeamPreviousHomeWins;
        private int _awayTeamPreviousHomeDraws;
        private int _awayTeamPreviousHomeLosses;
        private int _awayTeamPreviousHomeGoalsScored;
        private int _awayTeamPreviousHomeGoalsConceded;
        private int _awayTeamPreviousAwayWins;
        private int _awayTeamPreviousAwayDraws;
        private int _awayTeamPreviousAwayLosses;
        private int _awayTeamPreviousAwayGoalsConceded;
        private int _awayTeamPreviousAwayGoalsScored;               
        private int _awayTeamRecentformPoints;



        #region Non-Statistical Properties

        public string Division
        {
            get { return _division; }
            set { _division = value; }
        }
        
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


        public string Referee
        {
            get { return _referee; }
            set { _referee = value; }
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


        #region Statistical Properties

        public string Bet365HomeWinOdds
        {
            get { return _bet365HomeWinOdds; }
            set { _bet365HomeWinOdds = value; }
        }

        public string Bet365DrawOdds
        {
            get { return _bet365DrawOdds; }
            set { _bet365DrawOdds = value; }
        }

        public string Bet365AwayWinOdds
        {
            get { return _bet365AwayWinOdds; }
            set { _bet365AwayWinOdds = value; }
        }

        public string LadbrooksHomeWinOdds
        {
            get { return _ladbrooksHomeWinOdds; }
            set { _ladbrooksHomeWinOdds = value; }
        }

        public string LadbrooksAwayWinOdds
        {
            get { return _ladbrooksAwayWinOdds; }
            set { _ladbrooksAwayWinOdds = value; }
        }

        public string LadbrooksDrawOdds
        {
            get { return _ladbrooksDrawOdds; }
            set { _ladbrooksDrawOdds = value; }
        }

        public string WilliamHillHomeWinOdds
        {
            get { return _williamHillHomeWinOdds; }
            set { _williamHillHomeWinOdds = value; }
        }

        public string WilliamHillDrawOdds
        {
            get { return _williamHillDrawOdds; }
            set { _williamHillDrawOdds = value; }
        }

        public string WilliamHillAwayWinOdds
        {
            get { return _williamHillAwayWinOdds; }
            set { _williamHillAwayWinOdds = value; }
        }

        #endregion


        #region Derived Properties

        public int AwayTeamRecentFormGoalsScored
        {
            get { return _awayTeamRecentFormGoalsScored; }
            set { _awayTeamRecentFormGoalsScored = value; }
        }

        public int HomeTeamRecentFormGoalsConceded
        {
            get { return _homeTeamRecentFormGoalsConceded; }
            set { _homeTeamRecentFormGoalsConceded = value; }
        }

        public string MatchRating
        {
            get { return _matchRating; }
            set { _matchRating = value; }
        }

        public double HomeWinFairOdds
        {
            get { return _homeWinFairOdds; }
            set { _homeWinFairOdds = value; }
        }

        public double DrawFairOdds
        {
            get { return _drawFairOdds; }
            set { _drawFairOdds = value; }
        }

        public double AwayWinFairOdds
        {
            get { return _awayWinFairOdds; }
            set { _awayWinFairOdds = value; }
        }

        public Dictionary<string, int> LeagueScores
        {
            get { return _leagueScores; }
            set { _leagueScores = value; }
        }  

        #endregion


        #region New Statistics - Home Team


        public int HomeTeamPeviousMatchesPlayed
        {
            get { return _homeTeamPeviousMatchesPlayed; }
            set { _homeTeamPeviousMatchesPlayed = value; }
        }

        public int HomeTeamPreviousHomeWins
        {
            get { return _homeTeamPreviousHomeWins; }
            set { _homeTeamPreviousHomeWins = value; }
        }

        public int HomeTeamPreviousHomeDraws
        {
            get { return _homeTeamPreviousHomeDraws; }
            set { _homeTeamPreviousHomeDraws = value; }
        }

        public int HomeTeamPreviousHomeLosses
        {
            get { return _homeTeamPreviousHomeLosses; }
            set { _homeTeamPreviousHomeLosses = value; }
        }

        public int HomeTeamPreviousHomeGoalsScored
        {
            get { return _homeTeamPreviousHomeGoalsScored; }
            set { _homeTeamPreviousHomeGoalsScored = value; }
        }

        public int HomeTeamPreviousHomeGoalsConceded
        {
            get { return _homeTeamPreviousHomeGoalsConceded; }
            set { _homeTeamPreviousHomeGoalsConceded = value; }
        }

        public int HomeTeamPreviousAwayWins
        {
            get { return _homeTeamPreviousAwayWins; }
            set { _homeTeamPreviousAwayWins = value; }
        }

        public int HomeTeamPreviousAwayDraws
        {
            get { return _awayTeamPreviousHomeDraws; }
            set { _awayTeamPreviousHomeDraws = value; }
        }

        public int HomeTeamPreviousAwayLosses
        {
            get { return _homeTeamPreviousAwayLosses; }
            set { _homeTeamPreviousAwayLosses = value; }
        }

        public int HomeTeamPreviousAwayGoalsScored
        {
            get { return _homeTeamPreviousAwayGoalsScored; }
            set { _homeTeamPreviousAwayGoalsScored = value; }
        }

        public int HomeTeamPreviousAwayGoalsConceded
        {
            get { return _homeTeamPreviousAwayGoalsConceded; }
            set { _homeTeamPreviousAwayGoalsConceded = value; }
        }

        #endregion


        #region New Statistics - Away Team


        public int AwayTeamPreviousMatchesPlayed
        {
            get { return _awayTeamPreviousMatchesPlayed; }
            set { _awayTeamPreviousMatchesPlayed = value; }
        }

        public int AwayTeamPreviousHomeWins
        {
            get { return _awayTeamPreviousHomeWins; }
            set { _awayTeamPreviousHomeWins = value; }
        }

        public int AwayTeamPreviousHomeDraws
        {
            get { return _awayTeamPreviousHomeDraws; }
            set { _awayTeamPreviousHomeDraws = value; }
        }

        public int AwayTeamPreviousHomeLosses
        {
            get { return _awayTeamPreviousHomeLosses; }
            set { _awayTeamPreviousHomeLosses = value; }
        }

        public int AwayTeamPreviousHomeGoalsScored
        {
            get { return _awayTeamPreviousHomeGoalsScored; }
            set { _awayTeamPreviousHomeGoalsScored = value; }
        }

        public int AwayTeamPreviousHomeGoalsConceded
        {
            get { return _awayTeamPreviousHomeGoalsConceded; }
            set { _awayTeamPreviousHomeGoalsConceded = value; }
        }

        public int AwayTeamPreviousAwayWins
        {
            get { return _awayTeamPreviousAwayWins; }
            set { _awayTeamPreviousAwayWins = value; }
        }

        public int AwayTeamPreviousAwayDraws
        {
            get { return _awayTeamPreviousAwayDraws; }
            set { _awayTeamPreviousAwayDraws = value; }
        }


        public int AwayTeamPreviousAwayLosses
        {
            get { return _awayTeamPreviousAwayLosses; }
            set { _awayTeamPreviousAwayLosses = value; }
        }


        public int AwayTeamPreviousAwayGoalsConceded
        {
            get { return _awayTeamPreviousAwayGoalsConceded; }
            set { _awayTeamPreviousAwayGoalsConceded = value; }
        }

        public int AwayTeamPreviousAwayGoalsScored
        {
            get { return _awayTeamPreviousAwayGoalsScored; }
            set { _awayTeamPreviousAwayGoalsScored = value; }
        } 

        #endregion

    }
}
