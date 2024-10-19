using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource slash;
    
    public GameObject rock;
    private float rockDir;

    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    public int health;

    private Rigidbody2D rb;
    private Rigidbody2D groundRb;
    private Transform tr;
    private float inputDirection;
    public float speed;

    private bool isSprint;
    private float sprintMultiplier;

    private Vector2 moveDir;

    private bool isHit;
    private bool isDashing;

    private bool isSlowed;

    private bool isGrounded;
    private float jumpCount;

    private Vector2 groundVelocity;

    [SerializeField]
    private Vector2 knockBackDirection;

    private Animator anim;

    public GameObject defeatPanel;
    public GameObject victoryPanel;

    float dir;
    void Start()
    {
        slash = gameObject.GetComponent<AudioSource>();        
        Time.timeScale = 1f;

        GetComponents();

        jumpCount = 0f;
        isSprint = false;
        isHit = false;
        isDashing = false;

        defeatPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    private void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            defeatPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        Keyboard kb = Keyboard.current;

        //inputDirection = Input.GetAxis("Horizontal");

        RotatePlayer();

        SetRunAnim();

        ResetIfFall();

    }

    private void ResetIfFall()
    {
        if (transform.position.y < -4f)
        {
            transform.position = new Vector3(4.6f, 2f, 0f);
            health -= 1;
            uiManager.setHealth(health);
        }
    }

    private void FixedUpdate()
    {
        if (!isHit && !isDashing)
        {
            AddHorizontalVelocity();

            AddGroundVelocity();
        }
    }

    private void AddGroundVelocity()
    {
        if (isGrounded)
        {
            groundVelocity = groundRb.velocity;
            rb.velocity += groundVelocity;
        }
    }

    private void AddHorizontalVelocity()
    {
        sprintMultiplier = 1f;
        if (isSprint && isGrounded)
        {
            sprintMultiplier = 2f;
        }

        rb.velocity = new Vector2(moveDir.x * speed * Time.deltaTime * sprintMultiplier, rb.velocity.y);
    }

    private void SetRunAnim()
    {
        if (moveDir.x != 0f)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    }

    private void RotatePlayer()
    {
        if (moveDir.x < 0)
        {
            tr.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else if (moveDir.x > 0)
        {
            tr.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    public void OnSprint()
    {
        isSprint = !isSprint;
    }

    void OnMove(InputValue action)
    {
        moveDir = action.Get<Vector2>();
    }

    void OnJump()
    {
        if (jumpCount <= 1)
        {
            anim.SetBool("isJump", true);

            rb.AddForce(transform.up * 1.25f, ForceMode2D.Impulse);
            jumpCount += 1;
            StartCoroutine(waitSeconds(1f));
            isGrounded = false;
        }
    }

    void OnDash( )
    {
        if (!isGrounded && !isDashing)
        {
            isDashing = true;

            if (transform.rotation.y == 0f)
            {
                dir = 1f;
            }
            else
            {
                dir = -1f;
            }

            rb.AddForce(new Vector2(dir * 90f, 0f), ForceMode2D.Force);
            
            Debug.Log(dir);

            StartCoroutine(waitSeconds(1f));
        }
    }

    void OnAttack()
    {
        slash.Play();
        Instantiate(rock, transform.position, transform.rotation);
        Debug.Log((rock, transform.position, transform.rotation));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            rb.velocity = new Vector2(rb.velocity.y * 0.1f, rb.velocity.x);

            jumpCount = 0f;
            anim.SetBool("isJump", false);

            groundRb = collision.gameObject.GetComponent<Rigidbody2D>();
        }
        else if (collision.gameObject.tag == "Goal")
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (isHit)
            {
                return;
            }
            isHit = true;


            slash.Play();


            //isHit = true;
            //rb.AddForce(-rb.velocity * 7500f);
            Vector3 hitPosition = collision.gameObject.transform.position;
            if ((transform.position - hitPosition).x >= 0)
            {
                knockBackDirection = new Vector2(Mathf.Abs(knockBackDirection.x), knockBackDirection.y);
            }
            else
            {
                knockBackDirection = new Vector2(-Mathf.Abs(knockBackDirection.x), knockBackDirection.y);
            }
            rb.AddForce(knockBackDirection, ForceMode2D.Impulse);

            anim.SetTrigger("isHit");

            health -= 1;
            uiManager.setHealth(health);
            StartCoroutine(waitSeconds(0.5f));

            
        }
    }

    private IEnumerator waitSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        isHit = false;
        isDashing = false;
    }
}
