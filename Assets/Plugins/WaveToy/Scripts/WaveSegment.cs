using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSegment : MonoBehaviour
{
    public GameObject Splash;
    private ParticleSystem SplashPS;
    public float Acceleration;
	public float SmoothedAcceleration = 0;
    public float Velocity;
    public float ImpulseVelocity;

    private float TimeSinceImpulse = 0;
    private float LastImpulseTime = 0;

    public WaveSegment PreviousSegment;
    public WaveSegment NextSegment;

    public float Amplitude;

    void Awake()
    {
        SplashPS = Splash.GetComponent<ParticleSystem>();
    }

    public void AddImpuse(float force)
    {
        LastImpulseTime = Time.time;
		ImpulseVelocity += Mathf.Abs(force);//Mathf.Min( Mathf.Abs(force), 1f/PaddleManager.NUM_PLAYERS);
        if (ImpulseVelocity > 1f) ImpulseVelocity = 1f;

        Acceleration += ImpulseVelocity;
        if (Acceleration > 1f) Acceleration = 1f;
    }

	void Update()
	{
		TimeSinceImpulse = Time.time - LastImpulseTime;

		Acceleration += -Amplitude*0.5f;
		Acceleration *= 0.95f;

		if (Mathf.Abs(Acceleration) > 1f) Acceleration = Mathf.Sign(Acceleration) * 1f;

		Velocity = Acceleration * Time.fixedDeltaTime * 3f;

		ImpulseVelocity *= 0.97f;
		Amplitude += Velocity;

        var e = SplashPS.emission;
        e.enabled = (Amplitude > 0.3f);

        Acceleration += Random.Range(-0.05f, 0.05f);
	}

	public void SmoothAcceleration()
	{
		float fromBehind = Mathf.Lerp(Acceleration, PreviousSegment.Acceleration, 0.3f + Mathf.Abs(PreviousSegment.Acceleration) * 0.7f);
		float fromAhead = Mathf.Lerp(Acceleration, NextSegment.Acceleration, 0.3f + Mathf.Abs(NextSegment.Acceleration) * 0.7f);
		SmoothedAcceleration = Mathf.Max(fromBehind, fromAhead);
	}
	public void SmoothFinished()
	{
		Acceleration = SmoothedAcceleration;
	}

	public void SmoothVelocity()
	{
		Velocity = Mathf.Lerp(Velocity, PreviousSegment.Velocity, 0.3f + Mathf.Abs(PreviousSegment.Velocity) * 0.3f);
		Velocity = Mathf.Lerp(Velocity, NextSegment.Velocity, 0.3f + Mathf.Abs(NextSegment.Velocity) * 0.3f);
	}
}
