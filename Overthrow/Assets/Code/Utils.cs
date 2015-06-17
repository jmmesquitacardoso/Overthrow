using UnityEngine;
using System.Collections;

public enum EnemyState
{
	IDLE,
	PULLED,
	KNOCKUP,
	KNOCKUPDOWN,
	MINDCONTROLLED}
;

enum Mode
{
	ARPG,
	Stealth}
;

enum PlayerState
{
	IDLE,
	MOVING,
	BLINK}
;

public enum PlayerBuffs {
	DESTRUCTION,
	CRITICAL,
	SWIFT
};

public class Utils
{
	private static Utils instance;

	public Utils ()
	{
	}

	public static Utils Instance {
		get {
			if (instance == null) {
				instance = new Utils ();
			}
			return instance;
		}
	}

	// Calculates the attack damage based on the user's critical chance and critical hit damage
	public int CalculateDamage (float critChance, float criticalHitDamage, int damage)
	{
		var random = Random.Range (1, 100);
		// If it's an integer crit chance, i.e 15%
		if (Mathf.Floor (critChance) == critChance) {
			if (random <= critChance) {
				return ((int)(damage * criticalHitDamage));
			} else {
				return damage;
			}
		} else { // If it's a float crit chance, i.e 15.7%
			int percentileCritChance = (int)Mathf.Ceil (((critChance - Mathf.Floor (critChance))) * 100);
			var percentileRandom = Random.Range (1, 100);
			if (percentileRandom <= percentileCritChance) {
				random += 1;
				if (random <= critChance) {
					return ((int)(damage * criticalHitDamage));
				} else {
					return damage;
				}
			} else {
				if (random <= critChance) {
					return ((int)(damage * criticalHitDamage));
				} else {
					return damage;
				}
			}
		}
	}

	public bool Dodge (float dodgeChance)
	{
		var random = Random.Range (1, 100);
		// If it's an integer dodge chance, i.e 15%
		if (Mathf.Floor (dodgeChance) == dodgeChance) {
			if (random <= dodgeChance) {
				return true;
			} else {
				return false;
			}
		} else { // If it's a float dodge chance, i.e 15.7%
			int percentileCritChance = (int)Mathf.Ceil (((dodgeChance - Mathf.Floor (dodgeChance))) * 100);
			var percentileRandom = Random.Range (1, 100);
			if (percentileRandom <= percentileCritChance) {
				random += 1;
				if (random <= dodgeChance) {
					return true;
				} else {
					return false;
				}
			} else {
				if (random <= dodgeChance) {
					return true;
				} else {
					return false;
				}
			}
		}
	}

	public T GetRandomEnum<T>()
	{
		System.Array A = System.Enum.GetValues(typeof(T));
		T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
		return V;
	}
}
