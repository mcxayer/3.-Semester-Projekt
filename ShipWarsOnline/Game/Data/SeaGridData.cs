using System.Runtime.Serialization;

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
