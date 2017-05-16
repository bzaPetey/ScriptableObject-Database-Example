using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WeaponDatabase : ScriptableObject {
	[SerializeField]
	private List<Weapon> database;
	
	void OnEnable() {
		if( database == null )
			database = new List<Weapon>();
	}
	
	public void Add( Weapon weapon ) {
		database.Add( weapon );
	}
	
	public void Remove( Weapon weapon ) {
		database.Remove( weapon );
	}
	
	public void RemoveAt( int index ) {
		database.RemoveAt( index );
	}
	
	public int COUNT {
		get { return database.Count; }
	}
	
	//.ElementAt() requires the System.Linq
	public Weapon Weapon( int index ) {
		return database.ElementAt( index );
	}
	
	public void SortAlphabeticallyAtoZ() {
		database.Sort((x, y) => string.Compare(x.weaponName, y.weaponName));
	}
}