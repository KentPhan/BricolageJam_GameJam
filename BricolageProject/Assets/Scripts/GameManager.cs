using UnityEngine;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private PlayerScript PlayerPrefab;
        [SerializeField] private GameObject PlayerSpawn;
        [SerializeField] private BasicEnemyScript EnemyPrefab;
        [SerializeField] private WeaponPieceScript WeaponPrefab;

        public static GameManager Instance;
        private PlayerScript m_CurrentPlayer;

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
            // Spawn Player Object
            m_CurrentPlayer = Instantiate(PlayerPrefab, PlayerSpawn.transform.position, Quaternion.identity);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject GetPlayer()
        {
            return PlayerPrefab.gameObject;
        }
    }
}
