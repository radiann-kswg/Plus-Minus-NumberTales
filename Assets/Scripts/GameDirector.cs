using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// メインシーンでゲーム進行などの処理を行うクラス
/// </summary>
public class GameDirector : MonoBehaviour
{
    /// <summary>
    /// スコアを表示するTextコンポーネント
    /// </summary>
    [SerializeField]
    Text scoreText;
    /// <summary>
    /// レベルを表示するTextコンポーネント
    /// </summary>
    [SerializeField]
    Text levelText;

    /// <summary>
    /// 通常の数字を表示する画像(Sprite)リスト
    /// </summary>
    [SerializeField]
    List<Sprite> nornalNumImage = new List<Sprite>();
    /// <summary>
    /// 目標の数字を表示する画像(Sprite)リスト
    /// </summary>
    [SerializeField]
    List<Sprite> targetNumImage = new List<Sprite>();

    /// <summary>
    /// キャラクターのテーマBGM(AudioCilp)リスト
    /// </summary>
    [SerializeField]
    List<AudioClip> characterThemeClip = new List<AudioClip>();

    /// <summary>
    /// 数字が流動するベルト(Veltコンポーネント)
    /// </summary>
    [SerializeField]
    Velt vl;

    /// <summary>
    /// 目標の数字の絶対値(デフォルト:5)
    /// </summary>
    public int TargetNum = 5;

    /// <summary>
    /// スコア
    /// </summary>
    public int Score = 0;
    /// <summary>
    /// レベル
    /// </summary>
    public int Level = 1;

    /// <summary>
    /// 目標の数字が1つできた時の得点
    /// </summary>
    [SerializeField]
    int fiveScore = 200;
    /// <summary>
    /// 目標の数字が符号の異なる状態で2つできた時の得点
    /// </summary>
    [SerializeField]
    int plusMinusFiveScore = 2000;
    /// <summary>
    /// レベルアップに必要なスコア
    /// </summary>
    [SerializeField]
    int levelDist = 5000;
    /// <summary>
    /// バブルを消した時に加点されるバブル1つごとの得点
    /// </summary>
    [SerializeField]
    int bubbleScore = 50;

    /// <summary>
    /// バブル(Prefabオブジェクト)
    /// </summary>
    [SerializeField]
    GameObject[] bubblePrefab;
    
    /// <summary>
    /// バブルを発生させることができるかどうか
    /// </summary>
    private bool _isBubbling = true;

    /// <summary>
    /// 現在流入しようとしている数字
    /// </summary>
    int nowNum;
    /// <summary>
    /// 次に流入しようとしている数字
    /// </summary>
    int nextNum;
    /// <summary>
    /// 現在の左の数字
    /// </summary>
    int leftNum = 0;
    /// <summary>
    /// 現在の右の数字
    /// </summary>
    int rightNum = 0;

    /// <summary>
    /// 左の連鎖数
    /// </summary>
    private int _leftChainNum = 0;
    /// <summary>
    /// 右の連鎖数
    /// </summary>
    private int _rightChainNum = 0;

    /// <summary>
    /// 数字を確認するために用いる、それぞれの数字を2乗した値
    /// </summary>
    private int _sqareTarget, _sqareLeft, _sqareRight;

    /// <summary>
    /// ホールドされている数字(デフォルト,ホールドされていない状態:0)
    /// </summary>
    int holdNum = 0;
    /// <summary>
    /// 少なくとも1回ホールドされたか
    /// </summary>
    bool isFirstHolded = false;
    /// <summary>
    /// 現在の手でホールドが実行されたか
    /// </summary>
    bool isHolded = false;

    /// <summary>
    /// それぞれの数字を表示するImageコンポーネント
    /// </summary>
    [SerializeField]
    Image nowNumImage, nextNumImage, leftNumImage, rightNumImage, holdNumImage, targetImage_plus, targetImage_minus;
    /// <summary>
    /// キャラクターを表示するImageコンポーネント
    /// </summary>
    [SerializeField]
    Image characterDispImage;

    /// <summary>
    /// BGMを再生するAudioSourceコンポーネント
    /// </summary>
    [SerializeField]
    AudioSource bgmAudioSource;

    /// <summary>
    /// バブルのオブジェクトタグ(const string)
    /// </summary>
    [SerializeField]
    const string PLUS_BUBBLE_TAG = "PlusBubble", MINUS_BUBBLE_TAG = "MinusBubble";
    /// <summary>
    /// バブルの画像(Sprite),属性変更に用いる
    /// </summary>
    [SerializeField]
    Sprite plusBubbleImage, minusBubbleImage;

