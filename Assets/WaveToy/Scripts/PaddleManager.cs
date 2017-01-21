using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleManager : MonoBehaviour
{
    static public int NUM_PLAYERS = 2;
    public GameObject PaddlePrefab;

    static public List<PlayerPaddle> AllPlayerPaddles = new List<PlayerPaddle>();

    void Awake()
    {
        for( int i = 0; i < NUM_PLAYERS; ++i )
        {
            if (PaddlePrefab != null)
            {
                GameObject go = GameObject.Instantiate(PaddlePrefab);
                AllPlayerPaddles.Add(go.GetComponent<PlayerPaddle>());
            }
        }
    }
    
	
	void Update ()
    {
        if (Input.GetKey(KeyCode.A))
        {
            AllPlayerPaddles[0].MoveClockwise();
        }
        if (Input.GetKey(KeyCode.S))
        {
            AllPlayerPaddles[0].MoveAntiClockwise();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            AllPlayerPaddles[1].MoveClockwise();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            AllPlayerPaddles[1].MoveAntiClockwise();
        }


    }
}
