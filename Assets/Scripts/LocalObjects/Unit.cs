using System.Collections.Generic;
using UnityEngine;

public class Unit : BaseObject
{

	[SerializeField] private int _damage;
	[SerializeField] private int _wayLength;
	[SerializeField] private float _moveSpeed;
	[SerializeField] private WayDrawer _wayDrawer;

	public int Damage => _damage;
	public int WayLength => _wayLength;
	public float MoveSpeed => _moveSpeed;

	private List<Cell> _currentWay = new List<Cell>();

	private Cell _currentCell;
	private Level _level;

	private void Start()
	{
		_level = FindFirstObjectByType<Level>();
	}

	private void Update()
	{
		Move();
	}

	public override void SetCell(Cell cell)
	{
		_currentCell = cell;
	}
	public override Cell GetCell() => _currentCell;
	public override TeamColor GetTeamColor() => _currentCell.Color;

	public override void Select()
	{
		if (_level.player.ControlTeam.Color != GetTeamColor())
			return;

		foreach (var item in _level.GetAllCells())
		{
			item.SetActiveColor(false);
		}
		List<Cell> availableCells = GetAvaliableCells();
		foreach (var item in availableCells)
		{
			item.SetActiveColor(true);
		}
	}

	public override void Unselect()
	{
		foreach (var item in _level.GetAllCells())
		{
			item.SetActiveColor(true);
		}
		_wayDrawer.Clear();
	}

	public override void SelectCell(Cell cell)
	{
		if (_level.player.ControlTeam.Color != GetTeamColor())
		{
			Unselect();
			return;
		}

		if (_currentWay.Count > 0)
			return;

		if (_wayDrawer.IsActive && _wayDrawer.GetLastCell() == cell)
		{
			GoTo(cell);
			_wayDrawer.Clear();
		}
		else
			ShowWay(cell);
	}

	public void GoTo(Cell cell)
	{
		if (_currentWay != null && _currentWay.Count > 0)
			return;

		_currentWay = FindWay(cell);
	}

	public List<Cell> FindWay(Cell targetCell)
	{
		if (targetCell == _currentCell)
			return new List<Cell>();

		List<Cell> way = new List<Cell>();
		List<Cell> blockWays = new List<Cell>();
		int tries = 3;
		for (int t = 0; t < tries; t++)
		{
			List<Cell> exceptionCells = new List<Cell>() { _currentCell };
			Cell selectedCell = _currentCell;
			for (int i = 0; i < _wayLength; i++)
			{
				float dist = float.MaxValue;
				Cell closeCell = null;
				List<Cell> aroundCells = selectedCell.GetArroundCells();
				foreach (var item in aroundCells)
				{
					if (exceptionCells.Contains(item) ||
						blockWays.Contains(item) ||
						item == null ||
						item == _currentCell ||
						(item.Color == GetTeamColor() && item.PlacedObject != null))
						continue;
					if (item.Color != GetTeamColor() && item != targetCell)
						continue;
					float d = Vector3.Distance(item.transform.position, targetCell.transform.position);
					if (d < dist)
					{
						dist = d;
						closeCell = item;
					}
				}

				exceptionCells.AddRange(aroundCells);
				way.Add(closeCell);
				selectedCell = closeCell;

				if (closeCell == null || closeCell == targetCell)
					break;
			}

			if (way[way.Count - 1] == targetCell)
				break;

			blockWays.Add(way[0]);
			way.Clear();
		}
		return way;
	}

	public List<Cell> GetAvaliableCells()
	{
		List<Cell> avaliableCells = new List<Cell>();

		List<Cell> cellsRow = new List<Cell>() { _currentCell };
		for (int i = 0; i < _wayLength; i++)
		{
			List<Cell> aroundCells = new List<Cell>();

			foreach (var item in cellsRow)
			{
				//Vector2Int[] poses = Cell.GetPositionsAround(item.MapPosition);
				List<Cell> checkCells = item.GetArroundCells();
				foreach (var c in checkCells)
				{
					//Cell c = Map.Instance.GetCell(item.MapPosition + pos);

					if (avaliableCells.Contains(c) || IsAvaliableCell(c) == false)
						continue;

					aroundCells.Add(c);
				}
			}

			cellsRow = aroundCells;
			avaliableCells.AddRange(aroundCells);
		}

		return avaliableCells;
	}

	private void Move()
	{
		if (_currentWay.Count == 0)
			return;

		Cell cell = _currentWay[0];

		if (cell.PlacedObject != null)
		{
			cell.PlacedObject.health.Damage(_damage);
			_currentWay.Clear();
			return;
		}

		if (cell.Color != GetTeamColor())
		{
			cell.Color = GetTeamColor();
		}

		Vector3 targetPosition = cell.PlacedObjectPoint;
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
		if (transform.position == targetPosition)
		{
			_currentWay.RemoveAt(0);

			if (_currentWay.Count == 0 || _currentWay[0].PlacedObject != null)
			{
				_currentCell.SetObject(null);
				_currentCell = cell;
				cell.SetObject(this);
				Select();
			}
		}
	}

	private void ShowWay(Cell targetCell)
	{
		List<Cell> way = FindWay(targetCell);

		_wayDrawer.Draw(way);
	}

	private bool IsAvaliableCell(Cell c)
	{
		if (c == null || c == _currentCell || (c.Color == GetTeamColor() && c.PlacedObject != null))
			return false;

		if (c.Color != GetTeamColor())
		{
			List<Cell> arCells = c.GetArroundCells();
			if (arCells.Find(e => e.Color == _currentCell.Color) == null)
				return false;
		}

		return true;
	}
}
