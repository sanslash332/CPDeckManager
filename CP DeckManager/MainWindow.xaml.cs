using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using IrrKlang;
using System.Reflection;
using System.Windows.Automation;

namespace CP_DeckManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Card> allCards;
        private List<string> types;
        private Deck currentDeck;
        private List<Card> filteredCards;
        private bool NVDA = true;
        private Banlist banlist;
        private string secretWord;
        private ISoundEngine player;
        private ISoundSource soundsourse;


        public MainWindow()
        {
            InitializeComponent();
            this.secretWord = "";
            player = new ISoundEngine();
            soundsourse = player.AddSoundSourceFromIOStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("CP_DeckManager.secret.mp3"), "secret.mp3");
            this.KeyDown += MainWindow_KeyDown;
            cardList.SelectionChanged += cardList_SelectionChanged;
            deckCards.SelectionChanged += deckCards_SelectionChanged;
            currentDeck = new Deck();
            types = new List<string>();
            types.Add("todos");
            deckCards.ItemsSource = currentDeck.cards;
            
            
            loadAllCards();
            try
            {
                banlist = new Banlist("banlist.txt", this.allCards);

            }
            catch(Exception e)
            {
                ScreenReaderControl.speech("error terribleeee no hay banlist!:" + e.ToString(), true);

            }
            

            cardList.ItemsSource = allCards;
            ContextMenu sortMenu = new ContextMenu();
            foreach(string s in types)
            {
                MenuItem mi = new MenuItem();
                mi.Header = s;
                mi.Click+=mi_Click;
                
                

               



                //sortMenu.Items.Add(s);
                sortMenu.Items.Add(mi);
            }
            deckCards.ContextMenu = sortMenu;

            cardList.ContextMenu = sortMenu;
            sortMenu.KeyDown += sortMenu_KeyDown;
            sortMenu.MouseLeftButtonDown += sortMenu_MouseLeftButtonDown;




            cardList.KeyDown += cardList_KeyDown;
            deckCards.KeyDown += cardList_KeyDown;
            deckCards.GotKeyboardFocus += DeckCards_GotKeyboardFocus;
            cardList.GotKeyboardFocus+= DeckCards_GotKeyboardFocus;
            listaPrueba.Items.Add(allCards[30]);
            listaPrueba.Items.Refresh();
            ScreenReaderControl.speech("la lista tiene: " + listaPrueba.Items.Count, true);
            cardList.ItemContainerGenerator.StatusChanged+= ItemContainerGenerator_StatusChanged;


        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if(cardList.ItemContainerGenerator.Status!= System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                return;
            }
            cardList.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
            var cartitaitem = cardList.Items[0] as Card;
            ScreenReaderControl.speech("Nombre del objeto: " + cartitaitem.ToString(), false);
            ListBoxItem itemsito = cardList.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;
            if (itemsito == null)
            {
                ScreenReaderControl.speech("es nulo ", true);

            }




            AutomationProperties.SetName(itemsito, "carta loca");


        }

        private void saycard(Card c, bool quant, bool interruption)
        {
            if(c!=null)
            {
                if(quant)
                {
                    //ScreenReaderControl.speech("x" + c.quantity.ToString() + ", " + c.printToNvda(), interruption);
                }
                else
                {
                    //ScreenReaderControl.speech(c.printToNvda(), interruption);
                }
            }
        }

        private void DeckCards_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            
            if(lb.SelectedIndex==-1)
            {
                if(lb.Items.Count>0)
                {
                    lb.SelectedIndex = 0;
                }
            }
            else
            {
                Card c = (Card)lb.SelectedItem;
                if(lb==deckCards)
                {
                    saycard(c, true, false);
                }
                else
                {
                    saycard(c, false, false);
                }

            }
            
        }

        void deckCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NVDA)
            {
                Card c = (Card)deckCards.SelectedItem;
                saycard(c, true, true);
                
                
            }


        }

        void cardList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
