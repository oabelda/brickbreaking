﻿using UnityEngine;
using System.Collections;

public class DestroyOnTime : MonoBehaviour {

	public float destroyTime = 1.0f;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, destroyTime);
	}
}
