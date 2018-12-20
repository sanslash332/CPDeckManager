using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP_DeckManager
{
    class Card
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string level { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public string quantityText { get { return ("x" + quantity.ToString() + ", "); } }
        public bool isInDeck
        {
            get {
                if (quantity > 0)
                {
                    return (true);
                }
                else
                {
                    return (false);
                }
            } }

        public Card(string content)
        {
            string[] splitedContent = content.Split(',');
            string[] firstPart = splitedContent[0].Split(' ');
            this.id = firstPart[0];
            string fullName = "";
            for (int i = 2; i < firstPart.Length; i++)
            {
                fullName += firstPart[i] + " ";
            }
            this.name = fullName;
            string[] secondPart = splitedContent[1].Split(' ');
            this.type = splitedContent[1].Split('"')[1].Trim();
            this.level = "0";
            foreach (var i in secondPart)
            {
                bool isint = int.TryParse(i, out int num);
                if(isint)
                {
                    this.level = num.ToString();

                }

            }
            
            string desc = "";
            for (int i = 2; i < splitedContent.Length; i++)
            {
                desc += splitedContent[i] + ",";

            }
            this.description = desc;








        }

        public string printToNvda { get
        {
                string final = "";
                if(isInDeck)
                {
                    final +="x"+quantity.ToString()+": ";
                }
             final += name + ", " + type + " " + level + ", " + description;
            return (final);


        }
    }


        public string printToDeck
        {
            get
            {
                return (this.id + " " + this.quantity.ToString());

            }
        }
    }
}
