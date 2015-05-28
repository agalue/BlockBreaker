using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	static int liveRemaining = -1;

	public int maxLives = 5;
	public GameObject lifeIcon;
	
	private Ball ball;
	private float lifeSpawnOffset = 0.5f;
	private float lifeSpawnPadding = 1f;
	private GameObject[] lifes;
	
	public void Start() {
		if (liveRemaining < 0) { // Be sure to initialize once per game.
			liveRemaining = maxLives;
		}

		ball = GameObject.FindObjectOfType<Ball>();

		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 rightTop = Camera.main.ViewportToWorldPoint(new Vector3(0,1,distance));
		rightTop.x += lifeSpawnOffset + lifeSpawnPadding;
		rightTop.y -= lifeSpawnOffset;
		DisplayLifes(rightTop);
		
		DisplayLifes(rightTop);
	}
	
	public void LoadLevel(string name) {
		Debug.Log("Level load requested for: " + name);
		ReinitializeBrickCount();
		Application.LoadLevel(name);
	}

	public void PlayerLose() {
		DecreaseLife();
		if (liveRemaining == 0) {
			ReinitializeBrickCount();
			liveRemaining = -1; // Initialize the live remaining counter
			LoadLevel("Lose");
		} else {
			StartCoroutine(Reinitialize());
		}
	}

	public void QuitRequest() {
		Debug.Log("Quit has been requested");
		Application.Quit();
	}
	
	public void BrickDestroyed() {
		if (Brick.breakableCount <= 0) {
			StartCoroutine(LoadNextLevel());
		}
	}
	
	private IEnumerator Reinitialize() {
		yield return new WaitForSeconds(1f);
		ball.Reinitialize();
	}
	
	private IEnumerator LoadNextLevel() {
		ball.rigidbody2D.velocity = Vector2.zero; // Stop the ball prior loading the next level
		yield return new WaitForSeconds(1f);
		int nextLevel = Application.loadedLevel + 1;
		Debug.Log("Loading level " + nextLevel);
		ReinitializeBrickCount();
		Application.LoadLevel(nextLevel);
	}
	
	private void ReinitializeBrickCount() {
		Brick.breakableCount = 0; // Initialize the brick counter
	}
	
	private void DisplayLifes(Vector3 reference) {
		if (!Application.loadedLevelName.StartsWith("Level")) {
			return; // Showing remaining lifes makes sense only on playable scenes
		}
		lifes = new GameObject[liveRemaining];
		for (int i = 0; i < liveRemaining; i++) {
			Vector3 pos = new Vector3(reference.x + i * lifeSpawnOffset, reference.y, 5); // z=-5 means appear in front of everything
			lifes[i] = Instantiate(lifeIcon, pos, Quaternion.identity) as GameObject;
			lifes[i].transform.parent = transform;
		}
	}
	
	private void DecreaseLife() {
		liveRemaining--;
		Destroy(lifes[liveRemaining]);
	}
	
}
