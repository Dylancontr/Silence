using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private Collider2D box;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<Collider2D>();
        box.enabled = false;
        anim = GetComponent<Animator>();
        anim.SetBool("Idle", true);
    }

    void activate()
    {
        box.enabled = true;
    }

    void deactivate()
    {
        box.enabled = false;
        anim.SetBool("Idle", true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            anim.SetBool("Idle", false);
    }
}
