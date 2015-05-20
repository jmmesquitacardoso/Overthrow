using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public Texture2D mouseTexture;

	private Renderer rend;

	private Color originalColor;

	public Vector3 pulledToPosition;

	public bool pull = false;

	public float pullSpeed = 20f;

	public Text currentEnemyText;

	public Image currentEnemyHealthBar;

	public int maxHealth = 400;
	public int currentHealth = 250;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		if (pull) {
			transform.position = Vector3.MoveTowards(transform.position, pulledToPosition, Time.deltaTime * pullSpeed);
		}
	}

	//When the mouse hovers on the enemy, the enemy is highlighted and the cursor changes
	void OnMouseEnter()
	{
		Cursor.SetCursor(mouseTexture, new Vector2(0,0), CursorMode.Auto);
		originalColor = rend.material.color;
		rend.material.color = Color.yellow;
		currentEnemyText.text = gameObject.name;
		currentEnemyHealthBar.enabled = true;
		currentEnemyHealthBar.fillAmount = ((float)currentHealth / (float)maxHealth);
	}
	
	void OnMouseExit()
	{
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		rend.material.color = originalColor;
		currentEnemyText.text = "";
		currentEnemyHealthBar.enabled = false;
	}

	public void TakeDamage(int damage) {
		currentHealth -= damage;
		currentEnemyHealthBar.fillAmount = ((float)currentHealth / (float)maxHealth);
		if (currentHealth <= 0) {
			currentEnemyText.text = "";
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Trap") {
			Destroy(gameObject);
			Destroy(collision.gameObject);
		}
	}
}
