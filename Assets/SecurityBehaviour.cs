using UnityEngine;
using TMPro; 

public class SecurityBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tr;
    private GameObject player;

    public float speed = 1.0f;
    public float raycastDistance = 5.0f;

    public float minPatrolDuration = 2.0f;
    public float maxPatrolDuration = 5.0f;
    private float currentPatrolDuration;
    private float patrolTimer;

    private bool isChasing = false;
    private Vector3 initialRotation; 

    public StatsText text;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        player = GameObject.Find("Karakter Utama_0");
        initialRotation = tr.eulerAngles; 

        SetNewPatrolDuration();
    }

    void FixedUpdate()
    {
        if (!isChasing)
        {
            Patrol();
        }
        else
        {
            ChasePlayer();
        }

        UpdateStatusText();
    }

    void Patrol()
    {
        MoveRight(); // Bergerak ke kanan

        RaycastHit hit;
        if (Physics.Raycast(tr.position, tr.right, out hit, raycastDistance))
        {
            Debug.DrawRay(tr.position, tr.right * raycastDistance, Color.red);
            if (hit.collider.gameObject == player)
            {
                Debug.Log("Player terdeteksi!");
                isChasing = true;
            }
        }

        patrolTimer += Time.deltaTime;
        if (patrolTimer >= currentPatrolDuration)
        {
            FlipDirection(); // Balik arah (rotasi)
            SetNewPatrolDuration();
        }
    }

    void ChasePlayer()
    {
        MoveToPlayer(); // Bergerak ke arah player

        if (Vector3.Distance(tr.position, player.transform.position) > raycastDistance * 2)
        {
            isChasing = false;
        }
    }

    void MoveRight()
    {
        rb.MovePosition(tr.position + tr.right * speed * Time.fixedDeltaTime);
    }

    void MoveToPlayer()
    {
        // Hentikan perubahan rotasi saat mengejar
        tr.eulerAngles = initialRotation; 

        Vector3 directionToPlayer = (player.transform.position - tr.position).normalized;
        rb.MovePosition(tr.position + directionToPlayer * speed * Time.fixedDeltaTime);
    }

    void FlipDirection()
    {
        if (tr.eulerAngles == initialRotation)
        {
            tr.eulerAngles = initialRotation + new Vector3(0, 180, 0); 
        }
        else
        {
            tr.eulerAngles = initialRotation; 
        }
    }

    void UpdateStatusText()
    {
        if (!isChasing)
        {
            text.UpdateText("Patroli");
        }
        else
        {
            text.UpdateText("Mengejar");
        }
    }

    void SetNewPatrolDuration()
    {
        currentPatrolDuration = Random.Range(minPatrolDuration, maxPatrolDuration);
        patrolTimer = 0;
    }
}
