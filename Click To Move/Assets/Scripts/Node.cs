using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Node {

    public Vector3 worldPosition;
    public bool walkable;
    private StringBuilder sb;

    public int x;
    public int y;

    public int g, h;
    public int f { get { return g + h; } }

    public Node parent, child;

    public Node(Vector3 wp, bool w, int i, int j)
    {
        worldPosition = wp;
        walkable = w;
        x = i;
        y = j;
        sb = new StringBuilder();
    }

    public override string ToString()
    {
        sb.Append("Node ");
        sb.Append(x);
        sb.Append(" " + y);
        return sb.ToString();
    }
}
