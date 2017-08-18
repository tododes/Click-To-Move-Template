using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour {

    public static NavGrid instance { get; private set; }

    [SerializeField] private Vector2 gridWorldSize;
    [SerializeField] private float nodeRadius, nodeDiameter;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private Pathfinding myPathfinding;
    [SerializeField] private Transform player;

    private Node[,] grid;

    void Awake(){
        instance = this;

        nodeDiameter = 2 * nodeRadius;
        gridSize = new Vector2(gridWorldSize.x / nodeDiameter, gridWorldSize.y / nodeDiameter);
        grid = new Node[Mathf.RoundToInt(gridSize.x), Mathf.RoundToInt(gridSize.y)];
        CreateGrid();
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        for (int i = 0; i < gridSize.x; i++){
            for (int j = 0; j < gridSize.y; j++){
                grid[i, j].walkable = !(Physics.CheckSphere(grid[i, j].worldPosition, nodeRadius, wallMask));
            }
        }
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(grid != null){
            Node pNode = getNodeByPosition(player.position);
            foreach (Node n in grid){
                Gizmos.color = Color.yellow;

                if (myPathfinding.getPath() != null)
                {
                    List<Node> path = myPathfinding.getPath();
                    if (path.Contains(n))
                        Gizmos.color = Color.blue;
                }
                if (pNode == n)
                    Gizmos.color = Color.magenta;
                if (!n.walkable)
                    Gizmos.color = Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

    private void CreateGrid(){
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for(int i = 0; i < gridSize.x; i++){
            for (int j = 0; j < gridSize.y; j++){
                Vector3 pos = bottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) + Vector3.forward * (j * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(pos, nodeRadius, wallMask));
                grid[i, j] = new Node(pos, walkable, i, j);
            }
        }
    }

    public List<Node> GetNeighbours(Node node){
        List<Node> nodes = new List<Node>();
        for(int i = -1; i <= 1; i++){
            for (int j = -1; j <= 1; j++){
                if (i == 0 && j == 0)
                    continue;

                int checkX = node.x + i;
                int checkY = node.y + j;
                if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                    nodes.Add(grid[checkX, checkY]);
            }
        }
        return nodes;
    }

    public Node getNodeByPosition(Vector3 pos){
        float percentX = (pos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (pos.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
        int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);

        return grid[x, y];
    }
}
