using UnityEngine;
using System.Collections;

public class WieldBow : MonoBehaviour
{

	public GameObject player;
	float distance = 10.0f;
	GameObject hand;
	public bool display = false;

	// Use this for initialization
	void Start ()
	{
		hand = GameObject.Find ("Player/personagem_v6_/Aarmature-human/Espinha_Inferior/espinha_centro/espinha_sup/Shoulder.R/Braço_R/Ante_Braço_R/Mao_R");
		// Find the bone in the Player object where we want to attach the object	
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.parent = hand.transform; //Parenting this item to the hand bone position
		//this.transform.localPosition = new Vector3 (0.18f, 0.48f, -0.05f); // centering the sword handle
		this.transform.localRotation = Quaternion.identity; //must point y local, so reset rotation
		this.transform.localRotation = Quaternion.Euler (0, 0, -120); //and rotate the sword accordingly
	}
}
