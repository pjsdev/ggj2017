using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreCalculator : MonoBehaviour
{
    public PlayerController controller;
    public float DelayBeforeRepeatTriggerHit = 0.3f;

    private WaitForSeconds WaitForTriggerHit;
    private List<ScoreTrigger> RecentlyHitTriggers = new List<ScoreTrigger>();

    void Awake()
    {
        WaitForTriggerHit = new WaitForSeconds(DelayBeforeRepeatTriggerHit);
        if ( controller == null ) controller = GetComponentInParent<PlayerController>();
    }

    public void Hit( ScoreTrigger trigger )
    {
        if (trigger != null && !RecentlyHitTriggers.Contains(trigger) )
        {
            controller.Score += trigger.Score;
            RecentlyHitTriggers.Add(trigger);
            StartCoroutine(RemoveTriggerAfterDelay(trigger));

            trigger.ShowScore(trigger.Score, controller.SuitColor);
        }
    }

    private IEnumerator RemoveTriggerAfterDelay(ScoreTrigger trigger)
    {
        yield return WaitForTriggerHit;
        if ( RecentlyHitTriggers.Contains(trigger))
        {
            RecentlyHitTriggers.Remove(trigger);
        }
    }

    void OnDisable()
    {
        //Coroutines will stop firing so..
        StopAllCoroutines();
        RecentlyHitTriggers.Clear();
    }
}
