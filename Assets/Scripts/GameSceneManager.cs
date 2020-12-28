using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameSceneManager : MonoBehaviour
{
    public Tilemap Map1;
    public Tilemap Map2;

    public TileBase Ship;
    public TileBase Missed;
    public TileBase Damaged;
    public TileBase Destroyed;

    private GameLogic Logic;
    private Map[] Maps;

    private float Pause = 0.2f;
    private bool ShowAllData = true;

    void Start()
    {
        Logic = new GameLogic(new RandomPlayer(), new RandomPlayer());
        Maps = Logic.GetCurrentMaps(ShowAllData);
        Draw();

        //StartCoroutine("DrawMaps");
    }

    private void FixedUpdate()
    {
        Maps = Logic.GetNextMaps(ShowAllData);
        Draw();
    }

    private IEnumerator DrawMaps()
    {
        while (Logic.IsGameEnded)
        {
            //yield return new WaitForSeconds(Pause);
            Maps = Logic.GetNextMaps(ShowAllData);
            Draw();
        }
        return null;
    }

    private void Draw()
    {
        for (int mapNumber = 0; mapNumber < 2; mapNumber++)
            foreach (var cell in Maps[mapNumber].Cells)
            {
                TileBase currentTexture;
                switch (cell.Value.Type)
                {
                    case CellType.Damaged:
                        currentTexture = Damaged;
                        break;

                    case CellType.Destroyed:
                        currentTexture = Destroyed;
                        break;

                    case CellType.Miss:
                        currentTexture = Missed;
                        break;

                    case CellType.Ship:
                        currentTexture = Ship;
                        break;

                    default:
                        continue;
                }

                var coord = cell.Value._Coordinates;
                var pos = new Vector3Int((int)coord._Column, (int)coord._Row, 0);
                if (mapNumber == 0)
                    Map1.SetTile(pos, currentTexture);
                else
                    Map2.SetTile(pos, currentTexture);
            }
    }
}
