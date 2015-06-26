using UnityEngine;
using System.Collections;

public class AutoDestroyParticleSystem : MonoBehaviour {

	public static float BurningDuration = 0.5f;
	public GameObject player;

	public void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	public void Update(){
		if (Time.deltaTime > 4)
			Destroy (gameObject);
	}

	void OnTriggerEnter (Collider collider)
	{
		if (collider.gameObject == player) {
			player.GetComponent<PlayerControl> ().StartBurning(BurningDuration);
		}
		//player.GetComponent<PlayerControl> ().TakeDamage (enemyDamage);

	}
}
