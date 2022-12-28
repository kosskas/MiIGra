using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GeneratedPlatforms : MonoBehaviour
{
    public GameObject platformPrefab;
    public const int PLATFORMS_NUM = 2;
    private GameObject[] platforms;
    private Vector3[] positions;
    private Vector3[] DstPositions;
    public float speed = 4.0f; //4 inaczej nie dzia³a
    private float obrot = 0;
    private float R = 1.9f;//1 inaczej nie dzia³a
    void Start()
    {

    }
    void Awake()
    {
        platforms = new GameObject[PLATFORMS_NUM];
        positions = new Vector3[PLATFORMS_NUM];
        DstPositions = new Vector3[PLATFORMS_NUM];
        float fi = 0;
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            positions[i].x = R * (float)Math.Cos(fi) + this.transform.position.x;
            positions[i].y = R * (float)Math.Sin(fi) + this.transform.position.y;
            positions[i].z = 0;
            fi += 2 * (float)Math.PI / PLATFORMS_NUM;
            DstPositions[i] = positions[i];
        }
        for(int i = 0; i < PLATFORMS_NUM; i++)
        {
            platforms[i] = Instantiate(platformPrefab, positions[i], Quaternion.identity);
        }


    }
    void Update()
    {
        float fi = 0;
        
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            DstPositions[i].x = (R * (float)Math.Cos(fi + obrot)) + this.transform.position.x;
            DstPositions[i].y = (R * (float)Math.Sin(fi + obrot)) + this.transform.position.y;
            DstPositions[i].z = 0;
            fi += 2 * (float)Math.PI / PLATFORMS_NUM;
        }
        
        for (int i = 0; i < PLATFORMS_NUM; i++)
        {
            platforms[i].transform.position = Vector3.MoveTowards(platforms[i].transform.position , DstPositions[i], speed * Time.deltaTime);
        }
        obrot += (float)Math.PI / 180;
    }
}
