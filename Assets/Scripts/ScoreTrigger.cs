﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTrigger : MonoBehaviour
{
    public int Score = 100;
    public float ShowScoreDuration = 1.0f;
    public float FadeInDuration = 0.2f;
    public float FadeOutDuration = 0.2f;
    public Text TextPrefab;
    public GameObject TextCanvas;

    private Vector3 RotationComponents = new Vector3(1f,0f,1f);
    private string PlayerTag = "Player";
    static private List<Text> ScoreTextFields = new List<Text>();

    void Awake()
    {
        if (TextCanvas == null) TextCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        RotationComponents.x = -Mathf.Sin(transform.localEulerAngles.z * Mathf.Deg2Rad);
        RotationComponents.y = Mathf.Cos(transform.localEulerAngles.z * Mathf.Deg2Rad);
        RotationComponents = RotationComponents.normalized;
        Debug.Log(transform.name + " For rotation " + transform.localEulerAngles.z + " Components are: " + RotationComponents );
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if ( col.gameObject.CompareTag(PlayerTag))
        {
            PlayerScoreCalculator playerScorer = col.gameObject.GetComponentInParent<PlayerScoreCalculator>();

            if (playerScorer != null) playerScorer.Hit(this);
        }
    }

    public void ShowScore(int _Score, Color colour)
    {
        ShowScore(_Score, colour, null);
    }
    public void ShowScore(int _Score, Color colour, Vector3? _pos )
    {
        Vector3 pos = _pos ?? transform.position;

        Text txt = GetTextField();
        txt.DOKill(false);
        txt.transform.position = Camera.main.WorldToScreenPoint(pos);
        txt.transform.rotation = transform.rotation;

        txt.text = _Score.ToString();
        txt.color = new Color( colour.r, colour.g, colour.b, 0);
        txt.DOFade(1f, FadeInDuration);
        txt.DOFade(0f, FadeOutDuration).SetDelay(ShowScoreDuration);
        //txt.transform.DOLocalMoveY(txt.transform.position.y + 0.1f, FadeInDuration + FadeOutDuration + ShowScoreDuration);

        txt.transform.DOMove(Camera.main.WorldToScreenPoint(pos + RotationComponents * 0.8f), FadeInDuration + FadeOutDuration + ShowScoreDuration);
        //txt.transform.DOLocalMoveY(txt.transform.position.y + RotationComponents.x * 0.1f, FadeInDuration + FadeOutDuration + ShowScoreDuration);
        //txt.transform.DOScale(1.2f, FadeInDuration + FadeOutDuration + ShowScoreDuration);
    }

    // Checks pool for available textfield - creates one if needed
    private Text GetTextField()
    {
        Text txt = null;
        if ( ScoreTextFields.Count > 0 )
        {
            foreach(Text t in ScoreTextFields )
            {
                if ( t.color.a < 0.001f )
                {
                    txt = t;
                    continue;
                }
            }
        }
        if ( txt == null )
        {
            txt = GameObject.Instantiate(TextPrefab.gameObject).GetComponent<Text>();
            ScoreTextFields.Add(txt);
            txt.transform.SetParent(TextCanvas.transform, false);
            txt.transform.localScale = Vector3.one;
        }

        return txt;
    }
}
