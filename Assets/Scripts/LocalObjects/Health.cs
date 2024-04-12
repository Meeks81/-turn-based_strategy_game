using System;
using UnityEngine;

public class Health : MonoBehaviour
{

	[SerializeField] private int _value;

	public int Value => _value;
	public int Max { get; private set; }

	public Action<int> OnDamage;
	public Action OnDeath;

	private void Start()
	{
		Max = _value;
	}

	public void Damage(int value)
	{
		_value -= value;
		OnDamage?.Invoke(value);
		if (Value <= 0)
		{
			_value = 0;
			OnDeath?.Invoke();
			Destroy(gameObject);
		}
	}

	public void Heal(int value)
	{
		_value += value;
		if (Value > Max)
			_value = Max;
	}

}
