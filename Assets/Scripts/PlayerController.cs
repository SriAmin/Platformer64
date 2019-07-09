using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public Animator Animator;
    public float speed;
    public float jumpforce;
    float moveInput;
    Rigidbody2D rb;
    public BoxCollider2D boxCollider2D;
    public CircleCollider2D circleCollider2D;

    bool facingRight = true;

    bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    //public int extraJumpValue;
    bool jumped = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        Animator.SetFloat("Speed", Mathf.Abs(moveInput * speed));

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0) {
            Flip();
        }
    }

    private void Update()
    {
        if (isGrounded == true)
        {
            jumped = false;
            Animator.SetBool("isJumping", false);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && !jumped)
        {
            Animator.SetBool("isJumping", true);
            rb.velocity = Vector2.up * jumpforce;
            jumped = true;
            FindObjectOfType<AudioManager>().Play("Jump");
        }

    }
    void Flip() {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();

        if (enemy != null)
        {
            foreach (ContactPoint2D point in collision.contacts) {
                //Jump on Head
                if (point.normal.y >= 0.9f)
                {
                    rb.velocity = Vector2.up * 15;
                    enemy.Hurt();
                }

                //Attacked by Enemy
                else {
                    FindObjectOfType<AudioManager>().Play("PlayerDeath");

                    rb.velocity = Vector2.up * 5;
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                    Animator.SetBool("isDying", true);
                    boxCollider2D.enabled = false;
                    circleCollider2D.enabled = false;
                    Invoke("Hurt", 1.5f);
                }
            }
        }
    }

    void Hurt() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Application.LoadLevel(Application.loadedLevel);
    }
}
