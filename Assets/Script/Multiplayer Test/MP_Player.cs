using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_Player : NetworkBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform camPos;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer model;
    //[Networked] Vector3 playerPos { get; set;}
    [SerializableType] float teleportCooldownTime = 2f;

    bool canTeleport;
    [Networked] TickTimer teleportCooldown { get; set; }

    public override void Spawned()
    {
        base.Spawned();
        anim = GetComponentInChildren<Animator>();
        if (HasInputAuthority)
        {
            Camera.main.transform.position = camPos.position;
            Camera.main.transform.rotation = camPos.rotation;
            Camera.main.transform.SetParent(camPos);
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (GetInput(out NetworkInputData input))
        {
            transform.Translate(input.direction * moveSpeed * Runner.DeltaTime);
            anim.SetInteger ("Walk", (int)input.direction.magnitude);
            if (input.direction.x < 0)
            {
                model.flipX = true;
            }
            else if (input.direction.x > 0)
            {
                model.flipX = false;
            }
            canTeleport = input.buttons.IsSet(MyInputButton.Teleport);
        }

        teleportUpdate();
    }

    void teleportUpdate()
    {
        if (canTeleport)
        {
            if (teleportCooldown.ExpiredOrNotRunning(Runner))
            {
                if (Runner.IsServer)
                {
                    teleport();
                }
            }
        }
    }

    void teleport()
    {
        Vector3 newPos = new Vector3(Random.Range(-5, 5), 1f, Random.Range(-5, 5));
        transform.position = newPos;
        teleportCooldown = TickTimer.CreateFromSeconds(Runner, teleportCooldownTime);
    }
    

    //public override void Render()
    //{
    //    base.Render();
    //    transform.position = playerPos;
    //}
}