if(NVDA)
{
    Card c = (Card)cardList.SelectedItem;
                
                saycard(c, false, true);
}
        }

        void mi_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            ScreenReaderControl.speech(string.Format("Mostrando cartas del tipo {0}", mi.Header.ToString()), true);
            string filterType = mi.Header.ToString();
            if(filterType=="todos")
            {
                cardList.ItemsSource = allCards;
                cardList.Items.Refresh();
            }
            else
            {
                filterCards(filterType);
                cardList.ItemsSource = filteredCards;
                cardList.Items.Refresh();


            }



            

        }

        private void filterCards(string type)
        {
            filteredCards = new List<Card>();
            foreach(Card c in allCards)
            {
                if(c.type==type)
                {
                    filteredCards.Add(c);

                }
            }

        }
        void cardList_KeyDown(object sender, KeyEventArgs e)
        {
            

            ListBox lb = (ListBox)sender;
            if(e.Key== Key.Z || e.Key== Key.Space || e.Key== Key.Return)
            {
                if(currentDeck.totalCards==60)
                {
                    ScreenReaderControl.speech("no se pueden añadir más de 60 cartas", true);
                    return;

                }
                Card c = (Card)lb.SelectedItem;
                if(c==null)
                {
                    return;

                }
                
                if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    currentDeck.addCard(c);
                    currentDeck.addCard(c);
                    currentDeck.addCard(c);
                    currentDeck.addCard(c);
                    ScreenReaderControl.speech(string.Format("(x4) {0} añadida.", c.name), true);

                }
                else
                {
                    currentDeck.addCard(c);
                    ScreenReaderControl.speech(string.Format("{0} añadida.", c.name), true);

                }
                deckCards.Items.Refresh();

            }
            else if(e.Key== Key.X || e.Key== Key.Back)
            {
                Card c = (Card)lb.SelectedItem;
                if(c==null)
                {
                    return;

                }

                if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    currentDeck.removeCard(c);
                    currentDeck.removeCard(c);
                    currentDeck.removeCard(c);
                    currentDeck.removeCard(c);
                    ScreenReaderControl.speech(string.Format("(x4) {0} removida", c.name), true);

                }
                else
                {
                    currentDeck.removeCard(c);
                    ScreenReaderControl.speech(string.Format("{0} removida", c.name), true);

                }
                deckCards.Items.Refresh();



            }
            

        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key== Key.B)
            {
                this.secretWord = "";
            }
            this.secretWord += e.Key.ToString();
            
            

            if(this.secretWord.ToLower().Equals("banlist"))
            {
                player.Play2D(soundsourse, false, false, false);
                ScreenReaderControl.speech("editor de banlist", true);
                BanlistEditorWindow banlistwindow = new BanlistEditorWindow();
                banlistwindow.Owner= this;
                banlistwindow.ShowDialog();





                return;

            }

            if(e.Key== Key.T)
            {
                ScreenReaderControl.speech(string.Format("El maso actual contiene {0} cartas.", currentDeck.totalCards.ToString()), true);
            }
            else if(e.Key== Key.Delete)
            {
                currentDeck.clearDeck();
                ScreenReaderControl.speech("maso limpiado", true);
                deckCards.ItemsSource = currentDeck.cards;
                deckCards.Items.Refresh();
            }
            }

        void sortMenu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            sortMenuClicked((ContextMenu)sender);
        }

        void sortMenu_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key== Key.Enter)
            {
                sortMenuClicked((ContextMenu)sender);
            }
        }

        private void sortMenuClicked(ContextMenu sortmenu)
        {
            string clicked = sortmenu.Items.CurrentItem.ToString();
            ScreenReaderControl.speech("ordenando por: " + clicked, true);
        }
        private void loadAllCards ()
        {
            allCards = new List<Card>();
            string data = File.ReadAllText("cards reference.txt", Encoding.Default);
            string[] splitedData = data.Split('\n');
            foreach(string s in splitedData)
            {
                int comaCount = s.Split(',').Length;
                if(comaCount<3)
                {
                    continue;

                }
                Card carta = new Card(s);
                if(!types.Contains(carta.type))
                {
                    types.Add(carta.type);
                }
                allCards.Add(carta);


            }

            ScreenReaderControl.speech("Se cargaron un total de " + allCards.Count.ToString() + " cartas ", true);
        }

        private bool dettectValidCard(string cardID)
        {
            foreach(Card c in allCards)
            {
                if(c.id== cardID)
                {
                    return (true);
                }
            }

            return (false);
        }

        private void loadDeck(string deckContent)
        {
            string[] splitedData = deckContent.Split('\n');
            currentDeck.clearDeck();
            
            

            foreach(string s in splitedData)
            {
                string[] datacard = s.Split(' ');
                if(datacard.Length <2)
                {
                    continue;
                }
                string cardid = datacard[0];
                int quant = Convert.ToInt32(datacard[1]);

                if(dettectValidCard(cardid))
                {
                    Card c = getCard(cardid);
                    for(int i=0; i<quant;i++)
                    {
                        currentDeck.addCard(c);


                    }
                }



            }
            deckCards.ItemsSource = currentDeck.cards;
            deckCards.Items.Refresh();
            ScreenReaderControl.speech(string.Format("Se ha cargado un maso con un total de {0} cartas", currentDeck.totalCards.ToString()),true);
        }

        
        private Card getCard(string id)
        {
            foreach(Card c in allCards)
            {
                if(id==c.id)
                {
                    return (c);
                }
            }

            return (null);
        }
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Seleccione maso a cargar";
            op.Filter="archivo de texto|*.txt| cualquier wea |*.*";
            if(op.ShowDialog()  ==true)
            {
                string FileData = File.ReadAllText(op.FileName);
                loadDeck(FileData);
            }
            else
            {
                ScreenReaderControl.speech("Carga cancelada", true);
            }
            

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter =     "archivo de texto|*.txt| cualquier wea |*.*";
            sv.CheckFileExists = false;
            sv.AddExtension = true;

            if(sv.ShowDialog() == true)
            {
                if(File.Exists(sv.FileName))
                {
                    File.Delete(sv.FileName);
                }
                File.WriteAllText(sv.FileName, currentDeck.exportDeck(), Encoding.Default);
            }

            
        }

        private void validateButton_Click(object sender, RoutedEventArgs e)
        {
            if(banlist.checkDeck(this.currentDeck))
            {
                ScreenReaderControl.speech("Mazo válido:",true);


            }
            else
            {
                ScreenReaderControl.speech(banlist.getErrores(),true);

            }
        }
    }
}
