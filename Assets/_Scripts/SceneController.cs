using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [HideInInspector]
    public bool m_paused = false, m_loadingScene = false, m_unloadingScene = false;

    

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void PauseButton()
    {
        m_paused = !m_paused;
    }

    public void GameQuit()
    {
        if (Application.isPlaying)
        {
            Application.Quit();
        }
    }

    public void GameReset()
    {
        if (Application.isPlaying)
        {
            SceneManager.LoadScene(0);
            PauseButton();
        }
    }

    public void LoadScene(string name)
    {
        if(m_loadingScene)
        {
            return;
        }
        m_loadingScene = true;

        // Use a coroutine to load the Scene in the background
        StartCoroutine(AsyncLoadScene(name));
    }

    IEnumerator AsyncLoadScene(string name)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        m_loadingScene = false;
    }

    public void UnloadScene(string name)
    {
        if (m_unloadingScene)
        {
            return;
        }
        m_unloadingScene = true;

        // Use a coroutine to load the Scene in the background
        StartCoroutine(AsyncUnloadScene(name));
    }

    IEnumerator AsyncUnloadScene(string name)
    {
        // The Application unloads the Scene in the background as the current Scene runs.
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(name);

        // Wait until the asynchronous scene fully unloads
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        m_unloadingScene = false;
    }
}
