using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour
{
    public GameObject bandage;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            if (Random.Range(1, 4) == Random.Range(1,4))
            {
                
                Instantiate(bandage, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
