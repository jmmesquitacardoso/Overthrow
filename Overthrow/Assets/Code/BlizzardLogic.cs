using UnityEngine;
using System.Collections;

public class BlizzardLogic : MonoBehaviour
{

	public float critChance;
	public float criticalHitDamage;
	public int blizzardDuration = 6;
	private bool firstTime = true;
	private ArrayList collidedEnemies;
	public int damage;

	// Use this for initialization
	void Start ()
	{
		InvokeRepeating ("BlizzardDuration", 0, blizzardDuration);
		collidedEnemies = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Enemy") {
			Debug.Log ("Collided with " + collision.gameObject.name);
			collision.gameObject.GetComponent<EnemyScript> ().EnterBlizzard ();
			collidedEnemies.Add (collision.gameObject);
		}
	}

	void OnCollisionExit (Collision collision)
	{
		if (collision.gameObject.tag == "Enemy") {
			Debug.Log ("Collided with " + collision.gameObject.name);
			collision.gameObject.GetComponent<EnemyScript> ().ExitBlizzard ();
		}
	}

	void BlizzardDuration ()
	{
		if (!firstTime) {
			foreach (GameObject enemy in collidedEnemies) {
				enemy.GetComponent<EnemyScript> ().ExitBlizzard ();
			}
			Destroy (gameObject);
		}
		firstTime = false;
	}
}
