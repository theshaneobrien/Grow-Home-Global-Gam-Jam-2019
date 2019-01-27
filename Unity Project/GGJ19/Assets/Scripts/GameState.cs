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
	}
	

	void Update () {
		if (safeToChangeMusic)
		{
			musicPlayer.PlayOneShot(levelSong);
		}
	}

	public void LoadNextLevel()
	{
		musicPlayer.Stop();
		currentLevel++;
		SceneManager.LoadScene(currentLevel);
		musicPlayer.clip = levelSong;
		musicPlayer.Play();
	}

	public void ResetLevel()
	{
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
