using UnityEngine;
using System.Collections;

public class ShowMouse : MonoBehaviour {

	public bool ShowTheMouse;

	// Use this for initialization
	void Start () {
		Cursor.visible = ShowTheMouse;
	}
}
