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
		StartCoroutine (BlizzardDuration());
		collidedEnemies = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Enemy") {
			collision.gameObject.GetComponent<EnemyScript> ().EnterBlizzard (critChance, criticalHitDamage, damage);
			collidedEnemies.Add (collision.gameObject);
		} else if (collision.gameObject.tag == "Boss") {
			collision.gameObject.GetComponent<Boss> ().EnterBlizzard (critChance, criticalHitDamage, damage);
			collidedEnemies.Add (collision.gameObject);
		}
	}

	void OnCollisionExit (Collision collision)
	{
		if (collision.gameObject.tag == "Enemy") {
			collision.gameObject.GetComponent<EnemyScript> ().ExitBlizzard ();
		} else if (collision.gameObject.tag == "Enemy") {
			collision.gameObject.GetComponent<Boss> ().ExitBlizzard ();
		}
	}

	IEnumerator BlizzardDuration ()
	{
		yield return new WaitForSeconds (blizzardDuration);
		Destroy (gameObject);
	}
}
