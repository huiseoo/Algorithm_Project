using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public class Node
    {
        public Vector2Int Position;   // ��� ��ġ
        public float G;              // ���������� ���� �������� ���
        public float H;              // �޸���ƽ (��ǥ������ ���� ���)
        public float F => G + H;     // �� ���
        public Node Parent;          // ���� ��� (��� ������)

        public Node(Vector2Int position)
        {
            Position = position;
            G = 0;
            H = 0;
            Parent = null;
        }
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal, HashSet<Vector2Int> obstacles)
    {
        // Open list (Ž���ؾ� �� ���)
        List<Node> openList = new List<Node>();
        // Closed list (Ž�� �Ϸ�� ���)
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();

        // ���� ��� ����
        Node startNode = new Node(start);
        Node goalNode = new Node(goal);
        openList.Add(startNode);

        // A* �˰��� ����
        while (openList.Count > 0)
        {
            // f(n)�� ���� ���� ��带 ����
            Node currentNode = openList[0];
            foreach (var node in openList)
            {
                if (node.F < currentNode.F)
                {
                    currentNode = node;
                }
            }

            // ��ǥ�� �����ϸ� ��θ� ��ȯ
            if (currentNode.Position == goal)
            {
                return ReconstructPath(currentNode);
            }

            // Open list���� �����ϰ� Closed list�� �߰�
            openList.Remove(currentNode);
            closedList.Add(currentNode.Position);

            // ���� ����� �̿� Ž��
            foreach (var neighborPos in GetNeighbors(currentNode.Position))
            {
                if (closedList.Contains(neighborPos) || obstacles.Contains(neighborPos))
                {
                    continue; // �̹� Ž���߰ų� ��ֹ��� ��� ����
                }

                Node neighborNode = new Node(neighborPos);
                neighborNode.G = currentNode.G + 1; // g(n): ���� �������� ��� + �̵� ���
                neighborNode.H = GetHeuristic(neighborPos, goal); // h(n): �޸���ƽ
                neighborNode.Parent = currentNode;

                // Open list�� ���� ��� �߰�
                if (!openList.Exists(node => node.Position == neighborPos))
                {
                    openList.Add(neighborNode);
                }
            }
        }

        // ��θ� ã�� ���� ��� �� ����Ʈ ��ȯ
        return new List<Vector2Int>();
    }

    private List<Vector2Int> ReconstructPath(Node node)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (node != null)
        {
            path.Add(node.Position);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        // 4���� �̵�
        return new List<Vector2Int>
        {
            position + Vector2Int.up,
            position + Vector2Int.down,
            position + Vector2Int.left,
            position + Vector2Int.right
        };
    }

    private float GetHeuristic(Vector2Int a, Vector2Int b)
    {
        // ����ư �Ÿ� ���
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
