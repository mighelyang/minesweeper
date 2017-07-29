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

	// Use this for initialization
	void Start () {
        cellType = CellType.NORMAL;
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
