using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverCtroller : MonoBehaviour {
	// Use this for initialization
	public Text score;

	private string str;
	private float timer;
	private int index;

	void Start () {
		index = 0;
		timer = 0;
		score.text = "";
		str= "游戏结束，您消除"+PlayerPrefs.GetInt("score")+"行方块；请选择继续或退出";
	}
	
	// Update is called once per frame
	void Update () {
		if (index < str.Length) {
			if (timer < 0.25f && index < str.Length) {
				timer += Time.deltaTime;
			} else {
				timer = 0;
				index++;
				score.text = str.Substring (0, index);
			}
		}
	}

	public void OnContinueClick(){
		SceneManager.LoadScene ("scenes/Tetris");
	}

	public void OnExitClick(){
		Application.Quit ();
	}
}
