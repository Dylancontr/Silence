using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private float hitForce, speed;
    private float iFrames, duration;
    private float moveHorizontal, moveVertical;
    private float attackDelay, LastAttack;
    private int Health;
    public bool freeze;
    private bool trans;

    private Rigidbody2D myBody;
    private Animator anim;
    private SpriteRenderer rend;
    public GameObject space;
    private HealthController hearts;
    private KeyCode LastKey;


    // Use this for initialization
    void Start()
    {
        Health = 6;
        hearts = GetComponent<HealthController>();
        hearts.setHealth(Health);
        myBody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetBool("isAttacking", false);
        duration = 2.5f;
        iFrames = -duration;
        hitForce = 1f;
        speed = 2.5f;
        moveHorizontal = 0;
        moveVertical = 0;
        attackDelay = .5f;
        LastAttack = 0f;
        space.GetComponent<PolygonCollider2D>().enabled = false;
        LastKey = KeyCode.S;
        freeze = false;
        trans = false;

    }

    void Update()
    {
        if (!PauseMenu.gamePaused)
        {
            if (!freeze)
            {
                //get movement values
                if (Input.GetKey(KeyCode.D))
                {
                    if (!Input.GetKey(KeyCode.A)) moveHorizontal = 1f;

                    if (!Input.GetKey(LastKey))
                    {
                        setDirFalse();
                        anim.SetBool("Right", true);
                        anim.SetBool("Up", false);
                        anim.SetBool("Down", false);
                        anim.SetBool("Left", false);
                        space.transform.rotation = Quaternion.Euler(0, 0, 90);
                        LastKey = KeyCode.D;
                    }
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    moveHorizontal = -1f;
                    if (!Input.GetKey(LastKey))
                    {
                        setDirFalse();
                        anim.SetBool("Left", true);
                        anim.SetBool("Up", false);
                        anim.SetBool("Down", false);
                        anim.SetBool("Right", false);
                        space.transform.rotation = Quaternion.Euler(0, 0, 270);
                        LastKey = KeyCode.A;
                    }
                }
                else moveHorizontal = 0f;

                if (Input.GetKey(KeyCode.W))
                {
                    if (!Input.GetKey(KeyCode.S)) moveVertical = 1f;
                    if (!Input.GetKey(LastKey))
                    {
                        setDirFalse();
                        anim.SetBool("Up", true);
                        anim.SetBool("Right", false);
                        anim.SetBool("Down", false);
                        anim.SetBool("Left", false);
                        space.transform.rotation = Quaternion.Euler(0, 0, 180);
                        LastKey = KeyCode.W;
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    moveVertical = -1f;
                    if (!Input.GetKey(LastKey))
                    {
                        setDirFalse();
                        anim.SetBool("Down", true);
                        anim.SetBool("Up", false);
                        anim.SetBool("Right", false);
                        anim.SetBool("Left", false);
                        space.transform.rotation = Quaternion.Euler(0, 0, 0);
                        LastKey = KeyCode.S;
                    }
                }
                else moveVertical = 0f;


                if (!SoundController.audio[1].isPlaying)
                {
                    if (moveHorizontal != 0f || moveVertical != 0f)
                    {
                        SoundController.playerWalking = true;
                        anim.SetBool("Walking", true);
                    }
                    else if (moveHorizontal == 0f && moveVertical == 0f)
                    {
                        SoundController.playerWalking = false;
                        SoundController.audio[1].Stop();
                        anim.SetBool("Walking", false);
                    }
                }
                else if (moveHorizontal == 0f && moveVertical == 0f)
                {
                    SoundController.playerWalking = false;
                    SoundController.audio[1].Stop();
                    anim.SetBool("Walking", false);
                }




                Vector2 movement = new Vector2(moveHorizontal * speed, moveVertical * speed);
                myBody.velocity = movement;


                if (Input.GetKeyDown(KeyCode.Space) && Time.time - LastAttack >= attackDelay)
                {
                    anim.SetBool("isAttacking", true);
                    space.GetComponent<PolygonCollider2D>().enabled = true;
                    LastAttack = Time.deltaTime;
                    freeze = true;
                    movement = new Vector2(0, 0);
                    myBody.velocity = movement;
                    moveHorizontal = 0;
                    moveVertical = 0;
                }
            }
            else
            {
                Vector2 movement = new Vector2(0, 0);
                myBody.velocity = movement;
            }
            if(Health == 0)
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    private void FixedUpdate()
    {
    }

    /*
    //changes transparency of player
    void changeTrans()
    {
        for (float t = 0.0f; t < 1.0f; t += .1f)
        {
            Color nC = new Color(1, 1, 1, Mathf.Lerp(1, .5f, .1f));
            rend.color = nC;
        }
        for (float t = 0.0f; t < 1.0f; t += .1f)
        {
            Color nC = new Color(1, 1, 1, Mathf.Lerp(.5f, 1, .1f));
            rend.color = nC;
        }
    }
    */

    //on collison
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy" && iFrames <= Time.time - duration)
        {
            takeDamage(1);
        }

        if (collision.gameObject.tag == "Trap" && iFrames <= Time.time - duration)
        {
            takeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bandage")
        {
            healDamage(1);
            Destroy(collision.gameObject);
        }

        /*if (Time.time > iFrames)//if pressing e and not already been hit
        {
             if (true)
        {
            if (Time.time > iFrames)//if pressing e and not already been hit
            {
                Vector2 knockBack = new Vector2(-prevH * hitForce, -prevV * hitForce);//vector w/ opposite of last movement
                myBody.velocity = knockBack;//apply knockback
                iFrames = Time.time + duration;//time when control is resumed
            }
        }//time when control is resumed
        }*/
    }
    void healDamage(int heal)
    {
        if (Health + heal <= 6) Health += heal;
        else if (Health + heal > 6) Health = 6;
        StartCoroutine(hearts.updateHealth(Health));
    }

    void takeDamage(int damage)
    {
        SoundController.playerDmg = true;
        iFrames = Time.time;
        StartCoroutine(changeTrans());
        Health -= damage;
        if (Health < 0) Health = 0; //clamps min health
        else if (Health > 6) Health = 6;//clamps max health
        StopCoroutine(hearts.updateHealth(Health));
        StartCoroutine(hearts.updateHealth(Health));//updates heart bar
    }

    private void setAttack()
    {
        freeze = false;
        space.GetComponent<PolygonCollider2D>().enabled = false;
        anim.SetBool("isAttacking", false);
    }

    void setDirFalse()
    {
        anim.SetBool("Up", false);
        anim.SetBool("Down", false);
        anim.SetBool("Left", false);
        anim.SetBool("Right", false);
    }

    IEnumerator changeTrans()
    {
        while (iFrames > Time.time - duration)
        {
            if (!trans)
            {
                Color nC = new Color(1, 1, 1, .5f);
                rend.color = nC;
                trans = true;
            }
            else
            {
                Color nC = new Color(1, 1, 1, 1);
                rend.color = nC;
                trans = false;
            }
            yield return new WaitForSeconds(.25f);
        }
        if (trans)
        {
            Color nC = new Color(1, 1, 1, 1);
            rend.color = nC;
            trans = false;
        }
    }
}
