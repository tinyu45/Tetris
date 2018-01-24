using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clay : MonoBehaviour {

	float time;

	// Use this for initialization
	void Start () {
		time = 0;
		if (!isValidMove ()) {
			print ("GEME OVER");
			GameCtroller.staus = GameCtroller.GameStatu.OVER;
		}
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			transform.Rotate (0, 0, 90);
			if (isValidMove ()) {
				UpdateGrids ();
			} else {
				transform.Rotate (0, 0, -90);  //还原
			}

		}



		if (Input.GetKeyDown (KeyCode.DownArrow) || time>=1) {
			transform.position += new Vector3 (0,-1,0);
			if (isValidMove ()) {
				UpdateGrids ();
			} else {
				transform.position += new Vector3 (0,1,0);  //还原
				if(GameCtroller.staus==GameCtroller.GameStatu.PLAYING){
					GameCtroller.FallClay();	
				}
				enabled=false; //去除当前物体脚本
			}
			time = 0;
		}


		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			transform.position += new Vector3 (-1,0,0);
			if (isValidMove()) {
				UpdateGrids ();
			} else {
				transform.position += new Vector3 (1,0,0);  //还原
			}
		}



		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			transform.position += new Vector3 (1,0,0);
			if (isValidMove ()) {
				UpdateGrids ();
			} else {
				transform.position += new Vector3 (-1,0,0);  //还原
			}
		}


	}




	/**
	 * 
	 * 越界判断
	 * **/
	bool isOverRange(Vector3 v){
		Vector3 temp = new Vector3 (Mathf.RoundToInt(v.x),Mathf.RoundToInt(v.y),0);
		return temp.x >= 0 &&
			temp.x < GameCtroller.width &&
		temp.y >= 0;
	}



	/**
	 * 移动合法
	 * 
	 * */
	bool isValidMove(){
		foreach (Transform t in transform) {
			if (!isOverRange (t.position))  //越界判断
				return false;
			//判断 目标位置是否已有元素
			if (t.position.y < GameCtroller.height) {
				Transform temp = GameCtroller.Grids [(int)t.position.x, (int)t.position.y];
				if(temp!=null && temp.parent!=transform){
					return false;
				}
			}
		}
		return true;
	}




	/***
	 * 
	 * 更新格子信息
	 * */
	void UpdateGrids(){
		//遍历判断之前是否有元素  清空之前位置信息
		for (int x = 0; x < GameCtroller.width; x++) {
			for (int y = 0; y < GameCtroller.height; y++) {
				if (GameCtroller.Grids [x, y]!=null && GameCtroller.Grids [x, y].parent == transform) {
					GameCtroller.Grids [x, y] = null;
				}
			}
		}
		foreach (Transform t in transform) {
			if (t.position.y < GameCtroller.height) {
				GameCtroller.Grids [(int)t.position.x, (int)t.position.y] = t;
			}
		}
	}



}
