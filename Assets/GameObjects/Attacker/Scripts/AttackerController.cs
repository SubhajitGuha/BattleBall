using System.IO.Pipes;
using UnityEngine;

public class AttackerController : MonoBehaviour
{
    private Vector3 m_movePosition;
    private Vector2Int m_currentCoord;
    private Vector3 m_ballposition;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
        m_movePosition = transform.position;
        m_currentCoord = new Vector2Int(0, 0);
        m_ballposition = GameObject.FindGameObjectWithTag("Ball").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MazeGenerator.Graph node = MazeGenerator.mazeGraph[m_currentCoord.x, m_currentCoord.y];
        bool isGamePaused = MazeGameManager.instance.IsGamePaused();
        if(isGamePaused)
            return;

#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                            if (node.right != -Vector2Int.one)
                            {
                                m_currentCoord = node.right;
                                transform.position += new Vector3(1.0f, 0.0f, 0.0f); //move one unit in right
                            }
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                            if (node.left != -Vector2Int.one)
                            {
                                m_currentCoord = node.left;
                                transform.position += new Vector3(-1.0f, 0.0f, 0.0f); //move one unit in left
                            }
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                            if (node.top != -Vector2Int.one)
                            {
                                m_currentCoord = node.top;
                                transform.position += new Vector3(0.0f, 0.0f, 1.0f); //move one unit in top
                            }
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");
                            if (node.bottom != -Vector2Int.one)
                            {
                                m_currentCoord = node.bottom;
                                transform.position += new Vector3(0.0f, 0.0f, -1.0f); //move one unit in bottom
                            }
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
#endif

        if ((Input.GetKeyDown(KeyCode.W)))
        {
            if (node.top != -Vector2Int.one)
            {
                m_currentCoord = node.top;
                transform.position += new Vector3(0.0f, 0.0f, 1.0f); //move one unit in top
            }
        }
        else if ((Input.GetKeyDown(KeyCode.S)))
        {
            if (node.bottom != -Vector2Int.one)
            {
                m_currentCoord = node.bottom;
                transform.position += new Vector3(0.0f, 0.0f, -1.0f); //move one unit in bottom
            }
        }
        else if ((Input.GetKeyDown(KeyCode.A)))
        {
            if (node.left != -Vector2Int.one)
            {
                m_currentCoord = node.left;
                transform.position += new Vector3(-1.0f, 0.0f, 0.0f); //move one unit in left
            }
        }
        else if ((Input.GetKeyDown(KeyCode.D)))
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
