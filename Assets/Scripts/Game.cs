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

	public List<PlayerController> Players;

	public Slots SpawnSlots;

	void Start () 
	{
		Music = GetComponent <AudioSource> ();
		factory = GameObject.FindObjectOfType<Factory> ();
		Debug.Assert (factory != null);

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
