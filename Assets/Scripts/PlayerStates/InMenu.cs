using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class InMenu : State 
{
	static readonly float ReadyTimerTheshold = 1f;
	ColorIterator HairColors;
	ColorIterator SuitColors;

	PlayerController Controller;

	float ReadyTimer = 0f;

	GameObject SpriteGO;

	public InMenu(PlayerController _controller, GameObject _inMenuGO)
	{
		Controller = _controller;
		HairColors = new ColorIterator ();
		SuitColors = new ColorIterator ();
		Controller.HairColor = HairColors.NextColor();
		Controller.SuitColor = SuitColors.NextColor();

		SpriteGO = _inMenuGO;
		PlayerController.SetHairAndSuitColor (SpriteGO,
			Controller.HairColor, Controller.SuitColor);
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
			Controller.HairColor = HairColors.NextColor();
			PlayerController.SetHairAndSuitColor (SpriteGO,
				Controller.HairColor, Controller.SuitColor);
		}

		if (Input.GetKeyUp (Controller.KeyTwo))
		{
			Controller.SuitColor = SuitColors.NextColor();
			PlayerController.SetHairAndSuitColor (SpriteGO,
				Controller.HairColor, Controller.SuitColor);
		}
	}

	public void Enter ()
	{
		SpriteGO.SetActive (true);
		ReadyTimer = 0f;
	}

	public void Exit ()
	{
		SpriteGO.SetActive (false);
	}

	#endregion


}
