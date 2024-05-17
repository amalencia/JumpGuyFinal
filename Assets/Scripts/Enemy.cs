using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed;
    [SerializeField] private SpriteRenderer _enemySprite;
    [SerializeField] private float _enemyTimer;
    [SerializeField] private float _turnaroundTime;

    private void Start()
    {
        _enemySprite = GetComponent<SpriteRenderer>();
        _enemyTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _enemyTimer += Time.deltaTime;
        transform.Translate(-_enemySpeed * Time.deltaTime, 0, 0);
        if(_enemyTimer > _turnaroundTime)
        {
            _enemySpeed *= -1;
            _enemySprite.flipX = !_enemySprite.flipX;
            _enemyTimer = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Invoke("EnemyDestroy", 3f);
        }
    }

    private void EnemyDestroy()
    {
        Destroy(gameObject);
    }
}
