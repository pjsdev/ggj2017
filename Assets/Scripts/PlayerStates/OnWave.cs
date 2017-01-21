using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class OnWave : State 
{
	GameObject SpriteGO;
	PlayerController Controller;

	public OnWave(PlayerController _controller, GameObject _onWaveGO)
	{
		Controller = _controller;

		SpriteGO = _onWaveGO;
		SpriteGO.SetActive (false);
	}

	#region State implementation

	public void Update ()
	{
		
	}

	public void Enter ()
	{
		SpriteGO.SetActive (true);

		PlayerController.SetHairAndSuitColor (SpriteGO,
			Controller.HairColor, Controller.SuitColor);
	}

	public void Exit ()
	{
		SpriteGO.SetActive (false);
	}

	#endregion


}
