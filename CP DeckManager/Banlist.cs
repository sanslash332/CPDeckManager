using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CP_DeckManager
{
    class Banlist
    {
        List<Rule> restrictionsRules { get; set; }
        int mincards { get; set; }
        int maxcards { get; set; }
        List<string> errores;



        public Banlist(string banlistFile,List<Card> allCards)
        {
            restrictionsRules = new List<Rule>();
            maxcards = 60;
            mincards = 40;

            string fileData = File.ReadAllText(banlistFile, Encoding.Default);
            foreach(string l in fileData.Split('\n'))
            {
                if(l.Trim().Length< 3)
                {
                    continue;
                }

                if(l.Trim().StartsWith("#"))
                {
                    continue;

                }

                if(l.Contains(":"))
                {
                    string[] separed = l.Split(':');
                    if(separed[0].Equals("mincards"))
                    {
                        this.mincards = int.Parse(separed[1].Trim());

                    }
                    else if(separed[0].Equals("maxcards"))
                    {
                        this.maxcards = int.Parse(separed[1].Trim());

                    }
                    continue;
                }
                string[] splited = l.Trim().Split(' ');
                Rule r = new Rule(splited[0], int.Parse(splited[1]),allCards.First(c => c.id==splited[0]));

                restrictionsRules.Add(r);




            }


        }


        public string getErrores()
        {
            string errorout = "error: mazo no válido por las siguientes razones: ";
            int contador = 1;
            foreach(string e in errores)
            {
                errorout += contador.ToString() + ": " + e + ". ";
                contador++;


            }

            return (errorout);

        }

        public bool checkDeck(Deck deckToCheck)
        {
            errores = new List<string>();
            

            bool checkedbanlist = true;

            if (deckToCheck.totalCards<this.mincards)
            {
                checkedbanlist = false;
                errores.Add(string.Format("Los masos mínimo deben tener {0} cartas, y el tuyo tiene {1}",this.mincards,deckToCheck.totalCards));

            }

            if(deckToCheck.totalCards>this.maxcards)
            {
                checkedbanlist = false;
                errores.Add(string.Format("Los mazos como máximo deben tener {0} cartas, y el tuyo tiene {1}", this.maxcards,deckToCheck.totalCards));

            }
            
            foreach(Rule r in  this.restrictionsRules)
            {
                if(!r.checkRule(deckToCheck))
                {
                    checkedbanlist = false;
                    errores.Add(r.getRuleError());

                    
                    
                }
            }
            return (checkedbanlist);

        }
    }
}
