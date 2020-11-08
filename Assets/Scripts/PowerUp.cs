using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3f;
    [SerializeField] //0 = Triple Shot; 1 = Speed; 2 = Shield; 3 = Super Weapon
    private float _powerID;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.SuperActive();
                        break;
                    case 4:
                        player.AddHealth();
                        break;
                    case 5:
                        player.AddAmmo();
                        break;
                    default:
                        Debug.LogError("Power Up ID Not Found");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
