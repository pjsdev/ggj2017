﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class TitleScreen : State 
{
	Game game;
	GameObject TitleUI;
	AudioClip Music;

	public TitleScreen(Game _game)
	{
		game = _game;
		TitleUI = GameObject.FindGameObjectWithTag ("TitleScreen");
		Debug.Assert (TitleUI != null);

		Music = game.factory.GetMenuMusic ();
	}

	#region State implementation

	public void Update ()
	{
		if (Input.GetKeyUp (KeyCode.Space))
		{
			game.Enter<CharacterSelect> ();
		}
	}

	public void Enter ()
	{
		game.Music.clip = Music;
		game.Music.Play ();
		TitleUI.SetActive (true);
	}

	public void Exit ()
	{
		TitleUI.SetActive (false);
	}

	#endregion



}
