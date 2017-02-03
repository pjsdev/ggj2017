using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class ScoreTrigger : MonoBehaviour
{
    public int Score = 100;
    public float ShowScoreDuration = 1.0f;
    public float FadeInDuration = 0.2f;
    public float FadeOutDuration = 0.2f;
    public Text TextPrefab;
    public GameObject TextCanvas;

    private string PlayerTag = "Player";
    private List<Text> ScoreTextFields = new List<Text>();

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
        Text txt = GetTextField();
        txt.transform.SetParent(TextCanvas.transform, false);
        txt.transform.localScale = Vector3.one;
        txt.transform.position = Camera.main.WorldToScreenPoint( transform.position );
        txt.transform.rotation = transform.rotation;

        txt.text = _Score.ToString();
        txt.color = new Color( colour.r, colour.g, colour.b, 0);
        txt.DOFade(1f, FadeInDuration);
        txt.DOFade(0f, FadeOutDuration).SetDelay(ShowScoreDuration);
        //txt.transform.DOLocalMoveY(txt.transform.position.y + 0.1f, FadeInDuration + FadeOutDuration + ShowScoreDuration);
        txt.transform.DOScale(1.1f, FadeInDuration + FadeOutDuration + ShowScoreDuration);
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
        }

        return txt;
    }
}
