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
    public int scoreIncrement = 1;
    [SerializeField] UIManager gameOverManager;
   // private int lives = 3;
    private Vector2 startPosition;
    //private int keysFound = 0;
   // private const int keysNumber = 3;

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
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            // transform.Translate(moveSpeed, 0.0f, 0.0f, Space.World);
            HandleMovement();
            animator.SetBool("IsGrounded", isGrounded());
            animator.SetBool("IsWalking", isWalking);

            // Debug.DrawRay(transform.position, rayLength * Vector3.down, Color.white, 1,false);
        }
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
        // Bonus //
        if (other.CompareTag("Bonus"))
        {
            GameManager.instance.AddPoints(scoreIncrement);
            other.gameObject.SetActive(false);
            return;
        }

        // Enemy //
        if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                GameManager.instance.AddKill();
                //score += scoreIncrement;
                Debug.Log("Killed an enemy");
            }
            else
            {
                transform.position = startPosition;
                GameManager.instance.AddHeart(-1);
                //lives--;
                /*
                if (lives == 0)
                {
                    Debug.Log("gameover");
                }
                else
                {
                    Debug.Log("Num of lives: " + lives);
                    transform.position = startPosition;
                }
                */
            }
        }

        // FallLevel //
        if (other.CompareTag("FallLevel"))
        {
            Death();
        }

        // GameOver //
        if (other.CompareTag("Exit"))
        {
            if (GameManager.instance.keysFound == GameManager.instance.keysToFound)
            {
                GameManager.instance.AddPoints(100 * GameManager.instance.lives);
                //gameOverManager.SetGameOver();
                GameManager.instance.LevelCompleted();
            }

            return;
        }

        // Heart //
        if (other.CompareTag("Heart"))
        {
            GameManager.instance.AddHeart(1);
            //lives++;
            //Debug.Log("Lives: " + lives);
            other.gameObject.SetActive(false);
            return;
        }

        // Key // //test
        if (other.CompareTag("Key"))
        {
            //keysFound++;
            //Debug.Log("Keys: " + keysFound);
            GameManager.instance.AddKeys();
            other.gameObject.SetActive(false);
            /*
            if(keysFound == keysNumber)
            {
                Debug.Log("Found all keys!");
            }
            */
            return;
        }

        // MovingPlatform //
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }

    private void Death()
    {
        transform.position = startPosition;
        //lives--;
        GameManager.instance.AddHeart(-1);
        Debug.Log("gameover");
        
    }
}
