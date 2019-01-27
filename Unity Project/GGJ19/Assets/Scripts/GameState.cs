using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

	static public GameState Instance { get; private set; }

	private int currentLevel = 0;

	private AudioSource musicPlayer;

	public AudioClip menuSong;
	public AudioClip levelSong;
	public AudioClip endLevelSong;

	private Canvas stateCanvas;

	public GameObject gameOverPanel;
	public GameObject winPanel;

	public bool safeToChangeMusic = false;

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
		DontDestroyOnLoad(this);
	}


	void Start () {
		musicPlayer = this.GetComponent<AudioSource>();
		stateCanvas = this.GetComponentInChildren<Canvas>();
	}
	

	void Update () {
		if (safeToChangeMusic)
		{
			musicPlayer.PlayOneShot(levelSong);
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			ResetLevel();
		}
	}

	public void LoadNextLevel()
	{

		gameOverPanel.SetActive(false);
		winPanel.SetActive(false);
		musicPlayer.Stop();
		currentLevel++;
		SceneManager.LoadScene(currentLevel);
		musicPlayer.clip = levelSong;
		musicPlayer.Play();
	}

	public void ResetLevel()
	{
		gameOverPanel.SetActive(false);
		winPanel.SetActive(false);
		SceneManager.LoadScene(currentLevel);
	}

	public void LoadMainMenu()
	{
		gameOverPanel.SetActive(false);
		winPanel.SetActive(false);
		currentLevel = 0;
		SceneManager.LoadScene(currentLevel);
	}

	public void SetUpCamera()
	{
		stateCanvas.worldCamera = Camera.main;
	}

	public void ShowGameOver()
	{
		gameOverPanel.SetActive(true);
	}

	public void ShowWinScreen()
	{
		winPanel.SetActive(true);
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}
}
