using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GM : MonoBehaviour {

	public static GM instance = null;

	public int lives = 3;

	public float resetDelay = 1f;

	public Text livesText;
	public GameObject gameOver;
	public GameObject youWon;
	public NavigationController gameEnded;

	public GameObject paddle;

	private GameObject clonePaddle;
	public GameObject deathExplosion;


	public int bricksCount = 20;
	public GameObject[] bricks;

	
	// Use this for initialization
	void Awake () 
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		Setup();
		
	}
	
	public void Setup()
	{
		UpdatePrefs ();

		clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;

		bricks = new GameObject[gameObject.transform.childCount];
		bricksCount = bricks.Length;
		for (int i = 0; i < bricks.Length; i++) {
			GameObject brick = transform.GetChild (i).gameObject;
			bricks [i] = brick;
			if (brick.GetComponent<Bricks> ().unbreakeable) {
				bricksCount--;	
			}
		}
	}
	
	bool CheckGameOver()
	{
		if (bricksCount < 1)
		{
			// Won
			youWon.SetActive(true);
			Time.timeScale = .25f;
			Invoke ("GameEnded", resetDelay);
			gameEnded.NextLevelAllowed = true;
			return true;
		}
		
		else if (lives < 1)
		{
			// Game Over
			gameOver.SetActive(true);
			Time.timeScale = .25f;
			Invoke ("GameEnded", resetDelay);
			gameEnded.NextLevelAllowed =false;
			return true;
		}
		else return false;
		
	}

	void GameEnded(){
		gameEnded.active = true;
	}

	public void LoseLife()
	{
		lives--;
		livesText.text = "Lives: " + lives;
		Instantiate(deathExplosion, clonePaddle.transform.position, Quaternion.identity);
		Destroy(clonePaddle);
		if (!CheckGameOver()) Invoke ("SetupPaddle", resetDelay);
	}
	
	void SetupPaddle()
	{
		clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
	}
	
	public void DestroyBrick()
	{
		bricksCount--;
		CheckGameOver();
	}

	void UpdatePrefs(){
		int actualLevel = Application.loadedLevel;
		int highestLevel = PlayerPrefs.GetInt ("HighestLevel");
		if (actualLevel > highestLevel) {
			PlayerPrefs.SetInt("HighestLevel",actualLevel);
		}
		//Debug.Log ("Actual: "+actualLevel+", Highest: "+highestLevel);
	}
}
