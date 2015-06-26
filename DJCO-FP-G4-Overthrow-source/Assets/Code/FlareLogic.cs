using UnityEngine;
using System.Collections;

public class FlareLogic : MonoBehaviour
{

	private Bezier shotPath;
	private float t = 0;
	public Vector3 targetPosition;
	public Vector3 rotation;
	public int flareDuration = 10;
	private bool firstTime = true;

	// Use this for initialization
	void Start ()
	{
		InvokeRepeating ("FlareDuration", 0, flareDuration);
		shotPath = new Bezier (transform.position, new Vector3 (transform.position.x + (Mathf.Cos (this.rotation.x) * 3f), transform.position.y + 20f, transform.position.z + (Mathf.Cos (this.rotation.z) * 3f)), new Vector3 (targetPosition.x + (Mathf.Cos (this.rotation.x) * 3f), targetPosition.y + 20f, targetPosition.z + (Mathf.Cos (this.rotation.x) * 3f)), new Vector3 (targetPosition.x, 0, targetPosition.z));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (t <= 1f) {
			Vector3 vec = shotPath.GetPointAtTime (t);
			transform.position = vec;
		}

		t += 0.02f;
	}

	void FlareDuration ()
	{
		if (!firstTime) {
			Destroy (gameObject);
		}
		firstTime = false;
	}
}
