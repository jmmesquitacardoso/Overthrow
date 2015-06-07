using UnityEngine;
using System.Collections;

public class MissileLogic : MonoBehaviour {

	public Vector3 targetPosition;

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
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
		if ((targetPosition - transform.position).magnitude < 0.01) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Enemy") {
			collision.gameObject.GetComponent<EnemyScript>().TakeDamage(Utils.Instance.CalculateDamage(critChance,criticalHitDamage,damage));
			Destroy(gameObject);
		}
	}
}
