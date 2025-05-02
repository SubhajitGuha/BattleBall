using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    const int MAX_VALID_POSITIONS_TO_STORE = 30;

    [SerializeField] private MazeChunk m_maze;
    [SerializeField] private int m_width = 20; //width of the maze
    [SerializeField] private int m_height = 20; //height of the maze
    [SerializeField] private int m_spacing = 1;

    private MazeChunk[,] m_mazeChunks; // array that contains the Instantiated mesh chunks
    private bool[,] m_visited; //to identify the nodes that are already visited
   
    private Vector2Int[] m_directions; //stores 4 possible directions (left, right, top, down)

    [SerializeField] private GameObject m_ball; //ball object to spawn

    private List<Vector2Int> m_ballPositions = new List<Vector2Int>(); //stores possible reachable positions in the maze

    [SerializeField] private GameObject m_attacker;

    public struct Graph
    {
        public Vector2Int left;
        public Vector2Int right;
        public Vector2Int top;
        public Vector2Int bottom;
    };
    public static Graph[,] MazeGraph;//Adjacency list of our maze graph, this is used traverse the graph

    private void initilizeGraph(ref Graph graph)
    {
        graph.left = new Vector2Int(-1, -1);
        graph.right = new Vector2Int(-1, -1);
        graph.top = new Vector2Int(-1, -1);
        graph.bottom = new Vector2Int(-1, -1);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //m_ballPosition = new Vector2(float.MaxValue, float.MaxValue);

        m_directions = new Vector2Int[4];
        m_directions[0] = new Vector2Int(-1, 0);
        m_directions[1] = new Vector2Int(1, 0);
        m_directions[2] = new Vector2Int(0, 1);
        m_directions[3] = new Vector2Int(0,-1);
        m_mazeChunks = new MazeChunk[m_width,m_height];
        MazeGraph = new Graph[m_width,m_height];
        m_visited = new bool[m_width,m_height];

        Vector3 generatorPos = transform.position;
        for(int i=0;i<m_width;i+=m_spacing)
        {
            for(int j=0;j<m_height;j+=m_spacing)
            {
                m_visited[i,j] = false; //make every node to be not visited
                initilizeGraph(ref MazeGraph[i,j]);
                var gameObject = Instantiate(m_maze.gameObject, new Vector3(i + generatorPos.x, 0, j + generatorPos.z), Quaternion.identity);
                m_mazeChunks[i,j] = gameObject.GetComponent<MazeChunk>();
            }
        }

        //do depth first serch (in random order)
        dfs(0, 0, null, new Vector2Int(0,0));

        //choose a random rechable pos
        Vector2Int m_ballPosition = m_ballPositions[UnityEngine.Random.Range(0,m_ballPositions.Count)]; 

        //place the ball at a random valid locationin the maze
        Instantiate(m_ball, new Vector3(m_ballPosition.x + generatorPos.x, 0, m_ballPosition.y + generatorPos.z), Quaternion.identity);
        //spawn the attacker at starting position
        Instantiate(m_attacker, new Vector3(generatorPos.x, 0, generatorPos.z), Quaternion.identity);
    }

    private void removeWalls(int i, int j, in MazeChunk prevChunk, in Vector2Int moveDirection)
    {
        if(prevChunk == null)
        {
            return;
        }

        int i_prev = i - moveDirection.x;
        int j_prev = j - moveDirection.y;
        if (moveDirection.x == -1) //moving left from the prev chunk
        {
            prevChunk.RemoveLeftWall();
            m_mazeChunks[i,j].RemoveRightWall();
            MazeGraph[i_prev, j_prev].left = new Vector2Int(i,j); //connect the previous node to next node
            MazeGraph[i, j].right = new Vector2Int(i_prev, j_prev);
        }
        if (moveDirection.x == 1) //moving right from the prev chunk
        {
            prevChunk.RemoveRightWall();
            m_mazeChunks[i, j].RemoveLeftWall();
            MazeGraph[i_prev, j_prev].right = new Vector2Int(i, j);
            MazeGraph[i, j].left = new Vector2Int(i_prev, j_prev);
        }
        if (moveDirection.y == -1) //moving bottom from the prev chunk
        {
            prevChunk.RemoveBottomWall();
            m_mazeChunks[i, j].RemoveTopWall();
            MazeGraph[i_prev, j_prev].bottom = new Vector2Int(i, j);
            MazeGraph[i, j].top = new Vector2Int(i_prev, j_prev);
        }
        if (moveDirection.y == 1) //moving up from the prev chunk
        {
            prevChunk.RemoveTopWall();
            m_mazeChunks[i, j].RemoveBottomWall();
            MazeGraph[i_prev, j_prev].top = new Vector2Int(i, j);
            MazeGraph[i, j].bottom = new Vector2Int(i_prev, j_prev);
        }
    }
    public void dfs(int i, int j, in MazeChunk prevChunk, in Vector2Int moveDirection)
    {
        if((i < 0 || j < 0 || i>=m_width || j>=m_height))
        {
            return;
        }

        if(m_visited[i, j])
        { return; }

        removeWalls(i, j, prevChunk, moveDirection);
        m_visited[i,j] = true; //mark this node visited

        //visit all children
        foreach (Vector2Int position in m_directions.OrderBy(d => UnityEngine.Random.value)) 
        { 
            Vector2Int randomDir = position;
            dfs(i+randomDir.x, j+randomDir.y, m_mazeChunks[i,j], randomDir);
        }
        if(prevChunk != null && m_ballPositions.Count <= MAX_VALID_POSITIONS_TO_STORE) //collecting 30 possible valid locations to place the ball
            m_ballPositions.Add(new Vector2Int(i, j));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
