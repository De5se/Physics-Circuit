using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Button reloadSceneButton;

        private void Start()
        {
            reloadSceneButton.onClick.RemoveAllListeners();
            reloadSceneButton.onClick.AddListener(ReloadButtonListener);
        }
        
        private void ReloadButtonListener()
        {
            SceneManager.LoadScene(0);
        }
    }
}