using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterState : MonoBehaviour
{

	private int maxPlayerHealth = 3;
	private int currentPlayerHealth = 3;

	[SerializeField]
	List<PlayerController> tempArray;

	[SerializeField]
	private List<PlayerController> playerCharacters;
	private List<PlayerController> plantedCharacters;

	public string sproutName;
	public string rayName;
	public string spruceName;
	public string sproutDescription;
	public string rayDescription;
	public string spruceDescription;
	public Text characterName;

	public Text characterDescription;

	public GameObject health1;
	public GameObject health2;
	public GameObject health3;

	// Use this for initialization
	void Start()
	{
		plantedCharacters = new List<PlayerController>();
		Application.targetFrameRate = 60;

		//SetNextCharacters();
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

	public void HealDamage()
	{
		if (currentPlayerHealth < maxPlayerHealth)
		{
			currentPlayerHealth++;
		}
	}

	public void TakeDamage()
	{
		currentPlayerHealth--;
		if (currentPlayerHealth <= 0)
		{
				health1.SetActive(false);
				health2.SetActive(false);
				health3.SetActive(false);

			GameState.Instance.ResetLevel();
		}

		if (currentPlayerHealth == 3)
		{
			health1.SetActive(true);
			health2.SetActive(true);
			health3.SetActive(true);
		}

		if (currentPlayerHealth == 2)
		{
			health1.SetActive(true);
			health2.SetActive(true);
			health3.SetActive(false);
		}
		if (currentPlayerHealth == 1)
		{
			health1.SetActive(true);
			health2.SetActive(false);
			health3.SetActive(false);
		}

		//Make invincible for a second?
	}

	private void SetNextCharacters()
	{
		for (int i = 1; i < playerCharacters.Count; i++)
		{
			playerCharacters[i].nextCharacter = playerCharacters[i - 1].transform;
		}

		playerCharacters[0].nextCharacter = null;
		playerCharacters[1].nextCharacter = playerCharacters[0].transform;
		playerCharacters[2].nextCharacter = playerCharacters[1].transform;
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
		//SetNextCharacters();
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
		//SetNextCharacters();
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
		if (playerCharacters[0].gameObject.name == "Sprout")
		{
			characterName.text = sproutName;
			characterDescription.text = sproutDescription;
		}
		else if (playerCharacters[0].gameObject.name == "Ray")
		{
			characterName.text = rayName;
			characterDescription.text = rayDescription;
		}
		else
		{
			characterName.text = spruceName;
			characterDescription.text = spruceDescription;
		}
	}

	public void MoveToPlantList(PlayerController playerController)
	{
		plantedCharacters.Add(playerController);
		playerCharacters.Remove(playerController);
	}
}