using Poker.Contents;
using Poker.Contents.Character;
using Poker.Manager.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Cores.Manager
{
    public class GameManager
    {
        private CardManager cardManager;
        private Player player;
        private Dealer dealer;

        public void Init()
        {
            player = new Player();
            dealer = new Dealer();

            cardManager = new CardManager(player, dealer);
            cardManager.ReturnStart();
        }
    }
}
