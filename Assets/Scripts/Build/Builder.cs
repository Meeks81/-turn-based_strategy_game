using UnityEngine;

public class Builder : MonoBehaviour
{

	[SerializeField] private Transform m_buildingsContainer;
	[SerializeField] private Transform m_unitsContainer;

	public bool SpawnObject(Cell cell, BaseObject prefab)
	{
		if (prefab == null)
			throw new System.Exception("Prefab is NULL");
		if (cell.PlacedObject != null)
			return false;

		BaseObject spawnedObj = Instantiate(prefab);

		spawnedObj.SetCell(cell);
		cell.SetObject(spawnedObj);

		spawnedObj.transform.position = cell.PlacedObjectPoint;

		return true;
	}

	public GhostObject SpawnGhostObject(Cell cell, GameObject prefab)
	{
		if (prefab == null)
			throw new System.Exception("Prefab is NULL");
		if (cell.PlacedObject != null)
			return null;

		GameObject spawnedObj = Instantiate(prefab);

		if (spawnedObj.TryGetComponent(out BaseObject baseObject))
		{
			Destroy(baseObject);
		}

		GhostObject ghostObject = spawnedObj.AddComponent<GhostObject>();
		ghostObject.SetCell(cell);

		return ghostObject;
	}

}
