using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {

	private static float BOSS_SONG_DISTANCE = 60f;
	public Transform player;
	public Transform boss;
	public AudioClip bossMusic;
	public AudioClip gameMusic;

	private AudioSource bgMusic;
	// Use this for initialization
	void Start () {
		bgMusic = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		var temp = transform.position;
		temp.x = player.transform.position.x-5;
		temp.z = player.transform.position.z-5;
		transform.position = temp;

		handleMusic ();
	}

	void handleMusic () {
		if (Vector3.Distance (player.position, boss.position) < BOSS_SONG_DISTANCE && bgMusic.clip != bossMusic) {
			bgMusic.clip = bossMusic;
			bgMusic.Play();
		} else if (Vector3.Distance (player.position, boss.position) >= BOSS_SONG_DISTANCE && bgMusic.clip != gameMusic) {
			bgMusic.clip = gameMusic;
			bgMusic.Play();
		}

	}
}
