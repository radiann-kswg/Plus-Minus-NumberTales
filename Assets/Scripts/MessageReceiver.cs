using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageReceiver : MonoBehaviour
{
    int score = 0, level = 1, target = 5;
    [SerializeField]
    Text scoreText, levelText;
    [SerializeField]
    Image characterImage;

    bool isOpenedTweetUrl = false;

    // Start is called before the first frame update
    void Start()
    {
        score  = Messerger.instance.ScoreMessage;
        level  = Messerger.instance.LevelMessage;
        target = Messerger.instance.TargetNumMessage;
        
        scoreText.text = score.ToString();
        levelText.text = level.ToString();
        characterImage.sprite = Messerger.instance.charactersImage[target];
    }

    // Update is called once per frame
    void Update()
    {
        if (!TransitionManager.instance.IsTransitionAnimating() && !isOpenedTweetUrl)
        {
            _TweetResult();
        }
    }

    void _TweetResult()
    {
        TweetSystem tweet = new TweetSystem();
        tweet.TweetResult(target, score, level);
        isOpenedTweetUrl = true;
    }
}
