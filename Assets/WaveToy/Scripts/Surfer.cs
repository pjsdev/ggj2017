using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surfer : MonoBehaviour
{
    public List<WaveSegment> AllWaveSegmentsReference;

    public float HorizontalVelocity = 0;
    public int CurrentSegmentIndex = 0;

    private Quaternion q = Quaternion.identity;

    void Update()
    {
        if (AllWaveSegmentsReference == null) return;

        float _angle = transform.localEulerAngles.z - 90f;
        _angle = (_angle % 360f); if (_angle < 0) _angle += 360f;
        CurrentSegmentIndex = Mathf.RoundToInt(_angle / 360f * DiscreteWave.NUM_SEGMENTS);
        CurrentSegmentIndex = (CurrentSegmentIndex >= AllWaveSegmentsReference.Count ? CurrentSegmentIndex - AllWaveSegmentsReference.Count : (CurrentSegmentIndex < 0 ? CurrentSegmentIndex + AllWaveSegmentsReference.Count : CurrentSegmentIndex));

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

        //Debug.Log("AllWaveSegmentsReference[" + CurrentSegmentIndex + "] : " + AllWaveSegmentsReference[CurrentSegmentIndex].Amplitude);
        transform.localPosition = new Vector3(transform.localPosition.x, AllWaveSegmentsReference[CurrentSegmentIndex].Amplitude, transform.localPosition.z);

        //Debug.Log("AllWaveSegmentsReference[" + CurrentSegmentIndex + "] HorizontalVelocity : " + HorizontalVelocity);
        q.eulerAngles = new Vector3(0, 0, q.eulerAngles.z + HorizontalVelocity*2f);
        transform.localRotation = q;
		// transform.GetChild (3).transform.localRotation = q;
    }

}
