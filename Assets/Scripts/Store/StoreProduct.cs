using UnityEngine;

[System.Serializable]
public class StoreProduct
{

    [SerializeField] private GameObject m_prefab;
    [Space]
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private float m_price;

    public GameObject Prefab => m_prefab;
    public Sprite Sprite => m_sprite;
    public float Price => m_price;

}
