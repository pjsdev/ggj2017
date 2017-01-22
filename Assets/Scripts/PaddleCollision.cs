using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleCollision : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D _coll)
	{
		Debug.Log ("Hit!");
		Paddle.AddImpulse(
			Paddle.CurrentSegmentIndex - 1, 
			Paddle.CurrentSegmentIndex + 1, 0.5f);
	}

	PlayerPaddle Paddle;

	void Start () 
	{
		Paddle = transform.parent.GetComponent<PlayerPaddle>();
		Debug.Assert (Paddle != null);
	}
}
