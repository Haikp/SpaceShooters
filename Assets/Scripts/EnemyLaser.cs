using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (gameObject.transform.position.y < -7)
        {
            Destroy(this.gameObject);
            if (this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
        }
    }
}
