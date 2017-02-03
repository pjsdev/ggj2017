using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour
{
    public float RotateRate = 1f;
    private Vector3 CurrentRotation = Vector3.zero;
	public float Speed = 50f;

    void Awake()
    {
        CurrentRotation.z = Random.Range(0f, 360f);
        transform.localEulerAngles = CurrentRotation;
    }
    void Update()
    {
		CurrentRotation.z += RotateRate * Time.deltaTime * Speed;
		transform.localEulerAngles = CurrentRotation;
    }

}
