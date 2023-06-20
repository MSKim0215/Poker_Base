using FATCAT.Threading;
using Poker.Contents;
using Poker.Contents.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Poker.Manager.Contents
{
    public enum CardSetType
    {
        RoyalStraightFlush,
        BackStraightFlush,
        StraightFlush,
        FourOfAKind,
        FullHouse,
        Flush,
        Mountain,
        BackStraight,
        Straight,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        None
    }

    public class CardManager: Core
    {
        public const int PLAYER_COUNT = 2;
        public const int HAND_CARD_COUNT = 5;
        public const int TABLE_CARD_COUNT = 7;

        private Player player;
        private Dealer dealer;

        private int bettingGold;

        public int BettingGold
        {
            get => bettingGold;
            set
            {
                bettingGold = value;
                Managers.UI.Print_Card_BettingGold(this);
            }
        }

        public List<Card> AllCards { private set; get; } = new List<Card>();

        public CardManager (Player player, Dealer dealer)
        {
            this.player = player;
            this.dealer = dealer;
            Start();
        }

        public void ReturnStart()
        {
            // UI
            Managers.UI.Print_GameScene();
            Managers.UI.Print_GameSceneHand();
            Managers.UI.Print_Log();
            Managers.UI.Print_Card_BettingGold(this);
            Managers.UI.Print_Card_PlayerGold(player);
            Managers.UI.Print_EventLog($"{Console.Title}에 오신 것을 환영합니다!");

            MakeAllCard();
            DrawCard();
            BettingCheck();
        }

        private void MakeAllCard()
        {
            if (AllCards.Count > 0) AllCards.Clear();

            for (int i = 0; i < Managers.UI.CardPatterns.Length; i++)
            {
                for (int j = 0; j < Managers.UI.CardNumbers.Length; j++)
                {
                    AllCards.Add(new Card(Managers.UI.CardPatterns[i], Managers.UI.CardNumbers[j], i + 1, j + 1));
                }
            }
        }

        private void DrawCard()
        {
            player.MakeDeck(this);
            dealer.MakeDeck(this);
        }

        private void BettingCheck()
        {
            char input;
            while (true)
            {
                Managers.UI.Print_CardGameUI(player, this);

                dealer.PrintDeck();
                Managers.UI.Print_Card_DeckSet(this, dealer, 50, 4);
                player.PrintDeck();
                Managers.UI.Print_Card_DeckSet(this, player, 125, 27);

                int startX = 20;
                int startY = 16;

                Console.SetCursorPosition(startX, startY);
                Console.Write($"배팅을 진행하시겠습니까? (Y / N / E) ");
                Console.SetCursorPosition(startX, startY + 2);
                Console.Write($">> ");
                input = Console.ReadKey(true).KeyChar;

                switch (input)
                {
                    case 'Y':
                    case 'y':
                        {
                            if (player.Gold <= 0)
                            {
                                Managers.UI.Print_EventLog("소지금이 없습니다.".PadRight(30, ' '));
                                break;
                            }

                            Managers.UI.Print_EventLog("배팅을 선택하셨습니다.".PadRight(30, ' '));
                            BettingGame();
                            Thread.Sleep(1000);
                            RefreshCard();
                            Thread.Sleep(1000);
                            GameResult();
                            return;
                        }
                    case 'N':
                    case 'n':
                        {
                            Managers.UI.Print_EventLog("다이...".PadRight(30, ' '));
                            Thread.Sleep(1000);
                            Managers.UI.Print_EventLog("딜러가 다시 카드를 섞습니다.".PadRight(30, ' '));
                            Thread.Sleep(1000);
                            ReturnStart();
                            return;
                        }
                    case 'E':
                    case 'e':
                        {
                            Managers.UI.Print_EventLog("도박을 포기합니다.".PadRight(30, ' '));
                            Thread.Sleep(1000);
                            Environment.Exit(0);
                            return;
                        }
                    default:
                        {
                            Managers.UI.Print_EventLog("※ 옳바른 값을 입력하세요. ※".PadRight(30, ' '));
                        }
                        break;
                }
            }
        }

        private void BettingGame()
        {
            string betting;

            while (true)
            {
                Managers.UI.Print_CardGameUI(player, this);

                int startX = 20;
                int startY = 18;

                Console.SetCursorPosition(startX, startY);
                Console.Write($"배팅금을 입력해주세요.                       ");
                Console.SetCursorPosition(startX, startY + 2);
                Console.Write($">>                 ");
                Console.SetCursorPosition(startX + 4, startY + 2);
                betting = Console.ReadLine();

                if(int.TryParse(betting, out _))
                {
                    BettingGold = int.Parse(betting);

                    if (BettingGold > 0 && BettingGold <= player.Gold)
                    {
                        Managers.UI.Print_EventLog($"{BettingGold} 골드를 배팅하셨습니다.".PadRight(30, ' '));
                        break;
                    }
                    else
                    {
                        Managers.UI.Print_EventLog("※ 금액이 부족합니다. ※".PadRight(30, ' '));

                        Thread.Sleep(300);

                        BettingGold = 0;
                    }
                }
                else
                {
                    Managers.UI.Print_EventLog("※ 숫자를 입력해주세요. ※".PadRight(30, ' '));
                    Thread.Sleep(300);
                }
            }
        }

        private void RefreshCard()
        {   // TODO: 카드 두개 선택해서 새로 뽑기
            int startX = 20;
            int startY = 18;

            string input;

            Console.SetCursorPosition(startX, startY);
            Console.Write($"교체할 카드를 선택하세요. (1 ~ 5)  예시-> 1, 5                    ");
            Console.SetCursorPosition(startX, startY + 2);
            Console.Write($">>                 ");
            Console.SetCursorPosition(startX + 4, startY + 2);
            input = Console.ReadLine();

            string[] inputs = input.Split(',');
            int[] inputNums = new int[inputs.Length];
            for(int i = 0; i < inputs.Length; i++)
            {
                inputNums[i] = int.Parse(inputs[i]);
            }

            Card[] deleteCards = new Card[inputNums.Length];
            for(int i = 0; i < deleteCards.Length; i++)
            {
                deleteCards[i] = player.Decks[inputNums[i] - 1];
            }

            for(int i = 0; i < inputs.Length; i++)
            {
                Managers.UI.Print_EventLog($"{player.Decks[inputNums[i] - 1].PatternStr + player.Decks[inputNums[i] - 1].NumberStr} 카드를 교체합니다.".PadRight(30, ' '));
            }

            for (int i = 0; i < inputs.Length; i++)
            {
                player.ChangeDeck(this, deleteCards[i]);
            }

            player.PrintDeck();
            Managers.UI.Print_Card_DeckSet(this, player, 125, 27);
        }

        private void GameResult()
        {
            int playerIndex = (int)player.CardSet;
            int dealerIndex = (int)dealer.CardSet;

            int startX = 84;
            int startY = 16;

            if (playerIndex < dealerIndex)
            {
                Managers.UI.Print_EventLog("배팅에 성공했습니다!!!".PadRight(30, ' '));
                player.ResumeGold(BettingGold * 2);

                Console.SetCursorPosition(startX, startY + 2);
                Console.WriteLine($"배팅에 성공했습니다!!!");
            }
            else if(playerIndex > dealerIndex)
            {
                Managers.UI.Print_EventLog("배팅에 실패했습니다...".PadRight(30, ' '));
                player.ResumeGold(-BettingGold);

                Console.SetCursorPosition(startX, startY + 2);
                Console.WriteLine($"배팅에 실패했습니다...");
            }
            else
            {
                while (true)
                {
                    int playerMostNumber = player.Decks.OrderByDescending(x => x.Number).ToList().First().Number;
                    int dealerMostNumber = dealer.Decks.OrderByDescending(x => x.Number).ToList().First().Number;

                    if (playerMostNumber > dealerMostNumber)
                    {
                        Managers.UI.Print_EventLog("배팅에 성공했습니다!!!".PadRight(30, ' '));
                        player.ResumeGold(BettingGold * 2);

                        Console.SetCursorPosition(startX, startY + 2);
                        Console.WriteLine($"배팅에 성공했습니다!!!");
                        break;
                    }
                    else if (playerMostNumber < dealerMostNumber)
                    {
                        Managers.UI.Print_EventLog("배팅에 실패했습니다...".PadRight(30, ' '));
                        player.ResumeGold(-BettingGold);

                        Console.SetCursorPosition(startX, startY + 2);
                        Console.WriteLine($"배팅에 실패했습니다...");
                        break;
                    }
                    player.Decks.Remove(player.Decks.OrderByDescending(x => x.Number).ToList().First());
                    dealer.Decks.Remove(dealer.Decks.OrderByDescending(x => x.Number).ToList().First());
                }
            }

            BettingGold = 0;

            Thread.Sleep(2000);

            if(player.Gold <= 0)
            {
                Managers.UI.Print_EventLog("플레이어가 파산했습니다 ㅠㅠ".PadRight(30, ' '));

                Thread.Sleep(1000);

                Environment.Exit(0);
                return;
            }
            ReturnStart();
        }


        public CardSetType IsCardSet(List<Card> cards)
        {
            if (IsRoyalStraightFlush(cards))
                return CardSetType.RoyalStraightFlush;

            if (IsBackStraightFlush(cards))
                return CardSetType.BackStraightFlush;

            if (IsStraightFlush(cards))
                return CardSetType.StraightFlush;

            if (IsFourOfAKind(cards))
                return CardSetType.FourOfAKind;

            if (IsFullHouse(cards))
                return CardSetType.FullHouse;

            if (IsFlush(cards))
                return CardSetType.Flush;

            if (IsMountain(cards))
                return CardSetType.Mountain;

            if (IsBackStraight(cards))
                return CardSetType.BackStraight;

            if (IsStraight(cards))
                return CardSetType.Straight;

            if (IsThreeOfAKind(cards))
                return CardSetType.ThreeOfAKind;

            if (IsTwoPair(cards))
                return CardSetType.TwoPair;

            if (IsOnePair(cards))
                return CardSetType.OnePair;

            return CardSetType.None;
        }

        #region Check Card Set
        /// <summary>
        /// 주어진 카드 리스트가 로열 스트레이트 플러시인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>로열 스트레이트 플러시인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsRoyalStraightFlush(List<Card> cards)
        {
            int[] cardSet = new int[] { 1, 10, 11, 12, 13 };
            HashSet<int> numbers = new HashSet<int>();
            HashSet<int> patterns = new HashSet<int>();

            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < cardSet.Length; j++)
                {
                    if (cards[i].Number == cardSet[j])
                    {
                        numbers.Add(cards[i].Number);
                        patterns.Add(cards[i].Pattern);
                    }
                }
            }

            if (numbers.Count == 5)
            {
                if(patterns.Count == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 주어진 카드 리스트가 백 스트레이트 플러시인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>백 스트레이트 플러시인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsBackStraightFlush(List<Card> cards)
        {
            int[] cardSet = new int[] { 1, 2, 3, 4, 5 };        // 백 스트레이트
            HashSet<int> numbers = new HashSet<int>();          // 중복되지않는 배열
            HashSet<int> patterns = new HashSet<int>(); 

            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < cardSet.Length; j++)
                {
                    if (cards[i].Number == cardSet[j])
                    {
                        numbers.Add(cards[i].Number);
                        patterns.Add(cards[i].Pattern);
                    }
                }
            }

            if (numbers.Count == 5)
            {
                if(patterns.Count == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 주어진 카드 리스트가 스트레이트 플러시인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>스트레이트 플러시인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsStraightFlush(List<Card> cards)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                numbers.Add(cards[i].Number);
            }

            numbers.Sort();

            HashSet<int> patterns = new HashSet<int>();

            bool isCheck = true;
            int count = 0;
            for (int i = 1; i < numbers.Count; i++)
            {
                if (numbers[i] == numbers[i - 1] + 1)
                {
                    if (isCheck)
                    {
                        count++;
                        patterns.Add(cards[i].Pattern);
                    }
                }
                else
                {
                    if (isCheck)
                    {
                        isCheck = false;
                        count = 0;
                        patterns.Clear();
                    }
                }
            }

            if (count >= 4)
            {
                if (patterns.Count == 1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 주어진 카드 리스트가 포카드인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>포카드인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsFourOfAKind(List<Card> cards)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                numbers.Add(cards[i].Number);
            }

            numbers.Sort();

            bool isCheck = false;
            int count = 0;
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] == numbers[i + 1])
                {
                    if (!isCheck)
                    {
                        isCheck = true;
                        count++;
                    }
                    else
                    {
                        count++;
                        if (count == 3)
                        {
                            return true; // 네 개의 동일한 숫자가 있다면 true 반환
                        }
                    }
                }
                else
                {
                    isCheck = false;
                    count = 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 주어진 카드 리스트가 풀하우스인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>풀하우스인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsFullHouse(List<Card> cards)
        {
            return IsThreeOfAKind(cards) && IsOnePair(cards) && (!GetThreeOfAKind(cards).SequenceEqual(GetOnePair(cards)));
        }

        /// <summary>
        /// 주어진 카드 리스트가 플러시인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>플러시인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsFlush(List<Card> cards)
        {
            int[] patternCounts = new int[4]; // 4개의 패턴에 대한 카운트 배열

            foreach (Card card in cards)
            {
                patternCounts[card.Pattern - 1]++;
            }

            foreach (int count in patternCounts)
            {
                if (count == 5)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 주어진 카드 리스트가 마운틴인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>마운틴인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsMountain(List<Card> cards)
        {
            int[] cardSet = new int[] { 1, 10, 11, 12, 13 };
            HashSet<int> numbers = new HashSet<int>();

            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < cardSet.Length; j++)
                {
                    if (cards[i].Number == cardSet[j])
                    {
                        numbers.Add(cards[i].Number);
                    }
                }
            }

            if (numbers.Count == 5) return true;
            return false;
        }

        /// <summary>
        /// 주어진 카드 리스트가 백스트레이트인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>백스트레이트인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsBackStraight(List<Card> cards)
        {
            int[] cardSet = new int[] { 1, 2, 3, 4, 5 };        // 백 스트레이트
            HashSet<int> numbers = new HashSet<int>();          // 중복되지않는 배열

            for (int i = 0; i < cards.Count; i++)
            {
                for(int j = 0; j < cardSet.Length; j++)
                {
                    if (cards[i].Number == cardSet[j])
                    {
                        numbers.Add(cards[i].Number);
                    }
                }
            }

            if (numbers.Count == 5) return true;
            return false;
        }

        /// <summary>
        /// 주어진 카드 리스트가 스트레이트인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>스트레이트인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsStraight(List<Card> cards)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                numbers.Add(cards[i].Number);
            }

            numbers.Sort();

            bool isCheck = true;
            int count = 0;
            for(int i = 1; i < numbers.Count; i++)
            {
                if (numbers[i] == numbers[i - 1] + 1)
                {
                    if(isCheck)
                    {
                        count++;
                    }
                }
                else
                {
                    if(isCheck)
                    {
                        isCheck = false;
                        count = 0;
                    }
                }
            }

            if(count >= 4)
            {
                return true;
            }
            return false;
        }

        public List<int> GetThreeOfAKind(List<Card> cards)
        {
            List<int> numbers = new List<int>();
            List<int> set = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                numbers.Add(cards[i].Number);
            }

            numbers.Sort();

            bool isCheck = false;
            int count = 0;
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] == numbers[i + 1])
                {
                    if (!isCheck)
                    {
                        isCheck = true;
                        count++;
                        set.Add(numbers[i]);
                    }
                    else
                    {
                        count++;
                        set.Add(numbers[i]);
                        if (count == 2)
                        {
                            return set;
                        }
                    }
                }
                else
                {
                    isCheck = false;
                    count = 0;
                    set.Clear();
                }
            }
            return null;
        }

        /// <summary>
        /// 주어진 카드 리스트가 쓰리 오브 어 카인드인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>쓰리 오브 어 카인드인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsThreeOfAKind(List<Card> cards)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                numbers.Add(cards[i].Number);
            }

            numbers.Sort();

            bool isCheck = false;
            int count = 0;
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] == numbers[i + 1])
                {
                    if (!isCheck)
                    {
                        isCheck = true;
                        count++;
                    }
                    else
                    {
                        count++;
                        if (count == 2)
                        {
                            return true; // 세 개의 동일한 숫자가 있다면 true 반환
                        }
                    }
                }
                else
                {
                    isCheck = false;
                    count = 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 주어진 카드 리스트가 투 페어인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>투 페어인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsTwoPair(List<Card> cards)
        {
            List<int> numbers = new List<int>();
            for(int i = 0; i < cards.Count; i++)
            {
                numbers.Add(cards[i].Number);
            }

            numbers.Sort();

            int count = 0;
            int targetNumber = -1;
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] == numbers[i + 1])
                {
                    targetNumber = numbers[i];
                    count++;
                    break;
                }
            }

            for(int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] == numbers[i + 1])
                {
                    if(targetNumber != numbers[i] && count == 1)
                    {
                        count++;
                        break;
                    }
                }
            }

            if (count == 2) return true;
            return false;
        }

        public List<int> GetOnePair(List<Card> cards)
        {
            List<int> numbers = new List<int>();
            List<int> set = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                numbers.Add(cards[i].Number);
            }

            numbers.Sort();

            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] == numbers[i + 1])
                {
                    set.Add(numbers[i]);
                    set.Add(numbers[i + 1]);
                    break;
                }
            }
            return set;
        }

        /// <summary>
        /// 주어진 카드 리스트가 원 페어인지 확인합니다.
        /// </summary>
        /// <param name="cards">확인할 카드 리스트</param>
        /// <returns>원 페어인 경우 true, 그렇지 않은 경우 false를 반환합니다.</returns>
        public bool IsOnePair(List<Card> cards)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < cards.Count; i++)
            {
                numbers.Add(cards[i].Number);
            }

            numbers.Sort();

            for (int i = 0; i < numbers.Count - 1; i++)
            {
                if (numbers[i] == numbers[i + 1])
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
