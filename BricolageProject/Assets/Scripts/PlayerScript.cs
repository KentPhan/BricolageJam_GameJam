using UnityEngine;

namespace Assets
{
    public class PlayerScript : MonoBehaviour
    {

        [SerializeField] private float m_Speed = 100;

        private Rigidbody2D m_RigidBody;

        // Start is called before the first frame update
        void Start()
        {
            m_RigidBody = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 l_velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * m_Speed;

            m_RigidBody.position += (l_velocity * Time.deltaTime);
        }
    }
}
