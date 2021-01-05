using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public bool isEnabled;
    public GameObject nextPortal;

    AudioSource audio4;

    // Start is called before the first frame update
    void Start()
    {
        audio4 = GetComponent<AudioSource>();
        if (nextPortal == null)
        {
            isEnabled = false;
        }
        else
        {
            isEnabled = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player(Clone)")
        {
            //teleport player to next portal, disabling character controller because it prevents the change of transport.position
            if (isEnabled)
            {
                //teleport player, start the image fade script and play sound
                other.GetComponent<CharacterController>().enabled = false;
                other.gameObject.transform.position = nextPortal.transform.position;
                audio4.Play(0);
                isEnabled = false;
                nextPortal.GetComponent<Portal>().isEnabled = false;
                other.GetComponent<CharacterController>().enabled = true;
                Image temp = GameObject.Find("Flash").GetComponent<Image>();
                Color c = temp.color;
                c.a = 1.0f;
                temp.color = c;
                GameObject.Find("Flash").GetComponent<FlashImage>().flash = true;
            }
        }
    }
}
