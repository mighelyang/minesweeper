using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinesweeperBoard : MonoBehaviour, IComparer<Vector2>
{

    public GameObject cells;        // cells is set in the inspector as the cells prefab
    public GameObject board;        // board is set in the inspector as the board gameobject from scene
    public List<Cell> boardCells;   // list of cells that we will generate and populate

    public int boardWidthInCells = 10;
    public int boardHeightInCells = 10;
    public int numberOfBombsInBoard = 10;

    private List<string> bombPositions;

	// Use this for initialization
	void Start () {
        boardCells = new List<Cell>();

        // figure out the dimensions of the board
        RectTransform boardTransform = board.GetComponent<RectTransform>();
        float widthPerCell = boardTransform.rect.width / boardWidthInCells;
        float heightPerCell = boardTransform.rect.height / boardHeightInCells;

        // randomize locations of bomb and store positions in list
        bombPositions = new List<string>();
        for( int i = 0; i < numberOfBombsInBoard; )
        {
            int xCellIndex = Random.Range(0, boardWidthInCells);
            int yCellIndex = Random.Range(0, boardHeightInCells);
            string indexString = xCellIndex.ToString() + "_" + yCellIndex.ToString();
            if (!bombPositions.Contains(indexString))
            {
                bombPositions.Add(indexString);
                i++;
            }
        }

        bombPositions.Sort();

        // set up the board
        for (int i = 0; i < boardHeightInCells; i++ )
        {
            for (int j = 0; j < boardWidthInCells; j++)
            {
                GameObject go = Instantiate(cells, gameObject.transform);
                RectTransform goTransform = go.GetComponent<RectTransform>();
                goTransform.sizeDelta = new Vector2(widthPerCell, heightPerCell);
                goTransform.anchoredPosition = new Vector2(j * widthPerCell, -i * heightPerCell);
                goTransform.anchoredPosition += new Vector2(widthPerCell / 2, -heightPerCell / 2);
                string cellName = string.Format("Cell_{0}_{1}", i, j);
                go.name = cellName;

                boardCells.Add(go.GetComponent<Cell>());

                // check if this cell is in the list of bombs
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int Compare(Vector2 v1, Vector2 v2)
    {
        if( v1.x > v2.x )
        {
            return 1;
        }
        else if( v1.x < v2.x )
        {
            return -1;
        }
        else
        {
            if( v1.y > v2.y )
            {
                return 1;
            }
            else if( v1.y < v2.y )
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
