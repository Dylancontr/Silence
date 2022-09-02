using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private Collider2D eyes, attack; 
    private Rigidbody2D rb;
    public GameObject Vision, Attack;
    private Animator anim;
    private Vector3 playerPos,direction;
    private Vector2 knockBack;
    private float attackTime, stimer, sdelay;
    private int health;
    private float iFrames, iduration, kbForce, speed, attackDelay, searchTime, searchDuration;
    private int state;//0 = inactive, 1 = persuing, 2 = going to last scene location, 3 = searching
    private bool attacking;

    // Use this for initialization
    protected void Start()
    {
        attacking = false;
        attackTime = -1f;
        attack = Attack.GetComponent<Collider2D>();
        attack.enabled = false;
        anim = GetComponent<Animator>();
        anim.SetBool("Attacking", false);
        rb = GetComponent<Rigidbody2D>();
        health = 3;
        iFrames = -1f;
        iduration = 1f;
        searchTime = -1f;
        searchDuration = 7f;
        attackDelay = 2f;
        kbForce = 2f;
        speed = 1.25f;
        eyes = Vision.GetComponent<Collider2D>();
        playerPos = transform.position;
        state = 0;
        direction = new Vector3(0, 0, 0);
        stimer = Time.time;
        sdelay = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.gamePaused)
        {
            switch (state)
            {
                case 0://stands there
                    rb.velocity = new Vector2(0f, 0f);
                    break;
                case 1://moves towards the player
                    Vector2 target = playerPos - transform.position;
                    if (target.magnitude < 2.5f && Time.time > attackTime)
                    {
                        startAttack();
                    }
                    else
                    {
                        target.Normalize();
                        rb.velocity = target * speed;
                    }
                    break;
                default://stand there
                    rb.velocity = new Vector2(0f, 0f);
                    break;
            }
            rb.velocity = new Vector2(0, 0);
        }
    }

    void FixedUpdate()
    {
        if (!PauseMenu.gamePaused)
        {
            if (Time.time <= iFrames)
            {
                rb.velocity = knockBack;
            }
            else if (attacking)
            {
                rb.velocity = new Vector2(0f, 0f);
            }
            else if (state == 1)
            {
                Vector2 target = playerPos - transform.position;
                if (target.magnitude < 4.25f && Time.time > attackTime)
                {
                    startAttack();
                }
                float zAxis = getZRotation(target.x, target.y);
                Vision.transform.rotation = Quaternion.Euler(0, 0, zAxis);
                target.Normalize();
                rb.velocity = target * speed;
            }
            else if (state == 2)
            {
                Vector2 target = playerPos - transform.position;
                float zAxis = getZRotation(target.x, target.y);
                Vision.transform.rotation = Quaternion.Euler(0, 0, zAxis);
                if (target.magnitude < 1)
                {
                    state = 3;
                    searchTime = Time.time + searchDuration;
                }
                else
                {
                    target.Normalize();
                    rb.velocity = target * speed;
                }
            }
            else if (state == 3)
            {
                if (Time.time > searchTime)
                {
                    state = 0;
                }
                else
                {
                    if (Time.time - stimer >= sdelay)
                    {
                        Search();
                    }
                    rb.velocity = direction;
                }
            }
            else
            {
                rb.velocity = new Vector2(0f, 0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            SoundController.enemyDmg = true;
            --health;
            if (health == 0) die();
            else
            {
                knockBack = transform.position - collision.transform.position;
                if (knockBack.x < -0.5) knockBack.x = -kbForce;
                else if (knockBack.x > 0.5) knockBack.x = kbForce;
                else {
                    knockBack.x = 0;
                }

                if (knockBack.y < -0.5) knockBack.y = -kbForce;
                else if (knockBack.y > 0.5) knockBack.y = kbForce;
                else {
                    knockBack.y = 0;
                }
                rb.velocity = knockBack;//apply knockback
                iFrames = Time.time + iduration;//time when control is resumed
            }
        }

        if(collision.gameObject.tag == "Player")
        {
            state = 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {  
            state = 1;
            playerPos = collision.transform.position;
            Vector2 target = playerPos - transform.position;
            if (target.magnitude < 2.5f && Time.time > attackTime)
                startAttack();
            else{
                float zAxis = getZRotation(target.x, target.y);
                Vision.transform.rotation = Quaternion.Euler(0, 0, zAxis);
                if (zAxis > -45 && zAxis < 45)
                {
                    Attack.transform.rotation = Quaternion.Euler(0, 0, -90);
                    anim.SetBool("right", true);
                    anim.SetBool("up", false);
                    anim.SetBool("left", false);
                }
                else if (zAxis > 45 && zAxis < 135)
                {
                    Attack.transform.rotation = Quaternion.Euler(0, 0, 0);
                    anim.SetBool("right", false);
                    anim.SetBool("up", true);
                    anim.SetBool("left", false);
                }
                else
                {
                    Attack.transform.rotation = Quaternion.Euler(0, 0, 90);
                    anim.SetBool("right", false);
                    anim.SetBool("up", false);
                    anim.SetBool("left", true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            state = 2;
            Search();
        }
    }

    private void stopAttack()
    {
        SoundController.enemyAttack = false;
        attacking = false;
        attack.enabled = false;
        anim.SetBool("Attacking", false);

    }

    private void startAttack()
    {
        SoundController.enemyAttack = true;
        attackTime = Time.time + attackDelay;
        attacking = true;
        anim.SetBool("Attacking", true);
    }

    public void enableAttack()
    {
        attack.enabled = true;
    }

    //get the angle to rotate to
    private float getZRotation(float x, float y)
    {
        if (x == 0 || y == 0) return transform.rotation.z;
        float zAxis = Mathf.Atan(y / x) * Mathf.Rad2Deg;//Gets angle between it and player, coverts to degrees
        if (x < 0) zAxis -= 180;//needed to look left
        return zAxis;
    }

    int RandomDirection()
    { 
        return Random.Range(1,9);
    }
    void Search()
    {
        switch (RandomDirection())
        {
            case 1://north
                direction = new Vector3(0,1,0);
                break;
            case 2://east
                direction = new Vector3(1, 0, 0);
                break;
            case 3://south
                direction = new Vector3(0, -1, 0);
                break;
            case 4://west
                direction = new Vector3(-1, 0, 0);
                break;
            case 5://northeast
                direction = new Vector3(1, 1, 0);
                break;
            case 6://southeast
                direction = new Vector3(1, -1, 0);
                break;
            case 7://southwest
                direction = new Vector3(-1, -1, 0);
                break;
            case 8://northwest
                direction = new Vector3(-1, 1, 0);
                break;
        }
        stimer = Time.time;
    }

    protected void setHealth(int h)
    {
        health = h;
    }

    public virtual void die()
    {
        Destroy(gameObject);
    }
}
