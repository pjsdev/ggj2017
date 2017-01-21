using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorIterator
{
	readonly static Color[] Colors = new Color[]{
		Color.red,
		Color.blue,
		Color.green
	};

	int Current = 0;

	public ColorIterator()
	{
		Current = Random.Range (0, Colors.Length);
	}

	public Color NextColor()
	{
		// about to go out of range, circle back
		if (Current == Colors.Length - 1)
			Current = 0;
		else
			Current++;

		return Colors[Current];
	}
}
