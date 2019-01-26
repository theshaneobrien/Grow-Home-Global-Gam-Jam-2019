﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
	[SerializeField]
	List<PlayerController> tempArray;

	[SerializeField]
	private List<PlayerController> playerCharacters;
	private List<PlayerController> plantedCharacters;

	// Use this for initialization
	void Start()
	{
		plantedCharacters = new List<PlayerController>();
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
			GetNextCharacter();
		}
	}

	public void GetLastCharacter()
	{
		tempArray = new List<PlayerController>(new PlayerController[3]);
		for (int i = 0; i < playerCharacters.Count; i++)
		{
			if (i < playerCharacters.Count - 1)
			{
				tempArray[i] = playerCharacters[i + 1];
			}
			else
			{
				tempArray[i] = playerCharacters[0];
			}
		}
		playerCharacters = tempArray;

		RearrangeCharacterOrder();
	}

	public void GetNextCharacter()
	{
		tempArray = new List<PlayerController>(new PlayerController[3]);
		for (int i = 0; i < playerCharacters.Count; i++)
		{
			if (i < 1)
			{
				tempArray[i] = playerCharacters[playerCharacters.Count - 1];
			}
			else
			{
				tempArray[i] = playerCharacters[i - 1];
			}
		}
		playerCharacters = tempArray;

		RearrangeCharacterOrder();
	}

	public void RearrangeCharacterOrder()
	{
		int followOrder = 0;
		for (int i = 0; i < playerCharacters.Count; i++)
		{
			if (i < 1)
			{
				playerCharacters[i].isCurrentlyControlled = true;
			}
			else
			{
				playerCharacters[i].isCurrentlyControlled = false;
			}

			followOrder += 15;
			playerCharacters[i].followerOrder = followOrder;
		}
	}

	public void MoveToPlantList(PlayerController playerController)
	{
		plantedCharacters.Add(playerController);
		playerCharacters.Remove(playerController);
	}
}