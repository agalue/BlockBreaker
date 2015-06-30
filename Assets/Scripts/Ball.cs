using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	private Paddle paddle;
	private Rigidbody2D rigidbody2D;
	private AudioSource audioSource;

	private bool hasStarted = false;
	private Vector3 paddleToBallVector;
	
	void Start () {
		paddle = FindObjectOfType<Paddle>();
		rigidbody2D = GetComponent<Rigidbody2D>();
		audioSource = GetComponent<AudioSource>();
		paddleToBallVector = this.transform.position - paddle.transform.position;
	}
	
	void Update () {
		if (!hasStarted) {
			// Lock the ball relative to the paddle
			// In order to make this line work properly, the Script Execution Order must be updated to be: Paddle first, then Ball
			transform.position = paddle.transform.position + paddleToBallVector;
			// Wait for a mouse press to launch
			if (Input.GetMouseButtonDown(0)) {
				// Launch the Ball
				rigidbody2D.velocity = new Vector2(2f, 10f);
				hasStarted = true;
			}
		}
	}
	
	void OnCollisionEnter2D (Collision2D other) {
		// TODO Ball does not trigger sound when brick is destroyed (possibly because brick isn't there).
		if (hasStarted) {
			// Avoid entering into infinite loops by adding a random velocity
			rigidbody2D.velocity += new Vector2(Random.Range (0f, 0.2f), Random.Range(0f, 0.2f));
			audioSource.Play();
		}
	}
	
	public void Reinitialize() {
		hasStarted = false;
		transform.position = paddle.transform.position + paddleToBallVector;
	}
}
