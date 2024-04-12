using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreCatalog
{

    [SerializeField] private List<StoreProduct> m_products;

    public List<StoreProduct> products => m_products;

}
