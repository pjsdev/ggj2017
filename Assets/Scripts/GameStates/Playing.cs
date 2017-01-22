using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SimpleFSM;

public class Playing : State 
{
	Game game;
	GameObject GameUI;
	GameObject CentralCoral;
	GameObject ObstacleSpawners;
	AudioClip Music;

	float CurrentTime = 60f;
	Text Timer;

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

		Timer = GameUI.transform.Find ("Timer").GetComponent<Text>();

		Music = game.factory.GetGameMusic ();
	}

	IEnumerator CheckGameOver()
	{
		while (true)
		{
			yield return null;

			// check game over
			var activePlayers = game.Players.Where (p => p.CurrentState () != typeof(Wipeout));
			if (activePlayers.Count () == 0)
			{
				Debug.LogWarning ("Game Over wipedout");
				break;
			}

			// update timer if we are still playing
			CurrentTime -= Time.deltaTime;
			Timer.text = Mathf.CeilToInt (CurrentTime).ToString ();

			if (CurrentTime < 0)
			{
				Debug.LogWarning ("Game Over Time out");
				break;
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

		ObstacleSpawners.SetActive (true);
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
