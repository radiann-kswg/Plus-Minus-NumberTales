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
    bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!TransitionManager.instance.IsTransitionAnimating())
            GetGameControl();
    }

    void GetGameControl()
    {
        foreach (bool isPositive in new bool[] { false, true })
        {
            if (Input.GetButtonDown("Horizontal") || GetAxisDown("Horizontal", isPositive))
            {
                sw.Switching(isPositive);

                SE.instance.PlayClip(1);
                return;
            }
        }

        if (Input.GetButtonDown("Fire3"))
        {
            director.HoldNum();
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            vl.HardDrop();
            return;
        }

        if (isPressed && !GetAxisHold("Horizontal"))
        {
            isPressed = false;
            return;
        }
    }

    bool GetAxisHold(string name)
    {
        return Input.GetAxis(name) > AXIS_THRESHOLD || Input.GetAxis(name) < -AXIS_THRESHOLD;
    }

    bool GetAxisDown(string name, bool isPositive)
    {
        if (isPressed) return false;
        bool _res = false;
        if (isPositive ? Input.GetAxis(name) > AXIS_THRESHOLD : Input.GetAxis(name) < -AXIS_THRESHOLD)
        {
            isPressed = true;
            _res = true;
        }
        return _res;
    }
}
