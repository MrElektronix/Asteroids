﻿using UnityEngine;
using System.Collections;

public class EnemyDeath : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Bullet")) {

			Destroy (other.gameObject);
			Destroy (gameObject);

		}

	}
}
