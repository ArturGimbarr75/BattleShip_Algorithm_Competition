using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct Cell
{
    public CellType Type { get; set; }
    public readonly Coordinates _Coordinates;

    public Cell(Coordinates coordinates, CellType type = CellType.Water)
    {
        Type = type;
        _Coordinates = coordinates;
    }
}
