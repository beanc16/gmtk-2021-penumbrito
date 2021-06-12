using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;

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
        public readonly Dictionary<int, Light2D> IndexToLight = new Dictionary<int, Light2D>();
        public readonly Dictionary<int, Light2D> IndexToDark = new Dictionary<int, Light2D>();
    }
}