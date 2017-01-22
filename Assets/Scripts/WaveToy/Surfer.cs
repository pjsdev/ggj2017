﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surfer : MonoBehaviour
{
    public List<WaveSegment> AllWaveSegmentsReference;

    public float HorizontalVelocity = 0;
    public int CurrentSegmentIndex = 0;

    private Game GameComponent;
    private float PlayerVelocity = 0;
    private float PlayerHeight = 0;

    public bool InAir = false;
    public float LastTimeOnWater = 0;
    private Quaternion q = Quaternion.identity;

    void Update()
    {
        if (AllWaveSegmentsReference == null) return;

        GameComponent = GameObject.FindObjectOfType<Game>();

        float _angle = transform.eulerAngles.z - 90f;
        _angle = (_angle % 360f); if (_angle < 0) _angle += 360f;
        CurrentSegmentIndex = Mathf.RoundToInt(_angle / 360f * DiscreteWave.NUM_SEGMENTS);
        CurrentSegmentIndex = (CurrentSegmentIndex >= AllWaveSegmentsReference.Count ?
							   CurrentSegmentIndex - AllWaveSegmentsReference.Count : 
							   (CurrentSegmentIndex < 0 ? CurrentSegmentIndex + AllWaveSegmentsReference.Count : CurrentSegmentIndex));

        float _sumOfNearbySegmentToLeft = 0;
        float _sumOfNearbySegmentToRight = 0;
        int _segmentsToSample = Mathf.RoundToInt( DiscreteWave.NUM_SEGMENTS * 0.08f );
        if (_segmentsToSample % 2 == 0) _segmentsToSample++;

        int _halfSegmentsToSample = Mathf.RoundToInt((_segmentsToSample - 1f) * 0.5f);
        int _startIndex = Mathf.RoundToInt( CurrentSegmentIndex - _halfSegmentsToSample );
        int idx = _startIndex;
        for ( int i = 0; i < _segmentsToSample; ++i )
        {
            idx = _startIndex + i;
            idx = (idx >= AllWaveSegmentsReference.Count ? idx - AllWaveSegmentsReference.Count : (idx < 0 ? idx + AllWaveSegmentsReference.Count : idx));
            if ( i < _segmentsToSample * 0.5)
            {
                _sumOfNearbySegmentToLeft += AllWaveSegmentsReference[idx].Amplitude;
            }
            else if ( i > _segmentsToSample * 0.5 )
            {
                _sumOfNearbySegmentToRight += AllWaveSegmentsReference[idx].Amplitude;
            }
        }

        _sumOfNearbySegmentToLeft /= _halfSegmentsToSample;
        _sumOfNearbySegmentToRight /= _halfSegmentsToSample;

        HorizontalVelocity = _sumOfNearbySegmentToLeft - _sumOfNearbySegmentToRight;

        WaveSegment CurrentSegment = AllWaveSegmentsReference[CurrentSegmentIndex];
        PlayerVelocity = Mathf.Lerp( PlayerVelocity, CurrentSegment.Velocity, 0.01f + (CurrentSegment.Velocity >= 0 ? CurrentSegment.Velocity*0.3f : 0f));

        PlayerVelocity *= 0.95f;

        float heightDif = CurrentSegment.Amplitude - PlayerHeight;
        if ( heightDif > 0 )
        {
            PlayerVelocity += heightDif * 0.05f;
        }
        else
        {
            PlayerVelocity += heightDif * 0.005f;
        }
        //PlayerHeight = Mathf.Lerp( PlayerHeight, CurrentSegment.Amplitude, (heightDif > 0 ? 0.8f : 0.1f ) );

        PlayerHeight += PlayerVelocity;// * Time.fixedDeltaTime;


        if ( CurrentSegment.Amplitude >= (PlayerHeight - 0.3f) )
        {
            InAir = false;
            LastTimeOnWater = Time.time;
        }
        else
        {
            InAir = true;
            GameComponent.AddStyle(1);
        }

        //Debug.Log("AllWaveSegmentsReference[" + CurrentSegmentIndex + "] : " + AllWaveSegmentsReference[CurrentSegmentIndex].Amplitude);
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            PlayerHeight * DiscreteWave.MAX_WAVE_HEIGHT,
			transform.localPosition.z);

        //Debug.Log("AllWaveSegmentsReference[" + CurrentSegmentIndex + "] HorizontalVelocity : " + HorizontalVelocity);
        // q.eulerAngles = new Vector3(0, 0, q.eulerAngles.z + HorizontalVelocity*2f);
        // transform.localRotation = q;
		// transform.GetChild (3).transform.localRotation = q;
    }

}