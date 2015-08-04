#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;

public class BindAnimations : EditorWindow {
	public GameObject character;
	public AnimationClip idleAnim;
	public AnimationClip idleQuarterLeft;
	public AnimationClip idleQuarterRight;
	public AnimationClip idleHalfLeft;
	public AnimationClip idleHalfRight;
	public AnimationClip walkAnim;
	public AnimationClip walkTurnLeft;
	public AnimationClip walkTurnRight;
	public AnimationClip walkHardLeft;
	public AnimationClip walkHardRight;
	public AnimationClip runAnim;
	public AnimationClip runTurnLeft;
	public AnimationClip runTurnRight;
	public AnimationClip rollAnim;
	public AnimationClip sneakIdle;
	public AnimationClip sneakIdleLeft;
	public AnimationClip sneakIdleRight;
	public AnimationClip sneakFwd;
	public AnimationClip sneakFwdLeft;
	public AnimationClip sneakFwdRight;


	[MenuItem ("WoV/Animation Binder")]
	static void OpenWindow () {
		GetWindow<BindAnimations>();
	}

	//Editor Window GUI
	void OnGUI () {
		character = EditorGUILayout.ObjectField ("Character", character, typeof(GameObject), true) as GameObject;

		idleAnim = EditorGUILayout.ObjectField ("Idle", idleAnim, typeof(AnimationClip), false) as AnimationClip;
		idleQuarterLeft = EditorGUILayout.ObjectField ("Idle Quarter Left", idleQuarterLeft, typeof(AnimationClip), false) as AnimationClip;
		idleQuarterRight = EditorGUILayout.ObjectField ("Idle Quarter Right", idleQuarterRight, typeof(AnimationClip), false) as AnimationClip;
		idleHalfLeft = EditorGUILayout.ObjectField ("Idle Half Left", idleHalfLeft, typeof(AnimationClip), false) as AnimationClip;
		idleHalfRight = EditorGUILayout.ObjectField ("Idle Half Right", idleHalfRight, typeof(AnimationClip), false) as AnimationClip;

		walkAnim = EditorGUILayout.ObjectField ("Walk", walkAnim, typeof(AnimationClip), false) as AnimationClip;
		walkTurnLeft = EditorGUILayout.ObjectField ("Walk Left", walkTurnLeft, typeof(AnimationClip), false) as AnimationClip;
		walkTurnRight = EditorGUILayout.ObjectField ("Walk Right", walkTurnRight, typeof(AnimationClip), false) as AnimationClip;
		walkHardLeft = EditorGUILayout.ObjectField ("Walk Hard Left", walkHardLeft, typeof(AnimationClip), false) as AnimationClip;
		walkHardRight = EditorGUILayout.ObjectField ("Walk Hard Right", walkHardRight, typeof(AnimationClip), false) as AnimationClip;

		runAnim = EditorGUILayout.ObjectField ("Run", runAnim, typeof(AnimationClip), false) as AnimationClip;
		runTurnLeft = EditorGUILayout.ObjectField ("Run Left", runTurnLeft, typeof(AnimationClip), false) as AnimationClip;
		runTurnRight = EditorGUILayout.ObjectField ("Run Right", runTurnRight, typeof(AnimationClip), false) as AnimationClip;

		rollAnim = EditorGUILayout.ObjectField ("Roll", rollAnim, typeof(AnimationClip), false) as AnimationClip;

		sneakIdle = EditorGUILayout.ObjectField ("Sneak Idle", sneakIdle, typeof(AnimationClip), false) as AnimationClip;
		sneakIdleLeft = EditorGUILayout.ObjectField ("Sneak Left", sneakIdleLeft, typeof(AnimationClip), false) as AnimationClip;
		sneakIdleRight = EditorGUILayout.ObjectField ("Sneak Right", sneakIdleRight, typeof(AnimationClip), false) as AnimationClip;
		sneakFwd = EditorGUILayout.ObjectField ("Sneak Walk", sneakFwd, typeof(AnimationClip), false) as AnimationClip;
		sneakFwdLeft = EditorGUILayout.ObjectField ("Sneak Walk Left", sneakFwdLeft, typeof(AnimationClip), false) as AnimationClip;
		sneakFwdRight = EditorGUILayout.ObjectField ("Sneak Walk Right", sneakFwdRight, typeof(AnimationClip), false) as AnimationClip;

		if (GUILayout.Button ("Bind")) {

			if(character == null){
				Debug.LogError("Character missing!");
				return;
			}

			Bind();
		}
	}

