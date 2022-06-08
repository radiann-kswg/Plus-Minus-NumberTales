using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleTriger : MonoBehaviour
{
    [SerializeField]
    GameDirector director;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "PlusBubble" || collision.gameObject.tag == "MinusBubble")
        {
            director.SwitchGameOver();
        }
    }
}
