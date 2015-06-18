using UnityEngine;
using System.Collections;

public class AutoDestroyParticleSystem : MonoBehaviour {

	public void Start(){
	}
	
	public void Update(){
		if (Time.deltaTime > 4)
			Destroy (gameObject);
	}
}
