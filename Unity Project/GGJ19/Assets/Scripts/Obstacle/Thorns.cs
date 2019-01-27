using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour {
	private int damage = 1;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Character")
		{
			collision.gameObject.GetComponent<PlayerController>().parentController.TakeDamage();
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{

	}
}