    /// <summary>
    /// ゲームオーバーとなったかどうか
    /// </summary>
    bool isGameOver = false;

    /// <summary>
    /// リザルト画面(Scene)の名前
    /// </summary>
    [SerializeField]
    string resultSceneName;

    // Start is called before the first frame update
    void Start()
    {
        _InitTargetName();

        targetImage_minus.sprite = nornalNumImage[_ReturnNumImageIndex(-TargetNum)] = targetNumImage[_ReturnNumImageIndex(-TargetNum)];
        targetImage_plus.sprite  = nornalNumImage[_ReturnNumImageIndex( TargetNum)] = targetNumImage[_ReturnNumImageIndex( TargetNum)];

        characterDispImage.sprite = Messerger.instance.charactersImage[TargetNum];
        bgmAudioSource.clip = characterThemeClip[TargetNum];

        nowNum = ReturnRandomNum();
        nextNum = ReturnRandomNum();

        _UpdateUIs();

        TransitionManager.instance.Reset();
        bgmAudioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TransitionManager.instance.IsTransitionAnimating())
        {

            if (!isGameOver)
            {
                _UpdateUIs();
            }
            else
            {
                /* ゲームオーバー(リザルト画面へ) */
                SE.instance.PlayClip(3);
                Messerger.instance.ScoreMessage = Score;
                Messerger.instance.LevelMessage = Level;
                TransitionManager.instance.FadeOut();
            }
        }

