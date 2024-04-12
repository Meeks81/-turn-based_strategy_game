using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatalogsCollection", menuName = "ScriptableObjects/CatalogsCollection")]
public class CatalogsCollection : ScriptableObject
{

    [SerializeField] private List<StoreCatalog> m_catalogs;

    public List<StoreCatalog> catalogs => m_catalogs;

}
