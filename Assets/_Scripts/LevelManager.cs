using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Button infoButton;
        [SerializeField] private Button reloadSceneButton;
        [SerializeField] private Button settingsButton;


        private void Start()
        {
            infoButton.onClick.RemoveAllListeners();
            infoButton.onClick.AddListener(InfoButtonListener);
            reloadSceneButton.onClick.RemoveAllListeners();
            reloadSceneButton.onClick.AddListener(ReloadButtonListener);
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(SettingsButtonListener);
        }


        private void InfoButtonListener()
        {
            throw new NotImplementedException();
        }
        
        private void ReloadButtonListener()
        {
            SceneManager.LoadScene(0);
        }

        private void SettingsButtonListener()
        {
            throw new NotImplementedException();
        }
    }
}