using UnityEngine;
using System.Collections;

public class MissileLogic : MonoBehaviour {

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
				collision.gameObject.GetComponent<EnemyScript>().TakeDamage(Utils.Instance.calculateDamage(critChance,criticalHitDamage,damage));
			Destroy(gameObject);
		}
	}
}
