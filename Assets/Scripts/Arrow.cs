using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private Rigidbody2D rb;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gamePaused) { 
            Vector2 movement;
            if (transform.rotation.z == 0)
                movement = new Vector2(0, -speed);
            else if (transform.rotation.z == 90)
                movement = new Vector2(speed, 0);
            else if (transform.rotation.z == 180)
                movement = new Vector2(0, speed);
            else if (transform.rotation.z == 270)
                movement = new Vector2(-speed, 0);
            else
                movement = new Vector2(0, 0);
            rb.velocity = movement;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag != "Trap" || collision.gameObject.tag == "Bandage")
        {
            Destroy(this.gameObject);
        }
        
    }
}