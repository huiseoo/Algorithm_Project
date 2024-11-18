using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public class Node
    {
        public Vector2Int Position;   // 노드 위치
        public float G;              // 시작점에서 현재 노드까지의 비용
        public float H;              // 휴리스틱 (목표까지의 추정 비용)
        public float F => G + H;     // 총 비용
        public Node Parent;          // 이전 노드 (경로 추적용)

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
        // Open list (탐색해야 할 노드)
        List<Node> openList = new List<Node>();
        // Closed list (탐색 완료된 노드)
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();

        // 시작 노드 생성
        Node startNode = new Node(start);
        Node goalNode = new Node(goal);
        openList.Add(startNode);

        // A* 알고리즘 실행
        while (openList.Count > 0)
        {
            // f(n)이 가장 낮은 노드를 선택
            Node currentNode = openList[0];
            foreach (var node in openList)
            {
                if (node.F < currentNode.F)
                {
                    currentNode = node;
                }
            }

            // 목표에 도달하면 경로를 반환
            if (currentNode.Position == goal)
            {
                return ReconstructPath(currentNode);
            }

            // Open list에서 제거하고 Closed list에 추가
            openList.Remove(currentNode);
            closedList.Add(currentNode.Position);

            // 현재 노드의 이웃 탐색
            foreach (var neighborPos in GetNeighbors(currentNode.Position))
            {
                if (closedList.Contains(neighborPos) || obstacles.Contains(neighborPos))
                {
                    continue; // 이미 탐색했거나 장애물인 경우 무시
                }

                Node neighborNode = new Node(neighborPos);
                neighborNode.G = currentNode.G + 1; // g(n): 이전 노드까지의 비용 + 이동 비용
                neighborNode.H = GetHeuristic(neighborPos, goal); // h(n): 휴리스틱
                neighborNode.Parent = currentNode;

                // Open list에 없는 경우 추가
                if (!openList.Exists(node => node.Position == neighborPos))
                {
                    openList.Add(neighborNode);
                }
            }
        }

        // 경로를 찾지 못한 경우 빈 리스트 반환
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
        // 4방향 이동
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
        // 맨해튼 거리 계산
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
