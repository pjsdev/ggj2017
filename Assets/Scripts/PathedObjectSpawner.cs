using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class PathedObjectSpawner : MonoBehaviour 
{
	public Vector2 SpawnTimeRange = new Vector2(0, 1);
	public Vector2 TravelTimeRange = new Vector2(2, 5);
	public GameObject Prefab;

	public Ease Easing;

	Transform Pos1;
	Transform Pos2;

	void OnDrawGizmosSelected()
	{
		var p1 = transform.Find ("Pos1");
		var p2 = transform.Find ("Pos2");

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (p1.position, p2.position);

		Gizmos.color = Color.red;
		Gizmos.DrawCube (p1.position, Vector3.one * 0.25f);
		Gizmos.DrawCube (p2.position, Vector3.one * 0.25f);
	}

	GameObject Current;

	void OnEnable()
	{
		StartCoroutine (RunSpawns());
	}

	void Start()
	{
		StartCoroutine (RunSpawns());
	}

	IEnumerator RunSpawns () 
	{
		Pos1 = transform.Find ("Pos1");
		Pos2 = transform.Find ("Pos2");

		while (true)
		{
			if (Current != null)
			{
				yield return null;
				continue;
			}

			yield return new WaitForSeconds (Random.Range(SpawnTimeRange.x, SpawnTimeRange.y));

			Transform start = Pos1;
			Transform end = Pos2;
			// coin flip
			if (Random.Range (0, 100) > 50)
			{
				start = Pos2;
				end = Pos1;
			}

			Current = Instantiate(Prefab, start.position, Quaternion.identity);
			Vector3 targetDir = end.position - start.position;
			targetDir.Normalize ();

			Current.transform.rotation = Quaternion.Euler (0, 0,
				Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg);

			Current.transform.DOMove (
				end.position, Random.Range(TravelTimeRange.x, TravelTimeRange.y))
				.SetEase(Easing)
				.OnComplete(() => {
					Destroy(Current);
					Current = null;
				});
		}
	}
}
