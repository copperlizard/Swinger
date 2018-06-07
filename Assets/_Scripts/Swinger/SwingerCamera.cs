using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingerCamera : MonoBehaviour
{
    private Camera m_cam;

	// Use this for initialization
	void Start ()
    {
        m_cam = GetComponentInChildren<Camera>();
        if (m_cam == null)
        {
            Debug.Log("[SwingerCamera] m_cam not found!");
        }

        Cursor.lockState = CursorLockMode.Confined;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
