using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffect 
{
	Bailout1 = 0,
	Bailout2,
	Bailout3,
	Bailout4,
	Bailout5,
	Bailout6,
	Bailout7,
	BigKahuna,
	Duck,
	GreatWhite,
	Hammerhead,
	Jets,
	PlayerJoin1,
	PlayerJoin2,
	PlayerJoin3,
	PlayerJoin4,
	PlayerJoin5,
	Propeller,
	Seagull,
	Sparrow,
	SurfSelect1,
	SurfSelect2,
	SurfSelect3,
	SurfSelect4,
	SurfSelect5,
	SurfSelect6
}

public class Factory : MonoBehaviour 
{
	GameObject PaddlePrefab = null;
	GameObject PlayerController = null;

	Dictionary<SoundEffect, AudioClip> SFXClips;

	SoundEffect[] SoundEffects = new SoundEffect[]
	{
		SoundEffect.Bailout1,
		SoundEffect.Bailout2,
		SoundEffect.Bailout3,
		SoundEffect.Bailout4,
		SoundEffect.Bailout5,
		SoundEffect.Bailout6,
		SoundEffect.Bailout7,
		SoundEffect.BigKahuna,
		SoundEffect.Duck,
		SoundEffect.GreatWhite,
		SoundEffect.Hammerhead,
		SoundEffect.Jets,
		SoundEffect.PlayerJoin1,
		SoundEffect.PlayerJoin2,
		SoundEffect.PlayerJoin3,
		SoundEffect.PlayerJoin4,
		SoundEffect.PlayerJoin5,
		SoundEffect.Propeller,
		SoundEffect.Seagull,
		SoundEffect.Sparrow,
		SoundEffect.SurfSelect1,
		SoundEffect.SurfSelect2,
		SoundEffect.SurfSelect3,
		SoundEffect.SurfSelect4,
		SoundEffect.SurfSelect5,
		SoundEffect.SurfSelect6,
	};

	public List<Sprite> ReadyTexts;

	public Sprite RandomReadyText()
	{
		return ReadyTexts [Random.Range (0, ReadyTexts.Count)];
	}

	public AudioClip GetSoundEffect(SoundEffect _EffectType)
	{
		return SFXClips[_EffectType];
	}

	void Awake () 
	{
		PaddlePrefab = Resources.Load ("Paddle") as GameObject;
		PlayerController = Resources.Load ("PlayerController") as GameObject;

		SFXClips = new Dictionary<SoundEffect, AudioClip> ();
		string SfxPath = "Sounds/SFX/";

		foreach (SoundEffect i in SoundEffects)
		{
			var clip = Resources.Load (SfxPath + i.ToString()) as AudioClip;
			if (clip == null)
			{
				Debug.LogWarningFormat ("Could not load audio clip {0}", i.ToString());
				continue;
			}

			SFXClips.Add (i, clip);
		}
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
