using UnityEngine;
using System.Collections;

namespace WoVEssentials {

	public class MainCameraFollow : MonoBehaviour {

		/* How To Interact With This Script!!! *\

		 * Cutscenes should use a gameobject with lerp to move around
		 * Set a reference to this script on it's *Awake* to MainCameraFollow.FollowThis
		 * Set a child to the object it follows to set the direction the camera will look at
		 * Set the child *LookAt* object to MainCameraFollow.LookAtThis
		 * Cutscene Done!

		*/

		public float DistanceFromPlayer;

		public float Sensitivity;
		public float BlendSpeed;
		// This will allow us to add angle to the camera in the editor
		public Vector3 CamRotMod;
		public Vector3 LookRot;

		// Left this open so we can change it in game, useful for Cutscenes!
		public GameObject FollowThis;
		public GameObject LookAtThis;

		Vector3 FollowPos;
		Vector3 NewPos;
		Vector3 PlayerRot;

		RaycastHit hit;



		void FixedUpdate () {

			FollowPos.y = FollowThis.transform.position.y + (DistanceFromPlayer / 2);
			FollowPos.z = FollowThis.transform.position.z - DistanceFromPlayer;
			FollowPos.x = FollowThis.transform.position.x;
			/*  This Section Is For Camera movement!*/
			transform.position = Vector3.Lerp(transform.position, FollowPos, BlendSpeed);

			// Create a vector from the camera towards the player.
			Vector3 LkFollowPos = LookAtThis.transform.position - transform.position;
			// Create a rotation depending on the position of the player being the forward vector.
			Quaternion lookAtRotation = Quaternion.LookRotation (LkFollowPos, Vector3.up);
			
			
			/*  This Section Is For the Player rotation!*/

			// If not holding look key then simply look at character
			if (!Input.GetButton ("Look")) {
				LookRot.y = 0;
				LookRot.x = 0;
				LookRot.z = 0;


				// This rotates the player!!
				if (FollowThis.name == "CameraLock"){

					Transform ThePlayer = FollowThis.transform.parent.transform;
					if(Input.GetAxis("Mouse X")<0){
						//Code for action on mouse moving left
						PlayerRot.y += Input.GetAxis("Mouse X") * Sensitivity;
						//				Debug.Log ("Mouse Left");
					}
					if(Input.GetAxis("Mouse X")>0){
						//Code for action on mouse moving right
						PlayerRot.y += Input.GetAxis("Mouse X") * Sensitivity;
	//				Debug.Log ("Mouse Right");
					} 
					
					// This will set the new player rotation using 360 Degree angles
					Quaternion NewPlayerRot = Quaternion.Euler (PlayerRot.x, PlayerRot.y, PlayerRot.z);
					// Lerps the player rotation to the new angle
					ThePlayer.rotation = Quaternion.Lerp (ThePlayer.rotation, NewPlayerRot, 10.0F * Time.deltaTime);
				}



				/*  This Section Is For the Camera rotation!*/

				// This just makes it possible to do rotation using the classic degress by using .Euler
				Quaternion NewRotations = Quaternion.Euler (CamRotMod.x, CamRotMod.y, CamRotMod.z);
				lookAtRotation.z -= NewRotations.z;
				lookAtRotation.x -= NewRotations.x;
				lookAtRotation.y -= NewRotations.y;
				// Lerp the cameras rotation between it's current rotation and the rotation that looks at the player.
				transform.rotation = Quaternion.Lerp (transform.rotation, lookAtRotation, 5.0F * Time.deltaTime);
			}

			if (Input.GetButton ("Look") && LookAtThis.name == "Head") {
	//			Debug.Log ("Looking");
				if(Input.GetAxis("Mouse X")<0 && LookRot.y < 40){
					//Code for action on mouse moving left
					LookRot.y -= Input.GetAxis("Mouse X") * Sensitivity / 2;
	//				Debug.Log ("Mouse Left");
				}
				if(Input.GetAxis("Mouse X")>0 && LookRot.y > -40){
					//Code for action on mouse moving right
					LookRot.y -= Input.GetAxis("Mouse X") * Sensitivity / 2;
	//				Debug.Log ("Mouse Right");
				} 

				if(Input.GetAxis("Mouse Y")<0 && LookRot.x > -30){
					//Code for action on mouse moving Down
					LookRot.x += Input.GetAxis("Mouse Y") * Sensitivity / 2;
	//				Debug.Log ("Mouse Down");
				}
				if(Input.GetAxis("Mouse Y")>0 && LookRot.x < 20){
					//Code for action on mouse moving Up
					LookRot.x += Input.GetAxis("Mouse Y") * Sensitivity / 2;
	//				Debug.Log ("Mouse Up");
				}

				// This just makes it possible to do rotation using the classic degress by using .Euler
				Quaternion NewRotations = Quaternion.Euler (CamRotMod.x + LookRot.x, CamRotMod.y + LookRot.y, CamRotMod.z);
				lookAtRotation.z -= NewRotations.z;
				lookAtRotation.x -= NewRotations.x;
				lookAtRotation.y -= NewRotations.y;
				// Lerp the cameras rotation between it's current rotation and the rotation that looks at the player.
				transform.rotation = Quaternion.Lerp (transform.rotation, lookAtRotation, 5.0F * Time.deltaTime);
			}
		}
	}
}