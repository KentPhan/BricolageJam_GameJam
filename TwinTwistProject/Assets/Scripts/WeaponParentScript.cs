using Assets;
using UnityEngine;

public class WeaponParentScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnCollisionEnter2D(Collision2D i_collision)
    {
        if (i_collision.gameObject.CompareTag("Weapon"))
        {
            WeaponPieceScript l_weapon = i_collision.gameObject.GetComponent<WeaponPieceScript>();
            if (l_weapon.GetWeaponState() == WeaponStates.DORMANT)
            {
                l_weapon.ActivateArms();
            }
        }
        else if (i_collision.gameObject.CompareTag("Player"))
        {

        }
        else if (i_collision.gameObject.CompareTag("Enemy"))
        {
            WeaponPieceScript l_Weapon = i_collision.otherCollider.gameObject.GetComponentInParent<WeaponPieceScript>();
            if (l_Weapon == null)
                Debug.LogError("Could not Find Weapon");

            BasicEnemyScript l_Enemy = i_collision.gameObject.GetComponent<BasicEnemyScript>();
            if (l_Enemy == null)
                Debug.LogError("Could not Find Enemy");

            l_Weapon.DecreaseEnergy(l_Enemy.GetEnergyToll());
            l_Enemy.Die();
            GameManager.Instance.AddToScore();
        }
    }
}
