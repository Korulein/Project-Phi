using UnityEngine;

public class PartGenerator : MonoBehaviour
{
    public GameObject spritePrefab;

    void OnMouseDown()
    {
        Instantiate(spritePrefab, transform.position, transform.rotation);
    }
}
