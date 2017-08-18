using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour {

    private List<Node> path = new List<Node>();
    private NavMeshAgent agent;
    private Node current;
    private NavGrid grid;
    private Pathfinding pathFinder;
   // private Transform targetTransform;
    [SerializeField] private Node target;

    private RaycastHit hit;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
        grid = NavGrid.instance;
        pathFinder = Pathfinding.instance;
    }

	void Update () {
        current = grid.getNodeByPosition(transform.position);

        if (Input.GetMouseButtonDown(1)){
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hit, 150f))
                target = grid.getNodeByPosition(hit.point);
        }
	}

    void LateUpdate(){
        path = pathFinder.getPath();
        if (path != null && current.child != null)
            agent.SetDestination(current.child.worldPosition);
    }

    public Node getTarget() { return target; }
}
