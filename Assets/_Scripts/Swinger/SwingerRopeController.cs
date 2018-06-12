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

    private Rigidbody m_swingerRigidBody;

    private LineRenderer m_leftRope, m_rightRope;

    private RaycastHit m_leftTar, m_rightTar;

    private Vector3[] m_zeroLine = new Vector3[2];
    private List<Vector3> m_leftRopePoints = new List<Vector3>(), m_rightRopePoints = new List<Vector3>();

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
        else
        {
            m_swingerRigidBody = m_swinger.gameObject.GetComponent<Rigidbody>();
            if (m_swingerRigidBody == null)
            {
                Debug.Log("[SwingerRopeController] m_swingerRigidBody not found!");
            }
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

        m_leftRopePoints.Add(Vector3.zero);
        m_leftRopePoints.Add(Vector3.zero);
        m_rightRopePoints.Add(Vector3.zero);
        m_rightRopePoints.Add(Vector3.zero);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        UpdateRopePositions();
        ApplyRopePhysics();
	}

    //DEBUG
    //private List<Vector3> m_inters = new List<Vector3>();
    private void UpdateRopePositions()
    {
        Vector3 leftHand = m_swinger.transform.TransformPoint(m_swinger.m_leftHandPos);
        Vector3 rightHand = m_swinger.transform.TransformPoint(m_swinger.m_rightHandPos);

        m_leftRopePoints[0] = leftHand;
        m_rightRopePoints[0] = rightHand;

        //Check for intersections
        RaycastHit intersection;
        if(m_shootLeft)
        {   
            Vector3 leftDir = m_leftTar.point - leftHand;
            if (Physics.Raycast(leftHand, leftDir.normalized, out intersection, leftDir.magnitude - 0.1f, ~LayerMask.GetMask("Player", "Ignore Raycast")))
            {                
                Debug.Log("left rope interesected " + intersection.collider.name);
                
                m_leftRopePoints.Insert(1, Vector3.zero);
                m_leftTar = intersection;
            }

            m_leftRopePoints[1] = m_leftTar.point + m_leftTar.normal * 0.05f;

            m_leftRope.positionCount = m_leftRopePoints.Count;
            m_leftRope.SetPositions(m_leftRopePoints.ToArray());
        }
        
        if(m_shootRight)
        {   
            Vector3 rightDir = m_rightTar.point - rightHand;
            if (Physics.Raycast(rightHand, rightDir.normalized, out intersection, rightDir.magnitude - 0.1f, ~LayerMask.GetMask("Player", "Ignore Raycast")))
            {
                Debug.Log("left rope interesected " + intersection.collider.name);

                m_rightRopePoints.Insert(1, Vector3.zero);
                m_rightTar = intersection;
            }

            m_rightRopePoints[1] = m_rightTar.point + m_rightTar.normal * 0.05f;

            m_rightRope.positionCount = m_rightRopePoints.Count;
            m_rightRope.SetPositions(m_rightRopePoints.ToArray());
        }
    }

    private void ApplyRopePhysics()
    {
        //Left rope
        Vector3 leftDir = m_swinger.transform.TransformPoint(m_swinger.m_leftHandPos) - m_leftTar.point;
        leftDir = leftDir.normalized * 100.0f; //plenty of line to project onto
        float leftForce = Vector3.Dot(m_swinger.Momentum(), leftDir);

        //Debug.Log("leftForce == " + leftForce.ToString());

        if(leftForce > 0.0f && m_shootLeft)
        {
            m_swingerRigidBody.AddForce(-leftDir.normalized * leftForce);
        }

        //Right rope
        Vector3 rightDir = m_swinger.transform.TransformPoint(m_swinger.m_rightHandPos) - m_rightTar.point;
        rightDir = rightDir.normalized * 100.0f; //plenty of line to project onto
        float rightForce = Vector3.Dot(m_swinger.Momentum(), rightDir);

        //Debug.Log("rightForce == " + rightForce.ToString());

        if (rightForce > 0.0f && m_shootRight)
        {
            m_swingerRigidBody.AddForce(-rightDir.normalized * rightForce);
        }
    }

    public void ShootRope(Rope rope, RaycastHit tar)
    {
        //Debug.Log("Shoot Rope: " + rope.ToString());
        if (rope == Rope.Left)
        {
            m_shootLeft = true;
            m_leftTar = tar;
            m_lengthLeft = Vector3.Distance(m_leftTar.point, m_swinger.transform.TransformPoint(m_swinger.m_leftHandPos)) - 0.1f;
            m_leftRope.enabled = true;
        }
        else
        {
            m_shootRight = true;
            m_rightTar = tar;
            m_lengthRight = Vector3.Distance(m_rightTar.point, m_swinger.transform.TransformPoint(m_swinger.m_rightHandPos)) - 0.1f;
            m_rightRope.enabled = true;
        }
    }

    public void ReleaseRope(Rope rope)
    {        
        if (rope == Rope.Left && m_shootLeft)
        {
            m_shootLeft = false;
            m_leftRope.enabled = false;

            m_leftRopePoints.RemoveRange(2, m_leftRopePoints.Count - 2);
            m_leftRopePoints[0] = Vector3.zero;
            m_leftRopePoints[1] = Vector3.zero;
            m_leftRope.positionCount = 2;
            m_leftRope.SetPositions(m_leftRopePoints.ToArray());

            //Debug.Log("Release Rope: " + rope.ToString());            
        }
        else if (m_shootRight)
        {
            m_shootRight = false;
            m_rightRope.enabled = false;

            m_rightRopePoints.RemoveRange(2, m_rightRopePoints.Count - 2);
            m_rightRopePoints[0] = Vector3.zero;
            m_rightRopePoints[1] = Vector3.zero;
            m_rightRope.positionCount = 2;
            m_rightRope.SetPositions(m_rightRopePoints.ToArray());

            //Debug.Log("Release Rope: " + rope.ToString());
        }
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.DrawSphere(m_leftTar.point, 0.1f);
        Gizmos.DrawSphere(m_rightTar.point, 0.1f);

        Gizmos.color = Color.red;
        foreach(Vector3 v in m_inters)
        {
            Gizmos.DrawWireCube(v, Vector3.one * 0.1f);
        }*/

        if(m_leftRope != null)
        {
            Vector3[] points = new Vector3[m_leftRope.positionCount];
            m_leftRope.GetPositions(points);
            float b = 0.0f;
            foreach (Vector3 v in points)
            {
                Gizmos.color = Color.yellow * b;
                Gizmos.DrawWireSphere(v, 0.1f);
                b += 1.0f / m_leftRope.positionCount;
            }
        }
    }
}
