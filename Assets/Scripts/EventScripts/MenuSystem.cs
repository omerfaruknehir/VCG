using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EventScripts
{
    public class MenuSystem : MonoBehaviour
    {
        public GameObject loadingScreen;

        public IEnumerator UnloadScene()
        {
            Scene scene = SceneManager.GetSceneByName("GameScene");
            while (!scene.isLoaded)
            {
                yield return new WaitForEndOfFrame();
            }
            loadingScreen.SetActive(false);
            SceneManager.UnloadSceneAsync("MainMenu");
            yield return null;
        }

        public void StartMultiplayerGame()
        {
            loadingScreen.SetActive(true);
            StaticScripts.GlobalVariables.Set("Gamemode", (byte)2);
            SceneManager.LoadSceneAsync("GameScene");
            StartCoroutine(UnloadScene());
        }

        void Init()
        {
            StaticScripts.UIManager.ConfirmBox("Deneme", (bool data) => Debug.Log(data));
        }

        public void Start()
        {
            byte quitStatus = StaticScripts.GlobalVariables.Get<byte>("QuitStatus");
            if (quitStatus != 0)
            {
                Init();
            }
            else
            {
                Init();
            }
        }
    }
}