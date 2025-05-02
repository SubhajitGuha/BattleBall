using UnityEngine;

public class MazeChunk : MonoBehaviour
{
    [SerializeField] private GameObject m_leftWall;
    [SerializeField] private GameObject m_rightWall;
    [SerializeField] private GameObject m_topWall;
    [SerializeField] private GameObject m_bottomWall;

    public void RemoveLeftWall()
    {
        //m_leftWall.GetComponent<Collider>().enabled = false;
        //m_leftWall.GetComponent<MeshRenderer>().enabled = false;
        m_leftWall.SetActive(false);
    }

    public void RemoveRightWall()
    {
        //m_rightWall.GetComponent<Collider>().enabled = false;
        //m_rightWall.GetComponent<MeshRenderer>().enabled = false;
        m_rightWall.SetActive(false);
    }

    public void RemoveTopWall()
    {
        //m_topWall.GetComponent<Collider>().enabled = false;
        //m_topWall.GetComponent<MeshRenderer>().enabled = false;

        m_topWall.SetActive(false);
    }

    public void RemoveBottomWall()
    {
        //m_bottomWall.GetComponent<Collider>().enabled = false;
        //m_bottomWall.GetComponent<MeshRenderer>().enabled = false;

        m_bottomWall.SetActive(false);
    }
}
