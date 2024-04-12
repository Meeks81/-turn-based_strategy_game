using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    [SerializeField] private Player _player;
    [SerializeField] private GameMoves _gameMoves;
    [Space]
    [SerializeField] private Cell m_cellPrefab;
    [SerializeField] private Transform m_cellsContainer;

    public Map map { get; private set; }
    public Player player => _player;
    public GameMoves gameMoves => _gameMoves;

	private Cell[,] _cells;
    private List<Cell> _cellsList;

    public Cell GetCell(Vector2Int mapPosition) => GetCell(mapPosition.x, mapPosition.y);
    public Cell GetCell(int x, int y)
    {
        if (x < 0 ||
            y < 0 ||
            x >= _cells.GetLength(0) ||
            y >= _cells.GetLength(1))
            return null;

        return _cells[x, y];
    }
    public List<Cell> GetAllCells() => new List<Cell>(_cellsList);

    public void SpawnMap(Map map)
    {
        this.map = map;

        Vector2Int mapSize = map.Size;
        _cells = new Cell[mapSize.x, mapSize.y];
        _cellsList = new List<Cell>(mapSize.x * mapSize.y);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (map.Cells[x, y].height <= 0)
                    continue;

                Cell spawnedCell = SpawnCell(new Vector2Int(x, y), map.Cells[x, y]);
                _cells[x, y] = spawnedCell;
                _cellsList.Add(spawnedCell);
            }
        }

        Debug.Log($"Seed {map.Seed} spawned!");
    }

    private Cell SpawnCell(Vector2Int mapPosition, CellData data)
    {
        Cell cell = Instantiate(m_cellPrefab, m_cellsContainer);
        cell.SetPosition(mapPosition);
        cell.Color = data.color;
        cell.name = $"Cell {mapPosition.x}:{mapPosition.y}";

        return cell;
    }

}
