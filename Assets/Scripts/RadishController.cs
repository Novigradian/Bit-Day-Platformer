using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadishController : MonoBehaviour
{
    private Animator anim;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 3;

        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("e");
        if (collision.gameObject.tag == "Player")
        {
            GetHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("e");
        if (collision.gameObject.tag == "Rock")
        {
            //Debug.Log("hit");
            
            Destroy(collision.gameObject);
            GetHit();
        }
    }

    private void GetHit()
    {
        //Debug.Log("hit");

        health -= 1;
        anim.SetTrigger("isHit");

        if (health == 0)
        {
            Destroy(gameObject, 0.6f);
        }
    }

    
}
