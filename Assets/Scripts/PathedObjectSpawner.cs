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

	GameObject Current;

	IEnumerator Start () 
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



			Vector3 newDir = Vector3.RotateTowards(
				Current.transform.forward, 
				end.position - start.position, 1f, 0.0F);

			Current.transform.rotation = Quaternion.LookRotation (newDir);

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
