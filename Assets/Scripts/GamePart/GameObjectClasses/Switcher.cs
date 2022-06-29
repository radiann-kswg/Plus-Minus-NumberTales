using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switcher : MonoBehaviour
{
    [SerializeField]
    GameObject leftSwitch, rightSwitch;

    private bool _isRight = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Switching(bool isRight_new)
    {
        _isRight = isRight_new;
        leftSwitch.SetActive(!_isRight);
        rightSwitch.SetActive(_isRight);
    }

    public bool IsRight()
    {
        return _isRight;
    }
}
