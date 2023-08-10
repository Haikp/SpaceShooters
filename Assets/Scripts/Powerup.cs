using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    // [SerializeField]
    // private GameObject _player;
    //0 = Triple Short
    //1 = Speed
    //2 = Shields
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _powerUpSFX;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
            switch (powerupID)
            {
                case 0:
                    player.TripleShotActive();
                    break;
                case 1:
                    player.SpeedActive();
                    break;
                case 2:
                    player.ShieldsActive();
                    break;
                default:
                    Debug.Log("Default case");
                    break;
            }

            AudioSource.PlayClipAtPoint(_powerUpSFX, transform.position);
            Destroy(this.gameObject);
        }
    }
}
