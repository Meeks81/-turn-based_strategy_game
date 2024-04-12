using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class BaseObject : MonoBehaviour, ICellSelectable
{

	[SerializeField] private int _coinsPerMove;

	public Health health
	{
		get
		{
			if (_health == null)
				_health = GetComponent<Health>();
			return _health;
		}
	}
	private Health _health;

	public int coinsPerMove => _coinsPerMove;

	public abstract void SetCell(Cell cell);
	public abstract Cell GetCell();
	public abstract TeamColor GetTeamColor();

	public abstract void Select();
	public abstract void Unselect();

	public abstract void SelectCell(Cell cell);

}
