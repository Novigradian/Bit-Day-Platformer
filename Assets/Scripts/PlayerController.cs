using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector]public bool dashUnlocked;
    [HideInInspector]public bool jumpUnlocked;
    [HideInInspector]public bool sprintUnlocked;
    [HideInInspector]public float jumpMultiplier;
    private int bulletsCount=0;
    private AudioSource slash;
    
    public GameObject rock;
    public TextMeshProUGUI AmmoUI;
    private float rockDir;

    [SerializeField]
    private UIManager uiManager;
    
    [HideInInspector] public int health=3;

    private Rigidbody2D rb;
    private Rigidbody2D groundRb;
    private Transform tr;
    private float inputDirection;
    [HideInInspector]public float speed=90;

    private bool isSprint;
    private float sprintMultiplier;

    private Vector2 moveDir;

    private bool isHit;
    private bool isDashing;

    private bool isSlowed;

    private bool isGrounded;
    private float jumpCount;

    private Vector2 groundVelocity;

    private Vector2 knockBackDirection = new Vector2(0.9f,0.9f);

    private Animator anim;

    public GameObject defeatPanel;
    public GameObject victoryPanel;

    float dir;
    void Start()
    {
        dashUnlocked=false;
        jumpUnlocked=false;
        sprintUnlocked=false;
        jumpMultiplier=1f;
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

        if (AmmoUI != null) {
            AmmoUI.text = "Ammo Count: " + bulletsCount.ToString();
        }
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

    public void AddBullet(int n) {
        bulletsCount += n;
        Debug.Log("Bullet+1");
    }

    private void AddHorizontalVelocity()
    {
        sprintMultiplier = 1f;
        if (isSprint && isGrounded && sprintUnlocked)
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
        if (jumpCount <= 0 || jumpCount <= 1 && jumpUnlocked)
        {
            anim.SetBool("isJump", true);

            rb.AddForce(transform.up * 1.25f * jumpMultiplier, ForceMode2D.Impulse);
            jumpCount += 1;
            StartCoroutine(waitSeconds(1f));
            isGrounded = false;
        }
    }

    void OnDash( )
    {
        if (!isGrounded && !isDashing && dashUnlocked)
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
        if (bulletsCount>0) {
            bulletsCount--;
            slash.Play();
            Instantiate(rock, transform.position, transform.rotation);
            Debug.Log((rock, transform.position, transform.rotation));
        }
        
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
