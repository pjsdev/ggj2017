using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour 
{
	GameObject PaddlePrefab = null;
	GameObject PlayerController = null;

	public List<Sprite> ReadyTexts;

	public Sprite RandomReadyText()
	{
		return ReadyTexts [Random.Range (0, ReadyTexts.Count)];
	}

	void Awake () 
	{
		PaddlePrefab = Resources.Load ("Paddle") as GameObject;
		PlayerController = Resources.Load ("PlayerController") as GameObject;
	}
		
	public AudioClip GetMenuMusic()
	{
		return Resources.Load ("Sounds/MenuMusic") as AudioClip;
	}

	public AudioClip GetGameMusic()
	{
		return Resources.Load ("Sounds/GameMusic") as AudioClip;
	}

	public PlayerPaddle CreatePaddle()
	{
		var created = Instantiate (PaddlePrefab);
		var paddle = created.GetComponent<PlayerPaddle> ();
		return paddle;
	}

	public PlayerController CreatePlayerController(string _keyOne, string _keyTwo, Vector3 _pos)
	{
		var created = Instantiate (PlayerController, _pos, Quaternion.identity);
		var controller = created.GetComponent<PlayerController> ();
		controller.KeyOne = _keyOne;
		controller.KeyTwo = _keyTwo;
		return controller;
	}
}
