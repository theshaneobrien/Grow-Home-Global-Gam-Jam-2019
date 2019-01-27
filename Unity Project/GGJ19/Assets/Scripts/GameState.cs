using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

	static public GameState Instance { get; private set; }

	private int currentLevel = 0;

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

	public void LoadNextLevel()
	{
		currentLevel++;
		SceneManager.LoadScene(currentLevel);
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}
}
