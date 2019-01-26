using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
	[SerializeField]
	private int currentPlayer;
	private int totalPlayers = 3;
	private List<PlayerController> playerCharacters;

	// Use this for initialization
	void Start()
	{
		currentPlayer = 0;
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void GetNextCharacter()
	{
		playerCharacters[currentPlayer].isCurrentlyControlled = false;
		playerCharacters[currentPlayer+1].isCurrentlyControlled = true;

	}

	private void GetLastCharacter()
	{

	}
}