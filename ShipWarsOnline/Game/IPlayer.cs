using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWarsOnline
{
    public interface IPlayer
    {
        SeaGrid Grid { get; set; }
    }
}
