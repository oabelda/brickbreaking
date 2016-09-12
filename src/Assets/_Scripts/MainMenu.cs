using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject MainSet;
	public GameObject ChooseLevelSet;
	public GameObject CleanDataSet;

	private Text level;
	private int highestLevel;
	private int actualLevel;

	private int ActualLevel {
		get{
			return actualLevel;
		}
		set{
			if (value > highestLevel){
				value = highestLevel;
			}
			if (value < 1){
				value = 1;
			}
			level.text = value.ToString ();
			actualLevel = value;
		}}

	public void Setup_Main(){
		CleanDataSet.SetActive (false);
		ChooseLevelSet.SetActive (false);
		MainSet.SetActive (true);
	}

	public void Setup_ChooseLevel(){
		CleanDataSet.SetActive (false);
		ChooseLevelSet.SetActive (true);
		MainSet.SetActive (false);

		if (level == null) level = ChooseLevelSet.transform.FindChild("Text").gameObject.GetComponent<Text>(); 

		actualLevel = int.Parse (level.text);
		highestLevel = PlayerPrefs.GetInt ("HighestLevel");

		highestLevel = (highestLevel < 1) ? 1 : highestLevel;

		//Debug.Log (highestLevel);

		if (actualLevel > highestLevel) ActualLevel = highestLevel;

	}

	public void Setup_CleanData(){
		CleanDataSet.SetActive (true);
		ChooseLevelSet.SetActive (false);
		MainSet.SetActive (false);
	}

	public void Play(){
		Setup_ChooseLevel ();
	}

	public void RemoveData(){
		PlayerPrefs.DeleteAll ();
		Setup_Main ();
	}

	public void levelUp(){
		ActualLevel = actualLevel+1;
	}

	public void levelDown(){
		ActualLevel = actualLevel-1;
	}

	public void Initialize(){
		Application.LoadLevel (actualLevel);
	}
}
