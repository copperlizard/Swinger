using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static bool created = false;

    [HideInInspector]
    public bool m_paused = false, m_loadingScene = false, m_unloadingScene = false;

    private List<string> m_loadedSceneNames = new List<string>();

    // Use this for initialization
    void Start ()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            Debug.Log("Awake(don't destroy on load): " + this.gameObject);
        }
        else
        {
            Debug.Log("Destroying Duplicate: " + this.gameObject);
            Destroy(this.gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            m_loadedSceneNames.Add(SceneManager.GetSceneAt(i).name);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_loadedSceneNames.Add(scene.name);
    }

    void OnSceneUnloaded(Scene scene)
    {
        m_loadedSceneNames.Remove(scene.name);
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
        if(m_loadedSceneNames.Contains(name))
        {
            Debug.Log("scene[" + name + "] already loaded!");
            return;
        }

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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        // Set new scene as active scene (new objects instantiate in "active" scene...)  ...should maybe move to after scene is fully loaded...
        asyncLoad.allowSceneActivation = true;

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
