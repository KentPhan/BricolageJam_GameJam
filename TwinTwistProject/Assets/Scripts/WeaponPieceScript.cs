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
        private WeaponStates m_CurrentWeaponState = WeaponStates.DORMANT;

        [SerializeField] private float m_MaxEnergy = 100;
        [SerializeField] private float m_RechargeRate = 1.0f;

        [SerializeField] [Range(0.0f, 1.0f)] private float m_MinimumAlpha = 0.1f;
        private float m_CurrentEnergy;
        private Vector2 m_Direction;


        private Rigidbody2D m_RigidBody;
        private MeshRenderer m_MeshRenderer;



        // Start is called before the first frame update
        void Start()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            if (transform.parent.name == "WeaponShapes")
            {
                m_CurrentWeaponState = WeaponStates.ACTIVE;
                Destroy(GetComponent<Rigidbody2D>());
            }
            else
            {
                m_RigidBody = GetComponent<Rigidbody2D>();
                m_CurrentWeaponState = WeaponStates.DORMANT;
            }
            m_CurrentEnergy = m_MaxEnergy;
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.Instance.GetGameState() == GameStates.PLAY)
            {
                switch (m_CurrentWeaponState)
                {
                    case WeaponStates.DORMANT:
                        m_RigidBody.MovePosition(m_RigidBody.position + m_Direction * Speed * Time.deltaTime);

                        if (GameManager.Instance.IsOutsideBoundaries(m_RigidBody.position))
                        {
                            Destroy(this.gameObject);
                        }
                        break;
                    case WeaponStates.RECHARGING:
                        m_CurrentEnergy += Time.deltaTime * m_RechargeRate;

                        if (m_CurrentEnergy >= m_MaxEnergy)
                        {
                            m_CurrentEnergy = m_MaxEnergy;
                            m_CurrentWeaponState = WeaponStates.ACTIVE;
                        }
                        else
                        {
                            m_MeshRenderer.material.color = new Color(m_MeshRenderer.material.color.r, m_MeshRenderer.material.color.g, m_MeshRenderer.material.color.b, Mathf.Lerp(m_MinimumAlpha, 1.0f, m_CurrentEnergy / m_MaxEnergy));
                        }

                        break;
                    case WeaponStates.ACTIVE:

                        if (m_CurrentEnergy <= 0.0f)
                        {
                            m_MeshRenderer.material.color = new Color(m_MeshRenderer.material.color.r, m_MeshRenderer.material.color.g, m_MeshRenderer.material.color.b, m_MinimumAlpha);
                            m_CurrentWeaponState = WeaponStates.RECHARGING;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
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
            m_CurrentWeaponState = WeaponStates.ACTIVE;
            this.gameObject.transform.SetParent(GameManager.Instance.GetPlayer().GetComponent<PlayerScript>().GetWeaponParents());
        }

        public bool CanKill()
        {
            return m_CurrentWeaponState == WeaponStates.ACTIVE;
        }

        public void DecreaseEnergy(float i_Decrease)
        {
            m_CurrentEnergy -= i_Decrease;
        }

        public void OnCollisionEnter2D(Collision2D i_collider)
        {

            if (this.transform.parent.name == "WeaponShapes")
            {
                if (i_collider.gameObject.CompareTag("Weapon"))
                {
                    WeaponPieceScript l_weapon = i_collider.gameObject.GetComponent<WeaponPieceScript>();
                    if (l_weapon.m_CurrentWeaponState == WeaponStates.DORMANT)
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
