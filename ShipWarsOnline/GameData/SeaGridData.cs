using System.Runtime.Serialization;

namespace GameData
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
