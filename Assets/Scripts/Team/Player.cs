using UnityEngine;

public class Player : MonoBehaviour
{

	[SerializeField] private Level _level;

	public Team ControlTeam { get; private set; }

	private BaseObject _selectedObject;
	private CellSelectRequest _cellSelectRequest;

	public Level level => _level;

	public void Initialize(Team controlTeam)
	{
		CellSelector.OnCellSelected += OnCellSelected;
		ControlTeam = level.gameMoves.CurrentTeamMove;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			FinishMove();
		if (Input.GetKeyDown(KeyCode.C))
			ClearSelectedObject();
	}

	public void SetControlTeam(Team team)
	{
		ControlTeam = team;
	}

	public void FinishMove()
	{
		level.gameMoves.FinishMove();
		ControlTeam = level.gameMoves.CurrentTeamMove;
	}

	public void ClearSelectedObject()
	{
		foreach (var item in level.GetAllCells())
		{
			item.SetActiveColor(true);
		}
		if (_selectedObject != null)
		{
			_selectedObject.Unselect();
			_selectedObject = null;
		}
	}

	public void SetCellSelectRequest(CellSelectRequest request)
	{
		if (request == null)
			return;

		if (_cellSelectRequest != null)
		{
			_cellSelectRequest.Cancel();
			_cellSelectRequest = null;
		}
		_cellSelectRequest = request;
		ClearSelectedObject();
	}

	public void RemoveCellSelectRequest(CellSelectRequest request)
	{
		if (_cellSelectRequest == request)
			_cellSelectRequest = null;
	}

	private void OnCellSelected(Cell cell)
	{
		if (cell == null)
			return;

		if (_cellSelectRequest != null)
		{
			_selectedObject = _cellSelectRequest.SelectCell(cell);
			if (_selectedObject != null)
			{
				_cellSelectRequest = null;
			}
		}
		else if (_selectedObject != null)
		{
			_selectedObject.SelectCell(cell);
		}
		else if (cell.PlacedObject != null)
		{
			_selectedObject = cell.PlacedObject;
			_selectedObject.Select();
		}
		else
		{
			ClearSelectedObject();
		}
	}

}
