using Unity.VisualScripting;
using UnityEngine;

public class Store : MonoBehaviour
{

	[SerializeField] private CatalogsCollection m_catalogsCollection;
	[SerializeField] private ObjectPool<StoreProductUI> m_categoryPool;
	[SerializeField] private Builder m_builder;
	[SerializeField] private Level _level;

	public Level level => _level;

	private StoreProduct _selectedProduct;
	private CellSelectRequest _cellSelectRequest;
	private GhostObject _ghostObject;

	private void Start()
	{
		UpdateCategory();
		_cellSelectRequest = new CellSelectRequest(OnCellSelect, CancelPurchase);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			AcceptPurchase();
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			CancelPurchase();
		}
	}

	public void UpdateCategory()
	{
		m_categoryPool.HideEverything();

		StoreCatalog catalog = m_catalogsCollection.catalogs[0];

		foreach (var item in catalog.products)
		{
			StoreProductUI field = m_categoryPool.Get();
			StoreProduct product = item;

			field.Set(product, () => ProductClicked(field));
			field.gameObject.SetActive(true);
		}
	}

	public void AcceptPurchase()
	{
		if (_ghostObject == null)
			return;

		m_builder.SpawnObject(_ghostObject.GetCell(), _selectedProduct.Prefab.GetComponent<BaseObject>());
		Destroy(_ghostObject.gameObject);
		UnselectProduct();
	}

	public void CancelPurchase()
	{
		if (_ghostObject != null)
			Destroy(_ghostObject.gameObject);
		_ghostObject = null;
		UnselectProduct();
	}

	private BaseObject OnCellSelect(Cell cell)
	{
		if (_selectedProduct == null)
			return null;
		if (cell.Color != level.player.ControlTeam.Color)
			return null;

		_ghostObject = m_builder.SpawnGhostObject(cell, _selectedProduct.Prefab);

		return _ghostObject;
	}

	private void ProductClicked(StoreProductUI productUI)
	{
		if (_ghostObject != null)
			return;

		if (_selectedProduct == productUI.Product)
		{
			productUI.interactable = true;
			UnselectProduct();
		}
		else
		{
			productUI.interactable = false;
			SelectProduct(productUI.Product);
		}
	}

	private void SelectProduct(StoreProduct product)
	{
		_selectedProduct = product;
		_level.player.SetCellSelectRequest(_cellSelectRequest);
	}

	private void UnselectProduct()
	{
		_selectedProduct = null;
		_level.player.RemoveCellSelectRequest(_cellSelectRequest);
	}

}
