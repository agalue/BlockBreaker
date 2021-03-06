﻿using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	public static int breakableCount = 0;
	public AudioClip crack;
	public Sprite[] hitSprites;
	public GameObject smoke;

	private int timesHit;
	private bool isBreakable;
	private LevelManager levelManager;
	private SpriteRenderer spriteRenderer;
	
	void Start () {
		isBreakable = tag == "Breakable";
		// Keep track of breakable bricks
		if (isBreakable) {
			breakableCount++;
		}
		timesHit = 0;
		levelManager = FindObjectOfType<LevelManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void OnCollisionExit2D (Collision2D collision) {
		if (isBreakable) {
			// Be sure to play the sound even if the brick is destroyed.
			AudioSource.PlayClipAtPoint(crack, transform.position, 0.1f);
			HandleHits();
		}
	}

	void HandleHits () {
		timesHit++;
		int maxHits = hitSprites.Length + 1;
		if (timesHit >= maxHits) {
			breakableCount--;
			Debug.Log(breakableCount + " breakable bricks remaining.");
			levelManager.BrickDestroyed();
			PuffSmoke();
			Destroy(gameObject);
		} else {
			LoadSprites();
		}
	}
	
	void PuffSmoke() {
		GameObject smokePuff = Instantiate(smoke, transform.position, Quaternion.identity) as GameObject;
		smokePuff.GetComponent<ParticleSystem>().startColor = spriteRenderer.color;
	}

	void LoadSprites() {
		int spriteIndex = timesHit - 1;
		if (hitSprites[spriteIndex] != null) {
			spriteRenderer.sprite = hitSprites[spriteIndex];
		} else {
			Debug.LogError("Can't find brick sprite with index " + spriteIndex);
		}
	}
}
