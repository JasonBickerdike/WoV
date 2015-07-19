using UnityEngine;
using System.Collections;

/**
 * Class: 	InputManager
 * Author:	Victor
 * Package: WoV
 */

namespace WoVEssentials
{
	/// <summary>
	/// Handles linear movement for the player.
	/// </summary>
	public class LinearMovement : MonoBehaviour
	{
		/// <summary>
		/// The transformation speed of the player.
		/// </summary>
		public float PlayerSpeed = 0.1f;

		/// <summary>
		/// The player speed modifier.
		/// 
		/// Useful for effects on the player speed
		/// without modifying the base speed.
		/// </summary>
		private float playerSpeedModifier = 1.0f;
		public float PlayerSpeedModifier {
			get { return playerSpeedModifier; }
			set { playerSpeedModifier = value; }
		}

		/// <summary>
		/// The player sprint modifier.
		/// </summary>
		public float PlayerSprintModifier = 2.0f;

		/// <summary>
		/// The player sneak modifer.
		/// </summary>
		public float PlayerSneakModifer = 0.3f;

		/// <summary>
		/// The camera blend speed.
		/// </summary>
		public float BlendSpeed = 100f;

		/// <summary>
		/// The player position.
		/// </summary>
		public Vector3 PlayerPos = Vector3.zero;

		public float TimeReq = 0;
		public float CurTime = 0;

		/// <summary>
		/// The hit ground threshold for testing if we're grounded.
		/// </summary>
		public float hitGroundThreshold = 0.2f;

		/// <summary>
		/// The jump speed.
		/// Normalized value of 0 - 1.
		/// </summary>
		public float JumpForce = 250.0f;

		/// <summary>
		/// The jump force modifier.
		/// </summary>
		private float jumpForceModifier = 1.0f;
		public float JumpForceModifier {
			get { return jumpForceModifier; }
			set { jumpForceModifier = value; }
		}

		/// <summary>
		/// The enable automove flag.
		/// </summary>
		private bool enableAutomove = false;
		public bool EnableAutomove {
			get { return enableAutomove; }
			set { enableAutomove = value; }
		}

		/// <summary>
		/// The enable sprint flag.
		/// </summary>
		private bool enableSprintLock = false;
		public bool EnableSprintLock {
			get { return enableSprintLock; }
			set { enableSprintLock = value; }
		}

		/// <summary>
		/// The enable sneak flag.
		/// </summary>
		private bool enableSneak = false;
		public bool EnableSneak {
			get { return enableSneak; }
			set { enableSneak = value; }
		}

		/// <summary>
		/// The is jumping flag for the player.
		/// </summary>
		private bool isJumping = false;
		public bool IsJumping {
			get { return isJumping; }
		}

