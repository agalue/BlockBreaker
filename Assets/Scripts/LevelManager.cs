using UnityEngine;
using UnityEngine.SceneManagement;
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
	}
	
	public void LoadLevel(string name) {
		Debug.Log("Level load requested for: " + name);
		ReinitializeBrickCount();
		SceneManager.LoadScene(name);
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
		// Stop the ball prior loading the next level
		Rigidbody2D rigidbody2D = ball.GetComponent<Rigidbody2D>();
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.angularVelocity = 0;

		yield return new WaitForSeconds(1f);

		int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
		Debug.Log("Loading level " + nextLevel);
		ReinitializeBrickCount();
		SceneManager.LoadScene(nextLevel);
	}
	
	private void ReinitializeBrickCount() {
		Brick.breakableCount = 0; // Initialize the brick counter
	}
	
	private void DisplayLifes(Vector3 reference) {
		if (!SceneManager.GetActiveScene().name.StartsWith("Level")) {
			return; // Showing remaining lifes makes sense only on playable scenes
		}
		lifes = new GameObject[liveRemaining];
		Debug.Log ("Displaying " + liveRemaining + " lives");
		for (int i = 0; i < liveRemaining; i++) {
			Vector3 pos = new Vector3(reference.x + i * lifeSpawnOffset, reference.y, 5); // z=-5 means appear in front of everything
			lifes[i] = Instantiate(lifeIcon, pos, Quaternion.identity) as GameObject;
		}
	}
	
	private void DecreaseLife() {
		liveRemaining--;
		Debug.Log (liveRemaining + " lives");
		Destroy(lifes[liveRemaining]);
	}
	
}
