using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public static Pathfinding instance { get; private set; }

    [SerializeField]
    private Node target;

    [SerializeField]
    private Agent agent;

    private NavGrid navGrid;
    private List<Node> AIPath = new List<Node>();


    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        navGrid = GetComponent<NavGrid>();
	}
	
	// Update is called once per frame
	void Update () {
        target = agent.getTarget();
        if(target != null)
            FindPath(agent.transform.position, target.worldPosition);
    }

    private int GetDistance(Node a, Node b){
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        if (dx > dy)
            return 14 * dy + 10 * (dx - dy);
        return 14 * dx + 10 * (dy - dx);
    }

    public void FindPath(Vector3 start, Vector3 target){
        Node startNode = navGrid.getNodeByPosition(start);
        Node targetNode = navGrid.getNodeByPosition(target);
        Node currentNode = startNode;

        List<Node> neighbours = new List<Node>();
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0){
            currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++){
                if(openSet[i].f <= currentNode.f && openSet[i].h < currentNode.h){
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode){
                RetracePath(startNode, targetNode);
                return;
            }

            neighbours = navGrid.GetNeighbours(currentNode);
            
            foreach(Node n in neighbours){
                if (!n.walkable || closedSet.Contains(n))
                    continue;

                int newCostToNeighbour = currentNode.g + GetDistance(currentNode, n);
                if (n.g > newCostToNeighbour || !openSet.Contains(n)){
                    n.g = newCostToNeighbour;
                    n.h = GetDistance(n, targetNode);
                    n.parent = currentNode;
                    if (!openSet.Contains(n))
                        openSet.Add(n);
                }
            }
        }
    }

    public List<Node> getPath() {
        return AIPath;
    }
    
    public void RetracePath(Node start, Node end){
        Node current = end;
        List<Node> path = new List<Node>();
        while(current != start){
            path.Add(current);
            current.parent.child = current;
            current = current.parent;
        }
        path.Reverse();
        AIPath = path;
    }
}
