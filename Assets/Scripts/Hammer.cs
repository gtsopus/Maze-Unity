using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{

    public Animator hammerAnim;

    public int health = 100;
    GameObject gameManager;

    public Material hammerMat;

    public int hammersLeft;

    AudioSource audio2;

    // Start is called before the first frame update
    void Start()
    {
        audio2 = GetComponent<AudioSource>();
        hammerAnim = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager");
        hammerMat.color = new Color((214 * health / 100) / 255f, (57 * health / 100) / 255f, (31 * health / 100) / 255f);
    }

    // Update is called once per frame
    void Update()
    {

        bool paused = GameObject.Find("Player(Clone)").GetComponent<Player>().paused;

        if (!paused)
        {
            //code to destroy blocks
            if (Input.GetKeyDown("h"))
            {
                //get hammer values from gameManager
                hammersLeft = gameManager.GetComponent<CreateMaze>().hammers;
                if (health > 0 && hammersLeft > 0)
                {
                    //cast ray from current mouse position (cursor locked in the middle of the screen)
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        //if hit is cube
                        if (hit.transform.tag == "Cube")
                        {

                            audio2.Play(0);
                            //use hammer
                            gameManager.GetComponent<CreateMaze>().hammers = hammersLeft - 1;
                            hammersLeft -= 1;
                            gameManager.GetComponent<CreateMaze>().hammersUsed++;
                            health -= 10;

                            //animation
                            hammerAnim.Play("HammerHit", -1, 0f); 
                            //reduce cubes health
                            hit.transform.gameObject.GetComponent<Cube>().health -= 1;

                            //change color depending on health (1.0f,1.0f,1.0f) style
                            hammerMat.color = new Color((214 * health / 100) / 255f, (57 * health / 100) / 255f, (31 * health / 100) / 255f);
                        }
                    }
                }
            }
        }
    }
}