        if (TransitionManager.instance.IsReadyToNextSceneNow())
        {
            SceneManager.LoadScene(resultSceneName);
        }
    }

    /// <summary>
    /// 目標の数字を初期設定します
    /// </summary>
    private void _InitTargetName()
    {
        // シングルトンから目標の数字を受信
        TargetNum = Messerger.instance.TargetNumMessage;

        /* 目標の数字の例外処理 */
        // 負の値を正に(絶対値化)
        TargetNum *= TargetNum < 0 ? -1 : 1;
        // 0だった時の処理（0番(シークレット)のキャラを解禁しないのであれば最小値:1に）
        TargetNum = TargetNum == 0 ? 1 : TargetNum;
        // 2桁以上の値を最大値:9に
        TargetNum = TargetNum >= 10 ? 9 : TargetNum;
    }

    /// <summary>
    /// 数字の値から数字の画像リストの添え字を返します
    /// </summary>
    /// <param name="num">数字の値（-9～+9）</param>
    /// <returns></returns>
    private int _ReturnNumImageIndex(int num)
    {
        return num + 9;
    }

    /// <summary>
    /// 数字の画像を更新します
    /// </summary>
    /// <param name="isRight">右の数字:true,左の数字:false</param>
    /// <returns></returns>
    private void _UpdateNumImage(bool isRight)
    {
        int _num = isRight ? rightNum : leftNum;
        if (_ReturnNumImageIndex(_num) >= 0 && _ReturnNumImageIndex(_num) < nornalNumImage.Count)
            (isRight ? rightNumImage : leftNumImage).sprite = nornalNumImage[_ReturnNumImageIndex(_num)];
    }

    /// <summary>
    /// UIを更新します
    /// </summary>
    private void _UpdateUIs()
    {
        nowNumImage.sprite = nornalNumImage[_ReturnNumImageIndex(nowNum)];
        nextNumImage.sprite = nornalNumImage[_ReturnNumImageIndex(nextNum)];
        _UpdateNumImage(false);
        _UpdateNumImage(true);
        scoreText.text = Score.ToString();
        if (isFirstHolded)
            holdNumImage.sprite = nornalNumImage[_ReturnNumImageIndex(holdNum)];
        Level = Score / levelDist + 1;
        levelText.text = Level.ToString();
    }

    /// <summary>
    /// 指定した数字が目標の数字と0以外でないか、指定された数字の画像リストの範囲内の値かどうかを判定します
    /// </summary>
    /// <param name="num">数字の値</param>
    /// <returns>指定した数字が0,目標の数字,範囲外の数字のどれかであるか</returns>
    private bool _CheckNumberIsNotZeroOrTarget(int num)
    {
        return num == 0 || num == -TargetNum || num == TargetNum
            || _ReturnNumImageIndex(num) < 0 || _ReturnNumImageIndex(num) >= nornalNumImage.Count;
    }

    /// <summary>
    /// レベルデザインに応じたランダムな数字を返します
    /// </summary>
    /// <returns>抽選結果</returns>
    public int ReturnRandomNum()
    {
        int num = 0;
        while (_CheckNumberIsNotZeroOrTarget(num))
        {
            /* レベルデザインに基づく乱数生成 */
            if (Level <= 3)
            {
                num = (Mathf.FloorToInt(Mathf.Pow(Random.value, 3.5f) * 4.0f) + 1) * Mathf.CeilToInt(Mathf.Sign(Random.value - 0.5f));
                continue;
            }
            if (Level <= 7)
            {
                num = (Mathf.FloorToInt(Mathf.Pow(Random.value, 2.5f) * 7.0f) + 1) * Mathf.CeilToInt(Mathf.Sign(Random.value - 0.5f));
                continue;
            }
            num = (Mathf.FloorToInt(Mathf.Pow(Random.value, (float)(12 + Level) / (float)Level) * 9.0f) + 1) * Mathf.CeilToInt(Mathf.Sign(Random.value - 0.5f));
        }
        return num;
    }

    /// <summary>
    /// 次の数字に切り替えます
    /// </summary>
    public void SwitchNextNum()
    {
        nowNum = nextNum;
        nextNum = ReturnRandomNum();
    }

    /// <summary>
    /// 左右の数字を抽出します
    /// </summary>
    /// <param name="isRight">true:右の数字, false:左の数字</param>
    /// <returns>抽出結果</returns>
    private int _ReturnSelectedNum(bool isRight)
    {
        return isRight ? rightNum : leftNum;
    }

    /// <summary>
    /// 左右の数字を設定します
    /// </summary>
    /// <param name="isRight">true:右の数字, false:左の数字</param>
    /// <param name="newNum">設定する数字</param>
    private void _SetSelectedNum(bool isRight, int newNum)
    {
        if (isRight) rightNum = newNum;
        else leftNum = newNum;
    }

    /// <summary>
    /// バブルの半分消しと連鎖数のインクリメントを実行します
    /// 連鎖数が一定となった時は連鎖を終了します
    /// </summary>
    /// <param name="isRight">右の数字についてか</param>
    private void _FlushNumAndHalfBubble(bool isRight)
    {
        /* 半分消しエフェクト */
        DeleteBubblesAll(_ReturnSelectedNum(isRight) > 0);

        const int _LIMIT_CHAIN_NUM = 3;
        int _varChainNum = isRight ? _rightChainNum : _leftChainNum;
        if(_varChainNum + 1 >= _LIMIT_CHAIN_NUM)
        {
            _ResetChain(isRight, true);
            
            //インクリメントせず関数を離脱
            return;
        }
        if (isRight) ++_rightChainNum;
        else ++_leftChainNum;
    }

    /// <summary>
    /// 全消し処理を実行します
    /// </summary>
    private void _FlashAll()
    {
        // 加点
        Score += plusMinusFiveScore;

        /* 全消しエフェクト */
        DeleteBubblesAll(true, true);
        DeleteBubblesAll(false, true);
    }

    /// <summary>
    /// 連鎖を終了します
    /// </summary>
    /// <param name="isRight">右の数字についてか</param>
    /// <param name="isSetNum2Random">数字をランダムに変更するか</param>
    private void _ResetChain(bool isRight, bool isSetNum2Random)
    {
        if (isRight) _rightChainNum = 0;
        else _leftChainNum = 0;

        if (isSetNum2Random) _SetSelectedNum(isRight, ReturnRandomNum());
    }

    /// <summary>
    /// 数字を加算し、得点処理をします
    /// </summary>
    /// <param name="isEnter2Right">右の数字へ:true,左の数字へ:false</param>
    public void EnterNum2Stock(bool isEnter2Right)
    {
        // バブル発生可能でないときは処理しない
        if (!_isBubbling) return;

        bool isBurst = false; // バーストしたか
        int _addedNum = _ReturnSelectedNum(isEnter2Right) + nowNum;
        if (_CheckSqares(_addedNum) >= 100)
        {
            // ランダムな数字に更新
            _SetSelectedNum(isEnter2Right, ReturnRandomNum());
            isBurst = true;
        }
        else
        {
            // 加算した数字に更新
            _SetSelectedNum(isEnter2Right, _addedNum);
        }
        // 確認用の2乗数を更新
        _CheckSqares();

        // バーストしたとき
        if (isBurst)
        {
            /* ダメージエフェクト */
            SE.instance.PlayClip(3);

            BubbleCloning(true, true);
        }
        /* 左右で符号の異なる目標の数字を生成したとき */
        if (IsPlusMinusFive())
        {
            // SEを再生
            SE.instance.PlayClip(5);

            // 全消しを実行
            _FlashAll();

            // 連鎖数やその他の得点,エフェクト処理は不要なので関数を離脱
            return;
        }
        /* 目標の数字が生成されたときの処理(左右共通) */
        if (IsFive())
        {
            // SEを再生
            SE.instance.PlayClip(4);
            
            /* 目標の数字が生成されたときの左右ごとの処理 */
            foreach (bool _isRightBecomeFive in (new bool[] { !isEnter2Right, isEnter2Right }))
            {
                if (IsFive(true, _isRightBecomeFive))
                {
                    Score += fiveScore;
                    _FlushNumAndHalfBubble(_isRightBecomeFive);
                }
                else
                {
                    _ResetChain(_isRightBecomeFive, false);
                }
            }
        }
        /* 目標の数字が生成されていないときの処理 */
        else
        {
            // 連鎖数をリセット
            _ResetChain(true, false);
            _ResetChain(false, false);

            // 加点もバーストもされない場合の処理
            if (!isBurst)
            {
                // SEを再生
                SE.instance.PlayClip(2);
                // バブルを発生
                if (nowNum != 0)
                {
                    BubbleCloning(nowNum > 0);
                }
            }
        }
    }

    /// <summary>
    /// 2乗数の確認をします
    /// </summary>
    /// <param name="num">数字指定(任意)</param>
    /// <returns>数字を指定していない場合:0,指定した場合:指定した数字の2乗数</returns>
    private int _CheckSqares(int num = -256)
    {
        if (num == -256)
        {
            _sqareTarget = TargetNum * TargetNum;
            _sqareLeft = leftNum * leftNum;
            _sqareRight = rightNum * rightNum;
            return 0;
        }
        return num * num;
    }

    /// <summary>
    /// 目標の数字が生成されたかを確認します
    /// </summary>
    /// <param name="selection">左右を指定するか</param>
    /// <param name="isRight">(左右を指定する場合) true:右,false:左</param>
    /// <returns>目標の数字が生成されたか</returns>
    bool IsFive(bool selection = false, bool isRight = false)
    {
        /*
        // 確認用2乗数の更新
        _CheckSqares();
        */
        if (selection) {
            if (!isRight) return _sqareLeft == _sqareTarget;
            else return _sqareRight == _sqareTarget;
        }
        return (_sqareLeft == _sqareTarget) || (_sqareRight == _sqareTarget);
    }

    /// <summary>
    /// 目標の数字が左右異なる符号で生成されたかを確認します
    /// </summary>
    /// <returns>目標の数字が左右異なる符号で生成されたか</returns>
    bool IsPlusMinusFive()
    {
        /*
        // 確認用2乗数の更新
        _CheckSqares();
        */
        return (leftNum * rightNum == -_sqareTarget) && (_sqareLeft == _sqareRight);
    }

    /// <summary>
    /// ホールドを実行します
    /// </summary>
    public void HoldNum()
    {
        // すでにこの手でホールドされている場合は処理しない
        if (isHolded) return;

        // 1回はホールドされた場合ホールドされている数字をバッファに移す
        int num = isFirstHolded ? holdNum : 0;
        
        /* ホールド */
        holdNum = nowNum;

        // 1回もホールドされていない場合
        if (!isFirstHolded)
        {
            // 次の数字へ
            SwitchNextNum();
            
            isFirstHolded = true;
        }
        // 1回はホールドされた場合
        else
        {
            // バッファ(この手でホールドする前の数字)を現在の数字へ
            nowNum = num;
        }

        // SEを再生
        SE.instance.PlayClip(6);

        // ベルトを初期位置へ
        vl.ResetValue();
        
        isHolded = true;
    }

    /// <summary>
    /// ホールド状態のフラグを初期化します
    /// </summary>
    public void TurnOffHoldFlag()
    {
        isHolded = false;
    }

    /// <summary>
    /// ゲームオーバーのフラグを立てます
    /// </summary>
    public void SwitchGameOver()
    {
        isGameOver = true;
    }

    /// <summary>
    /// バブルを生成します
    /// </summary>
    /// <param name="isPlus">プラスのバブルを生成するかどうか</param>
    /// <param name="isBursted">バーストが発生したか</param>
    void BubbleCloning(bool isPlus, bool isBursted = false)
    {
        int _randomUnit = Random.Range(3, 6);
        if (isBursted) _randomUnit = 5;
        // Debug.Log(random_Unit.ToString());

        GameObject obj = null;
        float angleAnp = 40.0f;
        float positionXAnp = 2.0f, positionY = 6.0f;
        float speed = 3.0f;
        for (int i = 0; i < _randomUnit; i++)
        {
            // Debug.Log(random_Num.ToString("f0"));

            bool[] _bubbleFlag = new bool[] { (isPlus || isBursted), (!isPlus || isBursted) };
            for (int j = 0; j < 2; ++j)
            {
                if (_bubbleFlag[j])
                {
                    float _randomNum = Random.Range(1.25f, 3.0f + Mathf.Epsilon);
                    float _random_x = Mathf.Sin(Random.Range(0.0f, angleAnp) * Mathf.Deg2Rad * Mathf.Sign((float)(2 * Random.Range(0, 2) - 1)));
                    float _random_y = Mathf.Cos(Random.Range(0.0f, angleAnp) * Mathf.Deg2Rad);

                    obj = Instantiate(bubblePrefab[j],
                        new Vector3(Random.Range(-positionXAnp, positionXAnp), positionY, 0.0f), Quaternion.identity);
                    obj.transform.localScale = _randomNum * new Vector3(1.0f, 1.0f, 1.0f);
                    obj.GetComponent<Rigidbody2D>().velocity = speed * new Vector2(_random_x, _random_y);
                }
            }
        }
        _isBubbling = false;
    }

    /// <summary>
    /// プラスまたはマイナスのバブルを消します
    /// </summary>
    /// <param name="isPlus">消す属性</param>
    /// <param name="isPlusMinus">全消しするかどうか(true;属性変更しない)</param>
    void DeleteBubblesAll(bool isPlus, bool isPlusMinus = false)
    {
        var _bubble = GameObject.FindGameObjectsWithTag(isPlus ? PLUS_BUBBLE_TAG : MINUS_BUBBLE_TAG);
        if (_bubble.Length > 0)
        {
            Score += bubbleScore * _bubble.Length;
            for (int i = 0; i < _bubble.Length; ++i)
            {
                GameObject.Destroy(_bubble[i]);
            }
        }

        if (!isPlusMinus) ChangeBubblesHalf(!isPlus);
    }

    /// <summary>
    /// バブルを半分だけ属性変更します
    /// </summary>
    /// <param name="isPlus">属性変更するバブル</param>
    void ChangeBubblesHalf(bool isPlus)
    {
        var _bubble = GameObject.FindGameObjectsWithTag(isPlus ? PLUS_BUBBLE_TAG : MINUS_BUBBLE_TAG);
        if (_bubble.Length > 0)
        {
            int halfBubbleLength = _bubble.Length / 2;
            for (int i = 0; i < halfBubbleLength; ++i)
            {
                ChangeBubble(_bubble[i], !isPlus);
            }
        }
    }

    /// <summary>
    /// バブルのPrefab情報を属性変更します
    /// </summary>
    /// <param name="bubbleObject">属性変更するバブルのPrefab</param>
    /// <param name="isPlus">属性変更先がプラスかどうか</param>
    void ChangeBubble(GameObject bubbleObject, bool isPlus)
    {
        // Prefabが存在しない場合は実行しない
        if (!bubbleObject) return;
        
        bubbleObject.GetComponent<SpriteRenderer>().sprite = isPlus ? plusBubbleImage : minusBubbleImage;
        bubbleObject.tag = isPlus ? PLUS_BUBBLE_TAG : MINUS_BUBBLE_TAG;
    }

    /// <summary>
    /// バブルを生成可能にします
    /// </summary>
    public void TurnOnBubblingFlag()
    {
        _isBubbling = true;
    }

    /// <summary>
    /// 数字が加算可能な状態かどうか(=バブル生成の準備が完了しているか)を返します
    /// </summary>
    /// <returns>数字が加算可能であるか</returns>
    public bool ReturnStockingFlag()
    {
        return _isBubbling;
    }
}



