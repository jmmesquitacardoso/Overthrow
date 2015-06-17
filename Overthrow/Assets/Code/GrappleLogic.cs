using UnityEngine;
using System.Collections;

public class GrappleLogic : MonoBehaviour {

	public int duration = 6;
	public int pullRange = 40;

	// Use this for initialization
	void Start () {
		StartCoroutine (Duration ());
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Transform golem in GameObject.Find("Golems").transform) {
			if (Vector3.Distance (golem.position, transform.position) <= pullRange) {
				golem.GetComponent<EnemyScript>().pulledToPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
				golem.GetComponent<EnemyScript>().state = EnemyState.PULLED;
			}
		}
	}

	IEnumerator Duration() {
		yield return new WaitForSeconds (duration);
		Destroy (gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Enemy") {
		}
	}
}