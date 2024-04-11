using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerPathfinding : MonoBehaviour
{
    public TileManager tileManager;

    [SerializeField] List<Vector2Int> nodes = new List<Vector2Int>();
    [SerializeField] List<Vector2Int> parentNodes = new List<Vector2Int>();
    [SerializeField] List<bool> state = new List<bool>();
    [SerializeField] List<int> nodesGCost = new List<int>();

    [SerializeField] List<Vector2Int> cardinalDirections = new List<Vector2Int>
    {new Vector2Int(1,0), new Vector2Int(-1,0), new Vector2Int(0,1), new Vector2Int(0,-1)};

    [SerializeField] Vector2Int currentNode;
    [SerializeField] Vector2Int pointerNode;
    [SerializeField] Vector2Int startNode;
    [SerializeField] Vector2Int targetNode;

    [SerializeField] int nullCost;
    [SerializeField] int sandCost;

    void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
    }



    // Find the location of a given node within the nodes list
    int nodeLocation(Vector2Int givenNode)
    {
        // Check each node in the nodes list, and return the location of the node that matches givenNode
        foreach (Vector2Int item in nodes)
        {
            if (item == givenNode)
            {
                return nodes.IndexOf(item);
            }
        }
        // Otherwise, return zero
        return 0;
    }



    // Check whether a node exists in the nodes list
    bool inNodes(Vector2Int givenNode)
    {
        // Check each node in the nodes list, and return true if givenNode can be found
        foreach (Vector2Int item in nodes)
        {
            if (givenNode == item)
            {
                return true;
            }
        }
        // Otherwise, return false
        return false;
    }



    // Find the open node with the lowest F cost
    Vector2Int lowestFCost()
    {
        Vector2Int currentLowest = new Vector2Int(0,0);

        // Find a starting value for currentLowest
        foreach (Vector2Int item in nodes)
        {
            if (state[nodeLocation(item)] == true)
            {
                currentLowest = item;
                break;
            }
        }

        // Check each node against currentLowest, replacing it if a lower value is found
        foreach (Vector2Int item in nodes)
        {

            if (state[nodeLocation(item)] == true && fCost(item) < fCost(currentLowest))
            {
                currentLowest = item;
            }
        }

        // Return the lowest value found
        return currentLowest;
    }



    // Distance from start node
    int gCost(Vector2Int givenNode)
    {
        // Return 0 if givenNode is the start node
        if (givenNode == startNode)
        {
            return 0;
        }

        // Return the gCost of the trail of tiles from the current tile to the start tile
        nodesGCost.Add(0);
        int givenNodeGCost = nodesGCost[nodeLocation(parentNodes[nodeLocation(givenNode)])];
        nodesGCost[nodeLocation(givenNode)] = givenNodeGCost;
        return givenNodeGCost;
    }



    // Estimated relative distance from target node
    int hCost(Vector2Int givenNode)
    {
        // Use pythagoras' theorem to find the square of the distance to the target
        int xDistance = Mathf.Abs(givenNode.x - targetNode.x);
        int yDistance = Mathf.Abs(givenNode.y - targetNode.y);

        return (xDistance ^ 2) + (yDistance ^ 2);
    }



    // gCost + hCost
    int fCost(Vector2Int givenNode)
    {
        return gCost(givenNode) + hCost(givenNode);
    }


    // The main method
    [ContextMenu("Find Path")]
    public Vector2Int findPath(GameObject start, GameObject target)
    {
        // Reset lists
        nodes.Clear();
        parentNodes.Clear();
        state.Clear();
        // Set startNode to the enemy's position
        startNode = new Vector2Int((int)start.transform.position.x, (int)start.transform.position.y);
        // Set targetNode to the player's position
        targetNode = new Vector2Int((int)target.transform.position.x, (int)target.transform.position.y);
        // Add the startNode to the nodes list
        nodes.Add(startNode);
        parentNodes.Add(new Vector2Int(0,0));
        state.Add(true);
        nodesGCost.Add(0);

        // Do not move if the enemy is on the same tile as the player
        if (startNode == targetNode)
        {
            return new Vector2Int(0, 0);
        }

        // Loop until a path is found
        while (true)
        {
            // Set currentNode to the open node with the lowest fCost
            currentNode = lowestFCost();
            // Close currentNode
            state[nodeLocation(currentNode)] = false;
            // If the path has been found, stop the method and begin building the path
            if (currentNode == targetNode)
            {
                return firstStep(currentNode);
            }

            // Run for each of the cardinal directions
            foreach (Vector2Int neighbourDirection in cardinalDirections)
            {
                // Place the pointerNode one tile away from the currentNode in the direction currently being checked
                pointerNode = currentNode + neighbourDirection;
                Vector3Int pointerNodeVector3 = new Vector3Int(pointerNode.x, pointerNode.y, 0);
                // If the pointerNode is on an impassable tile, skip to the next direction
                if (tileManager.CheckTile(pointerNodeVector3) == "rock")
                {
                    continue;
                }
                // If the pointerNode is already in the nodes list, skip to the next direction
                if (inNodes(pointerNode))
                {
                    continue;
                }
                // Add the pointerNode to the nodes list
                nodes.Add(pointerNode);
                // Open the newly created node
                state.Add(true);
                state[nodeLocation(pointerNode)] = true;
                // Set the parent of the newly created node to the currentNode
                parentNodes.Add(new Vector2Int(0,0));
                parentNodes[nodeLocation(pointerNode)] = currentNode;

            }
        }
    }



    // Return a Vector2Int for the first step in the path between start and target
    Vector2Int firstStep(Vector2Int givenNode)
    {
        // If the givenNode's parent is startNode, return the distance between them
        if (parentNodes[nodeLocation(givenNode)] == startNode)
        {
            // Debug.Log(givenNode - startNode);
            return givenNode - startNode;
        }

        // Run the method for each parent in the chain until the startNode is found
        return firstStep(parentNodes[nodeLocation(givenNode)]);
    }



    // Mark the path from start to target
    void createPath(Vector2Int givenNode)
    {
        // If the pointer is at the start of the path, stop the script
        if (givenNode == startNode)
        {
            return;
        }
        // Retreat to the last node in the path
        createPath(parentNodes[nodeLocation(givenNode)]);
    }
}