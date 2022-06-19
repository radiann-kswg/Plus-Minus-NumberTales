using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameDirector director;
    [SerializeField]
    Switcher sw;
    [SerializeField]
    Velt vl;

    [SerializeField]
    const float AXIS_THRESHOLD = 0.7f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") < -AXIS_THRESHOLD)
        {
            sw.Switching(false);

            SE.instance.PlayClip(1);
        }

        if (Input.GetButtonDown("Horizontal") && Input.GetAxis("Horizontal") >  AXIS_THRESHOLD)
        {
            sw.Switching(true);

            SE.instance.PlayClip(1);
        }

        if (Input.GetButtonDown("Fire3"))
        {
            director.HoldNum();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            vl.HardDrop();
        }
    }
}
