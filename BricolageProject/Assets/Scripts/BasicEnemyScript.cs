using UnityEngine;

namespace Assets
{
    public class BasicEnemyScript : MonoBehaviour
    {
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
            GameObject l_Target = GameManager.Instance.GetPlayer();
            Vector2 l_Direction = (l_Target.GetComponent<Rigidbody2D>().position - m_RigidBody.position).normalized;
            m_RigidBody.MovePosition(m_RigidBody.position + l_Direction * m_Speed * Time.deltaTime);
        }

        public void OnTriggerEnter2D(Collider2D i_collider)
        {
            if (i_collider.gameObject.CompareTag("Weapon"))
            {
                if (i_collider.gameObject.GetComponent<WeaponPieceScript>().CanKill)
                    Destroy(this.gameObject);
            }
            else if (i_collider.gameObject.CompareTag("Player"))
            {
                Destroy(i_collider.gameObject);
                GameManager.Instance.TriggerGameOver();
            }
        }
    }
}
