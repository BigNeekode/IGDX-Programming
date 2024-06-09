using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
    private Transform tr;
    public Transform target;
    public Vector3 offset;
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        tr.position = target.position + offset;
        tr.LookAt(target);
    }
}
