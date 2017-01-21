using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleFSM;

public class PlayerController : StateMachine 
{	
	public string KeyOne;
	public string KeyTwo;
	public Color HairColor;
	public Color SuitColor;
		
	public static void SetHairAndSuitColor(GameObject _root, Color _hair, Color _suit)
	{
		var hair = _root.transform.Find ("Hair");
		var suit = _root.transform.Find ("Suit");

		hair.GetComponent<SpriteRenderer> ().color = _hair;
		suit.GetComponent<SpriteRenderer> ().color = _suit;
	}

	void Start () 
	{
		var InMenuGO = transform.Find ("InMenu").gameObject;
		var InMenuReadGO = transform.Find ("InMenuReady").gameObject;
		var OnWaveGO = transform.Find ("OnWave").gameObject;

		AddState (new InMenu(this, InMenuGO));
		AddState (new InMenuReady(this, InMenuReadGO));
		AddState (new OnWave (this, OnWaveGO));

		Enter<InMenu> ();
	}
}
