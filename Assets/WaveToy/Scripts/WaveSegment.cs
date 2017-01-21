using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSegment : MonoBehaviour
{
    public float Acceleration;
    public float Velocity;
    public float ImpulseVelocity;

    private float TimeSinceImpulse = 0;
    private float LastImpulseTime = 0;

    public WaveSegment PreviousSegment;
    public WaveSegment NextSegment;

    public float Amplitude;

    public void AddImpuse(float force)
    {
        LastImpulseTime = Time.time;
        ImpulseVelocity += Mathf.Min( Mathf.Abs(force), 1f/PaddleManager.NUM_PLAYERS);
        if (ImpulseVelocity > 1f) ImpulseVelocity = 1f;

        Acceleration += ImpulseVelocity;
        if (Acceleration > 1f) Acceleration = 1f;
    }

    void Update()
    {
        TimeSinceImpulse = Time.time - LastImpulseTime;

        Acceleration += -Amplitude*0.5f;
        Acceleration *= 0.995f;

        Velocity = Acceleration * Time.fixedDeltaTime * 3f;

        ImpulseVelocity *= 0.97f;
        Amplitude += Velocity;
    }

    public void SmoothAcceleration()
    {
        Acceleration = Mathf.Lerp(Acceleration, NextSegment.Acceleration, 0.3f + Mathf.Abs(NextSegment.Acceleration) * 0.3f);
		Acceleration = Mathf.Lerp(Acceleration, PreviousSegment.Acceleration, 0.3f + Mathf.Abs(PreviousSegment.Acceleration) * 0.3f);
        //Acceleration = Mathf.Lerp(Acceleration, (PreviousSegment.Acceleration + NextSegment.Acceleration) * 0.5f, 0.85f);
    }
    public void SmoothVelocity()
    {
        Velocity = Mathf.Lerp(Velocity, PreviousSegment.Velocity, 0.3f + Mathf.Abs(PreviousSegment.Velocity) * 0.3f);
        Velocity = Mathf.Lerp(Velocity, NextSegment.Velocity, 0.3f + Mathf.Abs(NextSegment.Velocity) * 0.3f);
        //Velocity = Mathf.Lerp(Velocity, (PreviousSegment.Velocity + NextSegment.Velocity) * 0.5f, 0.95f);
    }
}
