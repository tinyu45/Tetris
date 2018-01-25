using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCtroller : MonoBehaviour {

	//UI显示
	public Text time;
	public Text Score;
	public GameObject GameOver;

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
			GameOver.SetActive (true);
		}

	}


	void GenerateClay(){
		int index = Random.Range (0, Clays.Length);
		//要生成的对象 、位置、初始旋转 
		Instantiate (Clays[index], transform.position,Quaternion.identity);
	}




	void clearFullLine(){
		for (int y = 0; y < GameCtroller.height; y++) {
			if (isFullLine (y)) {
				for (int x = 0; x < GameCtroller.width; x++) {
					Destroy (GameCtroller.Grids[x,y].gameObject); //清空 图像
					GameCtroller.Grids [x, y] = null;   //对应格子置空
				}
				MoveaboveLines (y+1);
				UpdateGrade ();  //更新分数
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


	//更新分数
	void UpdateGrade(){
		grade += 1;
		Score.text = "Score：" + grade;
	}




//	void ClearFullLines(){
//		for (int y = 0; y < GameCtroller.height; y++) {
//			lines.Push (y);
//			for (int x = 0; x < GameCtroller.width; x++) {
//				if (GameCtroller.Grids [x, y] == null && lines.Contains(y)) {
//					lines.Pop ();
//				}
//			}
//		}
//
//
//		if (lines.Count != 0) {
//			print ("栈 元素数："+lines.Count+"栈顶:"+lines.Peek());
//		}
//
//
//		if(lines.Count>0) {
//			for (int h = lines.Peek()+1; h < GameCtroller.height; h++) {
//				for (int l = 0; l < GameCtroller.width; l++) {
//					Transform tran= GameCtroller.Grids [l, h];
//					if (tran != null) {
//						GameCtroller.Grids [l, h - lines.Count] = tran;
//						tran=null;
//						//tran.position=new Vector3(tran.position.x,tran.position.y-lines.Count,0);
//					}
//				}	
//			}
//			lines.Clear ();
//		}
//	}

}
