using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class OnWave : State 
{
	GameObject SpriteGO;
	PlayerController Controller;

	PlayerPaddle Paddle;
	Surfer surfer;

	public OnWave(PlayerController _controller, GameObject _onWaveGO)
	{
		Controller = _controller;
		SpriteGO = _onWaveGO;

		SpriteGO.SetActive (false);
	}

	#region State implementation

	public void Update ()
	{
		if (Input.GetKey(Controller.KeyOne))
		{
			Paddle.MoveClockwise();
		}
		if (Input.GetKey(Controller.KeyTwo))
		{
			Paddle.MoveAntiClockwise();
		}
	}

	public void Enter ()
	{
		SpriteGO.SetActive (true);

		PlayerController.SetHairAndSuitColor (SpriteGO,
			Controller.HairColor, Controller.SuitColor);

		Paddle = Controller.game.factory.CreatePaddle ();
		Paddle.AllWaveSegmentsReference = Controller.game.Waves.GetWaveSegments ();

		surfer = Controller.gameObject.AddComponent<Surfer> ();
		surfer.AllWaveSegmentsReference = Paddle.AllWaveSegmentsReference;

		Controller.transform.position = Vector3.zero;
	}

	public void Exit ()
	{
		SpriteGO.SetActive (false);
		Object.Destroy (surfer);
		GameObject.Destroy (Paddle.gameObject);
	}

	#endregion


}
