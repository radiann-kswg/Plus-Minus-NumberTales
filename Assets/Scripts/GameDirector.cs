using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameDirector : MonoBehaviour
{
    [SerializeField]
    Text scoreText, levelText;
    [SerializeField]
    List<Sprite> numImage = new List<Sprite>();

    [SerializeField]
    velt vl;

    public int Score = 0, Level = 1;
    int leftChainNum = 0, rightChainNum = 0;

    [SerializeField]
    int fiveScore = 200, plusMinusFiveScore = 2000, levelDist = 5000, bubbleScore = 50;

    [SerializeField]
    float random_Num;//大きさ

    [SerializeField]
    GameObject[] bubble_Prefab;
    
    int random_Unit;//個数
    bool isBubbling = true;

    int nowNum, nextNum, leftNum = 0, rightNum = 0;
    bool isFirstHolded = false, isHolded = false;
    int holdNum = 0;
    [SerializeField]
    Image nowNumImage, nextNumImage, leftNumImage, rightNumImage, holdNumImage;

    [SerializeField]
    Sprite plusBubbleImage, minusBubbleImage;

    bool isGameOver = false;
    [SerializeField]
    string resultSceneName;

    const string PLUS_BUBBLE_TAG = "PlusBubble";
    const string MINUS_BUBBLE_TAG = "MinusBubble";

    // Start is called before the first frame update
    void Start()
    {
        // float speed = 0;


        //int rnd = Random.Range(1, 10);
        //public static int Range(int min, int max);

        //textUI.text = "Game Start";

        nowNum = ReturnRandomNum();
        nextNum = ReturnRandomNum();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver) {
            nowNumImage.sprite = numImage[ReturnNumImageIndex(nowNum)];
            nextNumImage.sprite = numImage[ReturnNumImageIndex(nextNum)];
            if (ReturnNumImageIndex(leftNum) >= 0 && ReturnNumImageIndex(leftNum) < numImage.Count)
                leftNumImage.sprite = numImage[ReturnNumImageIndex(leftNum)];
            if (ReturnNumImageIndex(rightNum) >= 0 && ReturnNumImageIndex(rightNum) < numImage.Count)
                rightNumImage.sprite = numImage[ReturnNumImageIndex(rightNum)];
            scoreText.text = Score.ToString();
            if (isFirstHolded)
                holdNumImage.sprite = numImage[ReturnNumImageIndex(holdNum)];
            Level = Score / levelDist + 1;
            levelText.text = Level.ToString();
        }
        else
        {
            /* ゲームオーバー(リザルト画面へ) */
            ScoreManager.instance.ScoreMessage = Score;
            ScoreManager.instance.LevelMessage = Level;
            SceneManager.LoadScene(resultSceneName);
        }
    }

    private int ReturnNumImageIndex(int num)
    {
        return num + 9;
    }

    public int ReturnRandomNum()
    {
        int i = 0;
        while (i == 0 || i == -5 || i == 5 || ReturnNumImageIndex(i) < 0 || ReturnNumImageIndex(i) >= numImage.Count)
        {
            if (Level <= 3)
            {
                i = (Mathf.FloorToInt(Mathf.Pow(Random.value, 2.5f) * 4.0f) + 1) * Mathf.CeilToInt(Mathf.Sign(Random.value - 0.5f));
            }
            else
            {
                i = (Mathf.FloorToInt(Mathf.Pow(Random.value, (float)(6 + Level) / (float)Level) * 9.0f) + 1) * Mathf.CeilToInt(Mathf.Sign(Random.value - 0.5f));
            }
        }
        return i;
    }

    public void SwitchNextNum()
    {
        nowNum = nextNum;
        nextNum = ReturnRandomNum();
    }

    public void EnterNum2Stock(bool isRight)
    {
        if (isBubbling)
        {
            bool isBurst = false;
            if (!isRight)
            {
                leftNum += nowNum;
                if (leftNum * leftNum >= 100)
                {
                    /* ダメージエフェクト */
                    bubbleCloning(true, true);
                    leftNum = ReturnRandomNum();
                    isBurst = true;
                }
            }
            else
            {
                rightNum += nowNum;
                if (rightNum * rightNum >= 100)
                {
                    /* ダメージエフェクト */
                    bubbleCloning(true, true);
                    rightNum = ReturnRandomNum();
                    isBurst = true;
                }
            }
            if (!isBurst)
            {
                if (IsPlusMinusFive())
                {
                    Score += plusMinusFiveScore;

                    /* 「±5」エフェクト */
                    deleteBubblesAll(true, true);
                    deleteBubblesAll(false, true);
                    /*
                    leftNum = ReturnRandomNum();
                    rightNum = ReturnRandomNum();

                    leftChainNum = 0;
                    rightChainNum = 0;
                    */
                }
                else if (IsFive())
                {
                    if (IsFive(true, false))
                    {
                        Score += fiveScore;
                        leftChainNum++;
                        if (leftChainNum >= 3)
                        {
                            leftChainNum = 0;
                            /* 連鎖終了エフェクト */
                            leftNum = ReturnRandomNum();
                        }
                        else
                        {
                            /* バブル一掃エフェクト */
                            if (leftNum == 5)
                            {
                                deleteBubblesAll(true);
                            }
                            else
                            {
                                deleteBubblesAll(false);
                            }
                        }
                    }
                    else
                    {
                        leftChainNum = 0;
                    }
                    if (IsFive(true, true))
                    {
                        Score += fiveScore;
                        rightChainNum++;
                        if (rightChainNum >= 3)
                        {
                            rightChainNum = 0;
                            /* 連鎖終了エフェクト */
                            rightNum = ReturnRandomNum();
                        }
                        else
                        {
                            /* バブル一掃エフェクト */
                            if (rightNum == 5)
                            {
                                deleteBubblesAll(true);
                            }
                            else
                            {
                                deleteBubblesAll(false);
                            }
                        }
                    }
                    else
                    {
                        rightChainNum = 0;
                    }
                }
                else
                {
                    leftChainNum = 0;
                    rightChainNum = 0;
                    if (nowNum > 0)
                    {
                        bubbleCloning(true);
                    }
                    else if (nowNum < 0)
                    {
                        bubbleCloning(false);
                    }
                }
            }
        }
    }

    bool IsFive(bool selection = false, bool isRight = false)
    {
        if (selection) {
            if (!isRight) return leftNum * leftNum == 25;
            else return rightNum * rightNum == 25;
        }
        return (leftNum * leftNum == 25) || (rightNum * rightNum == 25);
    }

    bool IsPlusMinusFive()
    {
        return leftNum * rightNum == -25;
    }

    public void HoldNum()
    {
        if (!isHolded)
        {
            int num = 0;
            if (isFirstHolded) num = holdNum;
            holdNum = nowNum;
            if (num == 0 && !isFirstHolded)
            {
                SwitchNextNum();
                isFirstHolded = true;
            }
            else
            {
                nowNum = num;
            }
            vl.ResetValue();
            isHolded = true;
        }
    }

    public void TurnOffHoldFlag()
    {
        isHolded = false;
    }

    public void SwitchGameOver()
    {
        isGameOver = true;
    }

    void bubbleCloning(bool isPlus, bool isBursted = false)
    {
        random_Unit = Random.Range(3, 6);
        if (isBursted) random_Unit = 5;
        // Debug.Log(random_Unit.ToString());
        GameObject[] obj = new GameObject[random_Unit];
        float angleAnp = 40.0f;
        float positionXAnp = 2.0f, positionY = 6.0f;
        float speed = 3.0f;
        for (int i = 0; i < random_Unit; i++)
        {
            random_Num = Random.Range(1.25f, 3.0f + Mathf.Epsilon);
            float random_x = Mathf.Sin(Random.Range(0.0f, angleAnp) * Mathf.Deg2Rad * Mathf.Sign((float)(2 * Random.Range(0, 2) - 1)));
            float random_y = Mathf.Cos(Random.Range(0.0f, angleAnp) * Mathf.Deg2Rad);
            // Debug.Log(random_Num.ToString("f0"));
            if (isPlus || isBursted)
            {
                obj[i] = Instantiate(bubble_Prefab[0],
                    new Vector3(Random.Range(-positionXAnp, positionXAnp), positionY, 0.0f), Quaternion.identity);
                obj[i].transform.localScale = random_Num * new Vector3(1.0f, 1.0f, 1.0f);
                obj[i].GetComponent<Rigidbody2D>().velocity = speed * new Vector2(random_x, random_y);
                //break;
            }
            if (!isPlus || isBursted)
            {
                obj[i] = Instantiate(bubble_Prefab[1],
                    new Vector3(Random.Range(-positionXAnp, positionXAnp), positionY, 0.0f), Quaternion.identity);
                obj[i].transform.localScale = random_Num * new Vector3(1.0f, 1.0f, 1.0f);
                obj[i].GetComponent<Rigidbody2D>().velocity = speed * new Vector2(random_x, random_y);
                //break;
            }
        }
        isBubbling = false;
    }

    void deleteBubblesAll(bool isPlus, bool isPlusMinus = false)
    {
        if (isPlus)
        {
            var plusBubble = GameObject.FindGameObjectsWithTag(PLUS_BUBBLE_TAG);
            if (plusBubble.Length > 0)
            {
                Score += bubbleScore * plusBubble.Length;
                for (int i = 0; i < plusBubble.Length; ++i)
                {
                    GameObject.Destroy(plusBubble[i]);
                }
            }
        }
        else
        {
            var minusBubble = GameObject.FindGameObjectsWithTag(MINUS_BUBBLE_TAG);
            if (minusBubble.Length > 0)
            {
                Score += bubbleScore * minusBubble.Length;
                for (int i = 0; i < minusBubble.Length; ++i)
                {
                    GameObject.Destroy(minusBubble[i]);
                }
            }
        }
        if(!isPlusMinus) ChangeBubblesHalf(isPlus);
    }

    void ChangeBubblesHalf(bool isPlus)
    {
        if (isPlus)
        {
            var Bubble = GameObject.FindGameObjectsWithTag(MINUS_BUBBLE_TAG);
            if (Bubble.Length > 0)
            {
                int halfBubbleLength = Bubble.Length / 2;
                for (int i = 0; i < halfBubbleLength; ++i)
                {
                    ChangeBubble(Bubble[i], isPlus);
                }
            }
        }
        else
        {
            var Bubble = GameObject.FindGameObjectsWithTag(PLUS_BUBBLE_TAG);
            if (Bubble.Length > 0)
            {
                int halfBubbleLength = Bubble.Length / 2;
                for (int i = 0; i < halfBubbleLength; ++i)
                {
                    ChangeBubble(Bubble[i], isPlus);
                }
            }
        }
    }

    void ChangeBubble(GameObject bubbleObject, bool isPlus)
    {
        if (bubbleObject)
        {
            bubbleObject.GetComponent<SpriteRenderer>().sprite
                = isPlus ? plusBubbleImage : minusBubbleImage;
            bubbleObject.tag = isPlus ? PLUS_BUBBLE_TAG : MINUS_BUBBLE_TAG;
        }
    }

    public void TurnOnBubblingFlag()
    {
        isBubbling = true;
    }

    public bool ReturnStockingFlag()
    {
        return isBubbling;
    }
}



