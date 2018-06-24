using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Player : MonoBehaviour {
	public int id;
	public int current_x;
	public int current_y;
	private int[] dir = new int[2];
	private ThirdPersonUserControl personControl;

	// Use this for initialization
	void Start () {
		if (id == 1) {
			dir [0] = -1;
			dir [1] = 0;
		} else {
			dir [0] = 1;
			dir [1] = 0;
		}
		personControl = GetComponent<ThirdPersonUserControl> ();
		personControl.setOnRunFinished (() => {
			ensureIsOnBoard();
			GameManager.Instance.changeTurn ();
		});
		GameManager.Instance.setOnPlayerChangeListener (id, () => {
			int[] dim = defaultBlock();
			TileManager.Instance.selectBlock(dim[0], dim[1]);
		});
		GameManager.Instance.setOnLoseListener (id, () => {
			TileManager.Instance.disappear(current_x, current_y);
		});
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.isPlayersTurn (id)) {
			if (Input.GetKeyDown (KeyCode.LeftArrow)) {
				Select (KeyCode.LeftArrow);
			}
			if (Input.GetKeyDown (KeyCode.UpArrow)) {
				Select (KeyCode.UpArrow);
			}
			if (Input.GetKeyDown (KeyCode.RightArrow)) {
				Select (KeyCode.RightArrow);
			}
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				Select (KeyCode.DownArrow);
			}
			if (Input.GetKeyDown (KeyCode.B)) {
				placeBlock ();
			}
			if (Input.GetKeyDown (KeyCode.M)) {
				Move ();
			}
		}
	}

	private void Select(KeyCode keyCode) {
		switch (keyCode) {
		case KeyCode.UpArrow:
			if (current_x > 0) {
				dir [0] = -1;
				dir [1] = 0;
				rotate ();
				TileManager.Instance.selectBlock (current_x - 1, current_y);
			}
			break;
		case KeyCode.DownArrow:
			if (current_x < 4) {
				dir [0] = 1;
				dir [1] = 0;
				rotate ();
				TileManager.Instance.selectBlock (current_x + 1, current_y);
			}
			break;
		case KeyCode.LeftArrow:
			if (current_y > 0) {
				dir [0] = 0;
				dir [1] = -1;
				rotate ();
				TileManager.Instance.selectBlock (current_x, current_y - 1);
			}
			break;
		case KeyCode.RightArrow:
			if (current_y < 4) {
				dir [0] = 0;
				dir [1] = 1;
				rotate ();
				TileManager.Instance.selectBlock (current_x, current_y + 1);
			}
			break;
		default:
			break;
		}
	}

	private int[] defaultBlock(){
		int[] dim = new int[2] { current_x, current_y - 1 };
		if (current_y == 0) {
			if (current_x == 0) {
				dim [1] = current_y + 1;
			} else {
				dim [0] = current_x - 1;
				dim [1] = current_y;
			}
		}

		return dim;
	}

	private void placeBlock(){
		StartCoroutine ("generateSheep");
	}

	private IEnumerator generateSheep(){
		GameManager.Instance.generateSheep (current_x + dir [0], current_y + dir [1]);
		yield return new WaitForSeconds (0.2f);
		GameManager.Instance.changeTurn ();
	}

	private void ensureIsOnBoard(){
		transform.position = new Vector3(current_x *2, 0, current_y*2);
	}
	private void Move(){
		current_x += dir [0];
		current_y += dir [1];
		personControl.Run ();
	}

	private void rotate(){
		if (dir [0] == -1 && dir [1] == 0) {
			transform.rotation = Quaternion.Euler (new Vector3(0,270,0));
		}
		if (dir [0] == 1 && dir [1] == 0) {
			transform.rotation = Quaternion.Euler (new Vector3(0,90,0));
		}
		if (dir [0] == 0 && dir [1] == 1) {
			transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
		}
		if (dir [0] == 0 && dir [1] == -1) {
			transform.rotation = Quaternion.Euler (new Vector3(0,180,0));
		}
	}
}
