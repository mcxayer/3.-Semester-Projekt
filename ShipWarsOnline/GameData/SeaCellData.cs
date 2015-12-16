using System.Runtime.Serialization;

namespace GameData
{
    [DataContract]
    public enum CellType
    {
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
    public class SeaCellData
    {
        [DataMember]
        public int ShipIndex { get; set; }

        [DataMember]
        public CellType Type { get; set; }

        [DataMember]
        public bool Revealed { get; set; }
    }
}
