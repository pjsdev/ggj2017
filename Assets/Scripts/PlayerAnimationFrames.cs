using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerFrameData 
{
	public Sprite Body;
	public Sprite Hair;
	public Sprite Suit;
}

public class PlayerAnimationFrames : MonoBehaviour 
{
	public PlayerFrameData InMenu;
	public PlayerFrameData InMenuReady;
	public PlayerFrameData OnWave;

	SpriteRenderer Body;
	SpriteRenderer Hair;
	SpriteRenderer Suit;

	public void SetState(PlayerFrameData _frameData)
	{
		Body.sprite = _frameData.Body;
		Hair.sprite = _frameData.Hair;
		Suit.sprite = _frameData.Suit;
	}

	void Awake () 
	{
		Body = GetComponent<SpriteRenderer> ();
		Hair = transform.Find ("Hair").GetComponent<SpriteRenderer> ();
		Suit = transform.Find ("Suit").GetComponent<SpriteRenderer> ();
	}
}
