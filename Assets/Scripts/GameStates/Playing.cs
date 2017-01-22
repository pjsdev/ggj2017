using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

using SimpleFSM;

public class Playing : State 
{
	Game game;
	GameObject GameUI;
	GameObject CentralCoral;
	GameObject ObstacleSpawners;
	AudioClip Music;

	readonly static float RoundTime = 60f;
	float CurrentTime;
	Text Timer;
	Text StylePoints;
	GameObject OuttaTime;

	public Playing(Game _game)
	{
		game = _game;
		GameUI = GameObject.FindGameObjectWithTag ("PlayingUI");
		CentralCoral = GameObject.FindGameObjectWithTag ("CentralCoral");
		ObstacleSpawners = GameObject.FindGameObjectWithTag ("ObstacleSpawners");
		OuttaTime = GameObject.FindGameObjectWithTag ("OuttaTime");
		Debug.Assert (GameUI != null);
		GameUI.SetActive (false);
		OuttaTime.SetActive (false);
		CentralCoral.SetActive (false);
		ObstacleSpawners.SetActive (false);

		Timer = GameUI.transform.Find ("Timer").GetComponent<Text>();
		StylePoints = GameUI.transform.Find ("Style").GetComponent<Text>();

		Music = game.factory.GetGameMusic ();
	}

	IEnumerator CheckGameOver()
	{
		while (true)
		{
			yield return null;

			StylePoints.text = "StyLe - " + game.totPoints.ToString ();

			// update timer if we are still playing
			CurrentTime -= Time.deltaTime;
			Timer.text = Mathf.CeilToInt (CurrentTime).ToString ();

			if (CurrentTime < 0)
			{
				Debug.LogWarning ("Game Over Time out");
				OuttaTime.SetActive (true);
				OuttaTime.transform.DOScale (2.5f, 1f)
					.SetLoops(2, LoopType.Yoyo)
					.SetEase(Ease.OutElastic);

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

		game.totPoints = 0;

		CurrentTime = RoundTime;
		ObstacleSpawners.SetActive (true);
		CentralCoral.SetActive (true);
		OuttaTime.gameObject.SetActive (false);
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
		OuttaTime.SetActive (false);

		foreach(var o in GameObject.FindGameObjectsWithTag("Obstacle"))
		{
			GameObject.Destroy (o);
		}
	}

	#endregion
}
