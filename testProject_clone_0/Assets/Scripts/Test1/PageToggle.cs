using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageToggle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener((bool value) =>
        {
            if (value)
            {
                GameManager.instance.OnClick();
                GetComponent<Toggle>().interactable = false;
            }

        });
    }
}
