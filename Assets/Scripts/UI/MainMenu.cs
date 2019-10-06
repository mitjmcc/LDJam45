namespace HomeTakeover.UI
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;
    using Luminosity.IO;

    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainParent = null;
        [SerializeField]
        private GameObject mainSelected = null;
        [SerializeField]
        private GameObject creditsParent = null;
        [SerializeField]
        private GameObject creditsSelected = null;
        [SerializeField]
        private string scene = "";

        private GameObject currentSelected;
        private bool inMain;
        private bool inCredits;

        void Start()
        {
            inMain = true;
            inCredits = false;
            EventSystem.current.SetSelectedGameObject(mainSelected);
        }

        void Update()
        {
            if (inMain)
            {
                if (Input.GetKey(KeyCode.Escape))
                    Application.Quit();
                if (inMain && EventSystem.current.currentSelectedGameObject == null)
                {
                    if (inCredits)
                        EventSystem.current.SetSelectedGameObject(creditsSelected);
                    else
                        EventSystem.current.SetSelectedGameObject(mainSelected);
                }

                currentSelected = EventSystem.current.currentSelectedGameObject;

                if (InputManager.GetButton("UI_Up"))
                    Navigator.Navigate("UI_Up", currentSelected);
                if (InputManager.GetButton("UI_Down"))
                    Navigator.Navigate("UI_Down", currentSelected);
                if (InputManager.GetButton("UI_Submit"))
                    Navigator.CallSubmit();
                if (inCredits && InputManager.GetButton("UI_Cancel"))
                    GoToMain();
            }
            else if (inCredits)
            {
                if (Input.GetKey(KeyCode.Escape))
                    GoToMain();

                currentSelected = EventSystem.current.currentSelectedGameObject;

                if (InputManager.GetButton("UI_Up"))
                    Navigator.Navigate("UI_Up", currentSelected);
                if (InputManager.GetButton("UI_Down"))
                    Navigator.Navigate("UI_Down", currentSelected);
                if (InputManager.GetButton("UI_Submit"))
                    Navigator.CallSubmit();
                if (inCredits && InputManager.GetButton("UI_Cancel"))
                    GoToMain();
            }
        }

        public void GoToMain()
        {
            inMain = true;
            inCredits = false;
            mainParent.SetActive(true);
            creditsParent.SetActive(false);
            EventSystem.current.SetSelectedGameObject(mainSelected);
        }

        public void Play()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            SceneManager.LoadScene(this.scene);
        }

        public void GoToCredits()
        {
            inMain = false;
            inCredits = true;
            mainParent.SetActive(false);
            creditsParent.SetActive(true);
            EventSystem.current.SetSelectedGameObject(creditsSelected);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}