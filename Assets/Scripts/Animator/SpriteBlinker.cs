using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteBlinker : MonoBehaviour
{
    Image thisImage;
    float alpha = 1.0f;
    [SerializeField]
    float blinkTime = 2.0f;
    float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        thisImage = gameObject.GetComponent<Image>();
        _SetImageAlpha();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / blinkTime * Mathf.PI * 2.0f;
        if (t >= Mathf.PI * 2.0f) t = 0.0f;
        alpha = (Mathf.Sin(t) + 1.0f) / 2.0f;
        _SetImageAlpha();
    }

    private void _SetImageAlpha()
    {
        if (thisImage)
        {
            Color _thisColor = thisImage.color;
            thisImage.color = new Color(_thisColor.r, _thisColor.g, _thisColor.b, alpha);
        }
    }
}
