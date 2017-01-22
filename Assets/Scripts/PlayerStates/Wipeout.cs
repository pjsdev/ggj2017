using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;
using DG.Tweening;

public class Wipeout : State 
{
	PlayerController Controller;
	GameObject SpriteGO;

	public Wipeout(PlayerController _controller, GameObject _wipeoutGO)
	{
		SpriteGO = _wipeoutGO;
		Controller = _controller;

		SpriteGO.SetActive (false);
	}

	#region State implementation

	public void Update ()
	{
		
	}

	public void Enter ()
	{
		Vector3 dir = (SpriteGO.transform.position - Vector3.zero).normalized;

		SpriteGO.SetActive (true);

		PlayerController.SetHairAndSuitColor (SpriteGO,
			Controller.HairColor, Controller.SuitColor);

		SpriteGO.transform.DOMove (dir * 10f, 3f).SetRelative(true);
		SpriteGO.transform.DORotate (new Vector3(0, 0, 360f), 0.1f).SetRelative(true).SetLoops(30);
	}

	public void Exit ()
	{
		SpriteGO.SetActive (false);
	}

	#endregion


}
