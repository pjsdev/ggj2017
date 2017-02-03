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

	readonly static float RoundTime = 10f;
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
		while (game.CurrentStateType() == typeof(Playing))
		{
			yield return null;

			StylePoints.text = "StyLe - " + game.TotalScore.ToString ();

			// update timer if we are still playing

			if (CurrentTime < 0)
			{
				if(!game.IsGameOver)
				{
					Debug.LogWarning ("Game Over Time out");
					OuttaTime.SetActive (true);
					OuttaTime.transform.DOScale (2.5f, 1f)
						.SetLoops(2, LoopType.Yoyo)
						.SetEase(Ease.OutElastic)
						.OnComplete(()=>{
							foreach (PlayerController p in game.Players)
							{
								p.Enter<InMenu> ();	
							}

							game.Enter<CharacterSelect> ();					
						});

					game.IsGameOver = true;		
				}
			

			} 
			else 
			{
				CurrentTime -= Time.deltaTime;
				Timer.text = Mathf.CeilToInt (CurrentTime).ToString ();
			}
		}
	}

	#region State implementation

	public void Update (){}

	public void Enter ()
	{
		game.IsGameOver = false;
		game.Waves.gameObject.SetActive (true);

		// init all scoring metrics to 0
		game.TeamScores = new List<int>(ColorIterator.Colors.Select(x=>0));
		Debug.Assert(game.TeamScores.Count == ColorIterator.Colors.Length);
		game.TotalScore = 0;

		foreach (PlayerController p in game.Players)
		{
			p.TeamIndex = ColorIterator.IndexOf(p.SuitColor);
			p.Enter<OnWave> ();	
		}


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
