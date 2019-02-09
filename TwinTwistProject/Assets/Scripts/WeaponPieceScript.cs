using System;
using UnityEngine;

namespace Assets
{
    public enum WeaponStates
    {
        DORMANT,
        RECHARGING,
        ACTIVE
    }

    public class WeaponPieceScript : MonoBehaviour
    {

        [SerializeField]
        private float Speed = 2.0f;
        [SerializeField]
        private WeaponStates m_WeaponState = WeaponStates.DORMANT;
        private Vector2 m_Direction;


        private Rigidbody2D m_RigidBody;



        // Start is called before the first frame update
        void Start()
        {

            if (transform.parent.name == "WeaponShapes")
            {
                m_WeaponState = WeaponStates.ACTIVE;
                Destroy(GetComponent<Rigidbody2D>());
            }
            else
            {
                m_RigidBody = GetComponent<Rigidbody2D>();
                m_WeaponState = WeaponStates.DORMANT;
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GetGameState() == GameStates.PLAY)
            {
                switch (m_WeaponState)
                {
                    case WeaponStates.DORMANT:
                        m_RigidBody.MovePosition(m_RigidBody.position + m_Direction * Speed * Time.deltaTime);

                        if (GameManager.Instance.IsOutsideBoundaries(m_RigidBody.position))
                        {
                            Destroy(this.gameObject);
                        }
                        break;
                    case WeaponStates.RECHARGING:
                        break;
                    case WeaponStates.ACTIVE:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //if (!CanKill && GetComponent<Rigidbody2D>() != null)
                //{

                //}
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
            m_WeaponState = WeaponStates.ACTIVE;
            this.gameObject.transform.SetParent(GameManager.Instance.GetPlayer().GetComponent<PlayerScript>().GetWeaponParents());
        }

        public bool CanKill()
        {
            return m_WeaponState == WeaponStates.ACTIVE;
        }

        public void OnCollisionEnter2D(Collision2D i_collider)
        {

            if (this.transform.parent.name == "WeaponShapes")
            {
                if (i_collider.gameObject.CompareTag("Weapon"))
                {
                    WeaponPieceScript l_weapon = i_collider.gameObject.GetComponent<WeaponPieceScript>();
                    if (l_weapon.m_WeaponState == WeaponStates.DORMANT)
                    {
                        l_weapon.ActivateArms();
                    }
                }
                else if (i_collider.gameObject.CompareTag("Player"))
                {

                }
            }
            else
            {
                
            }
        }
    }
}
