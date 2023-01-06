using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
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
        
    }
    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (this.transform.position.x <= startPositionX + moveRange)
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

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        isMovingRight = !isMovingRight;
        

    }
}
