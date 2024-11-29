using System.Collections.Generic;
using UnityEngine;


/// A* 알고리즘을 사용한 경로찾기 시스템

public class Pathfinding : MonoBehaviour
{
    /// <summary>
    /// 경로찾기에 사용되는 각 격자점을 표현하는 노드 클래스
    /// </summary>
    public class Node
    {
        public Vector2Int Position;   // 노드의 그리드 상 위치
        public float G;              // g(n): 시작점에서 현재 노드까지의 실제 이동 비용
        public float H;              // h(n): 현재 노드에서 목표까지의 추정 비용 
        public float F => G + H;     // f(n): 총 예상 비용 (g(n) + h(n))
        public Node Parent;          // 경로 재구성을 위한 이전 노드 참조

        public Node(Vector2Int position)
        {
            Position = position;
            G = 0;
            H = 0;
            Parent = null;
        }
    }

  
    /// 시작점에서 목표점까지의 최단 경로를 찾는 A* 알고리즘
    /// <param name="start">시작 위치</param>
    /// <param name="goal">목표 위치</param>
    /// <param name="obstacles">장애물이 있는 위치들의 집합</param>
    /// <returns>찾은 경로의 위치 목록 (경로가 없으면 빈 리스트)</returns>
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal, HashSet<Vector2Int> obstacles)
    {
        List<Node> openList = new List<Node>();        // 탐색할 노드들의 목록
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();  // 이미 탐색한 노드들의 집합

        // 시작 노드 초기화 및 openList에 추가
        Node startNode = new Node(start);
        Node goalNode = new Node(goal);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // openList에서 f(n)이 가장 작은 노드를 현재 노드로 선택
            Node currentNode = openList[0];
            foreach (var node in openList)
            {
                if (node.F < currentNode.F)
                {
                    currentNode = node;
                }
            }

            // 목표에 도달했다면 경로를 재구성하여 반환
            if (currentNode.Position == goal)
            {
                return ReconstructPath(currentNode);
            }

            // 현재 노드를 openList에서 제거하고 closedList에 추가
            openList.Remove(currentNode);
            closedList.Add(currentNode.Position);

            // 현재 노드의 모든 이웃 노드들을 검사
            foreach (var neighborPos in GetNeighbors(currentNode.Position))
            {
                // 이미 탐색했거나 장애물인 위치는 건너뜀
                if (closedList.Contains(neighborPos) || obstacles.Contains(neighborPos))
                {
                    continue;
                }

                // 이웃 노드의 비용을 계산하고 openList에 추가
                Node neighborNode = new Node(neighborPos);
                neighborNode.G = currentNode.G + 1;  // 모든 이동의 비용을 1로 가정
                neighborNode.H = GetHeuristic(neighborPos, goal);
                neighborNode.Parent = currentNode;

                // 아직 탐색하지 않은 노드라면 openList에 추가
                if (!openList.Exists(node => node.Position == neighborPos))
                {
                    openList.Add(neighborNode);
                }
            }
        }

        // 경로를 찾지 못한 경우 빈 리스트 반환
        return new List<Vector2Int>();
    }

    /// <summary>
    /// 최종 노드부터 시작 노드까지 거슬러 올라가며 경로를 재구성
    /// </summary>
    private List<Vector2Int> ReconstructPath(Node node)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (node != null)
        {
            path.Add(node.Position);
            node = node.Parent;
        }
        path.Reverse();  // 시작점부터 목표점 순서로 경로 재정렬
        return path;
    }

    /// <summary>
    /// 주어진 위치에서 이동 가능한 상하좌우 네 방향의 위치를 반환
    /// </summary>
    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        return new List<Vector2Int>
        {
            position + Vector2Int.up,    // 위
            position + Vector2Int.down,  // 아래
            position + Vector2Int.left,  // 왼쪽
            position + Vector2Int.right  // 오른쪽
        };
    }

    /// <summary>
    /// 두 위치 사이의 맨해튼 거리를 계산 (휴리스틱 함수)
    /// </summary>
    private float GetHeuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}
