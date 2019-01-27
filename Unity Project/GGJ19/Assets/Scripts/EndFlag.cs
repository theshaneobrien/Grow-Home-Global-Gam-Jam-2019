﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFlag : MonoBehaviour {

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Character")
		{
			collision.gameObject.GetComponent<PlayerController>().SaveOneChar();
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{

	}
}
