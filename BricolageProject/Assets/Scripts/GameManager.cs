using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
                Tuple<Vector2, Vector2> l_randShit = GetRandomSpawnOutsideOfBoundary();
                Vector2 m_SpawnPosition = l_randShit.Item1;
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
            if (m_CurrentWeaponSpawnTimer <= 0)
            {
                Tuple<Vector2, Vector2> l_randShit = GetRandomSpawnOutsideOfBoundary();
                Vector2 m_SpawnPosition = l_randShit.Item1;
                WeaponPieceScript m_newWeapon = Instantiate(WeaponPrefab, m_SpawnPosition, Quaternion.identity);
                m_newWeapon.transform.SetParent(this.transform);
                m_newWeapon.SetMawDirectionBaby(l_randShit.Item2);

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

        public Tuple<Vector2, Vector2> GetRandomSpawnOutsideOfBoundary()
        {
            float l_offset = 1.0f;

            Vector2 l_RandomPosition = Vector2.zero;
            Vector2 l_DirectionTowardsCenter = Vector2.zero;
            int l_side = Random.Range(1, 5);
            Debug.Log(l_side);
            switch (l_side)
            {
                case 1:
                    l_RandomPosition = new Vector2((m_Boundary.x * -1) - l_offset, Random.Range(m_Boundary.y * -1, m_Boundary.y));
                    l_DirectionTowardsCenter = Vector2.right;
                    break;
                case 2:
                    l_RandomPosition = new Vector2((m_Boundary.x * 1) + l_offset, Random.Range(m_Boundary.y * -1, m_Boundary.y));
                    l_DirectionTowardsCenter = Vector2.left;
                    break;
                case 3:
                    l_RandomPosition = new Vector2(Random.Range(m_Boundary.x * -1, m_Boundary.x), (m_Boundary.y * -1) - l_offset);
                    l_DirectionTowardsCenter = Vector2.up;
                    break;
                case 4:
                    l_RandomPosition = new Vector2(Random.Range(m_Boundary.x * -1, m_Boundary.x), (m_Boundary.y * 1) + l_offset);
                    l_DirectionTowardsCenter = Vector2.down;
                    break;
                default:
                    Debug.Log("Unknown Side Picked");
                    break;
            }

            return new Tuple<Vector2, Vector2>(l_RandomPosition, l_DirectionTowardsCenter);
        }
    }
}
