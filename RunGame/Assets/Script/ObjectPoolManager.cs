using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//using ObjectType = System.Text.StringBuilder;
using ObjectType = System.String;

public class ObjectPool
{
	public List<GameObject> mInstance = new List<GameObject>();
	public List<bool> mUsing = new List<bool>();
	public GameObject mSourcePrefab;

	public int mReleasedCount;
	public List<int> mReleasedIndex = new List<int>();


	public ObjectPool(){}
	
	public ObjectPool(string prefabPath)
	{
		mSourcePrefab = (GameObject)Resources.Load(prefabPath);
	}
	
	public ObjectPool(GameObject sourcePrefab)
	{
		this.mSourcePrefab = sourcePrefab;
	}

	public void Add(GameObject instance)
	{
		mReleasedIndex.Add(mInstance.Count);
		mInstance.Add(instance);
		mUsing.Add(true);
		mReleasedCount++;
	}

	public GameObject Create() 
	{
		int index = mReleasedIndex[0];
		GameObject target;
		
		target = mInstance[index];
		target.SetActive( true);
		
		mReleasedCount--;
		mReleasedIndex.RemoveAt(0);
		mUsing[index] = true ;
		
		return  target ;
	}

	public bool Release(GameObject instance)
	{
		int index;
		
		if(mInstance.Contains(instance) ==false)
			return false;

		index = mInstance.IndexOf(instance);
		
		mReleasedCount++;
		mReleasedIndex.Add(index);
		mUsing[index] = false;
		
		mInstance[index].SetActive(false);
		return true;
	}

	public void Destroy()
	{
		mSourcePrefab = null;
		mInstance.Clear();
		mUsing = null;
		return;
	}
}

[System.Serializable]
public class PreloadObjectInfo
{
	public GameObject prefab;
	public int count;
}

[System.Serializable]
public class PreloadFileInfo
{
	public string name;
	public int count;
}

public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager>
{
	public Dictionary<ObjectType, ObjectPool> mPoolList = new Dictionary<ObjectType, ObjectPool>();
	public PreloadObjectInfo[] mPreloadInfo;
	public bool mIsNewAdd = false;

	new protected void Awake()
	{
		Debug.Log ("Awake");
		if(mPreloadInfo != null)
			ObjectPoolManager.Instance.PreloadPrefab(mPreloadInfo);
		base.Awake();
	}

	new protected void OnApplicationQuit() 
	{
		base.OnApplicationQuit();
	}

	public void PreloadPrefab(PreloadObjectInfo [] infoArr)
	{
		GameObject srcPrefab, instance;
		int count, i;
		string name;
		
		if(infoArr.Length == 0)
		{
			return;
		}
						
		foreach(PreloadObjectInfo info in infoArr)
		{
			srcPrefab = info.prefab;
			count = info.count;
			name = srcPrefab.name;

			if(!_LoadPrefab(srcPrefab))
				continue;

			for(i=0;i < count; i++)
			{
				instance= (GameObject)Instantiate(srcPrefab);
				instance.name = name;
				//instance.transform.parent= transform;
				instance.SetActive(false);
				mPoolList[name].Add( instance);
			}
		}
	}

	//by PreloadFileInfo 
	public void PreloadPrefab(PreloadFileInfo [] infoArr)
	{
		GameObject srcPrefab, instance;
		int count, i;
		string name;
		
		if(infoArr.Length == 0)
		{
			return;
		}
		
		foreach(PreloadFileInfo info in infoArr)
		{
			if(!_LoadPrefab(info.name, out srcPrefab))
				continue;

			count = info.count;
			name = srcPrefab.name;

			for(i=0;i < count; i++)
			{
				instance= (GameObject)Instantiate(srcPrefab);
				instance.name = name;
				//instance.transform.parent= transform;
				instance.SetActive(false);
				mPoolList[name].Add( instance);
			}
		}
	}

	public GameObject GetSourcePrefab(string prefabPath, bool isLoad)
	{
		string prefabName = _GetPrefabName(prefabPath);

		if(!mPoolList.ContainsKey(prefabName))
		{
			if(isLoad == false)
				return null;

			GameObject obj;
			_LoadPrefab(prefabName, out obj);

			if(obj == null)
				return null;
		}

		return mPoolList[prefabName].mSourcePrefab;
	}

	public GameObject Create(string prefabPath)
	{
		Transform _transform = transform;
		string prefabName = _GetPrefabName(prefabPath);
		return Create(prefabName, _transform.position, _transform.rotation);
	}

	public GameObject Create(string prefabPath, Vector3 position, Quaternion rotation)
	{
		ObjectPool objPool;
		GameObject prefab;
		string prefabName = _GetPrefabName(prefabPath);
		
		if(!mPoolList.ContainsKey(prefabName) && mIsNewAdd)
		{
			GameObject srcPrefab;
			_LoadPrefab(prefabName, out srcPrefab);
		}

		objPool = mPoolList[prefabName];
		if(objPool == null)
			return null;

		//Could not find released Instance
		if(objPool.mReleasedCount == 0 && mIsNewAdd)
		{
			prefab = (GameObject)Instantiate(objPool.mSourcePrefab, position, rotation);
			prefab.name = prefabName;
			objPool.Add(prefab);
			return prefab;
		}

		//if released Instance is
		if(objPool.mReleasedCount > 0)
		{
			prefab = objPool.Create();
			prefab.transform.position = position;
			prefab.transform.rotation = rotation;
			prefab.SetActive(true);
			return prefab;
		}
				
		return null;
	}

	public bool Release(GameObject prefab)
	{
		return Release(prefab.name, prefab);
	}

	public bool Release(string prefabPath, GameObject prefab)
	{
		ObjectPool objPool;
		string prefabName = _GetPrefabName(prefabPath);
		
		if(!mPoolList.ContainsKey(prefabName))
		{
			Debug.LogWarning("Source Prefab is Not loaded");
			return false;
		}
		
		prefab.transform.parent = transform;
		objPool = mPoolList[prefabName];
		
		return objPool.Release(prefab);
	}


	public void UnloadPrefab(string prefabPath)
	{
		string prefabName = _GetPrefabName(prefabPath);
		mPoolList[prefabName].Destroy();
		mPoolList[prefabName] = null;
		Resources.UnloadUnusedAssets();
	}


	private bool _LoadPrefab(GameObject prefab)
	{
		if(mPoolList.ContainsKey(prefab.name))
			return false;
		mPoolList[prefab.name] = new ObjectPool(prefab);
		return true;	
	}

	private bool _LoadPrefab(string prefabPath, out GameObject srcPrefab){
		string prefabName = _GetPrefabName(prefabPath);
		if(mPoolList.ContainsKey(prefabName))
		{
			srcPrefab = null;
			return false;
		}

		ObjectPool objPool = new ObjectPool(prefabPath);
		mPoolList[prefabName] = objPool;
		srcPrefab = objPool.mSourcePrefab;

		return true;
	}
		
	private string _GetPrefabName(string prefabPath)
	{
		string[] splited = prefabPath.Split("/"[0]);
		return splited[splited.Length-1];
	}
}

