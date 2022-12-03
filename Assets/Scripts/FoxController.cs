using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f;
    public float jumpForce = 6.0f;
    private Rigidbody2D rigidBody;
    public LayerMask groundLayer;
    const float rayLength = 0.9f;
    private Animator animator;
    private bool isWalking = false;
    private bool isFacingRight = true;
    public int score = 0;
    public int scoreIncrement = 1;
    [SerializeField] UIManager gameOverManager;
    private int lives = 3;
    private Vector2 startPosition;
    private int keysFound = 0;
    private const int keysNumber = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(moveSpeed, 0.0f, 0.0f, Space.World);
        HandleMovement();
        animator.SetBool("IsGrounded", isGrounded());
        animator.SetBool("IsWalking", isWalking);

        // Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1,false);
    }



    private void HandleMovement()
    {
        //Prawo
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
            if (!isFacingRight)
                Flip();
        }
        //Lewo
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
            isWalking = true;
            if (isFacingRight)
                Flip();
        }
        else
        {
            isWalking = false;
        }

        //Góra
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
    }

    private void Jump()
    {
        if(isGrounded())
        {
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("jumping");
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }
    private bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Heart"))
        {
            lives++;
            Debug.Log("Lives: " + lives);
            other.gameObject.SetActive(false);
            return;
        }
        if (other.CompareTag("Bonus"))
        {
            score += scoreIncrement;
            Debug.Log("Score: " + score);
            other.gameObject.SetActive(false);
            return;
        }



        if (other.CompareTag("Key"))
        {
            keysFound++;
            Debug.Log("Keys: " + keysFound);

            other.gameObject.SetActive(false);
            if(keysFound == keysNumber)
            {
                Debug.Log("Found all keys!");
            }
            return;
        }

        if (other.CompareTag("GameOver"))
        {
            gameOverManager.SetGameOver();
            return;
        }

        if(other.CompareTag("Enemy") && transform.position.y > other.gameObject.transform.position.y)
        {
            score += scoreIncrement;
            Debug.Log("Killed an enemy");
        }
        else
        {
            transform.position = startPosition;
            lives--;
            if(lives == 0)
            {
                Debug.Log("gameover");
            }
            else
            {
                Debug.Log("Num of lives: "+lives);
                transform.position = startPosition;
            }
        }

        if(other.CompareTag("FallLevel"))
        {
            Death();
        }
    }

    private void Death()
    {
        transform.position = startPosition;
        lives--;
        Debug.Log("gameover");
        
    }
}
