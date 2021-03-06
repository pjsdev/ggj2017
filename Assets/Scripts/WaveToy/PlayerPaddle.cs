﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour
{
    public float Acceleration = 0.2f;
    public float Decceleration = 0.001f;

    public int CurrentSegmentIndex = 0;
    public int PreviousSegmentIndex = 0;

    public float PreviousFrameAngle = 0;
    public float CurrentAngle = 0f;

    private Quaternion q = Quaternion.identity;

    public AnimationCurve WaveFalloff;

    public List<WaveSegment> AllWaveSegmentsReference;
    public List<float> PlayerWaveSegmentImpulses = new List<float>();

    public float Velocity = 0f;
    static public float MIN_VELOCITY = 0f;
    static public float MAX_VELOCITY = 2f;
    public bool MovedClockwise = false;
    public bool MovedAntiClockwise = false;

    public Game GameComponent;
    private float TimeSinceMove = 0;
    private float LastMoveTime = 0;

	[HideInInspector]
	public SpriteRenderer Renderer;

    void Awake()
    {
        GameComponent = GameObject.FindObjectOfType<Game>();
        Renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        foreach(WaveSegment ws in AllWaveSegmentsReference)
        {
            PlayerWaveSegmentImpulses.Add(0);
        }
	}

    public void MoveClockwise()
    {
        LastMoveTime = Time.time;
        MovedClockwise = true;
        if (Velocity < 0) Velocity *= 0.5f;
        Velocity += Acceleration;
    }
    public void MoveAntiClockwise()
    {
        LastMoveTime = Time.time;
        MovedAntiClockwise = true;
        if (Velocity > 0) Velocity *= 0.5f;
        Velocity -= Acceleration;
    }
    
    public void DecreaseVelocity()
    {
        Velocity *= 0.292f;
    }

    void Update()
    {
        TimeSinceMove = Time.time - LastMoveTime;
        PreviousFrameAngle = CurrentAngle;
		CurrentAngle += Velocity * Time.deltaTime * 40f;

        if (Mathf.Abs(Velocity) > MAX_VELOCITY)
			Velocity = Mathf.Sign(Velocity) * MAX_VELOCITY;

        CurrentAngle = (CurrentAngle % 360f); if (CurrentAngle < 0) CurrentAngle += 360f;

        CurrentSegmentIndex = Mathf.RoundToInt((CurrentAngle-90f) / 360f * DiscreteWave.NUM_SEGMENTS);
        PreviousSegmentIndex = Mathf.RoundToInt((PreviousFrameAngle-90f) / 360f * DiscreteWave.NUM_SEGMENTS);

        q.eulerAngles = new Vector3(0, 0, CurrentAngle);
        transform.localRotation = q;

        if (MovedClockwise || MovedAntiClockwise)
        {
            MovedClockwise = false;
            MovedAntiClockwise = false;
        }
        else
        {
            DecreaseVelocity();
        }

        AddImpulse(PreviousSegmentIndex, CurrentSegmentIndex, Velocity / PlayerPaddle.MAX_VELOCITY * 0.4f);// * WaveFalloff.Evaluate(TimeSinceMove));
    }

    public void AddImpulse(int startIndex, int endIndex, float _force)
    {
        if (Mathf.Abs(_force) <= 0.0001f) return;

        int totalSegments = Mathf.Abs(startIndex - endIndex);
        if (totalSegments > DiscreteWave.NUM_SEGMENTS * 0.5) totalSegments = DiscreteWave.NUM_SEGMENTS - totalSegments;

        float MaxForce = 1f / GameComponent.Players.Count;

        _force *= MaxForce;

        //if (Mathf.Abs(_force) > 1f) _force = Mathf.Sign(_force);

        int idx = 0;
        for (int i = 0; i < totalSegments; ++i)
        {
            idx = startIndex + ((_force > 0) ? i : -i);
            idx = (idx >= AllWaveSegmentsReference.Count ?
				   idx - AllWaveSegmentsReference.Count :
				   (idx < 0 ? idx + AllWaveSegmentsReference.Count : idx));

            PlayerWaveSegmentImpulses[idx] += _force;
            if (Mathf.Abs(PlayerWaveSegmentImpulses[idx]) > MaxForce) PlayerWaveSegmentImpulses[idx] = Mathf.Sign(_force) * MaxForce;
            //Debug.Log("PlayerWaveSegmentImpulses[idx]:" + PlayerWaveSegmentImpulses[idx]);
            AllWaveSegmentsReference[idx].AddImpuse(PlayerWaveSegmentImpulses[idx]);
            PlayerWaveSegmentImpulses[idx] = 0;
        }
    }
}
