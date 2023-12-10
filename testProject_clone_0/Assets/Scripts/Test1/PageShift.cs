using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageShift : MonoBehaviour
{
    public GameObject page1;
    public GameObject page2;
    
    Transform Page1Spotlight;
    GameObject page1Title;
    GameObject page1Content;
    public Transform Page2Spotlight;
    public GameObject page2Title;
    public GameObject page2Content;
    // Start is called before the first frame update
    void Start()
    {
        Page1Spotlight = page1.transform.Find("Spotlight");
        page1Title = page1.transform.Find("Text").gameObject;
        page1Content = page1.transform.Find("Text (1)").gameObject;
    }
    public void ShowPage1()
    {
        Page1Spotlight.GetComponent<Animator>().SetTrigger("LightDown");
        page1Title.GetComponent<TextFade>().FadeIn(10f);
        page1Content.GetComponent<TextFade>().FadeIn(10f);
    }

    public void ShowPage2()
    {
        page2.SetActive(true);
        Page1Spotlight.GetComponent<Animator>().SetTrigger("LightUp");
        page1.SetActive(false);
        Page2Spotlight.GetComponent<Animator>().SetTrigger("LightDown");
        page2Title.GetComponent<TextFade>().FadeIn(10f);
        page2Content.GetComponent<TextFade>().FadeIn(10f);
    }

}
