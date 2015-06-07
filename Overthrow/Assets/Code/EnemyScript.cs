using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour
{

	public Texture2D mouseTexture;
	private Renderer rend;
	private Color originalColor;
	public Vector3 pulledToPosition;
	private Vector3 knockUpPosition;
	private Vector3 knockUpDownPosition;
	private Vector3 transformPosition2D;
	public EnemyState state = EnemyState.IDLE;
	public float pullSpeed = 20f;
	public float knockUpSpeed = 30f;
	public Text currentEnemyText;
	public Image currentEnemyHealthBar;
	public Image currentEnemyOuterHealthBar;
	public int maxHealth = 400;
	public int currentHealth = 250;
	private bool inBlizzard = false;
	private float blizzardCritChance;
	private float blizzardCriticialHitDamage;
	private int blizzardDamage;

	// Use this for initialization
	void Start ()
	{
		rend = GetComponent<Renderer> ();
		InvokeRepeating ("TakeBlizzardDamage", 0, 1f);
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void FixedUpdate ()
	{

		switch (state) {
		case EnemyState.IDLE:
			break;
		case EnemyState.PULLED:
			transform.position = Vector3.MoveTowards (transform.position, pulledToPosition, Time.deltaTime * pullSpeed);
			if ((pulledToPosition - transform.position).magnitude < 0.1) {
				state = EnemyState.IDLE;
			}
			break;
		case EnemyState.KNOCKUP:
			transform.position = Vector3.MoveTowards (transform.position, knockUpPosition, Time.deltaTime * knockUpSpeed);
			if ((knockUpPosition - transform.position).magnitude < 0.1) {
				state = EnemyState.KNOCKUPDOWN;
				knockUpDownPosition = new Vector3 (transform.position.x, transform.position.y - 20, transform.position.z);
			}
			break;
		case EnemyState.KNOCKUPDOWN:
			transform.position = Vector3.MoveTowards (transform.position, knockUpDownPosition, Time.deltaTime * knockUpSpeed);
			if ((knockUpDownPosition - transform.position).magnitude < 0.1) {
				state = EnemyState.IDLE;
			}
			break;
		default:
			break;
		}
	}

	//When the mouse hovers on the enemy, the enemy is highlighted and the cursor changes
	void OnMouseEnter ()
	{
		Cursor.SetCursor (mouseTexture, new Vector2 (0, 0), CursorMode.Auto);
		originalColor = rend.material.color;
		rend.material.color = Color.yellow;
		currentEnemyText.text = gameObject.name;
		currentEnemyHealthBar.enabled = true;
		currentEnemyOuterHealthBar.enabled = true;
		currentEnemyHealthBar.fillAmount = ((float)currentHealth / (float)maxHealth);
	}
	
	void OnMouseExit ()
	{
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
		rend.material.color = originalColor;
		currentEnemyText.text = "";
		currentEnemyHealthBar.enabled = false;
		currentEnemyOuterHealthBar.enabled = false;
	}

	public void TakeDamage (int damage)
	{
		currentHealth -= damage;
		currentEnemyHealthBar.fillAmount = ((float)currentHealth / (float)maxHealth);
		if (currentHealth <= 0) {
			Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);
			currentEnemyText.text = "";
			currentEnemyOuterHealthBar.enabled = false;
			Destroy (gameObject);
		}
	}

	public void KnockUp ()
	{
		state = EnemyState.KNOCKUP;
		knockUpPosition = new Vector3 (transform.position.x, transform.position.y + 20, transform.position.z);
	}

	public void EnterBlizzard (float blizzardCritChance, float blizzardCriticialHitDamage, int blizzardDamage)
	{
		inBlizzard = true;
		this.blizzardCritChance = blizzardCritChance;
		this.blizzardCriticialHitDamage = blizzardCriticialHitDamage;
		this.blizzardDamage = blizzardDamage;
	}

	public void ExitBlizzard ()
	{
		inBlizzard = false;
	}

	void TakeBlizzardDamage ()
	{
		if (inBlizzard) {
			TakeDamage (Utils.Instance.CalculateDamage(blizzardCritChance,blizzardCriticialHitDamage,blizzardDamage));
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Trap") {
			Destroy (gameObject);
			Destroy (collision.gameObject);
		}
	}
}
