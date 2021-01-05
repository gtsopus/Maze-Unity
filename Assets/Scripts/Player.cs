using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public CharacterController playerController;

    public Transform isOnGround;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    public float speed = 4.5f;
    public float gravity = -10;
    public float jumpHeight = 2f;

    public Vector3 velocity;
    public bool isGrounded;

    GameObject gameManager;

    public bool paused = false;

    AudioSource audio3;

    Text score;
    Text gOver;

    void Start()
    {
        audio3 = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager");
        score = GameObject.Find("Score").GetComponent<Text>();
        gOver = GameObject.Find("GameOver").GetComponent<Text>();
        gOver.enabled = false;
        score.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //stop game
        if (Input.GetKeyDown("x"))
        {
            paused = true;
            gOver.enabled = true;
            score.enabled = true;
            if(gameObject.transform.position.y > 2.8)
            {
                int Num = gameManager.GetComponent<CreateMaze>().num;
                int totalScore = (int)(Num * Num - Time.realtimeSinceStartup - Mathf.Floor(gameManager.GetComponent<CreateMaze>().hammersUsed / 3) * 50);
                score.text = "Score: " + totalScore.ToString();
            }
            else
            {
                score.text = "Score: 0";
            }
        }
        //end game 
        if (Input.GetKeyDown("e") && gameObject.transform.position.y > 2.8)
        {
            paused = true;
            gOver.enabled = true;
            score.enabled = true;
            int Num = gameManager.GetComponent<CreateMaze>().num;
            int totalScore = (int)(Num * Num - Time.realtimeSinceStartup - Mathf.Floor(gameManager.GetComponent<CreateMaze>().hammersUsed/3) * 50);
            score.text = "Score: " + totalScore.ToString();
        }
        if (!paused)
        {
            //checks if there is anything below player
            isGrounded = Physics.CheckSphere(isOnGround.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            //get inputs
            //from -1 to 1 vertical , 1 is forward
            //from -1 to 1 horizontal, 1 is right
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //move the player
            Vector3 move = transform.right * x + transform.forward * z;

            playerController.Move(move * speed * Time.deltaTime);


            //free fall height = srtq(
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                audio3.Play(0);
            }

            velocity.y += gravity * Time.deltaTime;
            //Δy = 1/2 * gravity * time * time
            playerController.Move(velocity * Time.deltaTime);
        }
        }
    //get drops from ground and destroy the drop
    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.transform.tag == "Drop") {
            gameManager.GetComponent<CreateMaze>().hammers += 1;
            Destroy(hit.gameObject);
        }

    }
}
