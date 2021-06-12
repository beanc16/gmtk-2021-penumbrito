using System.Collections.Generic;
using UnityEngine;

namespace Assets.__.Scripts.EntangleView
{
    public class GameSoundController : MonoBehaviour
    {
        private const float PER_SEC_CHANGE = 2f;

        private GameModel gameModel;

        private Dictionary<string, float> baseVolume = new Dictionary<string, float>();
        private Dictionary<string, float> currentVolume = new Dictionary<string, float>();
        private Dictionary<string, bool> musicState = new Dictionary<string, bool>();

        private void Start()
        {
            gameModel = GameModel.GetInstance();
            AudioController.StopMusic("MainMenu");

            this.SetupForSong("Modern");
            this.SetupForSong("Post");
            this.SetupForSong("Pre");
            this.SetupForSong("Rebuilt");

            AudioController.PlayMusic("Ambient");

            GameModel.GetInstance().OnActivePanelChange += this.OnActivePanelChanged;

            this.OnActivePanelChanged();
        }

        private void SetupForSong(string song)
        {
            AudioController.PlayMusic(song);
            baseVolume.Add(song, AudioController.GetMusicVolume(song));
            currentVolume.Add(song, 0);
            musicState.Add(song, false);
        }

        private void OnDestroy()
        {
            GameModel.GetInstance().OnActivePanelChange -= this.OnActivePanelChanged;
        }

        private void OnActivePanelChanged()
        {
            musicState["Modern"] = gameModel.ActivePanels[0];
            musicState["Post"] = gameModel.ActivePanels[1];
            musicState["Pre"] = gameModel.ActivePanels[2];
            musicState["Rebuilt"] = gameModel.ActivePanels[3];
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            foreach (var sound in this.baseVolume)
            {
                if (musicState[sound.Key] && currentVolume[sound.Key] < 1f)
                {
                    currentVolume[sound.Key] = Mathf.Min(1f, currentVolume[sound.Key] + PER_SEC_CHANGE * deltaTime);
                    AudioController.FadeMusic(sound.Key, currentVolume[sound.Key]);
                }

                if (musicState[sound.Key] == false && currentVolume[sound.Key] > 0f)
                {
                    currentVolume[sound.Key] = Mathf.Max(0f, currentVolume[sound.Key] - PER_SEC_CHANGE * deltaTime);
                    AudioController.FadeMusic(sound.Key, currentVolume[sound.Key]);
                }
            }
        }
    }
}