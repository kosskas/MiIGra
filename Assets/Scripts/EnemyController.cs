using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private bool isFacingRight = true;
    [SerializeField] private float moveSpeed = 10f;
    private Animator animator;
    private Rigidbody2D rigidBody;
    private float startPositionX;
    public float moveRange = 1.0f;
    private bool isMovingRight = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
        startPositionX = this.transform.position.x;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isMovingRight)
        {
            if(this.transform.position.x <= startPositionX + moveRange)
            {
                MoveRight();
            }
            else
            {
                Flip();
                MoveLeft();
            }
        }
        else
        {
            if (this.transform.position.x > startPositionX - moveRange)
            {
                MoveLeft();
            }
            else
            {

                Flip();
                MoveRight();
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        isMovingRight = !isMovingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

    }

    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (!isFacingRight)
            Flip();
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
        if (isFacingRight)
            Flip();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("Player") && transform.position.y < other.gameObject.transform.position.y)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(KillOnAnimationEnd()); 
        }
    }
    private IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
