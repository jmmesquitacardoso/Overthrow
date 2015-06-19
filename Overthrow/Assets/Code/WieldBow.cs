using UnityEngine;
using System.Collections;

public class WieldBow : MonoBehaviour
{

	public GameObject player;
	GameObject hand;

	// Use this for initialization
	void Start ()
	{
		hand = GameObject.Find ("Player/Human/Aarmature-human/Espinha_Inferior/espinha_centro/espinha_sup/Shoulder_L/Bra_o_L/Ante_Bra_o_L/M_o_L");
		// Find the bone in the Player object where we want to attach the object	
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.parent = hand.transform; //Parenting this item to the hand bone position
		this.transform.localPosition = new Vector3 (0.15f, 0.4f, 0f); // centering the sword handle
		this.transform.localRotation = Quaternion.identity; //must point y local, so reset rotation
		this.transform.localRotation = Quaternion.Euler (180, 0, -120); //and rotate the sword accordingly
	}
}
