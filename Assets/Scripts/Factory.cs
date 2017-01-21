using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour 
{
	GameObject PlayerPrefab = null;

	void Awake () 
	{
		PlayerPrefab = Resources.Load ("Player") as GameObject;
	}
		
	public AudioClip GetMenuMusic()
	{
		return Resources.Load ("Sounds/MenuMusic") as AudioClip;
	}

	public AudioClip GetGameMusic()
	{
		return Resources.Load ("Sounds/GameMusic") as AudioClip;
	}

	public PlayerController CreatePlayer(string _keyOne, string _keyTwo, Vector3 _pos)
	{
		var created = Instantiate (PlayerPrefab, _pos, Quaternion.identity);
		var controller = created.GetComponent<PlayerController> ();
		controller.KeyOne = _keyOne;
		controller.KeyTwo = _keyTwo;
		return controller;
	}
}
