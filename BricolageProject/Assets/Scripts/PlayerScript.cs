using UnityEngine;

namespace Assets
{
    public class PlayerScript : MonoBehaviour
    {

        [SerializeField] private float m_Speed = 10;
        [SerializeField] private float m_RotationSpeed = 5;
        [SerializeField] private bool m_Invert = true;
        [SerializeField] private GameObject m_WeaponCenter;

        private Rigidbody2D m_RigidBody;

        private Vector2 m_Boundaries;

        // Start is called before the first frame update
        void Start()
        {
            m_RigidBody = GetComponent<Rigidbody2D>();
            m_Boundaries = GameManager.Instance.GetBoundaries();
        }

        // Update is called once per frame
        void Update()
        {
            // Player Movement
            Vector2 l_velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * m_Speed;
            Vector2 l_newPosition = (m_RigidBody.position + (l_velocity * Time.deltaTime));

            // Restrict based upon boundaries
            if (l_newPosition.x < (m_Boundaries.x * -1) || l_newPosition.x > (m_Boundaries.x))
                l_newPosition.x = m_RigidBody.position.x;

            if (l_newPosition.y < (m_Boundaries.y * -1) || l_newPosition.y > (m_Boundaries.y))
                l_newPosition.y = m_RigidBody.position.y;

            m_RigidBody.position = l_newPosition;


            // Player Weapon Rotation
            float l_spin = Input.GetAxis("Spin");
            float l_direction = m_Invert ? -1 : 1;
            m_WeaponCenter.transform.RotateAround(m_WeaponCenter.transform.position, Vector3.forward, l_spin * m_RotationSpeed * l_direction);
        }
    }
}
