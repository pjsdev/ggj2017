using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
	public string KeyOne;
	public string KeyTwo;
	public Color HairColor;
	public Color SuitColor;
}

public class PlayerController : MonoBehaviour 
{
	enum PlayerState {
		InMenu = 0,
		OnWave,
		InAir
	};

	PlayerState State;
	PlayerData Data;

	SpriteRenderer Hair;
	SpriteRenderer Suit;

	ColorIterator HairColors;
	ColorIterator SuitColors;

	void SetData(PlayerData _data)
	{
		Data = _data;
	}
		
	void Start () 
	{
		Hair = transform.Find ("Hair").GetComponent<SpriteRenderer> ();
		Suit = transform.Find ("Suit").GetComponent<SpriteRenderer> ();
		HairColors = new ColorIterator ();
		SuitColors = new ColorIterator ();
		Hair.color = HairColors.NextColor();
		Suit.color = SuitColors.NextColor();
	}

	void MenuInput()
	{
		if (Input.GetKeyDown (Data.KeyOne))
		{
			Hair.color = HairColors.NextColor();
		}

		if (Input.GetKeyDown (Data.KeyTwo))
		{
			Suit.color = SuitColors.NextColor();
		}
	}

	void WaveInput()
	{
		// move
	}

	void AirInput()
	{
		// do tricks
	}

	void Update () 
	{
		switch (State)
		{
		case PlayerState.InMenu:
			MenuInput ();
			break;
		case PlayerState.OnWave:
			WaveInput ();
			break;
		case PlayerState.InAir:
			AirInput ();
			break;
		}
	}

	static GameObject CachedPlayerPrefab = null;
	public static PlayerController CreatePlayer(PlayerData _data)
	{
		if (CachedPlayerPrefab == null)
		{
			CachedPlayerPrefab = Resources.Load ("Player") as GameObject;
		}

		var created = Instantiate (CachedPlayerPrefab);
		var controller = created.GetComponent<PlayerController> ();
		controller.SetData (_data);
		return controller;
	}
}
