using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class PlayerController : StateMachine 
{	
	public string KeyOne;
	public string KeyTwo;
	public Color SuitColor;
		
	int ScoreInternal = 0;
	public int Score 
	{
		get { return ScoreInternal; }
		set
		{
			if (game.IsGameOver) return;
			int diff = value - ScoreInternal;
			ScoreInternal = System.Math.Max(0, value);
			game.TeamScores[TeamIndex] = System.Math.Max(0, diff + game.TeamScores[TeamIndex]);
			game.TotalScore = System.Math.Max(0, diff + game.TotalScore);
		}
	}

	public int TeamIndex = -1;

	[HideInInspector]
	public Game game;

	public static void SetSuitColor(GameObject _root, Color _suit)
	{
		var hair = _root.transform.Find ("Hair");
		var suit = _root.transform.Find ("Suit");

		hair.GetComponent<SpriteRenderer> ().color = _suit;
		suit.GetComponent<SpriteRenderer> ().color = _suit;
	}

	void Start () 
	{
		game = GameObject.FindObjectOfType<Game> ();
		Debug.Assert (game != null);

		var InMenuGO = transform.Find ("InMenu").gameObject;
		var InMenuReadGO = transform.Find ("InMenuReady").gameObject;
		var OnWaveGO = transform.Find ("OnWave").gameObject;

		AddState (new InMenu(this, InMenuGO));
		AddState (new InMenuReady(this, InMenuReadGO));
		AddState (new OnWave (this, OnWaveGO));

		Enter<InMenu> ();
	}
}
