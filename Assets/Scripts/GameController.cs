using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameDirector director;
    [SerializeField]
    switching sw;
    [SerializeField]
    velt vl;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sw.Switching(false);
        }

        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            sw.Switching(true);
        }

        if (Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            director.HoldNum();
        }

        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            vl.HardDrop();
        }
    }
}
