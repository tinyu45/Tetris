using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

class Cat
{}

interface IPet
{
	void MaiMeng ();
}

class BOSIMAO:Cat ,IPet
{
	public void MaiMeng()
	{
		Debug.Log ("yaoweiba");
	}

}
class MeiDuan :Cat ,IPet
{
	public void MaiMeng()
	{
		Debug.Log ("miaomiaomiao~");
	}
}

class Dog
{}

class ERHa:Dog,IPet
{
	public void MaiMeng()
	{
		Debug.Log ("........");
	}
}