using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreReceiver : MonoBehaviour
{
    int score = 0, level = 1;
    [SerializeField]
    Text scoreText, levelText;
    // Start is called before the first frame update
    void Start()
    {
        score = ScoreManager.instance.ScoreMessage;
        level = ScoreManager.instance.LevelMessage;
        scoreText.text = score.ToString();
        levelText.text = level.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
