using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{

    public GameObject arrow,spawn;
    private float lastArrow, arrowDelay;


    // Start is called before the first frame update
    void Start()
    {
        arrowDelay = 2f;
        lastArrow = Time.time - arrowDelay;
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Time.time - lastArrow >= arrowDelay)
        {
            Instantiate(arrow, spawn.transform.position ,this.transform.rotation);
            lastArrow = Time.time;
        }
    }


}
