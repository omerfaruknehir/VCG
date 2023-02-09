using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VCG_Library
{
    public static class CustomMethods
    {
        public static byte[] ToBytes(this object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}