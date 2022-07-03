using UnityEngine;
using System.Web;

public class TweetSystem
{
    string[] characterName = {"000(チトセ)", "1(ハジメ)", "2(ツグ)", "3(ナオ)", "4(モチ)",
        "5(イズ)", "6(ムイ)", "7(ナナ)", "8(ワカツ)", "9(チカ)"};


    public void TweetResult(int characterId, int score, int level = 0)
    {
        string url = _ReturnTweetResultURL(characterId, score, level);

        Application.OpenURL(url);
    }

    private string _ReturnTweetResultURL(int characterId, int score, int level)
    {
        string scoreStr = score.ToString() + "点(Lv." + level.ToString() + ")";

        var url = "https://twitter.com/intent/tweet?";
            
        string query = 
            "url=" +
            HttpUtility.UrlEncode("https://unityroom.com/games/plus-minus-numbertales") +
            "&text= " + 
            HttpUtility.UrlEncode(
                "「" +
                characterName[characterId] +
                "」とゲームに挑み、" + scoreStr +
                "を獲得した!!"
                ) +
            "&hashtags=" +
            HttpUtility.UrlEncode("ナンバーテールズの主人より,SanukiXGame");

        return url + query;
    }
}
