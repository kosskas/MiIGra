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
    [SerializeField] AudioClip bSound;
    [SerializeField] AudioClip enemyDeathSound;
    [SerializeField] AudioClip playerDeathSound;
    [SerializeField] AudioClip heartSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip keySound;
    [SerializeField] AudioClip winSound;
    private AudioSource source;
    private bool canDoubleJump = false;
    public float offset = 0.2f;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {

        startPosition = transform.position;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
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
            Debug.DrawRay(transform.position + new Vector3(0, 0, 0), rayLength * Vector3.down, Color.cyan, 0.1f, false);
            Debug.DrawRay(transform.position + new Vector3(offset , 0, 0), rayLength * Vector3.down, Color.red, 0.1f, false);
            Debug.DrawRay(transform.position + new Vector3(-offset, 0, 0), rayLength * Vector3.down, Color.red, 0.1f, false);
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
            source.PlayOneShot(jumpSound, AudioListener.volume);
            rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("jumping");
            canDoubleJump = true;
        }
        else
        {
            if (canDoubleJump)
            {
                canDoubleJump = false;
                source.PlayOneShot(jumpSound, AudioListener.volume);
                rigidBody.AddForce(Vector2.up * jumpForce * 0.75f, ForceMode2D.Impulse);
            }
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
        //return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
        return Physics2D.Raycast(this.transform.position + new Vector3(0, 0, 0), Vector2.down, rayLength, groundLayer.value) ||
            Physics2D.Raycast(transform.position + new Vector3(offset, 0, 0), Vector2.down, rayLength, groundLayer.value) ||
            Physics2D.Raycast(transform.position + new Vector3(-offset, 0, 0), Vector2.down, rayLength, groundLayer.value);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Bonus //
        if (other.CompareTag("Bonus"))
        {
            source.PlayOneShot(bSound, AudioListener.volume);
            GameManager.instance.AddPoints(scoreIncrement);
            other.gameObject.SetActive(false);
           
            return;
        }

        // Enemy //
        if (other.CompareTag("Enemy"))
        {
            if (transform.position.y > other.gameObject.transform.position.y)
            {
                source.PlayOneShot(enemyDeathSound, AudioListener.volume);
                GameManager.instance.AddKill();
                //score += scoreIncrement;
                Debug.Log("Killed an enemy");
            }
            else
            {
                source.PlayOneShot(playerDeathSound, AudioListener.volume);
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
                source.PlayOneShot(winSound, AudioListener.volume);
                GameManager.instance.AddPoints(100 * GameManager.instance.lives);
                //gameOverManager.SetGameOver();
                GameManager.instance.LevelCompleted();
            }

            return;
        }

        // Heart //
        if (other.CompareTag("Heart"))
        {
            source.PlayOneShot(heartSound, AudioListener.volume);
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
            source.PlayOneShot(keySound, AudioListener.volume);
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
