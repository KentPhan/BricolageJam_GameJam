using UnityEngine;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        // Prefabs
        [SerializeField] private PlayerScript PlayerPrefab;
        [SerializeField] private GameObject PlayerSpawn;
        [SerializeField] private BasicEnemyScript EnemyPrefab;
        [SerializeField] private WeaponPieceScript WeaponPrefab;


        // Boundaries
        [SerializeField] private GameObject WidthBoundary;
        [SerializeField] private GameObject HeightBoundary;

        // Enemy Stuff
        [SerializeField] private float InitialEnemySpawnRate = 5.0f;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float SpawnRateSpeedUp = 0.9f;

        [SerializeField] private float SpawnRateStageTimer = 10.0f;

        private float m_CurrentEnemySpawnTimer;
        private float m_CurrentEnemySpawnRate;
        private float m_CurrentStageTimer;


        // Weapon Spawn Stuff
        [SerializeField] private float WeaponSpawnRate = 10.0f;
        private float m_CurrentWeaponSpawnTimer;


        public static GameManager Instance;

        private PlayerScript m_CurrentPlayer;

        private Vector2 m_Boundary;

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

            // Boundaries
            m_Boundary = new Vector2(Mathf.Abs(WidthBoundary.transform.position.x), Mathf.Abs((HeightBoundary.transform.position.y)));

            // Enemy Stuff
            m_CurrentEnemySpawnTimer = InitialEnemySpawnRate;
            m_CurrentEnemySpawnRate = InitialEnemySpawnRate;
            m_CurrentStageTimer = SpawnRateStageTimer;

            // Weapon Stuff
            m_CurrentWeaponSpawnTimer = WeaponSpawnRate;
        }

        // Update is called once per frame
        void Update()
        {
            float m_deltaTime = Time.deltaTime;

            // Enemy Spawner
            if (m_CurrentEnemySpawnTimer <= 0)
            {
                Vector2 m_SpawnPosition = new Vector2(0, 0);
                BasicEnemyScript m_newEnemy = Instantiate(EnemyPrefab, m_SpawnPosition, Quaternion.identity);
                m_newEnemy.transform.SetParent(this.transform);

                m_CurrentEnemySpawnTimer = m_CurrentEnemySpawnRate;
            }


            // Stage Enemy Rate Adjuster
            if (m_CurrentStageTimer <= 0)
            {
                m_CurrentEnemySpawnRate *= SpawnRateSpeedUp;

                m_CurrentStageTimer = SpawnRateStageTimer;
            }

            m_CurrentEnemySpawnTimer -= m_deltaTime;
            m_CurrentStageTimer -= m_deltaTime;


            // Weapon Spawner
            if (m_CurrentEnemySpawnTimer <= 0)
            {
                Vector2 m_SpawnPosition = new Vector2(0, 0);
                WeaponPieceScript m_newWeapon = Instantiate(WeaponPrefab, m_SpawnPosition, Quaternion.identity);
                m_newWeapon.transform.SetParent(this.transform);
                m_newWeapon.SetMawDirectionBaby(Vector2.down);

                m_CurrentWeaponSpawnTimer = WeaponSpawnRate;
            }
            m_CurrentWeaponSpawnTimer -= m_deltaTime;
        }

        public GameObject GetPlayer()
        {
            return m_CurrentPlayer.gameObject;
        }

        public Vector2 GetBoundaries()
        {
            return m_Boundary;
        }

        public void TriggerGameOver()
        {
            m_CurrentEnemySpawnTimer = InitialEnemySpawnRate;
            m_CurrentEnemySpawnRate = InitialEnemySpawnRate;
            m_CurrentStageTimer = SpawnRateStageTimer;
        }
    }
}
