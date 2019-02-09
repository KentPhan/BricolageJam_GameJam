using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private GameObject StartScreen;
        [SerializeField] private GameObject PlayScreen;
        [SerializeField] private TextMeshProUGUI ScoreValue;
        [SerializeField] private GameObject GameOverScreen;
        [SerializeField] private TextMeshProUGUI FinalScoreValue;




        public static CanvasManager Instance;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;

            else if (Instance != this)
                Destroy(this);

            DontDestroyOnLoad(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            StartScreen.SetActive(true);
            PlayScreen.SetActive(false);
            GameOverScreen.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DisplayStart()
        {
            StartScreen.SetActive(true);
            PlayScreen.SetActive(false);
            GameOverScreen.SetActive(false);
        }

        public void DisplayGameOver(int i_FinalScore)
        {
            StartScreen.SetActive(false);
            PlayScreen.SetActive(false);
            GameOverScreen.SetActive(true);
            FinalScoreValue.text = $"Final Score: {i_FinalScore}";
        }

        public void DisplayPlay()
        {
            StartScreen.SetActive(false);
            PlayScreen.SetActive(true);
            GameOverScreen.SetActive(false);
        }

        public void ResetScore()
        {
            ScoreValue.text = "0";
        }

        public void SetScore(int value)
        {
            ScoreValue.text = value.ToString();
        }

    }
}