		/// <summary>
		/// Awake this instance.
		/// </summary>
		public void Awake ()
		{
			// Players Position
			PlayerPos = this.transform.position;
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public void Update ()
		{
			// Handle any input keys that don't use the Input Manager.
			HandleInputKeys ();
		}

		/// <summary>
		/// Called every physics tick.
		/// </summary>
		public void FixedUpdate ()
		{
			PlayerPos.y = transform.position.y;

			if (Input.GetAxis ("Vertical") > 0) {
				HandleVerticalMovement ();
			}
				
			if (Input.GetAxis ("Vertical") < 0) {
				HandleVerticalMovement (-1);
			}
			
			if (Input.GetAxis ("Horizontal") < 0) {
				HandleHorizontalMovement (-1);
			}
			if (Input.GetAxis ("Horizontal") > 0) {
				HandleHorizontalMovement ();
			}

			// Handle the automove.
			if (EnableAutomove) {
				HandleVerticalMovement (1, false);
			}

			// Handle the jumping movement.
			if (IsJumping) {
				HandleJumpMovement ();
			}

			// Slowly move camera to that new position.
			transform.position = Vector3.Lerp (
				transform.position, 
				PlayerPos, 
				BlendSpeed * Time.deltaTime
			);			

			PlayerPos = transform.position;
		}

		/// <summary>
		/// Determines whether this player is grounded.
		/// 
		/// TODO: This could be better...
		/// </summary>
		/// <returns><c>true</c> if this player is grounded; otherwise, <c>false</c>.</returns>
		public bool IsGrounded ()
		{
			RaycastHit hit;
			if (Physics.Raycast (
				this.transform.position,
				-Vector3.up,
				out hit
			)) {
				if (hit.distance < hitGroundThreshold) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Handles the input keys.
		/// </summary>
		protected void HandleInputKeys ()
		{
			// Enable/disable automove.
			if (Input.GetButtonUp ("AutoMove")) {
				EnableAutomove = !EnableAutomove;
			}

			// Enable/disable sprint.
			if (Input.GetButtonUp ("SprintLock")) {
				EnableSprintLock = !EnableSprintLock;
			}

			// Enable/disable sneak.
			if (Input.GetButtonUp ("Sneak")) {
				EnableSneak = !enableSneak;

				// Setup the player speed modifier.
				if (EnableSneak) {
					PlayerSpeedModifier = PlayerSneakModifer;
				} else {
					playerSpeedModifier = 1.0f;
				}

				// Let others know we've modified the speed.
				// TODO: When we have effect modifiers in place.
				// SendMessage ("PlayerSpeedModifierUpdate");
			}

			// Set the IsJumping flag for the player.
			// TODO: May want to check for flying mechanic later.
			if (Input.GetButtonDown ("Jump") && !IsJumping) {
				float jumpForce = JumpForce * JumpForceModifier;
				isJumping = true;

				Rigidbody rigidBody = GetComponent<Rigidbody> ();
				rigidBody.AddForce (Vector3.up * jumpForce);
			}
		}

		/// <summary>
		/// Handles the vertical movement.
		/// </summary>
		/// <param name="Direction">Direction.</param>
		protected void HandleVerticalMovement (int Direction = 1, bool disableAutoMove = true)
		{
			// Exit if we're jumping.
			// TODO: Do we want to handle cases where the player is going backwards?
			if (IsJumping && Direction != 1) {
				return;
			}

			// Set the new base player speed.
			float newPlayerSpeed = PlayerSpeed * PlayerSpeedModifier;

			// Modify the position.
			if ((!Input.GetButton ("Sprint") && !enableSprintLock) || IsJumping) {
				PlayerPos += (transform.forward * newPlayerSpeed) * Direction;
			} else {
				PlayerPos += (transform.forward * (newPlayerSpeed * PlayerSprintModifier)) * Direction;
			}

			// Disable the automove.
			if (disableAutoMove) {
				EnableAutomove = false;
			}
		}

		/// <summary>
		/// Handles the horizontal movement.
		/// </summary>
		/// <param name="Direction">Direction.</param>
		protected void HandleHorizontalMovement (int Direction = 1)
		{
			// Exit if we're jumping.
			if (IsJumping) {
				return;
			}

			// Set the new base player speed.
			float newPlayerSpeed = PlayerSpeed * PlayerSpeedModifier;

			// Modify the position.
			if (!Input.GetButton ("Sprint") && !enableSprintLock) {
				PlayerPos += (transform.right * newPlayerSpeed) * Direction;
			} else {
				PlayerPos += (transform.right * (newPlayerSpeed * PlayerSprintModifier)) * Direction;
			}

			// Disable the automove.
			EnableAutomove = false;
		}

		/// <summary>
		/// Handles the jump movement.
		/// </summary>
		protected void HandleJumpMovement ()
		{
			// We should be grounded before attempting to
			// jump.
			Rigidbody rigidBody = GetComponent<Rigidbody> ();

			if (IsGrounded () && rigidBody.velocity.y < 0.0f) {
				isJumping = false;
				return;
			}

			// Disable the automove.
			EnableAutomove = false;
		}
	}
}