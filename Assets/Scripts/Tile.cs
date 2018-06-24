using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	private Animator animator;
	public int id;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			animator.SetBool ("Bounce", true);
			animator.SetBool ("BounceBack", false);
		}
	}

	void OnCollisionExit (Collision col)
	{
		if(col.gameObject.tag == "Player")
		{
			animator.SetBool ("BounceBack", true);
		}
	}

	public void OnBounceFinished(){
		animator.SetBool ("Bounce", false);
	}
}
