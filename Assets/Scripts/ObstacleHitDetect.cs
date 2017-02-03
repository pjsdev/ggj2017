using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHitDetect : MonoBehaviour 
{
	PlayerController Controller;

	void Start()
	{
		Controller = GetComponentInParent<PlayerController> ();
		Debug.Assert (Controller != null);
	}

	void OnTriggerEnter2D(Collider2D _coll)
	{
		if (_coll.CompareTag ("Obstacle"))
		{
			(Controller.CurrentState () as OnWave).HitObstacle ();
		}
	}
}
