using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    public int speed = 10;
    private Transform tr;
    void Start()
    {
        tr = GetComponent<Transform>();
    anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();
}

void Update()
{
    float horizontalInput = Input.GetAxisRaw("Horizontal");

    anim.SetInteger("Walk", (int)horizontalInput);
    if (horizontalInput == -1){
        tr.rotation = Quaternion.Euler(0, 180, 0);
    }else if (horizontalInput == 1){
        tr.rotation = Quaternion.Euler(0, 0, 0);
    }
    if (IsGrounded() && horizontalInput != 0)
    {
        Vector2 force = new Vector2(horizontalInput, 0) * speed * Time.deltaTime;
        rb.AddForce(force, ForceMode.Impulse);
    }
    if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
    {
        rb.AddForce(Vector3.up * 3, ForceMode.Impulse);
    }
}

bool IsGrounded()
{
    return true;
}
}
