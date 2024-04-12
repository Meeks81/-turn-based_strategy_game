using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cell : MonoBehaviour, ICellSelectable
{

    public const float WIDTH = 1.732f;
    public const float HEIGHT = 1.5f;

    [SerializeField] private Transform _placedObjectPoint;

    public Vector2Int MapPosition;
    public TeamColor Color
    {
        get => _color;
        set
        {
            _color = value;
            UpdateMeshColor();
        }
    }
    public BaseObject PlacedObject { get; private set; }
    public Vector3 PlacedObjectPoint => _placedObjectPoint.position;

	private TeamColor _color;
    private bool _isActive = true;

    private MeshRenderer _meshRenderer;
    private Level _level;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _level = FindFirstObjectByType<Level>();
    }

    public Cell GetCell() => this;

    public void UpdateMeshColor()
    {
        _meshRenderer.material.color = Team.GetColor(_color) * (_isActive ? 1f : 0.6f);
    }

    public void SetObject(BaseObject obj)
    {
        PlacedObject = obj;
    }

    public void SetActiveColor(bool value)
    {
        _isActive = value;
        UpdateMeshColor();
    }

    public void SetPosition(Vector2Int mapPosition)
    {
        MapPosition = mapPosition;
        transform.position = MapToWorldPosition(mapPosition);
    }

    // static methods
    public static Vector3 MapToWorldPosition(Vector2Int mapPosition)
    {
        return new Vector3(WIDTH * mapPosition.x - (Mathf.Abs(mapPosition.y) % 2 == 1 ? WIDTH / 2 : 0), 0, HEIGHT * mapPosition.y);
    }

    public List<Cell> GetArroundCells() => GetCellsInRadius(1);

    public List<Cell> GetCellsInRadius(int radius)
    {
        if (radius <= 0)
            return new List<Cell>();

        List<Cell> cellsInRadius = new List<Cell>();

        List<Cell> cellsRow = new List<Cell>() { this };
        for (int i = 0; i < radius; i++)
        {
            List<Cell> aroundCells = new List<Cell>();

            foreach (var item in cellsRow)
            {
                Vector2Int[] poses = GetPositionsAround(item.MapPosition);
                foreach (var pos in poses)
                {
                    Cell c = _level.GetCell(item.MapPosition + pos);

                    if (c == null || c == this || cellsInRadius.Contains(c))
                        continue;

                    aroundCells.Add(c);
                }
            }

            cellsRow = aroundCells;
            cellsInRadius.AddRange(aroundCells);
        }

        return cellsInRadius;
    }

    public static Vector2Int[] GetPositionsAround(Vector2Int cell)
    {
        int space = 0;
        if (cell.y % 2 != 0)
            space = 1;
        Vector2Int[] poses = new Vector2Int[]
        {
            new Vector2Int(1 - space,1),
            new Vector2Int(1,0),//
            new Vector2Int(1 - space,-1),
            new Vector2Int(0 - space,-1),//
            new Vector2Int(-1,0),//
            new Vector2Int(0 - space,1),//
        };
        return poses;
    }
}