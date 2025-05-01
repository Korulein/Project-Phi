using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    public static ProductManager instance { get; private set; }

    private List<int> requiredComponentIDs;

    [Header("Products")]
    [SerializeField] public List<ProductData> products = new List<ProductData>();
    private void Awake()
    {
        // instance declaration
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // generate product IDs
        for (int i = 0; i < products.Count; i++)
        {
            products[i].productID = i;
        }
    }
    // returns product ID
    public ProductData GetProductByID(int id)
    {
        return products.FirstOrDefault(product => product.productID == id);
    }
    public void EvaluateProduct()
    {

    }
}
