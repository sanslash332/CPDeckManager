using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP_DeckManager
{
    class Card
    {
        public string id {get; set;}
        public string name { get; set;}
        public string type { get; set; }
        public string level { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public string quantityText { get { return("x" + quantity.ToString() + ", "); } }
        public Card(string content)
        {
            string[] splitedContent = content.Split(',');
            string[] firstPart = splitedContent[0].Split(' ');
            this.id = firstPart[0];
            string fullName = "";
            for(int i=2; i< firstPart.Length;i++)
            {
                fullName += firstPart[i] + " ";
            }
            this.name = fullName;
            string[] secondPart = splitedContent[1].Split(' ');
            if(secondPart[1].Contains('"'))
            {
                this.type = secondPart[1];
            }
            else if(secondPart[2].Contains('"'))
            {
                this.type = secondPart[2];
            }

            this.type= this.type.Remove(0, this.type.IndexOf('"')).Trim();
            if(secondPart.Length >4)
            {
                this.level = secondPart[4];
            }
            else
            {
                this.level = "0";
            }

            string desc = "";
            for(int i=2; i< splitedContent.Length;i++)
            {
                desc += splitedContent[i] + ",";

            }
            this.description = desc;








        }

        public string printToNvda()
        {
            string final = name + ", " + type + " " + level + ", " + description;
            return (final);


        }

        public string printToDeck ()
        {
            return(this.id + " " + this.quantity.ToString());

        }
    }
}
