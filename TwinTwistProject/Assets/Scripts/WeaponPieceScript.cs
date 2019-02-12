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
        [SerializeField] private Color m_ChargeColorTint;
        private Color m_OriginalColor;
        private float m_CurrentEnergy;
        private Vector2 m_Direction;


        private Rigidbody2D m_RigidBody;
        private MeshRenderer[] m_MeshRenderers;
        private Collider2D[] m_Colliders;

        // Store
        private int m_WeaponLayerMask;
        private int m_WeaponLayerCooldownMask;

        void Awake()
        {
            m_WeaponLayerMask = LayerMask.NameToLayer("Weapon");
            m_WeaponLayerCooldownMask = LayerMask.NameToLayer("WeaponCooldown");
        }

        // Start is called before the first frame update
        void Start()
        {
            m_MeshRenderers = GetComponentsInChildren<MeshRenderer>();
            m_Colliders = GetComponentsInChildren<Collider2D>();

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

            m_OriginalColor = GetComponentInChildren<MeshRenderer>().material.color;

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

                        Debug.Log(m_CurrentEnergy);

                        if (m_CurrentEnergy >= m_MaxEnergy)
                        {
                            m_CurrentEnergy = m_MaxEnergy;
                            m_CurrentWeaponState = WeaponStates.ACTIVE;
                            GoToKillLayer();
                        }
                        else
                        {
                            foreach (MeshRenderer l_Mesh in m_MeshRenderers)
                            {
                                l_Mesh.material.color = new Color(l_Mesh.material.color.r, l_Mesh.material.color.g, l_Mesh.material.color.b, Mathf.Lerp(m_MinimumAlpha, 1.0f, m_CurrentEnergy / m_MaxEnergy)) * m_ChargeColorTint;
                            }
                        }

                        break;
                    case WeaponStates.ACTIVE:

                        if (m_CurrentEnergy <= 0.0f)
                        {
                            GoToChargeLayer();
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
        private void GoToKillLayer()
        {
            foreach (Collider2D l_Collider in m_Colliders)
            {
                l_Collider.gameObject.layer = m_WeaponLayerMask;
            }

            foreach (MeshRenderer l_Mesh in m_MeshRenderers)
            {
                l_Mesh.material.color = m_OriginalColor;
            }
        }

        private void GoToChargeLayer()
        {
            foreach (Collider2D l_Collider in m_Colliders)
            {
                l_Collider.gameObject.layer = m_WeaponLayerCooldownMask;
            }
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

        public WeaponStates GetWeaponState()
        {
            return m_CurrentWeaponState;
        }
    }
}
