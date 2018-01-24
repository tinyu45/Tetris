﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtroller : MonoBehaviour {

	//游戏状态枚举
	public enum GameStatu{
		PLAYING,
		OVER
	}
	public static GameStatu staus;



	//定义委托
	public delegate void Fall();
	public static Fall FallClay;


	//计时器
	public static float timer;


	/**
	 *方块类型 
	 * */
	public GameObject[] Clays;



	/**
	 * 网格区域 坐标
	 * */
	public static int width=10;
	public static int height=20;
	public static Transform[,] Grids;

	//满行号
	public static Stack<int> lines = new Stack<int> ();




	// Use this for initialization
	void Start () {
		staus = GameStatu.PLAYING;
		timer = Time.time;
		Grids = new Transform[width, height]; //初始化格子
		FallClay+=GenerateClay;
		FallClay ();
		//FallClay+= ClearFullLines;
	}



	// Update is called once per frame
	void Update () {
		
	}


	void GenerateClay(){
		int index = Random.Range (0, Clays.Length);
		//要生成的对象 、位置、初始旋转 
		Instantiate (Clays[index], transform.position,Quaternion.identity);
	}



	void ClearFullLines(){
		for (int y = 0; y < GameCtroller.height; y++) {
			lines.Push (y);
			for (int x = 0; x < GameCtroller.width; x++) {
				if (GameCtroller.Grids [x, y] == null && lines.Contains(y)) {
					lines.Pop ();
				}
			}
		}


		if (lines.Count != 0) {
			print ("栈 元素数："+lines.Count+"栈顶:"+lines.Peek());
		}


		if(lines.Count>0) {
			for (int h = lines.Peek()+1; h < GameCtroller.height; h++) {
				for (int l = 0; l < GameCtroller.width; l++) {
					Transform tran= GameCtroller.Grids [l, h];
					if (tran != null) {
						GameCtroller.Grids [l, h - lines.Count] = tran;
						tran=null;
						//tran.position=new Vector3(tran.position.x,tran.position.y-lines.Count,0);
					}
				}	
			}
			lines.Clear ();
		}
	}

}
