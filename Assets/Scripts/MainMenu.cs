using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour 
{
	List<PlayerData> Players;
	List<char> AlreadyUsedKeys;

	PlayerData CurrentlyJoining = null;

	public void StartGame()
	{
		if (Players.Count < 1)
		{
			Debug.Log ("Cannot start the game with no players");
			return;
		}
	}

	void Start () 
	{
		Players = new List<PlayerData>();
		AlreadyUsedKeys = new List<char> ();
	}

	void Update ()
	{
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
				CurrentlyJoining = new PlayerData ();
				CurrentlyJoining.KeyOne = _candidateKey.ToString();

			}
			// if we are finishing off the old player
			else
			{
				CurrentlyJoining.KeyTwo = _candidateKey.ToString();

				// TODO do we need to collect them here?
				Players.Add (CurrentlyJoining);

				Debug.LogFormat("Finishing new player {0},{1}",
					CurrentlyJoining.KeyOne, CurrentlyJoining.KeyTwo);

				PlayerController.CreatePlayer (CurrentlyJoining);

				CurrentlyJoining = null;
			}
		}
	}
}
