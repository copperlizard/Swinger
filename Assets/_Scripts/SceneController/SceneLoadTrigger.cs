using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadTrigger : MonoBehaviour
{
    [SerializeField]
    private string m_loadSceneName;

    [SerializeField]
    private bool m_load = true;

    private SceneController m_sceneController;

	// Use this for initialization
	void Start ()
    {
        m_sceneController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>();
        if (m_sceneController == null)
        {
            Debug.Log("[SceneLoadTrigger] m_sceneController not found!");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(m_load)
            {
                m_sceneController.LoadScene(m_loadSceneName);
            }
            else
            {
                m_sceneController.UnloadScene(m_loadSceneName);
            }
        }
    }
}
