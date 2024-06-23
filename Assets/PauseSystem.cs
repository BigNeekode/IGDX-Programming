using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

private void Update() {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (animator.GetBool("IsPaused"))
            {
                Resume();
                Time.timeScale = 1;
            }
            else
            {
                Pause();
            }
        }
}

    IEnumerator PauseCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
    }

    public void Pause()
    {
        animator.SetBool("IsPaused", true);
        StartCoroutine(PauseCoroutine());
    }

    public void Resume()
    {
        animator.SetBool("IsPaused", false);
    }

}
