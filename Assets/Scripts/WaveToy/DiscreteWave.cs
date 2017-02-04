using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteWave : MonoBehaviour
{
    static public float MAX_WAVE_HEIGHT = 2f;
    static public int NUM_SEGMENTS = 400;
    public int SOLVE_ITTERATIONS = 10;

    public GameObject WaveSegmentPrefab;

    private List<WaveSegment> AllWaveSegments = new List<WaveSegment>();

	public List<WaveSegment> GetWaveSegments()
	{
		return AllWaveSegments;
	}

    private List<float> TotalSegmentAmplitudes = new List<float>();
    private List<GameObject> SegmentGameObjects = new List<GameObject>();

    public GameObject SurferPrefab;

    private void Awake()
    {
        SetupSegments();
    }

    private void SetupSegments()
    {
        Quaternion rot = Quaternion.identity;
        WaveSegment ws;
        for( int i =0; i < NUM_SEGMENTS; ++i )
        {
            TotalSegmentAmplitudes.Add(( i == 0 ? 0f : 0));
            if (WaveSegmentPrefab != null)
            {
                GameObject go = GameObject.Instantiate(WaveSegmentPrefab);
				go.transform.SetParent (transform);
                rot.eulerAngles = new Vector3(0,0, ((float)i / (float)NUM_SEGMENTS * 360f)+90f);
                go.transform.rotation = rot;
                SegmentGameObjects.Add(go);
                ws = go.GetComponent<WaveSegment>();

                if ( i > 0 )
                {
                    ws.PreviousSegment = AllWaveSegments[AllWaveSegments.Count - 1];
                    ws.PreviousSegment.NextSegment = ws;
                }

                AllWaveSegments.Add(ws);
            }
        }

        AllWaveSegments[0].PreviousSegment = AllWaveSegments[AllWaveSegments.Count - 1];
        AllWaveSegments[AllWaveSegments.Count - 1].NextSegment = AllWaveSegments[0];
    }
		
    void LateUpdate ()
    {
        for (int i = 0; i < SOLVE_ITTERATIONS; ++i)
        {
            SolveSegmentAmplitudes();
        }
        Vector3 WavePos = Vector3.zero;
        for (int i = 0; i < AllWaveSegments.Count; ++i)
        {
            WavePos.y = 3f + (AllWaveSegments[i].Amplitude * MAX_WAVE_HEIGHT);
            //SegmentGameObjects[i].transform.GetChild(0).localScale = new Vector3(0, 1f + (AllWaveSegments[i].Amplitude * 1f), 0);
            SegmentGameObjects[i].transform.GetChild(2).localPosition = WavePos;
        }
    }
    private float SignedMod(float a, float n)
    {
        return a - Mathf.Floor(a / n) * n;
    }

    private void SolveSegmentAmplitudes()
    {
		foreach (WaveSegment ws in AllWaveSegments)
		{
			ws.SmoothAcceleration();
		}

		foreach (WaveSegment ws in AllWaveSegments)
		{
			ws.SmoothFinished();
		}
    }
}
