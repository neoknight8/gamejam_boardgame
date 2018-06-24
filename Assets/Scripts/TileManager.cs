using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : SingletonMonoBehaviour<TileManager> {
	[SerializeField]
	Material normalMaterial;
	[SerializeField]
	Material chosenMaterial;
	[SerializeField]
	Tile[] tiles;

	private int selectedBlock = 0;
	// Use this for initialization
	void Start () {
		selectBlock (4,3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void disappear (int x, int y){
		tiles [x * 5 + y].gameObject.SetActive (false);
	}

	private void changeMaterial(int x, int y, string tag){
		if (tag == "normal") {
			tiles [x * 5 + y].GetComponent<MeshRenderer> ().material = normalMaterial;
		} else if(tag == "chosen"){
			tiles [x * 5 + y].GetComponent<MeshRenderer> ().material = chosenMaterial;
		}
	}

	public void selectBlock(int x, int y){
		changeMaterial (selectedBlock / 5, selectedBlock % 5, "normal");
		selectedBlock = x * 5 + y;
		changeMaterial (x, y, "chosen");
	}

	public void initializeMaterials(){
		for (int i = 0; i < 25; i++) {
			tiles[i].GetComponent<MeshRenderer> ().material = normalMaterial;
		}
	}
}
