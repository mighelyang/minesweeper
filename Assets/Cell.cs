using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public enum CellType
    {
        NORMAL,
        BOMB
    }

    public CellType cellType;
    public Image image;
    public Text text;

    public int cellIdX;
    public int cellIdY;
    public int numberOfNeighborBombs;

	// Use this for initialization
	void Start () {
        cellType = CellType.NORMAL;
        numberOfNeighborBombs = 0;
	}

    public void Initialize(int cellIdX, int cellIdY)
    {
        this.cellIdX = cellIdX;
        this.cellIdY = cellIdY;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeState( CellType type )
    {
        cellType = type;
        switch (type)
        {
            case CellType.BOMB:
                image.color = Color.red;
                text.text = "b";
                break;
            case CellType.NORMAL:
                break;
        }
    }
}
