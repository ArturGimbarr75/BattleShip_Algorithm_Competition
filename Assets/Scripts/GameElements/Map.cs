using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

struct Map
{
    public Dictionary<Coordinates, Cell> Cells;

    public const int RowCount = 10;
    public const int ColumnCount = 10;
    public int DestroyersLeft_1CellsShip { get; set; }
    public int SubmarinesLeft_2CellsShip { get; set; }
    public int CruisersLeft_3CellsShip { get; set; }
    public int BattleshipsLeft_4CellsShip { get; set; }

    public Map(MapType type = MapType.Empty)
    {
        Cells = new Dictionary<Coordinates, Cell>();

        for (int i = 1; i < 11; i++)
            for (int j = 1; j < 11; j++)
                Cells.Add(new Coordinates(i, j), new Cell(new Coordinates(i, j), CellType.Water));

        DestroyersLeft_1CellsShip = 4;
        SubmarinesLeft_2CellsShip = 3;
        CruisersLeft_3CellsShip = 2;
        BattleshipsLeft_4CellsShip = 1;

        if (type == MapType.RandomShipPlacement)
            RandomShipPlacement();
    }

    public Map DeepCopy()
    {
        return new Map()
        {
            DestroyersLeft_1CellsShip = DestroyersLeft_1CellsShip,
            SubmarinesLeft_2CellsShip = SubmarinesLeft_2CellsShip,
            CruisersLeft_3CellsShip = CruisersLeft_3CellsShip,
            BattleshipsLeft_4CellsShip = BattleshipsLeft_4CellsShip,
            Cells = Cells.ToDictionary(x => new Coordinates(x.Key._Row, x.Key._Column), y => y.Value)
        };
    }

    private void RandomShipPlacement()
    {
        char[,] map = new char[10, 10];
        for (int shipSize = 4; shipSize > 0; shipSize--)
            for (int shipCount = 0; shipCount < 5 - shipSize; shipCount++)
                GererateShip(shipSize, ref map);

        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                if (map[i, j] == 'S')
                {
                    var cell = Cells[new Coordinates(i + 1, j + 1)];
                    cell.Type = CellType.Ship;
                    Cells[new Coordinates(i + 1, j + 1)] = cell;
                }
    }

    private void GererateShip(int index, ref char[,] map) // Legacy code
    {
        Random rand = new Random();
        bool ok = true;

        while (ok) 
        {
            ok = false;

            int I = rand.Next(10), J = rand.Next(10);

            switch (rand.Next(2))
            {
                case 0:
                    if (I - index + 1 >= 0)
                    {
                        for (int i = I; i > I - index; i--)
                            if (map[i, J] == 'S')
                                ok = true;

                        if (J - 1 >= 0)
                            for (int i = I; i > I - index; i--)
                                if (map[i, J - 1] == 'S')
                                    ok = true;

                        if (J + 1 <= 9)
                            for (int i = I; i > I - index; i--)
                                if (map[i, J + 1] == 'S')
                                    ok = true;

                        if (I - index + 1 > 0 && J - 1 >= 0)
                            if (map[I - index, J - 1] == 'S')
                                ok = true;
                        if (I - index + 1 > 0)
                            if (map[I - index, J] == 'S')
                                ok = true;
                        if (I - index + 1 > 0 && J + 1 < 10)
                            if (map[I - index, J + 1] == 'S')
                                ok = true;
                        if (I != 9 && J - 1 >= 0)
                            if (map[I + 1, J - 1] == 'S')
                                ok = true;
                        if (I != 9)
                            if (map[I + 1, J] == 'S')
                                ok = true;
                        if (I != 9 && J + 1 < 10)
                            if (map[I + 1, J + 1] == 'S')
                                ok = true;

                        if (!ok)
                            for (int i = I; i > I - index; i--)
                                map[i, J] = 'S';
                    }
                    else
                        ok = true;
                    break;

                case 1:
                    if (J - index + 1 >= 0)
                    {
                        for (int i = J; i > J - index; i--)
                            if (map[I, i] == 'S')
                                ok = true;

                        if (I - 1 >= 0)
                            for (int i = J; i > J - index; i--)
                                if (map[I - 1, i] == 'S')
                                    ok = true;

                        if (I + 1 <= 9)
                            for (int i = J; i > J - index; i--)
                                if (map[I + 1, i] == 'S')
                                    ok = true;

                        if (J - index + 1 > 0 && I - 1 >= 0)
                            if (map[I - 1, J - index] == 'S')
                                ok = true;
                        if (J - index + 1 > 0)
                            if (map[I, J - index] == 'S')
                                ok = true;
                        if (J - index + 1 > 0 && I + 1 < 10)
                            if (map[I + 1, J - index] == 'S')
                                ok = true;
                        if (J != 9 && I - 1 >= 0)
                            if (map[I - 1, J + 1] == 'S')
                                ok = true;
                        if (J != 9)
                            if (map[I, J + 1] == 'S')
                                ok = true;
                        if (J != 9 && I + 1 < 10)
                            if (map[I + 1, J + 1] == 'S')
                                ok = true;

                        if (!ok)
                            for (int i = J; i > J - index; i--)
                                map[I, i] = 'S';
                    }
                    else
                        ok = true;
                    break;
            }
        }
    }

    public enum MapType
    {
        Empty,
        RandomShipPlacement
    }
}
