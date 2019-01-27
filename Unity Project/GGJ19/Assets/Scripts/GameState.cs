using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	static public GameState Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}


	void Start () {
		
	}
	

	void Update () {
		
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}
}
