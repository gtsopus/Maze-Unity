using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public float dropChance = 30f;
    public GameObject hammerDrop;

    public int secondsOnGround = 4;

    float timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int seconds = (int)timer % 60;
        if(seconds >= secondsOnGround)
        {
            int dropped = Random.Range(1, 100);
            //drop hammers
            if(dropped <= dropChance)
            {
                GameObject drop = Instantiate(hammerDrop);
                Vector3 pos = this.transform.position;
                pos.y += 0.1f;
                drop.transform.position = pos;
                drop.transform.localScale = this.transform.localScale;
            }

            Destroy(this.gameObject);
        }
    }
}
