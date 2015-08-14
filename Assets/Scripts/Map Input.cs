using UnityEngine;
using System.Collections;

public class MapInput : MonoBehaviour {

	//Our array of map images
	public UnityEngine.UI.Image[] maps;

	//Array of maps names, should correspond with maps
	public string[] mapNames;

	//Our map name
	private UnityEngine.UI.Text mapText;

	// Use this for initialization
	void Start () {
		//Set our mapText
		mapText = GameObject.FindGameObjectWithTag ("PlayerHUD").GetComponent<UnityEngine.UI.Text> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
