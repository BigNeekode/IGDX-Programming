using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    float targetFrameRate = 60.0f;

    void Start()
    {
        Application.targetFrameRate = (int)targetFrameRate;
    }
}
