using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

struct PlayerElements
{
    public Map PlayerMap { get; set; }
    public Map PlayerMapForOpponent { get; set; }

    public IPlayer Player { get; set; }
}
