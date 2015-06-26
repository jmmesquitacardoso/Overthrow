using UnityEngine;
using System.Collections;

public class EnemyMissileLogic : MonoBehaviour {

	public GameObject player;
	public Vector3 targetPosition;
	
	public float speed = 3.0f;
	
	public int damage = 30;
	
	public float critChance;
	public float criticalHitDamage;
	
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		targetPosition = player.transform.position;
	}

	void FixedUpdate() {
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
		if ((targetPosition - transform.position).magnitude < 0.01) {
			Destroy(gameObject);

			// if player is nearby initial position when missile was fire
			if (Vector3.Distance(targetPosition, player.transform.position) <= 2f)
				player.GetComponent<PlayerControl>().TakeDamage(Utils.Instance.CalculateDamage(critChance,criticalHitDamage,damage));
		}
	}
}
