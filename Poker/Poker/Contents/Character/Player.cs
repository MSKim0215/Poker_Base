using Poker.Manager.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Contents.Character
{
    public class Player : Character
    {
        public int Gold { private set; get; } = 5000;

        public Player() : base(CharacterType.Player)
        {
            Name = "플레이어";
        }

        public void MakeDeck(CardManager cardManager)
        {
            if (Decks.Count > 0) Decks.Clear();
            Random rand = new Random();

            for (int i = 0; i < CardManager.HAND_CARD_COUNT; i++)
            {
                int randIndex = rand.Next(0, cardManager.AllCards.Count());
                Card newCard = cardManager.AllCards[randIndex];
                Decks.Add(newCard);
                cardManager.AllCards.Remove(newCard);
            }
            Decks = Decks.OrderBy(card => card.Number).ToList();
        }

        public void ChangeDeck(CardManager cardManager, Card deleteCard)
        {
            Random rand = new Random();

            Decks.Remove(deleteCard);

            int randIndex = rand.Next(0, cardManager.AllCards.Count());
            Card newCard = cardManager.AllCards[randIndex];
            Decks.Add(newCard);
            cardManager.AllCards.Remove(newCard);
        }

        public void PrintDeck()
        {
            int startX = 103;
            int startY = 27;

            Console.SetCursorPosition(startX, startY);
            Console.Write("[내가 뽑은 카드]");

            for(int i = 0; i < Decks.Count; i++)
            {
                Decks[i].Print((startX + 1) + (15 * i), startY + 2);
            }
        }

        public void ResumeGold(int gold)
        {
            Gold += gold;

            if(Gold <= 0)
            {
                Gold = 0;
            }
        }
    }
}
