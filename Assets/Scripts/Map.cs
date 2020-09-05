using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    GameObject tilePrefab;

    [SerializeField]
    internal Camera mainCamera;

    private float tileSize = 1;
    public int rows = 9;
    public int cols = 16;

    internal Dictionary<string, GameObject> objMap = new Dictionary<string, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        CenterCamera();
    }

    void CenterCamera()
    {
        float gridW = cols * tileSize;
        float gridH = rows * tileSize;
        mainCamera.transform.position = new Vector3(gridW / 2 - tileSize / 2, -gridH / 2 + tileSize / 2, -1);
    }

    void GenerateGrid()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GenerateTile(row, col);

            }
        }
    }

    internal void GenerateTile(int row, int col)
    {
        GameObject tile = SpawnUnit(row, col, tilePrefab);
    }

    public void PlaceUnit(int row, int col, GameObject reference)
    {
        float posX = col * tileSize;
        float posY = row * -tileSize;
        reference.transform.position = new Vector2(posX, posY);
        objMap.Add(row + ", " + col, reference);
    }

    public bool IsObjectAtPos(int row, int col)
    {
        string key = row + ", " + col;
        return objMap.ContainsKey(key);
    }

    public int[] PlaceUnitAtRandomPos(GameObject reference)
    {
        List<int[]> validCoordinates = new List<int[]>();
        for (int row = 1; row < rows; row++)
        {
            for (int col = 1; col < cols; col++)
            {
                if (!IsObjectAtPos(row, col))
                {
                    validCoordinates.Add(new int[] { row, col });
                }
            }
        }
        int randIndex = Random.Range(0, validCoordinates.Count);
        int[] randCoord = validCoordinates[randIndex];
        PlaceUnit(randCoord[0], randCoord[1], reference);
        return randCoord;
    }

    internal GameObject SpawnUnit(int row, int col, GameObject reference)
    {
        GameObject newObj = (GameObject)Instantiate(reference, transform);
        float posX = col * tileSize;
        float posY = row * -tileSize;
        newObj.transform.position = new Vector2(posX, posY);
        return newObj;
    }
}
