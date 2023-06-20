using Poker.Manager.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Contents.Character
{
    public enum CharacterType
    {
        None, Player, NPC
    }

    public class Character
    {
        protected CharacterType characterType = CharacterType.None;

        public List<Card> Decks { protected set; get; } = new List<Card>();
        public string Name { protected set; get; }
        public CardSetType CardSet { set; get; }

        protected Character(CharacterType type)
        {
            characterType = type;
        }
    }
}
