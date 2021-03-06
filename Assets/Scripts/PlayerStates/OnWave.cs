﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;
using UnityEngine.UI;

public class OnWave : State 
{
    GameObject SpriteGO;
    GameObject ScoreMultiplierCanvas;
    GameObject ScorePlacement;
    GameObject Surfboard;
    Vector3 ReverseSpriteVec = new Vector3(1f, -1f, 1f);
    PlayerController Controller;

	PlayerPaddle Paddle;
	Surfer surfer;

	Vector3 OldPos;

	public void HitObstacle()
	{
        surfer.ObstacleHit();
	}

	public OnWave(PlayerController _controller, GameObject _onWaveGO)
	{
		Controller = _controller;
		SpriteGO = _onWaveGO;

        Transform rotatingSurfer = SpriteGO.transform.FindChild("RotatingSurfer");
        Debug.Log("Rotating surfer: " + rotatingSurfer);
        ScorePlacement = SpriteGO.transform.FindChild("ScorePlacement").gameObject;
        ScoreMultiplierCanvas = rotatingSurfer.FindChild("MultiplierCanvas").gameObject;
        Surfboard = rotatingSurfer.FindChild("Surfboard 2").gameObject;

        SpriteGO.SetActive (false);
	}

	#region State implementation

	public void Update ()
	{
        if ( !surfer.Stunned )
        {
            if (Input.GetKey(Controller.KeyOne))
            {
                Surfboard.transform.localScale = ReverseSpriteVec;
                Paddle.MoveClockwise();
            }
            if (Input.GetKey(Controller.KeyTwo))
            {
                Surfboard.transform.localScale = Vector3.one;
                Paddle.MoveAntiClockwise();
            }
        }
        if ( surfer.ScoreMultiplier > 1 )
        {
            ScoreMultiplierCanvas.SetActive(true);
            Text t = ScoreMultiplierCanvas.transform.GetComponentInChildren<Text>();
            t.text = surfer.ScoreMultiplier + "X";
        }
        else
        {
            ScoreMultiplierCanvas.SetActive(false);
        }
    }

	public void Enter ()
	{
		SpriteGO.SetActive (true);

		Controller.Score = 0;

		PlayerController.SetSuitColor (SpriteGO.transform.GetChild(1).gameObject, Controller.SuitColor);

		Paddle = Controller.game.factory.CreatePaddle ();
		Paddle.AllWaveSegmentsReference = Controller.game.Waves.GetWaveSegments ();

		surfer = Controller.gameObject.AddComponent<Surfer> ();
		surfer.AllWaveSegmentsReference = Paddle.AllWaveSegmentsReference;
		surfer.Controller = Controller;
        surfer.ScoreTransform = ScorePlacement.transform;

        OldPos = Controller.transform.position;
		Controller.transform.position = Vector3.zero;

		Controller.transform.SetParent (Paddle.transform, true);

		var surfers = GameObject.FindObjectsOfType<Surfer> ();
		Paddle.CurrentAngle = 90f * surfers.Length;
		Paddle.Renderer.color = Controller.SuitColor;
	}

	public void Exit ()
	{
        surfer.Stunned = false;

		SpriteGO.SetActive (false);

		Object.Destroy (surfer);

		Controller.transform.SetParent (null);
		Controller.transform.position = OldPos;
		Controller.transform.localRotation = Quaternion.Euler (Vector3.zero);

		GameObject.Destroy (Paddle.gameObject);
	}

	#endregion


}
