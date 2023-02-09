using System.Net.Sockets;

namespace VCG_Objects
{
    public class Player
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; set; }

        public Player(string name, string ID, TcpClient client)
        {
            this.Name = name;
            this.ID = ID;
            this.Client = client;
            this.Stream = client.GetStream();
        }

        public override bool Equals(object? obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override string ToString()
        {
            return "Player{Name:" + Name + ",ID:" + ID + "}";
        }

        public override int GetHashCode()
        {
            var HashCode = 0;
            foreach (char chr in ID)
            {
                HashCode += chr.GetHashCode();
            }
            return HashCode;
        }
    }
}
