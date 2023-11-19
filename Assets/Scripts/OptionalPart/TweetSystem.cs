using UnityEngine;
using UnityEngine.UI;
using System.Web;
// https://sirohood.exp.jp/20191002-2908/
using ZXing;
using ZXing.QrCode;

public class TweetSystem : MonoBehaviour
{
    [SerializeField]
    string[] characterName = {"000(チトセ)", "1(ハジメ)", "2(ツグ)", "3(ナオ)", "4(モチ)",
        "5(イズ)", "6(ムイ)", "7(ナナ)", "8(ワカツ)", "9(チカ)"};

    [SerializeField]
    Image qRcodeSprite_Tweet, qRcodeSprite_Misskey;//最終的に表示するSpriteRendererオブジェクト

    private Texture2D _encodedQRTextire;//エンコードして出来たQRコードのTxture2Dが入る

    private int _qrTxtureW = 256;//作成するテクスチャサイズ
    private int _qrTxtureH = 256;//作成するテクスチャサイズ

    string url;//QRコード化したいURL

    public void TweetResult(int characterId, int score, int level = 0)
    {
        /* X/Twitter */
        url = _ReturnTweetResultURL(characterId, score, level);

        //新規の空のテクスチャを作成
        _encodedQRTextire = new Texture2D(_qrTxtureW, _qrTxtureH);

        //エンコード処理
        var color32_tweet = Encode(url, _encodedQRTextire.width, _encodedQRTextire.height);

        //https://docs.unity3d.com/2018.4/Documentation/ScriptReference/Texture2D.SetPixels32.html
        //ピクセルカラーのブロックを設定
        _encodedQRTextire.SetPixels32(color32_tweet);

        //https://docs.unity3d.com/ja/2017.4/ScriptReference/Texture2D.Apply.html
        //エンコードで取得した情報で変更を適用する
        _encodedQRTextire.Apply();

        //スプライトを作成してオブジェクトに張り付け
        qRcodeSprite_Tweet.sprite = Sprite.Create(_encodedQRTextire, new Rect(0, 0, _qrTxtureW, _qrTxtureH), Vector2.zero);

        /* Misskey */
        url = _ReturnMisskeyResultURL(characterId, score, level);

        //新規の空のテクスチャを作成
        _encodedQRTextire = new Texture2D(_qrTxtureW, _qrTxtureH);

        //エンコード処理
        var color32_misskey = Encode(url, _encodedQRTextire.width, _encodedQRTextire.height);

        //https://docs.unity3d.com/2018.4/Documentation/ScriptReference/Texture2D.SetPixels32.html
        //ピクセルカラーのブロックを設定
        _encodedQRTextire.SetPixels32(color32_tweet);

        //https://docs.unity3d.com/ja/2017.4/ScriptReference/Texture2D.Apply.html
        //エンコードで取得した情報で変更を適用する
        _encodedQRTextire.Apply();

        //スプライトを作成してオブジェクトに張り付け
        qRcodeSprite_Misskey.sprite = Sprite.Create(_encodedQRTextire, new Rect(0, 0, _qrTxtureW, _qrTxtureH), Vector2.zero);
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

    // https://misskeyshare.link/introduce.html
    private string _ReturnMisskeyResultURL(int characterId, int score, int level)
    {
        string scoreStr = score.ToString() + "点(Lv." + level.ToString() + ")";

        var url = "https://misskeyshare.link/share.html?";

        string query =
            "text= " +
            HttpUtility.UrlEncode(
                "「" +
                characterName[characterId] +
                "」とゲームに挑み、" + scoreStr +
                "を獲得した!!"
                ) +
            HttpUtility.UrlEncode("%0A%0A#ナンバーテールズの主人より%20#SanukiXGame%0A")+
                HttpUtility.UrlEncode("&url = URL") +
            HttpUtility.UrlEncode("https://unityroom.com/games/plus-minus-numbertales") ;

        return url + query;
    }

    //エンコード処理（ここはサンプル通り）
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,

            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

}
