using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    public float moveRange = 2.0f;
    private float startPositionX;
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
                ChangeDirection();
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
                ChangeDirection();
                MoveRight();
            }
        }
    }

    private void MoveRight()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    private void MoveLeft()
    {
        transform.Translate(-moveSpeed * Time.deltaTime, 0.0f, 0.0f, Space.World);
    }

    private void ChangeDirection()
    {
        isMovingRight = !isMovingRight;
    }
}
