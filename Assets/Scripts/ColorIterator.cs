using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorIterator
{
	public static int IndexOf(Color _color)
	{
		return System.Array.IndexOf(Colors, _color);	
	}

	public readonly static Color[] Colors = new Color[]{
		
		new Color(255 / 255.0f, 0 / 255.0f, 0 / 255.0f),
		new Color(0 / 255.0f, 255 / 255.0f, 0 / 255.0f),
		new Color(0 / 255.0f, 255 / 255.0f, 255 / 255.0f),
		new Color(0 / 255.0f, 128 / 255.0f, 255 / 255.0f),
		new Color(128 / 255.0f, 0 / 255.0f, 255 / 255.0f),
		new Color(255 / 255.0f, 0 / 255.0f, 255 / 255.0f),
		new Color(255 / 255.0f, 0/ 255.0f, 128 / 255.0f),
		new Color(255 / 255.0f, 255 / 255.0f, 0 / 255.0f)
	};

	int Current = 0;

	public ColorIterator()
	{
		Current = Random.Range (0, Colors.Length);
	}

	public Color PreviousColor()
	{
		// about to go out of range, circle back
		if (Current == 0)
			Current = Colors.Length - 1;
		else
			Current--;

		return Colors[Current];
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
