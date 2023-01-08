using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Vector2 grid;
    [SerializeField] private Vector2 additionlGrid;
    [SerializeField] private GameObject tile;
    public static Map instance;
    [SerializeField] private Vector2 tileSize;
    public Vector2 mapSize { get { return grid * tileSize; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GenerateMap();
        print(mapSize);
    }

    private float CenterPoint(float count, float size)
    {
        return (count * size / 2) - size / 2;
    }

    private void GenerateMap()
    {
        var fullGrid = grid + additionlGrid;

        var center = new Vector2(CenterPoint(fullGrid.x, tileSize.x), CenterPoint(fullGrid.y, tileSize.y));
        transform.position = -center;

        for(var i = 0; i < fullGrid.y; i++)
        {
            for(var j = 0; j < fullGrid.x; j++)
            {
                var cell = Instantiate(tile, transform);
                cell.transform.localScale = tileSize;
                cell.transform.localPosition = new Vector3(j * tileSize.x, i * tileSize.y, 0);
            }
        }
    }
}
