using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class CollisionSound : MonoBehaviour {

	private AudioSource hitSource;
	private void Start()
	{
		hitSource = this.GetComponent<AudioSource>();
	}
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.relativeVelocity.y > 1)
		{
			hitSource.Play();
		}
	}
}
