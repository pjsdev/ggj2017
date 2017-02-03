using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class Game : StateMachine
{
	[HideInInspector]
	public Factory factory;

	[HideInInspector]
	public AudioSource Music;

	[HideInInspector]
	public AudioSource SFX;

	public List<PlayerController> Players;

	public Slots SpawnSlots;

	public DiscreteWave Waves;

	public List<int> TeamScores;
	public int TotalScore = 0;

	void Start ()
	{
		Waves = GameObject.FindObjectOfType<DiscreteWave> ();
		Waves.gameObject.SetActive (false);

		Music = GetComponent <AudioSource> ();
		factory = GameObject.FindObjectOfType<Factory> ();
		Debug.Assert (factory != null);

		SFX = transform.Find ("SFX").GetComponent<AudioSource> ();
		Debug.Assert (SFX != null);

		var spawnSlotsGO = GameObject.FindGameObjectWithTag ("SpawnPositions");
		Debug.Assert (spawnSlotsGO != null);
		SpawnSlots = spawnSlotsGO.GetComponent<Slots> ();
		Debug.Assert (SpawnSlots != null);

		Players = new List<PlayerController> ();

		AddState (new TitleScreen (this));
		AddState (new CharacterSelect (this));
		AddState (new Playing (this));

		Enter<TitleScreen> ();
	}
}
