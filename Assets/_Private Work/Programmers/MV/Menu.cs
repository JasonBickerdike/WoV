using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject[] menus;
	int currentMenuID;
	int lastMenuID;

	void Start (){
		ChangeTo (menus [0]);
		currentMenuID = 0;
	}

	public void PlayButton(){
		lastMenuID = currentMenuID;
		ChangeTo (menus[1]);
		currentMenuID = 1;
	}

	public void OptionsButton(){
		lastMenuID = currentMenuID;
		ChangeTo (menus[2]);
		currentMenuID = 2;
	}

	public void QuitButton(){
#if UNITY_EDITOR
		Debug.Break();
#else
		Application.Quit();
#endif
	}

	public void NewGameButton(){
		Application.LoadLevel (1);
	}

	public void LoadGameButton(){

	}

	public void SoundOptionsButton(){
		lastMenuID = currentMenuID;
		ChangeTo (menus[3]);
		currentMenuID = 3;
	}

	public void GraphicsOptionsButton(){
		lastMenuID = currentMenuID;
		ChangeTo (menus[4]);
		currentMenuID = 4;
	}

	public void GameOptionsButton(){
		lastMenuID = currentMenuID;
		ChangeTo (menus[5]);
		currentMenuID = 5;
	}

	public void BackButton(){
		ChangeTo (menus[lastMenuID]);
		currentMenuID = lastMenuID;
		if (currentMenuID == 3 || currentMenuID == 4 || currentMenuID == 5)
			lastMenuID = 2;
		else
			lastMenuID = 0;
	}

	void ChangeTo(GameObject menu){
		for (int i = 0; i<menus.Length; i++) {
			menus[i].SetActive(false);
		}
		menu.SetActive (true);
	}
}
