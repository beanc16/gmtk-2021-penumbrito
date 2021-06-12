using Assets.__.Scripts.PlayerScripts;
using System;
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
                instance.Setup();
            }
            return instance;
        }

        public readonly List<bool> ActivePanels = new List<bool>();
        public readonly Dictionary<int, Light2D> IndexToLight = new Dictionary<int, Light2D>();
        public readonly Dictionary<int, Light2D> IndexToDark = new Dictionary<int, Light2D>();
        public readonly Dictionary<PlayerEffect, int> ActivePlayerEffects = new Dictionary<PlayerEffect, int>();
        public readonly Dictionary<int, PlayerEffect> IndexToPlayerEffect = new Dictionary<int, PlayerEffect>();
        public int CountInWinZone;
        public bool ReloadScene;

        private QuantumControlScript registeredQuantumControlScript;

        private void Setup()
        {
            foreach (PlayerEffect effect in Enum.GetValues(typeof(PlayerEffect)))
            {
                ActivePlayerEffects.Add(effect, 0);
            }
        }

        public void RegisterPlayerControlScript(QuantumControlScript quantumControlScript)
        {
            this.registeredQuantumControlScript = quantumControlScript;
        }

        public void UpdatePlayerEffect(int index, bool add)
        {
            if (add)
            {
                ActivePlayerEffects[IndexToPlayerEffect[index]]++;
            }
            else
            {
                ActivePlayerEffects[IndexToPlayerEffect[index]]--;
            }

            this.registeredQuantumControlScript.UpdatePlayerEffects();
        }

        public void Reset()
        {
            this.ActivePanels.Clear();
            this.IndexToDark.Clear();
            this.IndexToLight.Clear();
            this.IndexToPlayerEffect.Clear();
            this.ActivePlayerEffects.Clear();
            this.registeredQuantumControlScript = null;
            this.CountInWinZone = 0;
            this.ReloadScene = false;

            this.Setup();
        }
    }
}