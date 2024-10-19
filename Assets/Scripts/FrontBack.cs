using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontBack : MonoBehaviour
{
    private Rigidbody2D rb;
    private float time;
    public float startDirection;
    public float timeLimit;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2 (speed*startDirection,rb.velocity.y);

        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        
        if (time > timeLimit)
        {
            rb.velocity = new Vector2(rb.velocity.x * -1f, rb.velocity.y);
            time = 0f;
        }
    }
}
