using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	public int MaxHelath=2000;
	public int Health{ get; private set; }
	// Use this for initialization
	public void Awake () {
		Health = MaxHelath;
	}
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
}
