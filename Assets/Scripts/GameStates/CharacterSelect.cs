using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SimpleFSM;

public class CharacterSelect : State 
{
	public class KeyData 
	{
		public string KeyOne;
		public string KeyTwo;
	}

	List<char> AlreadyUsedKeys;

	KeyData CurrentlyJoining = null;

	Game game;

	GameObject CharacterSelectUI;

	// countdown
	static readonly float CountdownTime = 5f;
	GameObject CountdownPanel;
	Text CountdownText;
	float CountdownCurrent = CountdownTime;

	public CharacterSelect (Game _game)
	{
		AlreadyUsedKeys = new List<char> ();

		game = _game;
		CharacterSelectUI = GameObject.FindGameObjectWithTag ("CharacterSelectUI");
		Debug.Assert (CharacterSelectUI != null);

		CountdownPanel = CharacterSelectUI.transform.Find ("Countdown").gameObject;
		Debug.Assert (CountdownPanel != null);
		var textTransform = CountdownPanel.transform.Find ("Text");
		Debug.Assert (textTransform != null);
		CountdownText = textTransform.GetComponent<Text>();
		Debug.Assert (CountdownText != null);

		CharacterSelectUI.SetActive (false);
	}

	public void Enter ()
	{
		CountdownPanel.SetActive (false);
		CharacterSelectUI.SetActive (true);
	}

	public void Exit ()
	{
		CharacterSelectUI.SetActive (false);
	}

	public void Update ()
	{
		if (game.Players.Count > 0 &&
			game.Players.All (p => p.CurrentState () == typeof(InMenuReady)))
		{
			Debug.Log ("All Players Ready...");
			CountdownPanel.SetActive (true);
			CountdownCurrent -= Time.deltaTime;

			CountdownText.text = Mathf.Ceil(CountdownCurrent).ToString();

			if (CountdownCurrent < 0f)
			{
				game.Enter<Playing> ();
			}
		} 
		else
		{
			CountdownCurrent = CountdownTime;
			CountdownPanel.SetActive (false);
		}

		if (Input.anyKeyDown)
		{
			// filter out already used keys
			char[] _candidateKeys = Input.inputString
				.Where (c => !AlreadyUsedKeys.Contains(c))
				.ToArray();

			if (string.IsNullOrEmpty (new string(_candidateKeys)))
				return;

			char _candidateKey = _candidateKeys [0];
			AlreadyUsedKeys.Add (_candidateKey);
			Debug.LogFormat ("Candidate key is: {0}", _candidateKey);

			// if this is a new player
			if (CurrentlyJoining == null)
			{
				Debug.Log ("Creating new player");
				CurrentlyJoining = new KeyData ();
				CurrentlyJoining.KeyOne = _candidateKey.ToString();

			}
			// if we are finishing off the old player
			else
			{
				CurrentlyJoining.KeyTwo = _candidateKey.ToString();

				Debug.LogFormat("Finishing new player {0},{1}",
					CurrentlyJoining.KeyOne, CurrentlyJoining.KeyTwo);

				game.Players.Add(game.factory.CreatePlayer (
					CurrentlyJoining.KeyOne, CurrentlyJoining.KeyTwo, 
					Random.insideUnitCircle * 3f));

				CurrentlyJoining = null;
			}
		}
	}
}
