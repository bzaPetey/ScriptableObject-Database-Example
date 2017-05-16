using UnityEngine;

[System.Serializable]
public class Weapon {
	[SerializeField] public string weaponName;
	[SerializeField] public int damage;
	
	public Weapon( string name, int dmg ) {
		weaponName = name;
		damage = dmg;
	}
}