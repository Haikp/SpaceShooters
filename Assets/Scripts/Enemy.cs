using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    private bool destroyed = false;
    // Start is called before the first frame update
    private Animator m_Animator;
    private AudioSource _source;
    [SerializeField]
    private AudioClip _explosionSFX;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 1.5f;
    private float _fireCD = -1f;
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        if (m_Animator == null)
        {
            Debug.LogError("m_Animator is null.");
        }

        _source = gameObject.GetComponent<AudioSource>();
        if (_source == null)
        {
            Debug.LogError("Audio Source is null in Enemy.");
        }
        else
        {
            _source.clip = _explosionSFX;
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7 && !destroyed)
        {
            transform.position = new Vector3(Random.Range(-10f, 10f), 7, 0);
        }

        if (_fireCD < Time.time)
        {
            _fireCD = Time.time + _fireRate;
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (other.tag == "Laser")
        {
            if (player != null)
            {
                player.OnEnemyKilled(10); //add 10 points to score
            }

            destroyed = true;
            m_Animator.SetTrigger("OnEnemyDeath");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.38f);
            _source.Play();
        }

        if (other.tag == "Player")
        {
            // Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            destroyed = true;
            m_Animator.SetTrigger("OnEnemyDeath");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            Destroy(this.gameObject, 2.38f);
            _source.Play();
        }

    }
}

