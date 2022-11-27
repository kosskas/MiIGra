using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private Vector3 nextPosition;

    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform platform;

    [SerializeField]
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = platform.localPosition;
        targetPosition = target.localPosition;
        nextPosition = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        OnAnimatorMove();
    }

    private void OnAnimatorMove()
    {
        platform.localPosition = Vector3.MoveTowards(platform.localPosition, nextPosition, speed * Time.deltaTime);

        if(Vector3.Distance(platform.localPosition, nextPosition) <= 0.1)
        {
            ChangeTarget();
        }
    }

    private void ChangeTarget()
    {
        nextPosition = (nextPosition != startPosition) ? startPosition : targetPosition;
    }
}
