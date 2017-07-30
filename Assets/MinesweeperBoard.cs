using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinesweeperBoard : MonoBehaviour, IComparer<string>
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

        bombPositions.Sort(Compare);

        // grab the first entry
        string firstBomb = "";
        int xIndex = 0;
        int yIndex = 0;
        int bombIndex = 0;
        if (bombPositions.Count > 0)
        {
            firstBomb = bombPositions[bombIndex];
            xIndex = int.Parse(firstBomb.Split('_')[0]);
            yIndex = int.Parse(firstBomb.Split('_')[1]);
        }

        // set up the board
        for (int i = 0; i < boardHeightInCells; i++ )
        {
            for (int j = 0; j < boardWidthInCells; j++)
            {
                GameObject go = Instantiate(cells, gameObject.transform);
                RectTransform goTransform = go.GetComponent<RectTransform>();
                Cell c = go.GetComponent<Cell>();
                c.Initialize(i, j);
                goTransform.sizeDelta = new Vector2(widthPerCell, heightPerCell);
                goTransform.anchoredPosition = new Vector2(j * widthPerCell, -i * heightPerCell);
                goTransform.anchoredPosition += new Vector2(widthPerCell / 2, -heightPerCell / 2);
                string cellName = string.Format("Cell_{0}_{1}", i, j);
                go.name = cellName;

                boardCells.Add(go.GetComponent<Cell>());
                int currentIndex = i * boardHeightInCells + j;
                Cell currentCell = boardCells[currentIndex];

                // look around itself for bombs and update count for already existing bombs
                // cannot go off board left or right, check left
                if ( j != 0 )
                {
                    // check left cell
                    Cell leftCell = boardCells[currentIndex - 1];
                    if (leftCell.cellType == Cell.CellType.BOMB)
                    {
                        currentCell.numberOfNeighborBombs++;
                    }
                }
                
                if( i != 0 )
                {
                    // check top cell
                    Cell topCell = boardCells[(i - 1) * boardHeightInCells + j];
                    if( topCell.cellType == Cell.CellType.BOMB)
                    {
                        currentCell.numberOfNeighborBombs++;
                    }
                }
                
                if( j != 0 && i != 0 )
                {
                    // check top left cell
                    Cell topLeftCell = boardCells[(i - 1) * boardHeightInCells + (j - 1)];
                    if (topLeftCell.cellType == Cell.CellType.BOMB)
                    {
                        currentCell.numberOfNeighborBombs++;
                    }
                }

                if( j != boardWidthInCells - 1 && i != 0 )
                {
                    // check top right cell
                    Cell topRightCell = boardCells[(i - 1) * boardHeightInCells + (j + 1)];
                    if (topRightCell.cellType == Cell.CellType.BOMB)
                    {
                        currentCell.numberOfNeighborBombs++;
                    }
                }

                // check if this cell is in the list of bombs
                if ( xIndex == i && yIndex == j )
                {
                    c.ChangeState(Cell.CellType.BOMB);
                    bombIndex++;

                    // new bomb, update already created cells
                    if (j != 0)
                    {
                        // check left cell
                        Cell leftCell = boardCells[currentIndex - 1];
                        if (leftCell.cellType == Cell.CellType.NORMAL)
                        {
                            leftCell.numberOfNeighborBombs++;
                            leftCell.text.text = leftCell.numberOfNeighborBombs.ToString();
                        }
                    }

                    if (i != 0)
                    {
                        // check top cell
                        Cell topCell = boardCells[(i - 1) * boardHeightInCells + j];
                        if (topCell.cellType == Cell.CellType.NORMAL)
                        {
                            topCell.numberOfNeighborBombs++;
                            topCell.text.text = topCell.numberOfNeighborBombs.ToString();
                        }
                    }

                    if (j != 0 && i != 0)
                    {
                        // check top left cell
                        Cell topLeftCell = boardCells[(i - 1) * boardHeightInCells + (j - 1)];
                        if (topLeftCell.cellType == Cell.CellType.NORMAL)
                        {
                            topLeftCell.numberOfNeighborBombs++;
                            topLeftCell.text.text = topLeftCell.numberOfNeighborBombs.ToString();
                        }
                    }

                    if (j != boardWidthInCells - 1 && i != 0)
                    {
                        // check top right cell
                        Cell topRightCell = boardCells[(i - 1) * boardHeightInCells + (j + 1)];
                        if (topRightCell.cellType == Cell.CellType.NORMAL)
                        {
                            topRightCell.numberOfNeighborBombs++;
                            topRightCell.text.text = topRightCell.numberOfNeighborBombs.ToString();
                        }
                    }

                    if ( bombIndex < numberOfBombsInBoard )
                    {
                        firstBomb = bombPositions[bombIndex];
                        xIndex = int.Parse(firstBomb.Split('_')[0]);
                        yIndex = int.Parse(firstBomb.Split('_')[1]);
                    }
                }

                if (currentCell.cellType == Cell.CellType.NORMAL)
                {
                    currentCell.text.text = currentCell.numberOfNeighborBombs.ToString();
                }

            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int Compare(string s1, string s2)
    {
        string s1p1 = s1.Split('_')[0];
        string s1p2 = s1.Split('_')[1];
        string s2p1 = s2.Split('_')[0];
        string s2p2 = s2.Split('_')[1];
        bool firstValueEqual = false;

        // strip all leading zeros
        for( int i = 0; i < s1p1.Length; i++ )
        {
            if(s1p1[0] == '0')
            {
                s1p1.Remove(0);
            }
        }

        for( int i = 0; i < s2p1.Length; i++ )
        {
            if(s2p1[0] == '0')
            {
                s2p1.Remove(0);
            }
        }

        for( int i = 0; i < s1p2.Length; i++ )
        {
            if (s1p2[0] == '0')
            {
                s1p2.Remove(0);
            }
        }

        for( int i = 0; i < s2p2.Length; i++ )
        {
            if( s2p2[0] == '0')
            {
                s2p2.Remove(0);
            }
        }

        if (s1p1.Length > s2p1.Length)
        {
            return 1;
        }
        else if (s1p1.Length < s2p1.Length)
        {
            return -1;
        }
        else
        {
            for (int i = 0; i < s1p1.Length; i++)
            {
                if (s1p1[i] > s2p1[i])
                {
                    return 1;
                }
                else if (s1p1[i] < s2p1[i])
                {
                    return -1;
                }
                
                if( i == s1p1.Length - 1 )
                { 
                    firstValueEqual = true;
                    break;
                }
            }
        }

        if( firstValueEqual )
        {
            if (s1p2.Length > s2p2.Length)
            {
                return 1;
            }
            else if (s1p2.Length < s2p2.Length)
            {
                return -1;
            }
            else
            {
                for (int j = 0; j < s1p2.Length; j++)
                {
                    if (s1p2[j] > s2p2[j])
                    {
                        return 1;
                    }
                    else if (s1p2[j] < s2p2[j])
                    {
                        return -1;
                    }
                    
                    if( j == s1p2.Length - 1 )
                    {
                        return 0;
                    }
                }
            }
        }

        return 0;
    }
}
