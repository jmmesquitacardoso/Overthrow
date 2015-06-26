using UnityEngine;
using System.Collections;

public class TelekinesisLogic : MonoBehaviour {

	private bool firstTime = true;
	private bool hovering = false;
	public Vector3 hoverUponPlayerPosition;
	public float moveToHoverSpeed = 20f;
	public Transform playerTransform;

	// Use this for initialization
	void Start () {
		//StartCoroutine (HoverUponPlayer ());
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate () {
		HoverUponPlayer ();

		if (hovering) {
			var temp = transform.position;
			temp.x = playerTransform.transform.position.x + 2;
			temp.y = playerTransform.transform.position.y + 12;
			temp.z = playerTransform.transform.position.z + 2;
			transform.position = temp;
		}
	}

	IEnumerator FallUpon () {
		if (transform.rotation.eulerAngles.x % 90 != 0 || firstTime) {
			transform.Rotate (10*100f*Time.deltaTime, 0, 0);
			firstTime = false;
			yield return new WaitForSeconds (0.5f);
			Debug.Log(transform.rotation.eulerAngles.x);
			StartCoroutine (FallUpon ());
		} else {
			Destroy(gameObject);
		}
	}

	void HoverUponPlayer () {
		transform.position = Vector3.MoveTowards(transform.position, hoverUponPlayerPosition, Time.deltaTime * moveToHoverSpeed);
		if ((hoverUponPlayerPosition - transform.position).magnitude < 0.01) {
			hovering = true;
		}
	}
}
