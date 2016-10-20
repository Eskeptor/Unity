using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T mInstance;

	public static T Instance
    {
        get
        {
			if (mInstance == null)
            {
				mInstance = (T)FindObjectOfType(typeof(T));

				if (mInstance == null)
                {
                    Debug.LogError(typeof(T) + " is nothing");
                }
            }

			return mInstance;
        }
    }

	protected void OnApplicationQuit() 
	{
		mInstance = null;
	}

    protected void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (this == Instance)
        {
            return true;
        }
        Destroy(this);
        return false;
    }
}