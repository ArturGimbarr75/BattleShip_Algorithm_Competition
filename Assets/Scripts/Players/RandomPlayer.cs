using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RandomPlayer : IPlayer
{
    public Coordinates GetCoordinates(Map opponentMap)
    {
        var freePlace = opponentMap.Cells.Where(x => x.Value.Type == CellType.Water).Select(x => x.Value._Coordinates).ToList();
        var rand = new Random();
        return freePlace[rand.Next(0, freePlace.Count)];
    }
}
