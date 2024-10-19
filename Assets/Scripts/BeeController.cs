using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    private Animator anim;
    private int health;

    public GameObject player;
    [SerializeField]
    private float speed;
    //private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        health = 2;

        anim = gameObject.GetComponent<Animator>();
        //playerController = player.GetComponent<PlayerController>();

        speed = Random.Range(0.35f, 0.65f);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("isAttack");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rock")
        {
            Destroy(collision.gameObject);
            GetHit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
