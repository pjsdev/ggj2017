using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour
{
    public float RotateRate = 1f;
    private Vector3 CurrentRotation = Vector3.zero;

    void Awake()
    {
        CurrentRotation.z = Random.Range(0f, 360f);
    }
    void Update()
    {
        CurrentRotation.z += RotateRate;
        transform.localEulerAngles = CurrentRotation;
    }

}
