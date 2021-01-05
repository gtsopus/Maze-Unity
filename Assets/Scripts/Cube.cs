using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int health = 3;

    public GameObject Box;

    Renderer rend;

    AudioSource audio1;

    void Start()
    {
        rend = GetComponent<Renderer>();
        audio1 = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if (health == 0)
        {
            audio1.Play(0);
            int boxes = Random.Range(4, 8);
            for(int i = 0; i < boxes; i ++)
            {
                Vector3 curPos = this.gameObject.transform.position;
                
                curPos.x -= (rend.bounds.size.x/3);
                
                curPos.x += (float) 0.1 * i;
                curPos.y -= (rend.bounds.size.y / 2) - (float)0.07;

                GameObject box1 = Instantiate(Box) as GameObject;
                box1.transform.position = curPos;
                box1.gameObject.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
            }
            Destroy(this.gameObject);
            
        }    
    }
}
