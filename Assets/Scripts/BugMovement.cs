﻿using UnityEngine;

public class BugMovement : MonoBehaviour {

	public float moveSpeed;
	public float ceiling;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (new Vector3 (0, 10, 0) * Time.deltaTime * moveSpeed);
		transform.Rotate (new Vector3 (0, 0, 0.1f) * Mathf.Sin (10*Time.deltaTime));
		if (transform.position.y > ceiling) {
			Destroy (gameObject);
		    Lives.Instance.Value -= 1;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// You probably want a check here to make sure you're hitting a zombie
		// Note that this is not the best method for doing so.
		if ( other.CompareTag("Raindrop") && other.transform.localScale.x > 1f)
		{
			Destroy(other.gameObject);
			Destroy (this.gameObject);
		    Score.Instance.Value += 1;
		}
	}
		
}
