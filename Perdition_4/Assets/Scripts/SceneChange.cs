using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class SceneChange : MonoBehaviour {

	public int sceneToLoad; 

	void OnGUI(){
	
		GUI.Label (new Rect (Screen.width / 2 - 50, Screen.height - 80, 100, 30), "Current Scene: " + (SceneManager.GetActiveScene().buildIndex + 1)); 
		if (GUI.Button (new Rect (Screen.width / 2 - 50, Screen.height - 50, 100, 40), "Load Scene " )) {

			if (SceneManager.GetActiveScene().buildIndex > 0){
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1, LoadSceneMode.Single );
			}
			if (SceneManager.GetActiveScene().buildIndex < 1){
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1, LoadSceneMode.Single );
			}


		}
	}
}
