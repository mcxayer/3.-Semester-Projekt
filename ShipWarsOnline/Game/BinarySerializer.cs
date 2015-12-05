using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ShipWarsOnline
{
    public class BinarySerializer
    {
        public byte[] Serialize(object o)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, o);
                return stream.ToArray();
            }
        }

        public object Deserialize(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }
    }
}
