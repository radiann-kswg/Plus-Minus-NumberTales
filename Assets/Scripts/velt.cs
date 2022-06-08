using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class velt : MonoBehaviour
{
    [SerializeField]
    GameDirector director;
    [SerializeField]
    switching sw;
    [SerializeField]
    int steps = 4;

    public Slider slider;
    public float time;
    [SerializeField]
    public float velocity = 0.25f;
    float value = 1.00f;
    // Start is called before the first frame update
    void Start()
    {
        slider = this.GetComponent<Slider>();
        value = ReturnMaxValue();
    }

    // Update is called once per frame
    void Update()
    {
        value -= velocity * Time.deltaTime * (1 + (float)(director.Level - 1) * 0.125f);
        if (value <= 0.0f)
        {
            if (director.ReturnStockingFlag())
            {
                director.EnterNum2Stock(sw.isRight);
                director.TurnOffHoldFlag();
                director.SwitchNextNum();
                value = ReturnMaxValue();
            }
        }
        else
        {
            director.TurnOnBubblingFlag();
            slider.value = Mathf.Floor(value * (float)steps) / (float)steps;
        }
    }

    float ReturnMaxValue()
    {
        return 1.00f + 1.00f / (float)steps - Mathf.Epsilon;
    }

    public void ResetValue()
    {
        value = ReturnMaxValue();
        slider.value = value;
    }

    public void HardDrop()
    {
        value = -0.10f;
    }
}
