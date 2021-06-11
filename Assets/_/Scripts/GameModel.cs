using System.Collections.Generic;

namespace Assets.__.Scripts
{
    public class GameModel
    {
        private static GameModel instance;
        public static GameModel GetInstance()
        {
            if (instance == null)
            {
                instance = new GameModel();
            }
            return instance;
        }

        public readonly List<bool> ActivePanels = new List<bool>();
    }
}