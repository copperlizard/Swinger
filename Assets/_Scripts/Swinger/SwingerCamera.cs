using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingerCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject m_target;

    //[SerializeField]
    public Vector3 m_leftHandPos = new Vector3(-0.25f, 0.25f, 0.0f); //in local space...

    [HideInInspector]
    public Vector3 m_rightHandPos; //Ymirror of left hand pos

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

       
        /*m_cam = GetComponent<Camera>();
        if (m_cam == null)
        {
            Debug.Log("[SwingerCamera] m_cam not found!");
        }*/

        Cursor.lockState = CursorLockMode.Confined;

        m_rightHandPos = new Vector3(-m_leftHandPos.x, m_leftHandPos.y, m_leftHandPos.z);
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
