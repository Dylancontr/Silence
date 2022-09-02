using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject[] Array;
    public GameObject player, camera;
    private BoxCollider2D exit;
    private float speed;//must be less than 1
    private bool moveC, moveP; //if moving
    private Vector3 targetC, targetP, smoothC, smoothP; //target positions and smooth value
    private int interp, time; //interpolations needed and time

    // Start is called before the first frame update
    void Start()
    {
        exit = GetComponent<BoxCollider2D>();
        speed = .02f; //how long transitions take
        camera.transform.position = new Vector3(0, 0,-11);
        moveC = false;
        moveP = false;
        targetC = transform.position;
        targetP = player.transform.position;
        interp = (int)(1 / speed); //formula for interp
        time = 0;
    }

    //Update does it too quickly
    void FixedUpdate()
    {
        if (!PauseMenu.gamePaused) { 
            if (moveC)
            {
                MoveCamera();
            }
            if (moveP)
            {
                MovePlayer();
            }
            ++time;
            }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //disable transition hitboxes
            exit.enabled = false;
            Array[0].GetComponent<BoxCollider2D>().enabled = false;
            //move the camera
            moveC = true;

            //move the player
            moveP = true;

            collision.GetComponent<PlayerController>().freeze = true; //freezes player control

            //determines smooth for camera
            targetC = Array[0].transform.position;
            smoothC = Vector3.Lerp(camera.transform.position, targetC, speed);
            smoothC -= camera.transform.position;
            smoothC.z = 0;

            //determines smooth for player
            targetP = Array[1].transform.position;
            smoothP = Vector3.Lerp(player.transform.position, targetP, speed);
            smoothP -= player.transform.position;
            time = 0;//resets time for interp
        }
    }

    private void MoveCamera()
    {
        if (time >= interp)
        {
            moveC = false;
        }
        else
        {
            camera.transform.position += smoothC; //moves camera smoothC
        }
    }

    private void MovePlayer()
    {
        
        if (time >= interp)
        {
            moveP = false;
            player.GetComponent<PlayerController>().freeze = false; //restores player control
            exit.enabled = true; //reinstate transition hitboxes
            Array[0].GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            player.transform.position += smoothP; //moves player smoothP
        }
    }
}
