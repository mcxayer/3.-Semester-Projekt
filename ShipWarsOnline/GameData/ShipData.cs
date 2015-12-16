using System.Runtime.Serialization;

namespace ShipWarsOnline.Data
{
    [DataContract]
    public enum ShipType
    {
        [EnumMember]
        Carrier,
        [EnumMember]
        Battleship,
        [EnumMember]
        Destroyer,
        [EnumMember]
        Submarine,
        [EnumMember]
        PatrolBoat
    };

    [DataContract]
    public class ShipData
    {
        [DataMember]
        public ShipType Type { get; set; }

        [DataMember]
        public int Health { get; set; }

        [DataMember]
        public int PosX { get; set; }

        [DataMember]
        public int PosY { get; set; }

        [DataMember]
        public bool Horizontal { get; set; }
    }
}
