using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
	[SerializeField]
	List<PlayerController> tempArray;

	[SerializeField]
	private List<PlayerController> playerCharacters;

	// Use this for initialization
	void Start()
	{

		Application.targetFrameRate = 60;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			GetNextCharacter();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			GetLastCharacter();
		}
	}

	private void GetNextCharacter()
	{
		tempArray = new List<PlayerController>(new PlayerController[3]);
		tempArray[0] = playerCharacters[1];
		tempArray[1] = playerCharacters[2];
		tempArray[2] = playerCharacters[0];
		playerCharacters = tempArray;

		rearrangeCharacterOrder();
	}

	private void GetLastCharacter()
	{
		tempArray = new List<PlayerController>(new PlayerController[3]);
		tempArray[0] = playerCharacters[2];
		tempArray[1] = playerCharacters[0];
		tempArray[2] = playerCharacters[1];
		playerCharacters = tempArray;

		rearrangeCharacterOrder();
	}

	private void rearrangeCharacterOrder()
	{
		playerCharacters[0].isCurrentlyControlled = true;
		playerCharacters[1].isCurrentlyControlled = false;
		playerCharacters[2].isCurrentlyControlled = false;
		playerCharacters[0].followerOrder = 0;
		playerCharacters[1].followerOrder = 15;
		playerCharacters[2].followerOrder = 30;
	}
}