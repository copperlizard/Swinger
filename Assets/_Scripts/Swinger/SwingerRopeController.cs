using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingerRopeController : MonoBehaviour
{
    public enum Rope { Left, Right };

    private static bool created = false;

    [Header("RopeProperties")]
    [SerializeField]
    private float m_maxRopeForce = 100.0f;

    private SwingerController m_swinger;

    private LineRenderer m_leftRope, m_rightRope;

    private RaycastHit m_leftTar, m_rightTar;

    private float m_lengthLeft = 0.0f, m_lengthRight = 0.0f;

    private bool m_shootLeft = false, m_shootRight = false;

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

        m_swinger = GameObject.FindGameObjectWithTag("Player").GetComponent<SwingerController>();
        if (m_swinger == null)
        {
            Debug.Log("[SwingerRopeController] m_swinger not found!");
        }

        m_leftRope = GetComponentsInChildren<LineRenderer>()[0];
        if (m_leftRope == null)
        {
            Debug.Log("[SwingerController] m_leftRope not found!");
        }

        m_rightRope = GetComponentsInChildren<LineRenderer>()[1];
        if (m_rightRope == null)
        {
            Debug.Log("[SwingerController] m_rightRope not found!");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateRopePositions();
        ApplyRopePhysics();
	}

    private void UpdateRopePositions()
    {
        m_leftRope.SetPosition(0, m_swinger.transform.TransformPoint(m_swinger.m_leftHandPos));
        m_rightRope.SetPosition(0, m_swinger.transform.TransformPoint(m_swinger.m_rightHandPos));
        m_leftRope.SetPosition(1, m_leftTar.point);
        m_rightRope.SetPosition(1, m_rightTar.point);
    }

    private void ApplyRopePhysics()
    {
        //Get rope Dir
        //Project swinger m*velocity onto dir 
        //apply force -dir*projection


        //Left rope
        Vector3 leftDir = m_swinger.transform.TransformPoint(m_swinger.m_leftHandPos) - m_leftTar.point;
        leftDir = leftDir.normalized * 1000.0f; //plenty of line to project onto
        float leftForce = Vector3.Dot(m_swinger.Momentum(), leftDir);

        Debug.Log("leftForce == " + leftForce.ToString());

        //Right rope
    }

    public void ShootRope(Rope rope, RaycastHit tar)
    {
        if (rope == Rope.Left)
        {
            m_shootLeft = true;
            m_leftTar = tar;
            m_lengthLeft = Vector3.Distance(m_leftTar.point, m_swinger.transform.TransformPoint(m_swinger.m_leftHandPos));
            m_leftRope.enabled = true;
        }
        else
        {
            m_shootRight = true;
            m_rightTar = tar;
            m_lengthRight = Vector3.Distance(m_rightTar.point, m_swinger.transform.TransformPoint(m_swinger.m_rightHandPos));
            m_rightRope.enabled = true;
        }
    }

    public void ReleaseRope(Rope rope)
    {
        if (rope == Rope.Left)
        {
            m_shootLeft = false;
            m_leftRope.enabled = false;
        }
        else
        {
            m_shootRight = false;
            m_rightRope.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(m_leftTar.point, 0.1f);
        Gizmos.DrawSphere(m_rightTar.point, 0.1f);
    }
}
