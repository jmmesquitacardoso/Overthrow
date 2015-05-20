using UnityEngine;
using System.Collections;

public class MissileMovement : MonoBehaviour {

	public Vector3 targetPosition;

	private bool moving = false;

	public float speed = 3.0f;

	public int damage;

	public float critChance;
	public float criticalHitDamage;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		moving = true;
		
		if (moving) {
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Enemy") {
			var random = Random.Range (1,100);
			// If it's an integer crit chance, i.e 15%
			if (Mathf.Floor(critChance) == critChance) {
				if (random <= critChance) {
					collision.gameObject.GetComponent<EnemyScript>().TakeDamage((int)(damage*criticalHitDamage));
				} else {
					collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
				}
			} else { // If it's a float crit chance, i.e 15.7%
				int percentileCritChance = (int)Mathf.Ceil (((critChance - Mathf.Floor(critChance))) * 100);
				var percentileRandom = Random.Range(1,100);
				if (percentileRandom <= percentileCritChance) {
					random += 1;
					if (random <= critChance) {
						collision.gameObject.GetComponent<EnemyScript>().TakeDamage((int)(damage*criticalHitDamage));
					} else {
						collision.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
					}
				}
			}
			Destroy(gameObject);
		}
	}
}
