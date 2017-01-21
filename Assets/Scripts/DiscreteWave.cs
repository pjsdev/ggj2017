using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscreteWave : MonoBehaviour
{

    public int NUM_SEGMENTS = 100;
    public int SOLVE_ITTERATIONS = 10;

    public GameObject WaveSegmentPrefab;

    private List<float> m_SegmentAmplitudes = new List<float>();
    private List<GameObject> m_SegmentGameObjects = new List<GameObject>();

    private void Awake()
    {
        SetupSegments();
    }

    private void SetupSegments()
    {
        Quaternion rot = Quaternion.identity;

        for( int i =0; i < NUM_SEGMENTS; ++i )
        {
            m_SegmentAmplitudes.Add(( i == 0 ? 0f : 0));
            if (WaveSegmentPrefab != null)
            {
                GameObject go = GameObject.Instantiate(WaveSegmentPrefab);
                rot.eulerAngles = new Vector3(0,0, ((float)i / (float)NUM_SEGMENTS * 360f)+90f);
                go.transform.rotation = rot;
                m_SegmentGameObjects.Add(go);
            }
        }
    }


    // Update is called once per frame
    void Update ()
    {
        if ( Input.GetMouseButton(0))
        {

            float relX = (Input.mousePosition.x - (Screen.width * 0.5f)) / Screen.width;
            float relY = (Input.mousePosition.y - (Screen.height * 0.5f)) / Screen.height;

            float angle = Mathf.Atan2(relY, relX) + (float)Math.PI;
            int segment = Mathf.RoundToInt(angle/(Mathf.PI*2) * NUM_SEGMENTS);

            //Debug.Log("Add force at " + Input.mousePosition + " which is segment " + segment + " for angle" + angle);

            m_SegmentAmplitudes[segment] += 0.1f;
        }

        for (int i = 0; i < SOLVE_ITTERATIONS; ++i)
        {
            SolveSegmentAmplitudes();
        }
        for (int i = 0; i < m_SegmentAmplitudes.Count; ++i)
        {
            m_SegmentGameObjects[i].transform.GetChild(0).localPosition = new Vector3(0, 1f + (m_SegmentAmplitudes[i]*4f), 0);
            m_SegmentAmplitudes[i] *= 0.98f;
        }
    }

    private void SolveSegmentAmplitudes()
    {
        for( int i = 0;  i < m_SegmentAmplitudes.Count; ++i)
        {
            m_SegmentAmplitudes[i] = Mathf.Lerp(m_SegmentAmplitudes[i], ( i > 0 ? m_SegmentAmplitudes[i - 1] : m_SegmentAmplitudes[m_SegmentAmplitudes.Count-1]), 0.1f);
        }

    }
}
