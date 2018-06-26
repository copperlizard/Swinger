using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_levelPiece;

    [SerializeField]
    private int m_rows, m_cols, m_lays;

    [SerializeField]
    private float m_width, m_height, m_depth;

    private GameObject[] m_pieces;

	// Use this for initialization
	void Start ()
    {
        int row = 0, col = 0, lay = 0;
        float wStep = m_width / m_cols, hStep = m_height / m_rows, lStep = m_depth / m_lays;
        m_pieces = new GameObject[m_rows * m_cols * m_lays];
        for(int i = 0; i < m_pieces.Length; i++)
        {
            Vector3 posOffset = transform.position - new Vector3(m_width / 2.0f, 0.0f, m_depth / 2.0f) + new Vector3(row * wStep, lay * lStep, col * hStep);
            m_pieces[i] = GameObject.Instantiate(m_levelPiece, transform.position + posOffset + new Vector3(Random.value * 100.0f, 0.0f, Random.value * 100.0f), transform.rotation);
            row++;
            if(row >= m_rows)
            {
                row = 0;
                col++;

                if (col >= m_cols)
                {
                    col = 0;
                    lay++;
                }
            }

            //float t = m_pieces[i].transform.position.magnitude;
            //m_pieces[i].transform.localScale = new Vector3(2.0f + 7.0f * (Mathf.Sin(m_pieces[i].transform.position.x + t) + 1.0f), 
            //    2.0f + 7.0f * (Mathf.Sin(m_pieces[i].transform.position.y + t) + 1.0f), 2.0f + 7.0f * (Mathf.Sin(m_pieces[i].transform.position.z + t) + 1.0f));

            float t = Random.value;
            //float t = m_pieces[i].transform.position.x + m_pieces[i].transform.position.y + m_pieces[i].transform.position.z;
            m_pieces[i].transform.localScale = new Vector3(2.0f + 9.0f * (1.0f + Mathf.Cos(t + Random.value)), 2.0f + 9.0f * (1.0f + Mathf.Sin(t * 5.0f + Random.value)), 
                2.0f + 9.0f * (1.0f + Mathf.Sin(t + Random.value)));


            m_pieces[i].transform.rotation = Random.rotation;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		/*foreach(GameObject piece in m_pieces)
        {   
            piece.transform.localScale = new Vector3(7.0f + 5.0f * Mathf.Sin(piece.transform.position.x + t),
                7.0f + 5.0f * Mathf.Sin(piece.transform.position.y + t), 7.0f + 5.0f * Mathf.Sin(piece.transform.position.z + t));
        }*/
	}
}
