using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashImage : MonoBehaviour
{
    public bool flash = false;
    Image flashImage;
    // Update is called once per frame

    void Start()
    {
        flashImage = GameObject.Find("Flash").GetComponent<Image>();
    }

    void Update()
    {
        //gradually lower image alpha
        if (flash)
        {
            Color start = flashImage.color;
            start.a = Mathf.Lerp(start.a, 0.0f, Time.deltaTime * 0.4f);
            flashImage.color = start;
            if(start.a == 0f)
            {
                flash = false;
            }
        }
    }
}
