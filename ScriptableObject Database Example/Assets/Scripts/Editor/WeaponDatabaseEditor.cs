using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

public class WeaponDatabaseEditor : EditorWindow {
	private enum State
	{
		BLANK,
		EDIT,
		ADD
	}
	
	private State state;
	private int selectedWeapon;
	private string newWeaponName;
	private int newWeaponDamage;
	
	private const string DATABASE_PATH = @"Assets/Database/weaponDB.asset";
	
	private WeaponDatabase weapons;
	private Vector2 _scrollPos;
	
	[MenuItem("BZA/Database/Weapon Database %#w")]
	public static void Init() {
		WeaponDatabaseEditor window = EditorWindow.GetWindow<WeaponDatabaseEditor>();
		window.minSize = new Vector2(800, 400);
		window.Show();
	}
	
	void OnEnable() {
		if (weapons == null)
			LoadDatabase();
		
		state = State.BLANK;
	}
	
	void OnGUI() {
		EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
		DisplayListArea();
		DisplayMainArea();
		EditorGUILayout.EndHorizontal();
	}
	
	void LoadDatabase() {
		weapons = (WeaponDatabase)AssetDatabase.LoadAssetAtPath(DATABASE_PATH, typeof(WeaponDatabase));
		
		if (weapons == null)
			CreateDatabase();
	}
	
	void CreateDatabase() {
		weapons = ScriptableObject.CreateInstance<WeaponDatabase>();
		AssetDatabase.CreateAsset(weapons, DATABASE_PATH);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}
	
	void DisplayListArea() {
		EditorGUILayout.BeginVertical(GUILayout.Width(250));
		EditorGUILayout.Space();
		
		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, "box", GUILayout.ExpandHeight(true));
		
		for (int cnt = 0; cnt < weapons.COUNT; cnt++)
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("-", GUILayout.Width(25)))
			{
				weapons.RemoveAt(cnt);
				weapons.SortAlphabeticallyAtoZ();
				EditorUtility.SetDirty(weapons);
				state = State.BLANK;
				return;
			}
			
			if (GUILayout.Button(weapons.Weapon(cnt).weaponName, "box", GUILayout.ExpandWidth(true)))
			{
				selectedWeapon = cnt;
				state = State.EDIT;
			}
			
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.EndScrollView();
		
		EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
		EditorGUILayout.LabelField("Weapons: " + weapons.COUNT, GUILayout.Width(100));
		
		if (GUILayout.Button("New Weapon"))
			state = State.ADD;
		
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();
	}
	
	void DisplayMainArea()
	{
		EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
		EditorGUILayout.Space();
		
		switch (state)
		{
		case State.ADD:
			DisplayAddMainArea();
			break;
		case State.EDIT:
			DisplayEditMainArea();
			break;
		default:
			DisplayBlankMainArea();
			break;
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();
	}
	
	void DisplayBlankMainArea()
	{
		EditorGUILayout.LabelField(
			"There are 3 things that can be displayed here.\n" +
			"1) Weapon info for editing\n" +
			"2) Black fields for adding a new weapon\n" +
			"3) Blank Area",
			GUILayout.ExpandHeight(true));
	}
	
	void DisplayEditMainArea()
	{
		weapons.Weapon(selectedWeapon).weaponName = EditorGUILayout.TextField(new GUIContent("Name: "), weapons.Weapon(selectedWeapon).weaponName);
		weapons.Weapon(selectedWeapon).damage = int.Parse(EditorGUILayout.TextField(new GUIContent("Damage: "), weapons.Weapon(selectedWeapon).damage.ToString()));
		
		EditorGUILayout.Space();
		
		if (GUILayout.Button("Done", GUILayout.Width(100)))
		{
			weapons.SortAlphabeticallyAtoZ();
			EditorUtility.SetDirty(weapons);
			state = State.BLANK;
		}
	}
	
	void DisplayAddMainArea()
	{
		newWeaponName = EditorGUILayout.TextField(new GUIContent("Name: "), newWeaponName);
		newWeaponDamage = Convert.ToInt32(EditorGUILayout.TextField(new GUIContent("Damage: "), newWeaponDamage.ToString()));
		
		EditorGUILayout.Space();
		
		if (GUILayout.Button("Done", GUILayout.Width(100)))
		{
			weapons.Add(new Weapon(newWeaponName, newWeaponDamage));
			weapons.SortAlphabeticallyAtoZ();
			
			newWeaponName = string.Empty;
			newWeaponDamage = 0;
			EditorUtility.SetDirty(weapons);
			state = State.BLANK;
		}
	}
}