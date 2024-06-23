using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    private Animator anim;
    private Rigidbody rb;
    public int speed = 10;
    [SerializeField]
    private float jumpForce = 1;
    public float currentSpeed;
    private Transform tr;
    void Start()
    {
        tr = GetComponent<Transform>();
    anim = GetComponent<Animator>();
    rb = GetComponent<Rigidbody>();
}

void Update()
{
    currentSpeed = rb.velocity.magnitude;
    float horizontalInput = Input.GetAxisRaw("Horizontal");

    anim.SetInteger("Walk", (int)horizontalInput);
    if (horizontalInput == -1)
    {
        //tr.rotation = Quaternion.Euler(0, 180, 0);
    }
    else if (horizontalInput == 1)
    {
        tr.rotation = Quaternion.Euler(0, 0, 0);
    }
    if (IsGrounded() && horizontalInput != 0)
    {
        Vector2 force = new Vector2(horizontalInput, 0) * speed * Time.deltaTime;
        rb.AddForce(force, ForceMode.Impulse);

        // Clamp velocity
        Vector3 velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -speed, speed);
        rb.velocity = velocity;
    }
    if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}

bool IsGrounded()
{
    return true;
}
}
