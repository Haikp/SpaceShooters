using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;
    [SerializeField]
    private float _speed = 5f;
    private float _speedMultiplier = 1;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = .15f;
    private float _fireTimer = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool tripleShotActive = false;
    private bool shieldsActive = false;
    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private int _score = 0;
    private UIManager _UIManager;
    private GameManager _GameManager;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;
    private AudioSource _source;
    [SerializeField]
    private AudioClip _laserSFX;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null.");
        }

        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UIManager == null)
        {
            Debug.LogError("UI Manager is null.");
        }

        _GameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_GameManager == null)
        {
            Debug.LogError("Game Manager is null.");
        }

        _source = GetComponent<AudioSource>();
        if (_source == null)
        {
            Debug.LogError("Audio source is null in player.");
        }
        else
        {
            _source.clip = _laserSFX;
        }
        
        if (_GameManager.isCoopMode == false)
        {
            transform.position = new Vector3(0, -3, 0);
        }
        
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("_animator is null in player.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOne)
        {
            CalculateMovement();
        }
        else if (isPlayerTwo)
        {
            CalculateMovementP2();
        }

#if UNITY_IOS

        if (CrossPlatformInputManager.GetButton("Fire") && Time.time > _fireTimer)
        {
            FireLaser();
        }

#elif UNITY_ANDROID

        if (CrossPlatformInputManager.GetButton("Fire") && Time.time > _fireTimer)
        {
            FireLaser();
        }

#else

        if (isPlayerOne)
        {
            if (Input.GetKey(KeyCode.Space) && Time.time > _fireTimer)
            {
                FireLaser();
            }
        }
        else if (isPlayerTwo)
        {
            if (Input.GetKey(KeyCode.Keypad0) && Time.time > _fireTimer)
            {
                FireLaserP2();
            }
        }

#endif
    }

    void CalculateMovement()
    {
        float horizontalInput = CrossPlatformInputManager.GetAxisRaw("Horizontal"); // Input.GetAxisRaw("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxisRaw("Vertical"); // Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKeyDown(KeyCode.A))
        {
            // _animator.SetBool("Release_Key", false);
            _animator.SetBool("Left_Turn", true);
            _animator.SetBool("Right_Turn", false);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            // _animator.SetBool("Release_Key", true);
            _animator.SetBool("Left_Turn", false);
            _animator.SetBool("Right_Turn", false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // _animator.SetBool("Release_Key", false);
            _animator.SetBool("Right_Turn", true);
            _animator.SetBool("Left_Turn", false);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            // _animator.SetBool("Release_Key", true);
            _animator.SetBool("Right_Turn", false);
            _animator.SetBool("Left_Turn", false);
        }

        transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        
        float upperBound = 5f;
        float lowerBound = upperBound * -1;
        float rightBound = 10.8f;
        float leftBound = rightBound * -1;

        Vector3 playerPos = new Vector3(transform.position.x, transform.position.y, 0);

        if (playerPos.x < leftBound)
        {
            transform.position = new Vector3(rightBound, transform.position.y, 0);
        }
        else if (playerPos.x > rightBound)
        {
            transform.position = new Vector3(leftBound, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, lowerBound, upperBound), 0);
    }

    void CalculateMovementP2()
    {
        // float horizontalInput = CrossPlatformInputManager.GetAxisRaw("Horizontal"); // Input.GetAxisRaw("Horizontal");
        // float verticalInput = CrossPlatformInputManager.GetAxisRaw("Vertical"); // Input.GetAxisRaw("Vertical");
        // Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.Translate(Vector3.up * _speed * _speedMultiplier * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Keypad5))
        {
            transform.Translate(Vector3.down * _speed * _speedMultiplier * Time.deltaTime);
        }
        // transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.Translate(Vector3.left * _speed * _speedMultiplier * Time.deltaTime);
            _animator.SetBool("Left_Turn", true);
            _animator.SetBool("Right_Turn", false);
        }
        else if(Input.GetKeyUp(KeyCode.Keypad4))
        {
            _animator.SetBool("Left_Turn", false);
            _animator.SetBool("Right_Turn", false);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            transform.Translate(Vector3.right * _speed * _speedMultiplier * Time.deltaTime);
            _animator.SetBool("Right_Turn", true);
            _animator.SetBool("Left_Turn", false);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad6))
        {
            _animator.SetBool("Right_Turn", false);
            _animator.SetBool("Left_Turn", false);
        }

        float upperBound = 5f;
        float lowerBound = upperBound * -1;
        float rightBound = 10.8f;
        float leftBound = rightBound * -1;

        Vector3 playerPos = new Vector3(transform.position.x, transform.position.y, 0);

        if (playerPos.x < leftBound)
        {
            transform.position = new Vector3(rightBound, transform.position.y, 0);
        }
        else if (playerPos.x > rightBound)
        {
            transform.position = new Vector3(leftBound, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, lowerBound, upperBound), 0);
    }

    void FireLaser()
    {
        _fireTimer = Time.time + _fireRate;
        if (tripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + .7f, 0), Quaternion.identity);
        }

        _source.Play();
    }

    void FireLaserP2()
    {
        _fireTimer = Time.time + _fireRate;
        if (tripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + .7f, 0), Quaternion.identity);
        }

        _source.Play();
    }

    public void Damage()
    {
        if (shieldsActive)
        {
            _shieldPrefab.SetActive(false);
            shieldsActive = false;
            StartCoroutine(invincibilityFrames());
            return;
        }

        _lives -= 1;

        StartCoroutine(invincibilityFrames());

        _UIManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives < 1)
        {
            _GameManager.GameOver();
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        tripleShotActive = true;
        StartCoroutine(TripleShotRoutine());
    }

    IEnumerator TripleShotRoutine()
    {
        yield return new WaitForSeconds(5);
        tripleShotActive = false;
    }

    public void SpeedActive()
    {
        _speedMultiplier *= 1.2f;
        StartCoroutine(SpeedRoutine());
    }

    IEnumerator SpeedRoutine()
    {
        yield return new WaitForSeconds(5);
        _speedMultiplier = 1;
    }

    public void ShieldsActive()
    {
        _shieldPrefab.SetActive(true);

        shieldsActive = true;
    }

    public void OnEnemyKilled(int pts)
    {
        _score += pts;
        _UIManager.UpdateScore(_score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            Damage();
        }
    }

    IEnumerator invincibilityFrames()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        for (int i = 0; i < 10; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(.05f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(.05f);
        }

        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
