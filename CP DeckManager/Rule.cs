using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP_DeckManager
{
    class Rule
    {
        public string restrictedId { get; set;  }
        public int quantity { get; set; }
        public Card card;

        /// <summary>
        /// Rule constructor
        /// </summary>
        /// <param name="id"> Id of restricted Card </param>
        /// <param name="quant"> Max quantity of the restricted Card</param>
        /// 
        public Rule(string id, int quant, Card c)
        {
            this.restrictedId = id;
            this.quantity = quant;
            this.card = c;



        }


        
        public string getRuleError()
        {
            
            return (string.Format("No se pueden tener mas de {0} copias de {1} en tu mazo",this.quantity,this.card.name));

        }
        public bool checkRule(Deck decktoCheck)
        {
            bool checkedRule= true;
            
            foreach(Card c in decktoCheck.cards)
            {
                if(c.id==this.restrictedId)
                {
                    if(c.quantity>this.quantity)
                    {
                        checkedRule= false;
                        break;
                    }
                }

                    
            }
            return (checkedRule);


            }
    }
}
