using UnityEngine;

namespace Assets
{
    public class WeaponPieceScript : MonoBehaviour
    {
        public bool CanKill { get; private set; }

        [SerializeField]
        private float Speed = 2.0f;
        private Vector2 m_Direction;


        private Rigidbody2D m_RigidBody;


        // Start is called before the first frame update
        void Start()
        {

            if (transform.parent.name == "WeaponShapes")
            {
                CanKill = true;
                Destroy(GetComponent<Rigidbody2D>());
            }
            else
            {
                m_RigidBody = GetComponent<Rigidbody2D>();
                CanKill = false;
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GetGameState() == GameStates.PLAY)
            {
                if (!CanKill && GetComponent<Rigidbody2D>() != null)
                {
                    m_RigidBody.MovePosition(m_RigidBody.position + m_Direction * Speed * Time.deltaTime);

                    if (GameManager.Instance.IsOutsideBoundaries(m_RigidBody.position))
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        public void SetMawDirectionBaby(Vector2 i_Direction)
        {
            m_Direction = i_Direction;
        }


        public void ActivateArms()
        {
            Destroy(GetComponent<Rigidbody2D>());
            CanKill = true;
        }

        public void OnCollisionEnter2D(Collision2D i_collider)
        {

            if (this.transform.parent.name == "WeaponShapes")
            {
                if (i_collider.gameObject.CompareTag("Weapon"))
                {
                    WeaponPieceScript l_weapon = i_collider.gameObject.GetComponent<WeaponPieceScript>();
                    if (!l_weapon.CanKill)
                    {
                        l_weapon.ActivateArms();
                        l_weapon.gameObject.transform.SetParent(GameManager.Instance.GetPlayer().GetComponent<PlayerScript>().GetWeaponParents());
                    }
                }
                else if (i_collider.gameObject.CompareTag("Player"))
                {

                }
            }
        }
    }
}
