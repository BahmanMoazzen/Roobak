using System;
using System.Collections.Generic;
using UnityEngine;


public static class A
{
    public static class Tags
    {
        public static string OutOfStockTag = "Out Of Stock";
        public static string PurchaseFailedTag = "Purchase Failed!";
        public static string PurchaseSuccessTag = "Purchase Succeeded!";
        public static string ShopIsNotReadyTag = "Shop is not ready!";
        public static string BuyOneTag = "You do not have enough to activate. Buy one?";
        public static string IsLockedTag = "Locked";
        public static string CheckInternetConnection = "Check Your Internet Connection.";
        public static class LootLocker
        {
            public static string ShowRankSuccess = "Data Recieved Successfuly";
            public static string ShowRankFailed = "Data Recieve Failed!";
            public static string SubmitRankSuccess = "Score Submitted Successfuly";
            public static string SubmitRankFailed = "Score Submission Failed!";
        }
        public static string ScoreSaveTag()
        {
            return "BestScoreTag_"+((int)Levels.DifficultyLevel).ToString();
        }


    }

    

    

    public static class Tools
    {
        public static bool IntToBool(int iInput)
        {
            return iInput == 1 ? true : false;
        }
        public static int BoolToInt(bool iInput)
        {
            return iInput ? 1 : 0;
        }
        public static string ScoreToTitle(int iScore)
        {
            const int digits = 6;
            string scoreTitle = string.Empty;
            for (int i = 0; i < digits - iScore.ToString().Length; i++)
            {
                scoreTitle += "0";
            }

            return $"{scoreTitle}{iScore.ToString()}";
        }
    }
    public static class Levels
    {
        public static GameModes DifficultyLevel;
        const string THISROUNDSCORETAG = "ThisRoundScoreTag";
        public static int ThisRoundScore
        {
            get
            {
                return PlayerPrefs.GetInt(THISROUNDSCORETAG, 0);
            }
            set
            {
                PlayerPrefs.SetInt(THISROUNDSCORETAG, value);
            }
        }
        public static int BestScore
        {
            get
            {
                return PlayerPrefs.GetInt(Tags.ScoreSaveTag(), 0);
            }
            set
            {
                PlayerPrefs.SetInt(Tags.ScoreSaveTag(), value);
            }
        }
        public static bool SetBestScore(int iScore)
        {
            if (iScore > BestScore)
            {
                BestScore = iScore;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    

    


}
public enum GameModes { Easy, Normal, Hard }


