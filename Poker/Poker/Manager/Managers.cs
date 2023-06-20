using FATCAT.Threading;
using Poker.Cores.Manager;

namespace Poker.Manager
{
    public class Managers : Core
    {
        private static Managers instance;

        private Managers() { }

        public static Managers Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Managers();
                }
                return instance;
            }
        }

        private readonly GameManager game = new GameManager();
        private readonly UIManager ui = new UIManager();

        public static GameManager Game => Instance.game;
        public static UIManager UI => Instance.ui;

        public override void Start()
        {
            UI.Init();
            Game.Init();
            base.Start();
        }
    }
}
