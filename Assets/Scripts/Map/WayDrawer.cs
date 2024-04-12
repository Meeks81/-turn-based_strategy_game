using System.Collections.Generic;
using UnityEngine;

public class WayDrawer : MonoBehaviour
{

	public bool IsActive => currentWay != null;

	public List<Cell> currentWay { get; private set; }

	public Cell GetLastCell()
	{
		if (currentWay == null || currentWay.Count == 0)
			return null;
		return currentWay[currentWay.Count - 1];
	}

	public void Draw(List<Cell> way)
	{
		currentWay = way;
	}

	public void Clear()
	{
		currentWay = null;
	}

	private void OnDrawGizmos()
	{
		if (currentWay == null)
			return;

		Gizmos.color = Color.green;
		for (int i = 0; i < currentWay.Count; i++)
		{
			Gizmos.DrawSphere(currentWay[i].PlacedObjectPoint, 0.5f);
			if (i > 0)
			{
				Gizmos.DrawLine(currentWay[i - 1].PlacedObjectPoint, currentWay[i].PlacedObjectPoint);
			}
		}
	}

}
