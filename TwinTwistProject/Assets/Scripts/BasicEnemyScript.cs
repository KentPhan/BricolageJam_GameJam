using UnityEngine;

namespace Assets
{
    public class BasicEnemyScript : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ParticleDeathPrefab;
        [SerializeField] private float m_EnergyToKill = 10.0f;
        [SerializeField]
        private float m_Speed;




        private Rigidbody2D m_RigidBody;

        // Start is called before the first frame update
        void Start()
        {

            m_RigidBody = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GetGameState() == GameStates.PLAY)
            {
                GameObject l_Target = GameManager.Instance.GetPlayer();
                Vector2 l_Direction = (l_Target.GetComponent<Rigidbody2D>().position - m_RigidBody.position).normalized;
                m_RigidBody.MovePosition(m_RigidBody.position + l_Direction * m_Speed * Time.deltaTime);

                if (GameManager.Instance.IsOutsideBoundaries(m_RigidBody.position))
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Die();
            }
        }

        public void OnCollisionEnter2D(Collision2D i_collider)
        {
            if (i_collider.gameObject.CompareTag("Weapon"))
            {
                //WeaponPieceScript l_Weapon = i_collider.gameObject.GetComponent<WeaponPieceScript>();
                //if (l_Weapon == null)
                //    l_Weapon = i_collider.gameObject.GetComponentInParent<WeaponPieceScript>();
                //if (l_Weapon)
                //    Debug.LogError("Could not find weapon script");
                //if (l_Weapon.CanKill())
                //{
                //    l_Weapon.DecreaseEnergy(m_EnergyToKill);
                //    GameManager.Instance.AddToScore();
                //    Die();
                //}

            }
            else if (i_collider.gameObject.CompareTag("Player"))
            {
                i_collider.gameObject.GetComponent<PlayerScript>().Die();
                GameManager.Instance.TriggerGameOver();
            }
        }

        public void Die()
        {
            Instantiate(ParticleDeathPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        public float GetEnergyToll()
        {
            return m_EnergyToKill;
        }
    }
}
