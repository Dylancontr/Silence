using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthController : MonoBehaviour
{
    public GameObject h1;
    private Animator h1a;
    public GameObject h2;
    private Animator h2a;
    public GameObject h3;
    private Animator h3a;
    public int health;
    // Start is called before the first frame update
    void Start()
    {
        h1a = h1.GetComponent<Animator>();
        h1a.SetBool("full", true);
        h1a.SetBool("half", false);
        h2a = h2.GetComponent<Animator>();
        h2a.SetBool("full", true);
        h2a.SetBool("half", false);
        h3a = h3.GetComponent<Animator>();
        h3a.SetBool("full", true);
        h3a.SetBool("half", false);
    }

    // Update is called once per frame
    void Update()
    {/*
        health = player.GetComponent<TestPlayerController>().Health;
        if(health == 6)
        {
            h3a.SetBool("full", true);
            h3a.SetBool("half", false);
        }
        if (health == 5)
        {
            h3a.SetBool("half", false);
            h3a.SetBool("full", false);
            h3a.SetBool("empty", true);
        }
        if (health == 4)
        {
            h3a.SetBool("half", false);
            h3a.SetBool("full", false); 
            h2a.SetBool("full", true);
            h2a.SetBool("half", false);
        }
        if(health == 3)
        {
            h2a.SetBool("half", true);
            h2a.SetBool("full", false);
        }
        */
    }

    //Called during start of Player
    public void setHealth(int hp)
    {
        health = hp;
    }

    //Called by player when taking damage
    public IEnumerator updateHealth(int hp)
    {
        while (hp != health && health > 0 && health <= 6) ///Keeps going until Health matches passed health
        {
            switch (health)
            {
                case 6:
                    if (hp < health)
                    {
                        h3a.SetBool("full", false);
                        h3a.SetBool("half", true);
                        health--;
                    }
                    break;
                case 5:
                    if (hp < health)
                    {
                        h3a.SetBool("half", false);
                        health--;
                    }else if(hp > health)
                    {
                        h3a.SetBool("full", true);
                        h3a.SetBool("half", false);
                        health++;
                    }
                    break;
                case 4:
                    if(hp < health)
                    {
                        h2a.SetBool("full", false);
                        h2a.SetBool("half", true);
                        health--;
                    }else if(hp > health)
                    {
                        h3a.SetBool("half", true);
                        health++;
                    }
                    break;
                case 3:
                    if(hp < health)
                    {
                        h2a.SetBool("half", false);
                        health--;
                    }else if(hp > health)
                    {
                        h2a.SetBool("full", true);
                        h2a.SetBool("half", false);
                        health++;
                    }
                    break;
                case 2:
                    if(hp < health)
                    {
                        h1a.SetBool("full", false);
                        h1a.SetBool("half", true);
                        health--;
                    }else if(hp > health)
                    {
                        h2a.SetBool("half", true);
                        health++;
                    }
                    break;
                case 1:
                    if(hp < health)
                    {
                        h1a.SetBool("half", false);
                        health--;
                    }else if(hp > health)
                    {
                        h1a.SetBool("half", false);
                        h1a.SetBool("full", true);
                        health++;
                    }
                    break;
                default: //stops the loop
                    health = hp;
                    break;
            }
            yield return new WaitForSeconds(.28f);//Pauses and resumes after x seconds
        }
    }
}
