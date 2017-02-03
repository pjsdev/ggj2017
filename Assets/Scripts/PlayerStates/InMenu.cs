using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SimpleFSM;

public class InMenu : State 
{
	static readonly float ReadyTimerTheshold = 1f;
	ColorIterator SuitColors;

	PlayerController Controller;

	float ReadyTimer = 0f;

	GameObject SpriteGO;

	Text ScoreText;

	public InMenu(PlayerController _controller, GameObject _inMenuGO)
	{
		Controller = _controller;
		SuitColors = new ColorIterator ();
		Controller.SuitColor = SuitColors.NextColor();

		SpriteGO = _inMenuGO;
		PlayerController.SetSuitColor (SpriteGO, Controller.SuitColor);

		ScoreText = SpriteGO.transform
			.Find ("Canvas")
			.Find ("Text")
			.GetComponent<Text> ();

		Debug.Assert (ScoreText != null);
	}

	#region State implementation

	public void Update ()
	{
		// Debug.LogFormat ("{0} [{1}]", IsReady, ReadyTime);
		if (Input.GetKey (Controller.KeyOne))
		{
			ReadyTimer += Time.deltaTime;

			if (ReadyTimer > ReadyTimerTheshold)
			{
				Controller.Enter<InMenuReady> ();
				return;
			}
		}

		if (Input.GetKeyUp (Controller.KeyOne))
		{
			Controller.SuitColor = SuitColors.PreviousColor();
			PlayerController.SetSuitColor (SpriteGO, Controller.SuitColor);
		}

		if (Input.GetKeyUp (Controller.KeyTwo))
		{
			Controller.SuitColor = SuitColors.NextColor();
			PlayerController.SetSuitColor (SpriteGO, Controller.SuitColor);	
		}
	}

	public void Enter ()
	{
		SpriteGO.SetActive (true);
		ReadyTimer = 0f;

		if (Controller.Score != 0)
		{
			ScoreText.enabled = true;
			ScoreText.text = Controller.Score.ToString ();
		} 
		else
		{
			ScoreText.enabled = false;
		}
	}

	public void Exit ()
	{
		SpriteGO.SetActive (false);
	}

	#endregion


}
