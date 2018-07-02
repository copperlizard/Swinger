using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingerCamera : MonoBehaviour
{
    [SerializeField]
    private Texture2D m_cursorSprite;

    [SerializeField]
    private GameObject m_target;

    //[SerializeField]
    public Vector3 m_leftHandPos = new Vector3(-0.25f, 0.25f, 0.0f); //in local space...

    [HideInInspector]
    public Vector3 m_rightHandPos; //Ymirror of left hand pos

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

        Cursor.lockState = CursorLockMode.Confined;

        if(m_cursorSprite != null)
        {
            Cursor.SetCursor(m_cursorSprite, new Vector2(32.0f, 32.0f), CursorMode.Auto);
        }

        m_rightHandPos = new Vector3(-m_leftHandPos.x, m_leftHandPos.y, m_leftHandPos.z);
    }
	
	// LateUpdate is called once per frame
	void LateUpdate ()
    {
        //transform.position = m_target.transform.position;
        transform.position = Vector3.Lerp(transform.position, m_target.transform.position, 17.0f * Time.deltaTime);
        //transform.rotation = m_target.transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, m_target.transform.rotation, 17.0f * Time.deltaTime);
	}

    void OnGUI()
    {
        GUI.skin.settings.cursorColor = Color.blue;        
    }
}
