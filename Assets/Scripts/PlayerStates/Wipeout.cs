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
		SpriteGO.transform.DOMove (Random.insideUnitCircle, 3f);
		SpriteGO.transform.DORotate (Random.insideUnitCircle, 3f);
	}

	public void Exit ()
	{
		SpriteGO.SetActive (false);
	}

	#endregion


}
