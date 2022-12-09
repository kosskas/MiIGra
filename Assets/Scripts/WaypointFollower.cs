using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{

    [SerializeField] private GameObject[] waypoints;
    private int currentWaypoint = 0;
    [SerializeField] private float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);
        if (Vector2.Distance(waypoints[currentWaypoint].transform.position,this.transform.position) < 0.1f)
        {
            currentWaypoint = (currentWaypoint +1)%waypoints.Length;
            Debug.Log(currentWaypoint);


        }
        
    }
}
