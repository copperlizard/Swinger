using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingerCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject m_target;

    private Camera m_cam;

	// Use this for initialization
	void Start ()
    {
        if(m_target == null)
        {
            m_target = GameObject.FindGameObjectWithTag("Player");
            if(m_target == null)
            {
                Debug.Log("[SwingerCamera] m_target not found!");
            }
        }

        m_cam = GetComponentInChildren<Camera>();
        if (m_cam == null)
        {
            Debug.Log("[SwingerCamera] m_cam not found!");
        }

        Cursor.lockState = CursorLockMode.Confined;
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        //transform.position = m_target.transform.position;
        transform.position = Vector3.Lerp(transform.position, m_target.transform.position, 15.0f * Time.deltaTime);
        //transform.rotation = m_target.transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, m_target.transform.rotation, 15.0f * Time.deltaTime);
	}
}
