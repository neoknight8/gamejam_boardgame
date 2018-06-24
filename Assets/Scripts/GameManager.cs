using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerChangeHandler();
public delegate void LoseHandler();

public class GameManager : SingletonMonoBehaviour<GameManager> {
	bool[,] panels = new bool[5,5];
	int currentTurn = 1;

	[SerializeField]
	GameObject sheepPrehab;

	PlayerChangeHandler[] onPlayerChangeListener = new PlayerChangeHandler[2];
	LoseHandler[] onPlayerLoseListener = new LoseHandler[2];

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 5; j++) {
				panels [i, j] = false;
			}
		}
//		panels [0, 0] = true;
//		panels [4, 4] = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void generateSheep(int x, int y){
		panels [x, y] = true;
		Instantiate (sheepPrehab, new Vector3 (x * 2, 1, y * 2), Quaternion.Euler (0, 0, 0));
	}

	public void setOnPlayerChangeListener(int id, PlayerChangeHandler handler){
		onPlayerChangeListener [id - 1] = handler;
	}

	public void setOnLoseListener(int id, LoseHandler handler){
		onPlayerLoseListener [id - 1] = handler;
	}

	public void Clear(){
		onPlayerLoseListener [currentTurn - 1] ();
		Debug.Log ("クリア！");
	}

	public void checkStatus(){
		bool is_true;
		// 縦を調べる
		for (int i = 0; i < 5; i++) {
			is_true = true;
			for (int j = 0; j < 5; j++) {
				if (!panels [i, j]) {
					is_true = false;
				}
			}
			if (is_true) {
				Clear();
			}
		}
		// 横を調べる
		for (int i = 0; i < 5; i++) {
			is_true = true;
			for (int j = 0; j < 5; j++) {
				if (!panels [j, i]) {
					is_true = false;
				}
			}
			if (is_true) {
				Clear();
			}
		}

		// 斜めを調べる
		is_true = true;
		for (int i = 0; i < 5; i++) {
			if (!panels [i, i]) {
				is_true = false;
			}
		}
		if (is_true) {
			Clear();
		}
		is_true = true;
		for (int i = 0; i < 5; i++) {
			if (!panels [i, 4-i]) {
				is_true = false;
			}
		}
		if (is_true) {
			Clear();
		}
	}

	public bool isPlayersTurn(int id){
		return currentTurn == id;
	}

	public void changeTurn(){
		currentTurn = 3 - currentTurn;
		checkStatus ();
		TileManager.Instance.initializeMaterials ();
		if (onPlayerChangeListener [currentTurn-1] != null) {
			onPlayerChangeListener [currentTurn-1] ();
		}
	}
}


