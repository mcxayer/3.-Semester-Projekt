using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShipWarsOnline.Data
{
    [DataContract]
    public enum SquareType
    {
        [EnumMember]
        Unknown,
        [EnumMember]
        Water,
        [EnumMember]
        Undamaged,
        [EnumMember]
        Damaged,
        [EnumMember]
        Sunk
    }

    [DataContract]
    public class SeaSquareData
    {
        [DataMember]
        public int ShipIndex { get; set; }

        [DataMember]
        public SquareType Type { get; set; }

        [DataMember]
        public bool Revealed { get; set; }
    }
}
