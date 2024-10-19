using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockController : MonoBehaviour
{
    // Start is called before the first frame update

    private float dir;
    private Rigidbody2D rb;

    //private AudioSource hit;

    [SerializeField]
    private float speed;
    void Start()
    {
        //hit = gameObject.GetComponent<AudioSource>();
        
        //Debug.Log(transform.rotation.y);
        
        if (transform.rotation.y == 1f)
        {
            dir = -1f;
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else
        {
            dir = 1f;
            transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }

        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(speed * dir, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
