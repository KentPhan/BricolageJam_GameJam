using Assets;
using UnityEngine;

public class PlayerBulletComponent : MonoBehaviour
{

    [SerializeField] private float m_Speed = 1.0f;
    private Vector2 m_Direction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsOutsideBoundaries(transform.position))
            Destroy(gameObject);
    }

    public void LaunchBullet(Vector2 i_Direction)
    {
        m_Direction = i_Direction;
        GetComponent<Rigidbody2D>().velocity = m_Direction * m_Speed;
    }


    public void OnCollisionEnter2D(Collision2D i_collider)
    {
        //Debug.Log("Bullet collided:" + i_collider.gameObject.tag);
        if (i_collider.gameObject.CompareTag("Enemy"))
        {
            BasicEnemyScript l_Enemy = i_collider.gameObject.GetComponent<BasicEnemyScript>();
            if (l_Enemy != null)
            {
                l_Enemy.Die();
                GameManager.Instance.AddToScore();
                Destroy(gameObject);
            }
        }
    }
}
