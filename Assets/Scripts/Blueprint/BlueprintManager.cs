using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    public static BlueprintManager instance { get; private set; }

    [Header("Grid")]
    private int gridWidth;
    private int gridHeight;
    private float cellSize;
    private Vector2 gridOrigin;
    private ComponentData[,] grid;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        grid = new ComponentData[gridWidth, gridHeight];
    }
}
