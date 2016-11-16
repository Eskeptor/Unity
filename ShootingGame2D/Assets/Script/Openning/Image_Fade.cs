using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// This class is Openning Effect
public class Image_Fade : MonoBehaviour
{
    public float FadeTime = 5f;
    public float DelayTime = 5f;

	// Use this for initialization
	void Start ()
    {
        Screen.SetResolution(400, 600, false);
        GetComponent<Animator>().SetBool("FadeStart", true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(DelayFade());
	}

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(FadeTime);
        GetComponent<Animator>().SetBool("FadeStart", false);
        StartCoroutine(DelayMain());
    }

    IEnumerator DelayMain()
    {
        yield return new WaitForSeconds(DelayTime);
        SceneManager.LoadScene("Main");
    }
}
