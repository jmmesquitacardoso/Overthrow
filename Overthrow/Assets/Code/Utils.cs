using UnityEngine;
using System.Collections;

public class Utils
{
	private static Utils instance;

	public Utils() {
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
	public int calculateDamage (float critChance, float criticalHitDamage, int damage)
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
}
