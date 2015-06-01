using UnityEngine;
using System.Collections;

public class Flare : MonoBehaviour {

	public Bezier shotPath;
	private float t = 0;
	public Vector3 targetPosition;

	// Use this for initialization
	void Start () {

		shotPath = new Bezier( transform.position, new Vector3(transform.position.x + 2f, transform.position.y + 3f, transform.position.z + 2f), new Vector3(targetPosition.x - 2f, targetPosition.y + 1f, targetPosition.z - 2f), targetPosition);
	}
	
	// Update is called once per frame
	void Update () {

		if (t <= 1f) {
			Vector3 vec = shotPath.GetPointAtTime( t );
			transform.position = vec;
		}

		t += 0.02f;
			
	}
}
