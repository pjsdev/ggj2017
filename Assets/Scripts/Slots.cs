using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slots : MonoBehaviour 
{
	public Transform Consume() 
	{
		foreach (Transform c in transform)
		{
			if (c.childCount == 0)
			{
				var go = new GameObject ();
				go.transform.SetParent (c);
				return c;
			}
		}

		return null;
	}
}
