using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

	public bool autoPlay = false;
	public float minX, maxX;

	private int numberOfBlocks = 16;
	private Ball ball;
	
	void Start () {
		ball = GameObject.FindObjectOfType<Ball>();
	}

	void Update () {
		if (!autoPlay) {
			MoveWithMouse();
		} else {
			AutoPlay();
		}
	}
	
	void MoveWithMouse () {
		// Relative possition of the mouse on the screen (from 0 to 1), no matter the current size/resolution of the screen; expressed in blocks
		setPositionX((Input.mousePosition.x / Screen.width) * numberOfBlocks);
	}

	void AutoPlay () {
		setPositionX(ball.transform.position.x);
	}
	
	void setPositionX(float x) {
		float fixedX = Mathf.Clamp(x, minX, maxX);
		transform.position = new Vector3(fixedX, this.transform.position.y, 0f);
	}
}
