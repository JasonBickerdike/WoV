using UnityEngine;
using System.Collections;

public class MainCameraFollow : MonoBehaviour {

	public int DistanceFromPlayer;

	public float BlendSpeed;
	// This will allow us to add angle to the camera in the editor
	public Vector3 CamRotMod;

	// Left this open so we can change it in game, useful for Cutscenes!
	public GameObject FollowThis;
	public GameObject LookAtThis;

	Vector3 FollowPos;
	Vector3 NewPos;
	
	RaycastHit hit;


	void Update () {

		FollowPos = FollowThis.transform.position;

		/*  This Section Is For Camera movement!*/
		transform.position = Vector3.MoveTowards(transform.position, FollowPos, 0.07f);



		/*  This Section Is For Camera rotation!*/

		// Create a vector from the camera towards the player.
		Vector3 LkFollowPos = LookAtThis.transform.position - transform.position;
		// Create a rotation depending on the position of the player being the forward vector.
		Quaternion lookAtRotation = Quaternion.LookRotation(LkFollowPos, Vector3.up);

		// This just makes it possible to do rotation using the classic degress by using .Euler
		Quaternion NewRotations = Quaternion.Euler (CamRotMod.x, CamRotMod.y, CamRotMod.z);
		lookAtRotation.z -= NewRotations.z;
		lookAtRotation.x -= NewRotations.x;
		lookAtRotation.y -= NewRotations.y;
		
		// Lerp the cameras rotation between it's current rotation and the rotation that looks at the player.
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, 5.0F * Time.deltaTime);
	}
}
