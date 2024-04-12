using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CellSelector : MonoBehaviour
{

	public static event UnityAction<Cell> OnCellSelected;

	private Vector3 _downMousePosition;
	private Vector3 _lastMousePosition;
	private Cell _clickedCell;
	private IMovableObject _movableObject;
	private bool _isMouseDown;
	private Camera _mainCamera;

	private void Start()
	{
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
		{
			_downMousePosition = GetRayPosition();
			_lastMousePosition = _downMousePosition;
			_clickedCell = GetCelSelecteablelByScreenPosition(Input.mousePosition).GetCell();
			_isMouseDown = true;
		}
		if (Input.GetMouseButton(0) && _isMouseDown)
		{
			Vector3 mousePosition = GetRayPosition();
			Vector3 delta = mousePosition - _downMousePosition;
			float distance = delta.magnitude;
			if (distance > 0.5f && _movableObject == null)
			{
				if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit) &&
					hit.transform != null &&
					hit.transform.TryGetComponent(out IMovableObject obj))
				{
					_movableObject = obj;
					_movableObject.Take();
				}
				else
				{
					_isMouseDown = false;
				}
			}
			if (_movableObject != null)
			{
				Vector3 moveDelta = mousePosition - _lastMousePosition;
				_lastMousePosition = mousePosition;
				_movableObject.Move(moveDelta);
			}
		}
		if (Input.GetMouseButtonUp(0) && _isMouseDown)
		{
			if (_movableObject != null)
			{
				_movableObject.Put(GetCellByScreenPosition(Input.mousePosition), GetRayPosition());
				_movableObject = null;
			}
			Vector3 mousePosition = GetRayPosition();
			Vector3 delta = mousePosition - _downMousePosition;
			float distance = delta.magnitude;
			if (distance < 0.5f)
			{
				ClickCell(_clickedCell);
			}
		}
	}

	public void Click(Vector2 clickScreenPosition)
	{
		ICellSelectable cellSelectable = GetCelSelecteablelByScreenPosition(clickScreenPosition);
		if (cellSelectable != null)
			OnCellSelected?.Invoke(cellSelectable.GetCell());
	}

	private void ClickCell(Cell cell)
	{
		OnCellSelected?.Invoke(cell);
	}

	private ICellSelectable GetCelSelecteablelByScreenPosition(Vector2 screenPosition)
	{
		if (Physics.Raycast(_mainCamera.ScreenPointToRay(screenPosition), out RaycastHit hit) &&
				hit.transform != null &&
				hit.transform.TryGetComponent(out ICellSelectable obj))
		{
			return obj;
		}
		return null;
	}

	private Cell GetCellByScreenPosition(Vector2 screenPosition)
	{
		if (Physics.Raycast(_mainCamera.ScreenPointToRay(screenPosition), out RaycastHit hit, 100f, LayerMask.GetMask("Cell")) &&
				hit.transform != null &&
				hit.transform.TryGetComponent(out Cell obj))
		{
			return obj;
		}
		return null;
	}

	private Vector3 GetRayPosition()
	{
		Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
		Plane hPlane = new Plane(Vector3.up, Vector3.zero);
		if (hPlane.Raycast(ray, out float distance))
		{
			return ray.GetPoint(distance);
		}
		return Vector3.zero;
	}

}
