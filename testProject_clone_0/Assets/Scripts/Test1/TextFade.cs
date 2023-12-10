using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextFade : MonoBehaviour
{
    public float fadeInAlpha;
    // Start is called before the first frame update
    private void Awake()
    {
        FadeOut(0.1f);
    }

    public void FadeIn(float second)
    {
        GetComponent<Graphic>().CrossFadeAlpha(fadeInAlpha, second, false);
    }

    public void FadeOut(float second)
    {
        GetComponent<Graphic>().CrossFadeAlpha(0f, second, false);
    }
}
