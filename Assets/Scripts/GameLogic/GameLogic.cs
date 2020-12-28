using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic
{
    private PlayerElements Player1;
    private PlayerElements Player2;

    private bool IsFirstPlayerQueue;
    public bool IsGameEnded { get
        {
            return Player1.PlayerMap.ShipsLeft > 0
                && Player2.PlayerMap.ShipsLeft > 0;
        } }

    public GameLogic(IPlayer player1, IPlayer player2)
    {
        Player1 = new PlayerElements()
        {
            Player = player1,
            PlayerMap = new Map(Map.MapType.RandomShipPlacement),
            PlayerMapForOpponent = new Map()
        };

        Player2 = new PlayerElements()
        {
            Player = player2,
            PlayerMap = Player1.PlayerMap,
            PlayerMapForOpponent = new Map()
        };

        IsFirstPlayerQueue = true;
    }

    public Map[] GetCurrentMaps()
        => new Map[] { Player1.PlayerMapForOpponent, Player2.PlayerMapForOpponent };

    public Map[] GetNextMaps()
    {
        var coord = IsFirstPlayerQueue
            ? Player1.Player.GetCoordinates(Player2.PlayerMapForOpponent.DeepCopy())
            : Player2.Player.GetCoordinates(Player1.PlayerMapForOpponent.DeepCopy());
        ChangeMap(coord);

        var maps = new Map[]{ Player1.PlayerMapForOpponent, Player2.PlayerMapForOpponent};
        IsFirstPlayerQueue = !IsFirstPlayerQueue;
        return maps;
    }

    private void ChangeMap(Coordinates coord)
    {
        var map = IsFirstPlayerQueue
            ? Player2.PlayerMap
            : Player1.PlayerMap;
        var mapForOpponent = IsFirstPlayerQueue
            ? Player2.PlayerMapForOpponent
            : Player1.PlayerMapForOpponent;

        var cell = map.Cells[coord];

        switch (cell.Type)
        {
            case CellType.Water:
                cell.Type = CellType.Miss;
                map.Cells[coord] = cell;
                mapForOpponent.Cells[coord] = cell;
                break;

            case CellType.Ship:
                cell.Type = CellType.Damaged;
                map.Cells[coord] = cell;
                mapForOpponent.Cells[coord] = cell;
                var res = CheckShipRecursively(coord, map);
                if (res.destroyed)
                    ChangeToDestroyed(coord, ref map, ref mapForOpponent);
                break;
        }

        if (IsFirstPlayerQueue)
        {
            Player2.PlayerMap = map;
            Player2.PlayerMapForOpponent = mapForOpponent;
        }
        else
        {
            Player1.PlayerMap = map;
            Player1.PlayerMapForOpponent = mapForOpponent;
        }
    }

    private void ChangeToDestroyed(Coordinates coord, ref Map map, ref Map mapForOpponent)
    {
        var cell = map.Cells[coord];
        if (cell.Type != CellType.Damaged)
            return;

        cell.Type = CellType.Destroyed;
        map.Cells[coord] = cell;
        mapForOpponent.Cells[coord] = cell;

        int x = coord.ToVector2Int().x, y = coord.ToVector2Int().y;
        if (y + 1 < 11)
            ChangeToDestroyed(new Coordinates(y + 1, x), ref map, ref mapForOpponent);
        if (y - 1 > 0)
            ChangeToDestroyed(new Coordinates(y - 1, x), ref map, ref mapForOpponent);
        if (x + 1 < 11)
            ChangeToDestroyed(new Coordinates(y, x + 1), ref map, ref mapForOpponent);
        if (x - 1 > 0)
            ChangeToDestroyed(new Coordinates(y, x - 1), ref map, ref mapForOpponent);
    }

    (bool destroyed, int cellsCount) CheckShipRecursively(Coordinates coord, Map map)
    {
        var ver1 = CheckVertical(-1, coord, map);
        var ver2 = CheckVertical(1, coord, map);
        var hor1 = CheckHorizontal(-1, coord, map);
        var hor2 = CheckHorizontal(1, coord, map);

        var ver = (ver1.destroyed && ver2.destroyed, ver1.cellsCount + ver2.cellsCount);
        var hor = (hor1.destroyed && hor2.destroyed, hor1.cellsCount + hor2.cellsCount);

        return (ver.Item1 && hor.Item1, Mathf.Max(ver.Item2, hor.Item2));
    }

    (bool destroyed, int cellsCount) CheckVertical(int direction, Coordinates coord, Map map)
    {
        int y = coord.ToVector2Int().y + direction;
        if (y > 10 && y < 1)
            return (true, 0);
        var newCoord = new Coordinates(y, (int)coord._Column);
        var type = map.Cells[newCoord].Type;

        if (type == CellType.Damaged)
        {
            var res = CheckVertical(direction, newCoord, map);
            return (res.destroyed, 1 + res.cellsCount);
        }
        else if (type == CellType.Ship)
        {
            return (false, 0);
        }
        else
        {
            return (true, 0);
        }
    }

    (bool destroyed, int cellsCount) CheckHorizontal(int direction, Coordinates coord, Map map)
    {
        int x = coord.ToVector2Int().x + direction;
        if (x > 10 && x < 1)
            return (true, 0);
        var newCoord = new Coordinates(coord.ToVector2Int().y, x);
        var type = map.Cells[newCoord].Type;

        if (type == CellType.Damaged)
        {
            var res = CheckHorizontal(direction, newCoord, map);
            return (res.destroyed, 1 + res.cellsCount);
        }
        else if (type == CellType.Ship)
        {
            return (false, 0);
        }
        else
        {
            return (true, 0);
        }
    }

    
}
