using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mover : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    
    [SerializeField]
    private float LifeTime = 5f;
    private Transform obstacle;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        obstacle = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        obstacle.Translate(Vector3.left * speed * Time.deltaTime);
        Destroy(gameObject, LifeTime);
    }
}
