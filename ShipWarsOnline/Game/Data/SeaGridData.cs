using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShipWarsOnline.Data
{
    [DataContract]
    public class SeaGridData
    {
        [DataMember]
        public ShipData[] Ships { get; set; }

        [DataMember]
        public SeaCellData[][] Cells { get; set; }
    }
}
