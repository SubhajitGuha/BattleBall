using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class AttackerController : MonoBehaviour
{
    private Vector3 m_movePosition;
    private Vector2Int m_currentCoord;
    private Vector3 m_ballposition;

    private Vector2 m_startTouchPos;
    private Vector2 m_endTouchPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_movePosition = transform.position;
        m_currentCoord = new Vector2Int(0, 0);
        m_ballposition = GameObject.FindGameObjectWithTag("Ball").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MazeGenerator.Graph node = MazeGenerator.mazeGraph[m_currentCoord.x, m_currentCoord.y];
        bool isGamePaused = MazeGameManager.instance.IsGamePaused();

        //swipe input
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            m_startTouchPos = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            m_endTouchPos = Input.GetTouch(0).position;
        }

        if ((Input.GetKeyDown(KeyCode.W) || m_endTouchPos.y > m_startTouchPos.y) && !isGamePaused)
        {
            if(node.top != -Vector2Int.one)
            {
                m_currentCoord = node.top;
                transform.position += new Vector3(0.0f, 0.0f, 1.0f); //move one unit in top
            }
        }
        else if ((Input.GetKeyDown(KeyCode.S) || m_endTouchPos.y < m_startTouchPos.y) && !isGamePaused)
        {
            if (node.bottom != -Vector2Int.one)
            {
                m_currentCoord = node.bottom;
                transform.position += new Vector3(0.0f, 0.0f, -1.0f); //move one unit in bottom
            }
        }
        else if ((Input.GetKeyDown(KeyCode.A) || m_endTouchPos.x < m_startTouchPos.x) && !isGamePaused)
        {
            if (node.left != -Vector2Int.one)
            {
                m_currentCoord = node.left;
                transform.position += new Vector3(-1.0f, 0.0f, 0.0f); //move one unit in left
            }
        }
        else if ((Input.GetKeyDown(KeyCode.D) || m_endTouchPos.x > m_startTouchPos.x) && !isGamePaused)
        {
            if (node.right != -Vector2Int.one)
            {
                m_currentCoord = node.right;
                transform.position += new Vector3(1.0f, 0.0f, 0.0f); //move one unit in right
            }
        }

        if (transform.position == m_ballposition)
        {
            MazeGameManager.instance.AttackerWins();
            Debug.Log("Maze Attacker Wins");
        }
    }
}
