using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CreateMaze : MonoBehaviour
{

    public Camera rotateCamera;
    public Camera mainCamera;
    bool enableCam;

    public Material[] cubeMaterials;

    public int hammers;
    public int num;
    public int levels;

    public int hammersUsed;

    private int[] playerSpawn = new int[2];

    public GameObject greenPrefab;
    public GameObject redPrefab;
    public GameObject bluePrefab;
    public GameObject T1Prefab;
    public GameObject T2Prefab;
    public GameObject T3Prefab;
    public GameObject WPrefab;
    public GameObject playerPrefab;

    //temporarily saving previous portal in order to connect each other
    GameObject prevPortal;
    
    //limit fps in order to retain smoothness in movement
    void Awake()
    {
        QualitySettings.vSyncCount = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/Source/file.maz";
        //Read the text from directly from the file.maz file
        StreamReader reader = new StreamReader(path);
        string line;
        //number of maze levels
        line = reader.ReadLine();
        levels = line[2] - '0';
        //number of N
        line = reader.ReadLine();
        string numString = "";
        for (int i = 2; i < line.Length; i++)
        {
            numString += line[i];
        }
        num = System.Convert.ToInt32(numString);
        //number of hammers
        line = reader.ReadLine();
        hammers = line[2] - '0';

        for (int lev = 0; lev < levels; lev++)
        {
            //list for empty location in 1st level.
            ArrayList emptyLocations = new ArrayList();

            //skip first line for each level ("LEVEL 1")
            line = reader.ReadLine();
            //read text lines and place blocks
            for (int x = 0; x < num; x++)
            {
                //get line
                line = reader.ReadLine();
                //remove spaces
                line = line.Replace(" ", string.Empty);
                //counter of items placed for this line.
                int count = 0;
                
                //places objects depending on type in .maz file
                // for the position vector: (x,z) and y is the height
                for (int y = 0; y < line.Length; y++)
                {
                    if(line[y] == 'E') {
                        if (lev == 0)
                        {
                            int[] location = { count, x };
                            emptyLocations.Add(location);
                        }
                        count++;
                        //place nothing
                    }
                    else if (line[y] == 'G') {
                        Instantiate(greenPrefab, new Vector3(count, lev, x), Quaternion.identity);
                        count++;
                    }
                    else if (line[y] == 'W')
                    {
                        GameObject portal = Instantiate(WPrefab, new Vector3(count, lev, x), Quaternion.identity);
                        if(prevPortal == null)
                        {
                            prevPortal = portal;
                        }
                        else
                        {
                            prevPortal.GetComponent<Portal>().nextPortal = portal;
                            prevPortal = portal;
                        }
                        count++;
                    }
                    else if (line[y] == 'R') {
                        Instantiate(redPrefab, new Vector3(count, lev, x), Quaternion.identity);
                        count++;
                    }
                    else if (line[y] == 'B') {
                        Instantiate(bluePrefab, new Vector3(count, lev, x), Quaternion.identity);
                        count++;
                    }
                    else if (line[y] == 'T') {
                        if (line[y+1] == '1') {
                            Instantiate(T1Prefab, new Vector3(count, lev, x), Quaternion.identity);
                            count++;
                        }
                        else if (line[y + 1] == '2') {
                            Instantiate(T2Prefab, new Vector3(count, lev, x), Quaternion.identity);
                            count++;
                        }
                        else if (line[y + 1] == '3') {
                            Instantiate(T3Prefab, new Vector3(count, lev, x), Quaternion.identity);
                            count++;
                        }
                    }
                }
            }
            //choose a random empty location from arraylist and spawm the player in it.
            if(lev == 0)
            {
                playerSpawn = (int[])emptyLocations[(int)Random.Range(0, emptyLocations.Count)];
                Instantiate(playerPrefab, new Vector3(playerSpawn[0], lev, playerSpawn[1]), Quaternion.identity);
            }
        }
        reader.Close();

        //make a wall around tha maze
        GameObject ground0 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground0.transform.position = new Vector3(num / 2, levels+1, num / 2);
        ground0.transform.localScale = new Vector3(num + 1, 1, num + 1);
        ground0.GetComponent<MeshRenderer>().enabled = false;


        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.transform.position = new Vector3(num/2, -1, num / 2);
        ground.transform.localScale = new Vector3(num + 1, 1, num + 1);
        ground.GetComponent<MeshRenderer>().enabled = false;
        
        GameObject ground2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground2.transform.position = new Vector3(num / 2, levels/2, num);
        ground2.transform.localScale = new Vector3(num + 1, levels+1, 1);
        ground2.GetComponent<MeshRenderer>().enabled = false;

        GameObject ground3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground3.transform.position = new Vector3(num / 2, levels / 2, -1);
        ground3.transform.localScale = new Vector3(num + 1, levels + 1, 1);
        ground3.GetComponent<MeshRenderer>().enabled = false;
        
        GameObject ground4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground4.transform.position = new Vector3(-1, levels / 2, num / 2 -1);
        ground4.transform.localScale = new Vector3(1, levels + 1, num +1);
        ground4.GetComponent<MeshRenderer>().enabled = false;

        GameObject ground5 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground5.transform.position = new Vector3(num, levels / 2, num / 2);
        ground5.transform.localScale = new Vector3(1, levels + 1, num+1);
        ground5.GetComponent<MeshRenderer>().enabled = false;
        
        //find player camera
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        rotateCamera.enabled = false;

        //reset material transparency
        for (int i = 0; i < cubeMaterials.Length; i++)
        {
            Color matColor = cubeMaterials[i].color;
            matColor.a = 1.0f;
            cubeMaterials[i].color = matColor;
        }

        rotateCamera.transform.position = new Vector3(num/2, 2, -10);
    }

    void Update()
    {

        //change cameras on player input
        if (Input.GetKeyDown("c"))
        {
            if (mainCamera.enabled)
            {
                mainCamera.enabled = false;
                rotateCamera.enabled = true;
                GameObject.Find("Player(Clone)").GetComponent<Player>().paused = true;
                //make objects transparent
                for(int i = 0; i < cubeMaterials.Length; i++)
                {
                    Color matColor = cubeMaterials[i].color;
                    matColor.a = 0.37f;
                    cubeMaterials[i].color = matColor;
                }
                enableCam = true;

            }else
            {
                mainCamera.enabled = true;
                rotateCamera.enabled = false;
                Text score = GameObject.Find("Score").GetComponent<Text>();
                //if player has pressed X or E then retain paused state.
                if (score.enabled == false)
                {
                    GameObject.Find("Player(Clone)").GetComponent<Player>().paused = false;
                }
                for (int i = 0; i < cubeMaterials.Length; i++)
                {
                    Color matColor = cubeMaterials[i].color;
                    matColor.a = 1.0f;
                    cubeMaterials[i].color = matColor;
                }
                enableCam = false;
            }
        }
        //rotate camera left and right
        Vector3 Pivot = new Vector3(num/2 , levels/2, num/2);
        if (Input.GetKey("j")&&enableCam)
        {
            rotateCamera.transform.RotateAround(Pivot, rotateCamera.transform.up, Time.deltaTime * 10.0f);
        }
        if (Input.GetKey("k") && enableCam)
        {
            rotateCamera.transform.RotateAround(Pivot, -rotateCamera.transform.up, Time.deltaTime * 10.0f);
        }
    }

}
