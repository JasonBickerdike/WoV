using UnityEngine;
using System.Collections;

public class Temp_Linear_Movement : MonoBehaviour {

	public float PSpeed;	//Transform movement
	public float BlendSpeed;	// Blend speed

	public Vector3 PlayerPos; // Store where the player is

	public float TimeReq;
	public float CurTime;

	void Awake () {
		PlayerPos = this.transform.position; // Players Position
	}

	void FixedUpdate () {
		Debug.Log ("Temporary Movement Script In Use");
		PlayerPos.y = transform.position.y;
		
		if (!Input.GetButton ("Sprint")) {
			Debug.Log ("Walking");
			if (Input.GetAxis ("Vertical") > 0) {
				PlayerPos += transform.forward * PSpeed;
			}
			if (Input.GetAxis ("Vertical") < 0) {
				PlayerPos -= transform.forward * PSpeed;
			}

			if (Input.GetAxis ("Horizontal") < 0) {
				PlayerPos -= transform.right * PSpeed;
			}
			if (Input.GetAxis ("Horizontal") > 0) {
				PlayerPos += transform.right * PSpeed;
			}
		}
		if (Input.GetButton ("Sprint")) {
			Debug.Log ("Running");
			if (Input.GetAxis ("Vertical") > 0) {
				PlayerPos += transform.forward * (PSpeed * 2);
			}
			if (Input.GetAxis ("Vertical") < 0) {
				PlayerPos -= transform.forward * (PSpeed * 2);
			}
			
			if (Input.GetAxis ("Horizontal") < 0) {
				PlayerPos -= transform.right * (PSpeed * 2);
			}
			if (Input.GetAxis ("Horizontal") > 0) {
				PlayerPos += transform.right * (PSpeed * 2);
			}
		}
		transform.position = Vector3.Lerp (transform.position, PlayerPos, BlendSpeed * Time.deltaTime); // slowly move camera to that new position

		PlayerPos = transform.position;
	}


}