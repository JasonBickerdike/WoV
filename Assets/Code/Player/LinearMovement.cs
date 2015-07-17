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
	/// The player movement direction.
	/// </summary>
	public enum PlayerMovementDirection
	{
		Forward = 0,
		Left,
		Back,
		Right,
	}

	public class LinearMovement : MonoBehaviour
	{
		/// <summary>
		/// The player object.
		/// </summary>
		private GameObject playerObject = null;

		/// <summary>
		/// The player walk speed.
		/// </summary>
		public float playerBaseSpeed = 5.0f;
		private float playerSpeed = 5.0f;

		/// <summary>
		/// The player sprint speed.
		/// </summary>
		public float playerSprintSpeedModifier = 2.5f;

		/// <summary>
		/// The player sneak speed.
		/// </summary>
		public float playerSneakSpeed = 1.0f;

		/// <summary>
		/// The current player direction.
		/// </summary>
		private PlayerMovementDirection playerDirection = PlayerMovementDirection.Forward;
		public PlayerMovementDirection PlayerDirection {
			get { return playerDirection; }
			set { playerDirection = value; }
		}

		/// <summary>
		/// The auto move flag for the player.
		/// </summary>
		private bool enableAutoMove = false;
		public bool EnableAutoMove {
			get { return enableAutoMove; }
			set { enableAutoMove = value; }
		}

		/// <summary>
		/// The sprint flag for the player.
		/// </summary>
		private bool enableSprint = false;
		public bool EnableSprint {
			get { return enableSprint; }
			set { enableSprint = value; }
		}

		/// <summary>
		/// The enable sprint lock.
		/// </summary>
		private bool enableSprintLock = false;

		/// <summary>
		/// The sneak flag for the player.
		/// </summary>
		private bool enableSneak = false;
		public bool EnableSneak {
			get { return enableSneak; }
			set { enableSneak = value; }
		}

		/// <summary>
		/// The player is moving.
		/// </summary>
		private bool isMoving = false;
		public bool IsMoving {
			get { return isMoving; }
		}

		/// <summary>
		/// Start of this instance.
		/// </summary>
		public void Start ()
		{
			// Grab the player object.
			playerObject = GameObject.FindGameObjectWithTag ("Player");

			// Ensure we have a player game object.
			if (playerObject == null) {
				throw new UnityException ("The player object must be set!");
			}
		}

		/// <summary>
		/// Update is called once per frame
		/// </summary>
		public void Update ()
		{
			// Reset our moving flag.
			playerSpeed = playerBaseSpeed;

			// Modify speed for case where we are sneaking.
			if (EnableSneak) {
				playerSpeed = playerSneakSpeed;
			}

			// Modify speed for case where we should be running.
			if (EnableSprint) {
				playerSpeed += playerSprintSpeedModifier;
			}

			// Move forward.
			if (Input.GetKeyDown (KeyCode.W)) {
				PlayerDirection = PlayerMovementDirection.Forward;
				isMoving = true;
			} else if (Input.GetKeyUp (KeyCode.W)) {
				isMoving = false;
			}

			// Move left.
			if (Input.GetKeyDown (KeyCode.A)) {
				PlayerDirection = PlayerMovementDirection.Left;
				isMoving = true;
			} else if (Input.GetKeyUp (KeyCode.A)) {
				isMoving = false;
			}

			// Move back.
			if (Input.GetKeyDown (KeyCode.S)) {
				PlayerDirection = PlayerMovementDirection.Back;
				isMoving = true;
			} else if (Input.GetKeyUp (KeyCode.S)) {
				isMoving = false;
			}

			// Move right.
			if (Input.GetKeyDown (KeyCode.D)) {
				PlayerDirection = PlayerMovementDirection.Right;
				isMoving = true;
			} else if (Input.GetKeyUp (KeyCode.D)) {
				isMoving = false;
			}

			// Move the player.
			HandleMovement ();

			// Jump.
			if (Input.GetKeyDown (KeyCode.Space)) {
				// Handle jump call.
			}

			// Enable/disable automove.
			if (Input.GetKeyUp (KeyCode.X)) {
				EnableAutoMove = !enableAutoMove;
			}

			// Enable/disable sprint.
			if (Input.GetKey (KeyCode.CapsLock)) {
				enableSprintLock = !enableSprintLock;
			}

			if (Input.GetKeyDown (KeyCode.LeftShift) || 
				Input.GetKeyDown (KeyCode.RightShift)) {
				EnableSprint = !enableSprint;
			} else if ((Input.GetKeyUp (KeyCode.LeftShift) ||
				Input.GetKeyUp (KeyCode.RightShift)) && !enableSprintLock) {
				EnableSprint = false;
			}

			// If enableSprintLock is true we should sprint.
			if (enableSprintLock) {
				EnableSprint = true;
			}

			// Enable/disable sneak.
			if (Input.GetKeyUp (KeyCode.LeftAlt)) {
				EnableSneak = !enableSneak;
			}
		}

		/// <summary>
		/// Handles the movement.
		/// </summary>
		protected void HandleMovement ()
		{
			if (isMoving || EnableAutoMove) {
				switch (PlayerDirection) {
				case PlayerMovementDirection.Forward:
					playerObject.transform.position += 
						transform.forward * playerSpeed * Time.deltaTime;
					break;
				case PlayerMovementDirection.Left:
					playerObject.transform.position -=
						transform.right * playerSpeed * Time.deltaTime;
					break;
				case PlayerMovementDirection.Back:
					playerObject.transform.position -= 
						transform.forward * playerSpeed * Time.deltaTime;
					break;
				case PlayerMovementDirection.Right:
					playerObject.transform.position +=
						transform.right * playerSpeed * Time.deltaTime;
					break;
				}
			}
		}
	}
}