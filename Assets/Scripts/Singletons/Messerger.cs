using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messerger : MonoBehaviour
{
    public static Messerger instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public List<Sprite> charactersImage = new List<Sprite>();

    public int ScoreMessage = 0;
    public int LevelMessage = 1;
    public int TargetNumMessage = 5;
    public bool IsUnlockedNo0 = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