	void Bind(){
		//Create a controller
		AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath ("Assets/" + character.name + ".controller");

		//Controller parameters
		controller.AddParameter ("Speed", AnimatorControllerParameterType.Float);
		controller.AddParameter ("Turn", AnimatorControllerParameterType.Float);
		controller.AddParameter ("Grounded", AnimatorControllerParameterType.Bool);
		controller.AddParameter ("Sneak", AnimatorControllerParameterType.Bool);
		controller.AddParameter ("Roll", AnimatorControllerParameterType.Trigger);

		//Grounded Blend Tree
		BlendTree groundedBT;
		AnimatorState grounded = controller.CreateBlendTreeInController ("Grounder", out groundedBT);

		//Setup Grounded Blend Tree
		groundedBT.blendType = BlendTreeType.FreeformDirectional2D;
		groundedBT.blendParameter = "Turn";
		groundedBT.blendParameterY = "Speed";
		groundedBT.AddChild (idleAnim, new Vector2 (0f, 0f));
		groundedBT.AddChild (idleHalfLeft, new Vector2 (-1f, 0f));
		groundedBT.AddChild (idleHalfRight, new Vector2 (1f, 0f));
		groundedBT.AddChild (idleQuarterLeft, new Vector2 (-0.5f, 0f));
		groundedBT.AddChild (idleQuarterRight, new Vector2 (0.5f, 0f));
		groundedBT.AddChild (walkAnim, new Vector2 (0f, 0.5f));
		groundedBT.AddChild (walkTurnLeft, new Vector2 (-0.5f, 0.5f));
		groundedBT.AddChild (walkTurnRight, new Vector2 (0.5f, 0.5f));
		groundedBT.AddChild (walkHardLeft, new Vector2 (-1f, 0.5f));
		groundedBT.AddChild (walkHardRight, new Vector2 (1f, 0.5f));
		groundedBT.AddChild (runAnim, new Vector2 (0f, 1f));
		groundedBT.AddChild (runTurnLeft, new Vector2 (-1f, 1f));
		groundedBT.AddChild (runTurnRight, new Vector2 (1f, 1f));

		//Sneak Blend Tree
		BlendTree sneakBT;
		AnimatorState sneak = controller.CreateBlendTreeInController ("Sneak", out sneakBT);

		//Setup Sneak Blend Tree
		sneakBT.blendType = BlendTreeType.FreeformDirectional2D;
		sneakBT.blendParameter = "Turn";
		sneakBT.blendParameterY = "Speed";
		sneakBT.AddChild (sneakIdle, new Vector2 (0f, 0f));
		sneakBT.AddChild (sneakIdleLeft, new Vector2 (-1f, 0f));
		sneakBT.AddChild (sneakIdleRight, new Vector2 (1f, 0f));
		sneakBT.AddChild (sneakFwd, new Vector2 (0f, 1f));
		sneakBT.AddChild (sneakFwdLeft, new Vector2 (-1f, 1f));
		sneakBT.AddChild (sneakFwdRight, new Vector2 (1f, 1f));

		//Add roll stat
		AnimatorState roll = controller.layers [0].stateMachine.AddState ("Roll");
		roll.motion = rollAnim;

		//Add transitions
		AnimatorStateTransition startSneak = grounded.AddTransition (sneak);
		AnimatorStateTransition leaveSneak = sneak.AddTransition (grounded);
		AnimatorStateTransition startRoll = grounded.AddTransition (roll);

		//Set transition conditions
		startSneak.AddCondition (AnimatorConditionMode.If, 1f, "Sneak");
		leaveSneak.AddCondition (AnimatorConditionMode.IfNot, 1f, "Sneak");
		startRoll.AddCondition (AnimatorConditionMode.If, 1f, "Roll");

		//Add the controller to the character
		character.GetComponent<Animator> ().runtimeAnimatorController = controller;
	}
}
#endif