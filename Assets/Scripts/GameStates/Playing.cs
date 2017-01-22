using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class Playing : State 
{
	Game game;
	GameObject GameUI;
	GameObject CentralCoral;
	GameObject ObstacleSpawners;
	AudioClip Music;

	public Playing(Game _game)
	{
		game = _game;
		GameUI = GameObject.FindGameObjectWithTag ("PlayingUI");
		CentralCoral = GameObject.FindGameObjectWithTag ("CentralCoral");
		ObstacleSpawners = GameObject.FindGameObjectWithTag ("ObstacleSpawners");
		Debug.Assert (GameUI != null);
		GameUI.SetActive (false);
		CentralCoral.SetActive (false);
		ObstacleSpawners.SetActive (false);

		Music = game.factory.GetGameMusic ();
	}

	IEnumerator CheckGameOver()
	{
		while (true)
		{
			yield return null;
			var activePlayers = game.Players.Where (p => p.CurrentState () != typeof(Wipeout));
			if (activePlayers.Count () == 1)
			{
				Debug.LogWarning ("Game Over");
				break;
			} 
			else if(activePlayers.Count () == 0)
			{
				Debug.LogError ("Game Over With no winner");
				Debug.Break();
			}
		}

		yield return new WaitForSeconds (5f);
		foreach (PlayerController p in game.Players)
		{
			p.Enter<InMenu> ();	
		}

		game.Enter<CharacterSelect> ();
	}

	#region State implementation

	public void Update (){}

	public void Enter ()
	{
		game.Waves.gameObject.SetActive (true);

		foreach (PlayerController p in game.Players)
		{
			p.Enter<OnWave> ();	
		}

		// ObstacleSpawners.SetActive (true);
		CentralCoral.SetActive (true);
		GameUI.SetActive (true);
		game.Music.clip = Music;
		game.Music.Play ();

		game.StartCoroutine (CheckGameOver());
	}

	public void Exit ()
	{
		game.Waves.gameObject.SetActive (false);
		CentralCoral.SetActive (false);
		GameUI.SetActive (false);
		ObstacleSpawners.SetActive (false);

		foreach(var o in GameObject.FindGameObjectsWithTag("Obstacle"))
		{
			GameObject.Destroy (o);
		}
	}

	#endregion
}
