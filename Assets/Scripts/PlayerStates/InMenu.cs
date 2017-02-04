using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SimpleFSM;

public class InMenu : State 
{
	static readonly float ReadyTimerTheshold = 1f;
	ColorIterator SuitColors;

	PlayerController Controller;

	float ReadyTimer = 0f;

	GameObject SpriteGO;

	Text ScoreText;
	Transform KeysUI;

	static Text MakeText(GameObject _textCanvas, string _data, Vector3 _base,
		Vector3 _offset, float _size, Color _col)
	{
		GameObject textGo = GameObject.Instantiate(Resources.Load("ScoreText")) as GameObject;
		Text ScoreText = textGo.GetComponent<Text>();
		ScoreText.text = _data;
        ScoreText.transform.SetParent(_textCanvas.transform, false);
		ScoreText.transform.position = Camera.main.WorldToScreenPoint(_base) + _offset;

		ScoreText.transform.localScale = Vector3.one * _size;
		ScoreText.color = _col;	
		return ScoreText;
	}

	public InMenu(PlayerController _controller, GameObject _inMenuGO)
	{
		Controller = _controller;
		SuitColors = new ColorIterator ();
		Controller.SuitColor = SuitColors.NextColor();

		SpriteGO = _inMenuGO;
		PlayerController.SetSuitColor (SpriteGO, Controller.SuitColor);

		GameObject TextCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
		ScoreText = MakeText(
			TextCanvas, "0", SpriteGO.transform.position,
			new Vector3(0, -220f, 0), 0.8f, Color.white);

		GameObject textGo = GameObject.Instantiate(Resources.Load("KeysUI")) as GameObject;
		KeysUI = textGo.transform;
			
		KeysUI.Find("KeyOne").GetChild(0).GetComponent<Text>().text = Controller.KeyOne.ToString().ToUpper();
		KeysUI.Find("KeyTwo").GetChild(0).GetComponent<Text>().text = Controller.KeyTwo.ToString().ToUpper();
		KeysUI.transform.SetParent(TextCanvas.transform, false);
		KeysUI.transform.position = Camera.main.WorldToScreenPoint(SpriteGO.transform.position) + new Vector3(0, -240f, 0);
	}

	#region State implementation

	public void Update ()
	{
		// Debug.LogFormat ("{0} [{1}]", IsReady, ReadyTime);
		if (Input.GetKey(Controller.KeyOne))
		{
			ReadyTimer += Time.deltaTime;

			if (ReadyTimer > ReadyTimerTheshold)
			{
				Controller.Enter<InMenuReady> ();
				return;
			}
		}

		if (Input.GetKeyUp (Controller.KeyOne))
		{
			Controller.SuitColor = SuitColors.PreviousColor();
			PlayerController.SetSuitColor (SpriteGO, Controller.SuitColor);
		}

		if (Input.GetKeyUp (Controller.KeyTwo))
		{
			Controller.SuitColor = SuitColors.NextColor();
			PlayerController.SetSuitColor (SpriteGO, Controller.SuitColor);	
		}
	}

	public void Enter ()
	{
		SpriteGO.SetActive (true);
		ReadyTimer = 0f;
		KeysUI.gameObject.SetActive(true);

		if (Controller.Score != 0)
		{
			ScoreText.enabled = true;
			ScoreText.text = Controller.Score.ToString ();
		} 
		else
		{
			ScoreText.enabled = false;
		}
	}

	public void Exit ()
	{
		KeysUI.gameObject.SetActive(false);
		ScoreText.enabled = false;
		SpriteGO.SetActive (false);
	}

	#endregion


}
