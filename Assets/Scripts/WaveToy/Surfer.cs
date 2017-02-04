using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surfer : MonoBehaviour
{
    public List<WaveSegment> AllWaveSegmentsReference;

    public float HorizontalVelocity = 0;
    public int CurrentSegmentIndex = 0;

    private int MAX_SCORE_MULTIPLIER = 1;
    private float TIME_TO_INCREASE_MULTIPLIER = 5f;
    public int ScoreMultiplier = 1;
    public float TimeIncreasedMultiplier = 0;

    //private Game GameComponent;
    private float PlayerVelocity = 0;
    private float PlayerHeight = 0;

    public float STUN_TIME = 2f;
    public bool Stunned = false;
    public float TimeStunned = 0;

    private int Spins = 0;
    private float SpinAngle = 0;
    public bool InAir = false;
    public float LastTimeOnWater = 0;
    private Quaternion q = Quaternion.identity;

    public Transform ScoreTransform;

    public PlayerController Controller;

    private ScoreTrigger PlayerScoreTrigger;

    void Awake()
    {
        PlayerScoreTrigger = GetComponentInParent<ScoreTrigger>();
    }

    public void ObstacleHit()
    {
        if (Stunned) return;
        PlayerVelocity = 0;
        InAir = false;
        Stunned = true;
        TimeStunned = Time.time;
        ScoreMultiplier = 1;
        Spins = 0;
        SpinAngle = 0;

        PlayerScoreTrigger.ShowScore(-50, Color.red, ScoreTransform.position);
        AddScore(-50);
    }
    void Update()
    {
        if (AllWaveSegmentsReference == null) return;

        Controller.PlayerIsStunned = Stunned;

        if ( Stunned )
        {
			q.eulerAngles = new Vector3(0, 0, Mathf.Lerp( q.eulerAngles.z * Time.deltaTime * 40f, 180f, 0.3f) );
            if (Time.time - TimeStunned > STUN_TIME) Stunned = false;
            TimeIncreasedMultiplier = Time.time;
        }
        else
        {
            if (ScoreMultiplier < MAX_SCORE_MULTIPLIER && (Time.time - TimeIncreasedMultiplier) > TIME_TO_INCREASE_MULTIPLIER)
            {
                TimeIncreasedMultiplier = Time.time;
                ScoreMultiplier++;
                Debug.Log("SCORE MULTIPLIER INCREASED! " + ScoreMultiplier);
            }
        }
        //GameComponent = GameObject.FindObjectOfType<Game>();

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
            SpinAngle = 0;
            InAir = false;
            LastTimeOnWater = Time.time;
            if ( !Stunned ) q.eulerAngles = new Vector3(0, 0, q.eulerAngles.z * 0.7f );
        }
        else
        {
            InAir = true;

            if ( !Stunned )
            {
                AddScore(1);

                float spinAmount = 13f * Time.deltaTime * 40f;
                if (Input.GetKey(Controller.KeyOne))
                {
                    SpinAngle += spinAmount;
                    q.eulerAngles = new Vector3(0, 0, q.eulerAngles.z + spinAmount);
                }
                if (Input.GetKey(Controller.KeyTwo))
                {
                    SpinAngle -= spinAmount;
                    q.eulerAngles = new Vector3(0, 0, q.eulerAngles.z - spinAmount);
                }
            }
        }
        if (Mathf.Abs(SpinAngle) > 300f)
        {
            Spins++;
        }
        transform.GetChild(3).GetChild(1).localRotation = q;

        if ( Spins > 0 )
        {
            //PlayerScoreTrigger.transform.position = transform.position;
            //PlayerScoreTrigger.transform.rotation = transform.rotation;
            PlayerScoreTrigger.ShowScore(100,Controller.SuitColor, ScoreTransform.position);
            AddScore(100);
            Spins = 0;
            SpinAngle = 0;
        }

        //Debug.Log("AllWaveSegmentsReference[" + CurrentSegmentIndex + "] : " + AllWaveSegmentsReference[CurrentSegmentIndex].Amplitude);
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            (PlayerHeight * DiscreteWave.MAX_WAVE_HEIGHT) - (Stunned ? 0.5f : 0f),
			transform.localPosition.z);
    }

    void AddScore(int score)
    {
        Controller.Score += score * ScoreMultiplier;
        //PlayerScore += score * ScoreMultiplier;
        //GameComponent.AddStyle(score*ScoreMultiplier);
    }
}
