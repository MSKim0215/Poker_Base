using Poker.Manager.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Contents.Character
{
    public class NonPlayer : Character
    {
        protected NonPlayer() : base(CharacterType.NPC) { }
    }

    public class Dealer: NonPlayer
    {
        public Dealer() : base() 
        {
            Name = "딜러";
        }

        public void MakeDeck(CardManager cardManager)
        {
            if (Decks.Count > 0) Decks.Clear();
            Random rand = new Random();

            for (int i = 0; i < CardManager.TABLE_CARD_COUNT; i++)
            {
                int randIndex = rand.Next(0, cardManager.AllCards.Count());
                Card newCard = cardManager.AllCards[randIndex];
                Decks.Add(newCard);
                cardManager.AllCards.Remove(newCard);
            }
            Decks = Decks.OrderBy(card => card.Number).ToList();
        }

        public void PrintDeck()
        {
            int startX = 15;
            int startY = 4;

            Console.SetCursorPosition(startX + 10, startY);
            Console.Write("[딜러가 뽑은 카드]");

            for (int i = 0; i < Decks.Count; i++)
            {
                Decks[i].Print((startX + 4) + (15 * i), startY + 2);
            }
        }
    }
}
