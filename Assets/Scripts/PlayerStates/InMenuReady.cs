using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class InMenuReady : State 
{
	PlayerController Controller;
	GameObject SpriteGO;

	SpriteRenderer Ready;

	public InMenuReady(PlayerController _controller, GameObject _inMenuReadyGO)
	{
		Controller = _controller;
		SpriteGO = _inMenuReadyGO;
		SpriteGO.SetActive (false);

		Ready = Controller.transform.Find (
			"ReadyText").GetComponent<SpriteRenderer>();

		Ready.enabled = false;
	}

	#region State implementation

	public void Update ()
	{
		if (Input.GetKeyUp (Controller.KeyOne))
		{
			Controller.Enter<InMenu> ();	
			return;
		}
	}

	public void Enter ()
	{
		SpriteGO.SetActive (true);

		PlayerController.SetSuitColor (SpriteGO, Controller.SuitColor);

		Ready.sprite = Controller.game.factory.RandomReadyText ();
		Ready.enabled = true;
	}

	public void Exit ()
	{
		SpriteGO.SetActive (false);
		Ready.enabled = false;
	}

	#endregion


}
