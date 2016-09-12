using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NavigationController : MonoBehaviour {

	public Button nextLevelButton;
	
	public bool NextLevelAllowed{set{nextLevelButton.interactable = value;}}

	public bool active {set{this.gameObject.SetActive(value);}}

	public void NextLevel(){
		Time.timeScale = 1f;
		Application.LoadLevel(Application.loadedLevel + 1);
	}
	
	public void Reset()
	{
		Time.timeScale = 1f;
		Application.LoadLevel(Application.loadedLevel);
	}
	
	public void MainMenu(){
		Time.timeScale = 1f;
		Application.LoadLevel (0);
	}
}
