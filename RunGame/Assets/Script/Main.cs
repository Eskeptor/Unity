using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	CGameObjectPool<GameObject> monster_pool;
	List<GameObject> monsters;
	List<GameObject> remove_list;
	
	// Use this for initialization
	void Start () {
		int number = 0;
		this.monster_pool = new CGameObjectPool<GameObject>(5, () => 
		{ 
			++number;
			GameObject obj = new GameObject("monster" + number);
			return obj;
		});

		this.monsters = new List<GameObject>();
		this.remove_list = new List<GameObject>();
	}
	
	void OnGUI()
	{
		if (GUI.Button(new Rect(200,100,100,50), "Generate"))
		{
			// Get object from pool.
			GameObject obj = this.monster_pool.pop();
			
			// Add object to show on GUI.
			this.monsters.Add(obj);
		}
		
		int index = 0;
		this.monsters.ForEach(obj =>
		{
			if (GUI.Button(new Rect(0, index * 50, 100, 50), obj.name))
			{
				this.remove_list.Add(obj);
				
				// Return to pool.
				this.monster_pool.push(obj);
			}
			++index;
		});
		
		if (this.remove_list.Count > 0)
		{
			this.remove_list.ForEach(obj => this.monsters.Remove(obj));
			this.remove_list.Clear();
		}
	}
}
