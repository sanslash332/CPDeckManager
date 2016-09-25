using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP_DeckManager
{
    class Deck
    {
        public List<Card> cards { get; set; }
        public int totalCards { get; set; }

        public Deck()
        {
            cards = new List<Card>();
            totalCards = 0;


        }

        public void clearDeck()
        {
            
            totalCards = 0;
            foreach(Card c in cards)
            {
                c.quantity = 0;
            }
            cards = new List<Card>();


        }
        public void addCard(Card c)
        {
            if(cards.Contains(c) && c.quantity<4)
            {
                c.quantity+=1;
                totalCards+=1;
            }
            else if(!cards.Contains(c))
            {
                cards.Add(c);
                totalCards+=1;
                c.quantity=1;
            }
        }

        public void removeCard(Card c)
        {
            if(c.quantity>1 && cards.Contains(c))
            {
                c.quantity -= 1;
                totalCards -= 1;

            }
            else
            {
                c.quantity = 0;
                
                if(cards.Contains(c))
                {
                    cards.Remove(c);
                    totalCards -= 1;
                }
                

            }
        }
        public string exportDeck()
        {
            string outdata= "";
            foreach(Card c in cards)
            {
                outdata += c.printToDeck() + "\r\n";
            }
            outdata = outdata.Remove(outdata.LastIndexOf('\r'));
            return (outdata);

        }
    }
    
}
