using System.Collections.Generic;
using UnityEngine;

public class GhostObject : BaseObject, IMovableObject
{

    private List<SaveMaterial> saveMaterials;
    private Cell _currentCell;

    private void OnEnable()
    {
        Shader shader = Shader.Find("Shader Graphs/Ghost");
        saveMaterials = new List<SaveMaterial>();

        Renderer[] mehes = GetComponentsInChildren<Renderer>();
        foreach (var renderer in mehes)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                saveMaterials.Add(new SaveMaterial(renderer.materials[i]));
                renderer.materials[i].shader = shader;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var item in saveMaterials)
            item.BackShader();
        saveMaterials = null;
    }

	public override void SetCell(Cell cell)
	{
        _currentCell = cell;
        transform.position = cell.PlacedObjectPoint;
	}

    public override Cell GetCell() => _currentCell;

    public override TeamColor GetTeamColor() => TeamColor.none;

	public override void Select()
	{

	}

	public override void Unselect()
	{

	}

	public override void SelectCell(Cell cell)
	{

	}

	public void Take()
	{
		
	}

	public void Move(Vector3 delta)
	{
        if (delta == Vector3.zero)
            return;

		transform.position += delta;

		if (Physics.Raycast(transform.position + Vector3.up * 100f, Vector3.down, out RaycastHit hit, 1000f, LayerMask.GetMask("Cell")))
		{
			transform.position = hit.point + Vector3.up * 0.5f;
		}
    }

	public void Put(Cell cell, Vector3 poition)
	{
        if (cell.Color != _currentCell.Color || cell.PlacedObject != null)
            cell = _currentCell;
        SetCell(cell);
	}

	private class SaveMaterial
    {
        public Material material;
        public Shader shader;

        public SaveMaterial(Material material) :
            this(material, material.shader)
        {

        }

        public SaveMaterial(Material material, Shader shader)
        {
            this.material = material;
            this.shader = shader;
        }

        public void BackShader()
        {
            material.shader = shader;
        }
    }

}
