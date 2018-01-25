using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCtroller : MonoBehaviour {

	//UI显示
	public Text time;
	public Text Score;

	//计时器
	public static float timer;
	//分数
	private int grade;

	//游戏状态枚举
	public enum GameStatu{
		PLAYING,
		OVER
	}
	public static GameStatu staus;



	//定义委托
	public delegate void Fall();
	public static Fall FallClay;


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
		Screen.SetResolution (1000,800,false);
		staus = GameStatu.PLAYING;
		grade = 0;
		timer = Time.time;
		Grids = new Transform[width, height]; //初始化格子
		FallClay+=GenerateClay;
		FallClay ();
		FallClay+= clearFullLine;

		Score.text = "Score：" + grade; //初始分数
	}



	// Update is called once per frame
	void Update () {
		int min = (int)(Time.time - timer)/60;
		int sec = (int)(Time.time - timer) % 60;
		//string str = string.Format ("{0:00}:{1:00}",min,sec);
		time.text= string.Format ("Time：{0:00}:{1:00}",min,sec);
		if (staus == GameStatu.OVER) {
			FallClay -= GenerateClay;
			FallClay -= clearFullLine;
			PlayerPrefs.SetInt ("score", grade);  //保存数据
			SceneManager.LoadScene ("scenes/Over");
		}

	}


	void GenerateClay(){
		int index = Random.Range (0, Clays.Length);
		//要生成的对象 、位置、初始旋转 
		Instantiate (Clays[index], transform.position,Quaternion.identity);
	}



	/****
	 * 
	 *    清除行数算法1； 循环 

	void clearFullLine(){
		for (int y = 0; y < GameCtroller.height; y++) {
			if (isFullLine (y)) {
				for (int x = 0; x < GameCtroller.width; x++) {
					Destroy (GameCtroller.Grids[x,y].gameObject); //清空 图像
					GameCtroller.Grids [x, y] = null;   //对应格子置空
				}
				MoveaboveLines (y+1);
				UpdateGrade (1);  //更新分数
				y--;
			}
		}
	}


	//下移 移动消除后上航
	void MoveaboveLines(int line){
		for (int y = line; y < GameCtroller.height; y++) {
			for (int x = 0; x < GameCtroller.width; x++) {
				if (GameCtroller.Grids [x, y] != null) {
					GameCtroller.Grids [x, y - 1] = GameCtroller.Grids [x, y];
					GameCtroller.Grids [x, y] = null;
					GameCtroller.Grids [x, y - 1].position = new Vector3 (GameCtroller.Grids [x, y - 1].position.x, GameCtroller.Grids [x, y - 1].position.y-1,0);
				}
			}
		}
	}


	//获取全满行
	bool isFullLine(int y){
		for (int x = 0; x < GameCtroller.width; x++) {
			if (GameCtroller.Grids [x, y] == null)
				return false;
		}
		return true;
	}

	*****
	*/







	/**** 
	*       方法2 ： 借用栈实现 
	**/


	//更新分数
	void UpdateGrade(int values){
		grade += values;
		Score.text = "Score：" + grade;
	}


	//获取满行 并存入栈
	void GetFullLines(){
		for (int y = 0; y < GameCtroller.height; y++) {
			lines.Push (y);
			for (int x = 0; x < GameCtroller.width; x++) {
				if (GameCtroller.Grids [x, y] == null && lines.Contains(y)) {
					lines.Pop ();
				}
			}
		}
	}

	//清空满行
	void RemoveBlocks(ref int hfl){
		while (lines.Count != 0) {
			if (hfl == -1) {
				hfl = lines.Peek();
			}
			int y=lines.Pop();
			for (int x = 0; x < GameCtroller.width; x++) {
				Destroy(GameCtroller.Grids [x, y].gameObject); //清空满行 并销毁游戏对象
				GameCtroller.Grids [x, y] = null;
			}
		}
	}

	//下移上行
	void FallabovelineBlocks(int line, int count){
		for (int y = line; y < GameCtroller.height; y++) {
			for (int x = 0; x < GameCtroller.width; x++) {
				if (GameCtroller.Grids[x,y] != null) {
					GameCtroller.Grids [x, y - count] = GameCtroller.Grids[x,y];  //移动数据
					GameCtroller.Grids [x, y]=null;                               //清空当前位置数据
					GameCtroller.Grids [x, y - count].position=new Vector3(GameCtroller.Grids [x, y - count].position.x, GameCtroller.Grids [x, y - count].position.y-count, 0);  //移动方块
				}
			}	
		}
	}

	void clearFullLine(){
		int hfl=-1;    //hfl: high full line
		GetFullLines (); 
		int fulls = lines.Count;
		RemoveBlocks (ref hfl);
		if (fulls > 0) {  
			FallabovelineBlocks(hfl+1, fulls);  //下移上行
		}
		UpdateGrade (fulls);  //更新分数
	}



}
