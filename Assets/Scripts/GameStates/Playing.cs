using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class Playing : State 
{
	Game game;
	GameObject GameUI;

	AudioClip Music;

	public Playing(Game _game)
	{
		game = _game;
		GameUI = GameObject.FindGameObjectWithTag ("PlayingUI");
		Debug.Assert (GameUI != null);
		GameUI.SetActive (false);

		Music = game.factory.GetGameMusic ();
	}

	#region State implementation

	public void Update ()
	{
		
	}

	public void Enter ()
	{
		game.Waves.gameObject.SetActive (true);

		foreach (PlayerController p in game.Players)
		{
			p.Enter<OnWave> ();	
		}

		GameUI.SetActive (true);
		game.Music.clip = Music;
		game.Music.Play ();
	}

	public void Exit ()
	{
		GameUI.SetActive (false);
	}

	#endregion
}
