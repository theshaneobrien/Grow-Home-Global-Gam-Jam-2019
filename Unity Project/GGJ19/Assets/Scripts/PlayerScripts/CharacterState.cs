using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
	private int currentPlayer;
	private int totalPlayers = 3;

	[SerializeField]
	private List<PlayerController> playerCharacters;

	// Use this for initialization
	void Start()
	{
		currentPlayer = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			GetNextCharacter();
		}
	}

	private void GetNextCharacter()
	{

		currentPlayer++;
		if (currentPlayer >= 3)
		{
			currentPlayer = 0;
		}
		if (currentPlayer < 3)
		{
			foreach (PlayerController player in playerCharacters)
			{
				player.isCurrentlyControlled = false;
			}
			playerCharacters[currentPlayer].isCurrentlyControlled = true;
		}
	}

	private void GetLastCharacter()
	{

	}
}