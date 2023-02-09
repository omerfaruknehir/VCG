using VCG_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCG_Objects
{
    public enum CardType
    {
        blue,
        green,
        orange,
        yellow,
        powered,
    }

    [Serializable]
    public class Card
    {
        [Serializable]
        public string Type;
        [System.SerializeField]
        public int Figure

        public static readonly string[] Types = new string[] { "blue", "green", "orange", "yellow", "powered" };
        public static readonly int ColoredFigures = 13;
        public static readonly int PoweredFigures = 3;

        public readonly static Random random = new Random();

        public Card(string type, int figure)
        {
            this.Type = type;
            this.Figure = figure;
        }

        public static Card Parse(string data)
        {
            return new Card(data.Split(":")[0], Int32.Parse(data.Split(":")[1]));
        }

        //public static Card Random()
        //{
        //    int typeNum = random.Next(0, Types.Length - 1);
        //    return new Card(Types[typeNum], typeNum == 4 ? random.Next(0, PoweredFigures + 1) : random.Next(0, ColoredFigures + 1));
        //}

        public static Card Random()
        {
            NetManager.SendData("RandomCard", "");
            string[] res = new string[] { "s", "orange:2" };
            return res[1];
        }

        public override string ToString()
        {
            return Type + ":" + Figure;
        }

        public static implicit operator string(Card card)
        {
            return card.ToString();
        }
        public static implicit operator Card(string text)
        {
            return Parse(text);
        }
    }
}
