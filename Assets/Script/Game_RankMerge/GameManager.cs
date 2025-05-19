using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gridWidth = 7;
    public int GridHeight = 7;
    public float cellSize = 1.4f;
    public GameObject cellPrefabs;
    public Transform gridContainer;

    public GameObject rankPrefabs;
    public Sprite[] rankSprites;
    public int maxRankLevel = 7;

    public GridCell[,] grid;

    void initializeGrid()
    {
        grid = new GridCell[gridWidth, GridHeight];

        for(int x =0; x<gridWidth; x++)
        {
            for(int y=0; y<GridHeight; y++)
            {
                Vector3 position = new Vector3(
                    x * cellSize - (gridWidth * cellSize / 2) + cellSize / 2,
                    y * cellSize - (GridHeight * cellSize / 2) + cellSize / 2,
                    1f
                    );

                GameObject cellObj = Instantiate(cellPrefabs, position, Quaternion.identity, gridContainer);
                GridCell cell = cellObj.AddComponent<GridCell>();
                cell.initialize(x, y);

                grid[x, y] = cell;


            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        initializeGrid();  
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            SpawnNewRank();
        }
    }

    public DraggableRank CreateRankingCell(GridCell cell, int level)
    {
        if (cell == null || !cell.isEmpty()) return null;

        level = Mathf.Clamp(level, 1, maxRankLevel);

        Vector3 rankPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0f);

        GameObject rankObj = Instantiate(rankPrefabs, rankPosition, Quaternion.identity, gridContainer);
        rankObj.name = "Rank_Level" + level;

        DraggableRank rank = rankObj.AddComponent<DraggableRank>();
        rank.SetRankLevel(level);

        cell.SetRank(rank);

        return rank;
    }

    private GridCell FindEmptyCell()
    {
        List<GridCell> emptyCells = new List<GridCell>();

        for(int x = 0; x< gridWidth; x++)
        {
            for (int y= 0; y < GridHeight; y++)
            {
                if (grid[x,y].isEmpty())
                {
                    emptyCells.Add(grid[x, y]);
                }
            }
        }
        
        if(emptyCells.Count == 0)
        {
            return null;
        }

        return emptyCells[Random.Range(0, emptyCells.Count)];
    }
    public bool SpawnNewRank()      //새 계급장 생성
    {
        GridCell emptyCell = FindEmptyCell();       //1. 비어있는 칸 찾기
        if (emptyCell == null) return false;         //2. 비어있는 칸이 없으면 실패

        int rankLevel = Random.Range(0, 100) < 80 ? 1 : 2;  //80% 확률로 레벨 1, 20%확률로 레벨 2

        CreateRankingCell(emptyCell, rankLevel);     //3. 계급장 생성 및 설정

        return true;
    }

    
    public GridCell FindClosestCell(Vector3 position)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                if (grid[x,y].ContainsPosition(position))
                {
                    return grid[x, y];
                }
            }
        }

        GridCell closestCell = null;
        float closestDistance = float.MaxValue;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
               float distance = Vector3.Distance(position, grid[x, y].transform.position);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCell = grid[x, y];
                }
            }
        }

        if(closestDistance > cellSize * 2)
        {
            return null;
        }

        return closestCell;

    }

    public void MergeRanks(DraggableRank draggedRank , DraggableRank targetRank)
    {
        if(draggedRank == null || targetRank.RankLevel != targetRank.RankLevel)
        {
            if (draggedRank != null) draggedRank.ReturnToOriginalPosition();
            return;
        }

        int newLevel = targetRank.RankLevel + 1;
        if(newLevel>maxRankLevel)
        {
            RemoveRank(draggedRank);
            return;
        }

        targetRank.SetRankLevel(newLevel);
        RemoveRank(draggedRank);

        if(Random.Range(0,100) < 60)
        {
            SpawnNewRank();
        }
    }

    public void RemoveRank(DraggableRank rank)
    {
        if (rank == null) return;

        if (rank.currentCell != null)
        {
            rank.currentCell.currentRank = null;
        }
            Destroy(rank.gameObject);
        
    }


}

