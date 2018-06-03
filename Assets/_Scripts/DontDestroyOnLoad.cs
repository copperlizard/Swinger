using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static bool created = false;

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
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
