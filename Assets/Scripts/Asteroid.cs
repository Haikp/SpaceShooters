using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 20f;
    private Animator m_Animator;
    private SpawnManager _spawnManager;
    private AudioSource _source;
    [SerializeField]
    private AudioClip _explosionSFX;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        if (m_Animator == null)
        {
            Debug.LogError("m_Anim for asteroid is null.");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager for asteroid is null.");
        }

        _source = gameObject.GetComponent<AudioSource>();
        if(_source == null)
        {
            Debug.LogError("Audio source is null in asteroid.");
        }
        else
        {
            _source.clip = _explosionSFX;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(new Vector3(0,0,1) * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Laser")
        {
            m_Animator.SetTrigger("OnLaserContact");
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            _spawnManager.BeginSpawning();
            Destroy(other.gameObject);
            _source.Play();
            Destroy(this.gameObject, 2.38f);
        }

    }
}
