using Poker.Contents.Character;
using Poker.Manager;
using Poker.Manager.Contents;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Poker.Cores.Manager
{
    public class UIManager
    {
        #region Default Variable
        private List<string> systemLogList = new List<string>();
        #endregion

        #region Card System Variable
        public string[] CardPatterns { private set; get; } = { "♠", "◇", "♡", "♣" };
        public string[] CardNumbers { private set; get; } = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        #endregion

        public void Init()
        {
            Console.Title = "노잼 포커";
            Console.SetWindowSize(180  , 40);

            Console.CursorVisible = false;
        }

        #region Default UI
        public void Print_GameScene()
        {
            int startX = 5;
            int startY = 1;

            Console.SetCursorPosition(startX, startY);
            Console.Write($"┌── {"< 게임 화면 >".PadRight(120, '─')}┐");

            for (int i = 0; i < 21; i++)
            {
                PaddingSpace(startX, startY + (1 + i), 127);
            }

            Console.SetCursorPosition(startX, startY + 22);
            Console.Write($"└{"".PadRight(127, '─')}┘");
        }

        public void Print_GameSceneHand()
        {
            int startX = 100;
            int startY = 25;

            Console.SetCursorPosition(startX, startY);
            Console.Write($"┌── {"< 게임 화면 >".PadRight(70, '─')}┐");

            for (int i = 0; i < 21; i++)
            {
                PaddingSpace(startX, startY + (1 + i), 77);
            }

            Console.SetCursorPosition(startX, startY + 22);
            Console.Write($"└{"".PadRight(70, '─')}┘");
        }

        public void Print_Log()
        {
            int startX = 5;
            int startY = 25;

            Console.SetCursorPosition(startX, startY);
            Console.Write($"┌── {"< 행동 기록 >".PadRight(85, '─')}┐");

            for(int i = 0; i < 11; i++)
            {
                PaddingSpace(startX, startY + (1 + i), 92);
            }

            Console.SetCursorPosition(startX, startY + 12);
            Console.Write($"└{"".PadRight(92, '─')}┘");
        }

        public void Print_EventLog(string getLog)
        {
            int startX = 10;
            int startY = 27;

            if (systemLogList.Count >= 5)
            {
                systemLogList.Remove(systemLogList[0]);
            }

            string log = $"System: {getLog}";
            systemLogList.Add(log);

            for (int i = 0; i < systemLogList.Count; i++)
            {
                Console.SetCursorPosition(startX, startY + (i * 2));
                Console.WriteLine(systemLogList[i]);
            }
        }
        #endregion

        #region Card System Method
        public void Print_CardGameUI(Player player, CardManager cardManager)
        {
            Print_Card_Guide();
            Print_Card_PlayerGold(player);
            Print_Card_BettingGold(cardManager);
        }

        public void Print_Card_Guide()
        {
            int startX = 135;
            int startY = 1;

            Console.SetCursorPosition(startX, startY);
            Console.Write($"┌── {"< 게임 정보 >".PadRight(35, '─')}┐");

            for (int i = 0; i < 21; i++)
            {
                PaddingSpace(startX, startY + (1 + i), 42);
            }

            Console.SetCursorPosition(startX, startY + 22);
            Console.Write($"└{"".PadRight(42, '─')}┘");
        }

        public void Print_Card_PlayerGold(Player player)
        {
            int startX = 138;
            int startY = 3;

            Console.SetCursorPosition(startX, startY);
            Console.Write($"┌{"".PadRight(23, '─')}┐");
            Console.SetCursorPosition(startX, startY + 1);
            string playerGold = player.Gold.ToString();
            playerGold = playerGold.PadLeft(10, ' ');
            Console.Write("│ 소지금: {0}골드│", playerGold);
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write($"└{"".PadRight(23, '─')}┘");
        }

        public void Print_Card_BettingGold(CardManager cardManager)
        {
            int startX = 138;
            int startY = 6;

            Console.SetCursorPosition(startX, startY);
            Console.Write($"┌{"".PadRight(23, '─')}┐");
            Console.SetCursorPosition(startX, startY + 1);
            string bettingGold = cardManager.BettingGold.ToString();
            bettingGold = bettingGold.PadLeft(10, ' ');
            Console.Write("│ 배팅금: {0}골드│", bettingGold);
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write($"└{"".PadRight(23, '─')}┘");
        }

        public void Print_Card_DeckSet(CardManager cardManager, Character player, int startX, int startY)
        {
            player.CardSet = cardManager.IsCardSet(player.Decks);
            switch (player.CardSet)
            {
                case CardSetType.RoyalStraightFlush:
                    Managers.UI.Print_EventLog($"{player.Name}: 로열 스트레이트 플러쉬".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("로열 스트레이트 플러쉬");
                    break;

                case CardSetType.BackStraightFlush:
                    Managers.UI.Print_EventLog($"{player.Name}: 백 스트레이트 플러쉬".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("백 스트레이트 플러쉬");
                    break;

                case CardSetType.StraightFlush:
                    Managers.UI.Print_EventLog($"{player.Name}: 스트레이트 플러쉬".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("스트레이트 플러쉬");
                    break;

                case CardSetType.FourOfAKind:
                    Managers.UI.Print_EventLog($"{player.Name}: 포카드".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("포카드");
                    break;

                case CardSetType.FullHouse:
                    Managers.UI.Print_EventLog($"{player.Name}: 풀 하우스".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("풀 하우스");
                    break;

                case CardSetType.Flush:
                    Managers.UI.Print_EventLog($"{player.Name}: 플러쉬".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("플러쉬");
                    break;

                case CardSetType.Mountain:
                    Managers.UI.Print_EventLog($"{player.Name}: 마운틴".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("마운틴");
                    break;

                case CardSetType.BackStraight:
                    Managers.UI.Print_EventLog($"{player.Name}: 백 스트레이트".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("백 스트레이트");
                    break;

                case CardSetType.Straight:
                    Managers.UI.Print_EventLog($"{player.Name}: 스트레이트".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("스트레이트");
                    break;

                case CardSetType.ThreeOfAKind:
                    Managers.UI.Print_EventLog($"{player.Name}: 트리플".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("트리플");
                    break;

                case CardSetType.TwoPair:
                    Managers.UI.Print_EventLog($"{player.Name}: 투페어".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("투페어");
                    break;

                case CardSetType.OnePair:
                    Managers.UI.Print_EventLog($"{player.Name}: 원페어".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("원페어");
                    break;

                case CardSetType.None:
                    Managers.UI.Print_EventLog($"{player.Name}: 존망".PadRight(30, ' '));
                    Console.SetCursorPosition(startX, startY);
                    Console.Write("존망");
                    break;
            }
        }

        #endregion

        private void PaddingSpace(int x, int y, int spaceCount)
        {
            Console.SetCursorPosition(x, y);
            Console.Write($"│{" ".PadRight(spaceCount, ' ')}│");
        }
    }
}
