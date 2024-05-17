using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _backgroundPrefab;
    [SerializeField] private Sprite[] _backgroundSprites;
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private Tile[] _groundTiles;
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _collectable;
    
    [Header("Sprite Management")]
    [SerializeField] private int _numberOfBackgrounds;
    [SerializeField] private int _numberOfGrounds;
    [SerializeField] private int _numberOfEnemies;
    [SerializeField] private int _totalCollectables;
    [SerializeField] private int _backgroundLimit;
    [SerializeField] private int _groundLimit;
    [SerializeField] private int _enemyLimit;
    [SerializeField] private int _collectableLimit;

    [Header("Unity Objects")]
    [SerializeField] private PlayerInput _player;
    [SerializeField] private int _onScreenCollect;
    [SerializeField] private int _generateCollect;
    [SerializeField] private Vector3 _backgroundTransform;
    [SerializeField] private Vector3 _groundTransform;
    [SerializeField] private float _xFirst;
    [SerializeField] private float _xLast;
    [SerializeField] private int _generateEnemy;
    [SerializeField] private Vector3 _obstacleTransform;
    [SerializeField] private Vector3 _enemyTransform;
    [SerializeField] private Vector3 _collectableTransform;

    public UnityEvent<int> OnTotalCoinChanged;

    private int TotalCoins
    {
        get
        {
            return _totalCollectables;
        }
        set
        {
            _totalCollectables = value;
            OnTotalCoinChanged.Invoke(_totalCollectables);
        }
    }

    private void Start()
    {
        _player = FindObjectOfType<PlayerInput>();
        InitialCollects();
        _backgroundTransform = new Vector3(0, 0, 0);
        _groundTransform = new Vector3(32, -2.89f, 0);
        _obstacleTransform = new Vector3(0, -2.36f, 0);
        _enemyTransform = new Vector3(0, -2.41f, 0);
        _collectableTransform = new Vector3(0, -2.36f, 0);
        TotalCoins = 0;
    }

    private void Update()
    {
        if(_onScreenCollect == 0)
        {
            GenerateBackground();
        }

        if(Input.GetKey(KeyCode.Tab))
        {
            PauseMenu();
        }
    }

    public void Death()
    {
        SceneManager.LoadScene("Assignment Base");
    }

    private void PauseMenu()
    {
        SceneManager.LoadScene("PauseScreen");
    }


    public void CollectCollectables()
    {
        _totalCollectables++;
        TotalCoins = _totalCollectables;
        _onScreenCollect--;
    }

    private void InitialCollects()
    {
        Vector2 initialCollectsTrans = _player.transform.position;
        for(int i = 0; i < 3; i++)
        {
            initialCollectsTrans += new Vector2(2, 0);
            Instantiate(_collectable, initialCollectsTrans, Quaternion.identity);
            _onScreenCollect++;
        }
        initialCollectsTrans += new Vector2(16, 0);
        Instantiate(_collectable, initialCollectsTrans, Quaternion.identity);
        _onScreenCollect++;
    }

    private void GenerateBackground()
    {
        _backgroundTransform += new Vector3(64, 0, 0);
        Instantiate(_backgroundPrefab, _backgroundTransform, Quaternion.identity);
        GenerateGround();
    }

    private void GenerateGround()
    {
        _groundTransform += new Vector3(1f, 0, 0);
        for(int i = 0; i < 3; i++)
        {
            _groundTransform += new Vector3(10, 0, 0);
            _xFirst = _groundTransform.x - 10;
            _xLast = _groundTransform.x + 10;
            Instantiate(_groundPrefab, _groundTransform, Quaternion.identity);
            _groundTransform += new Vector3(11, 0, 0);
            GenerateItems();
        }
    }

    private void GenerateItems()
    {
        _generateCollect = Random.Range(0, _collectableLimit);
        _generateEnemy = Random.Range(0, _enemyLimit);
        
        _obstacleTransform.x = Random.Range(_xFirst, _xLast);
        Instantiate(_obstacle, _obstacleTransform, Quaternion.identity);

        GenerateCollects(_xFirst, _xLast, _generateCollect);
        GenerateEnemy(_xFirst, _xLast, _generateEnemy);
    }

    private void GenerateCollects(float xFirst, float xLast, int number)
    {
        if (number == 0)
        {
            return;
        }
        float firstStart = (20 - number) / number;
        float Starters = firstStart + xFirst;
        _collectableTransform.x = Random.Range(xFirst, Starters);

        for(int i = 0; i < number; i++)
        {
            if ((_obstacleTransform.x + 0.5f) > _collectableTransform.x && (_obstacleTransform.x - 0.5f) < _collectableTransform.x)
            {
                if ((_obstacleTransform.x + 2) > xLast)
                {
                    _collectableTransform.x -= 2;
                    firstStart -= 2;
                }
                else
                {
                    _collectableTransform.x += 2;
                    firstStart -= 2;
                }
            }
            _obstacleTransform.x = Mathf.Clamp(_obstacleTransform.x, xFirst, xLast);
            Instantiate(_collectable, _collectableTransform, Quaternion.identity);
            _onScreenCollect++;
            _collectableTransform.x += firstStart;
        }
    }

    private void GenerateEnemy(float xFirst, float xLast, int number)
    {
        for (int i = 0; i <= number; i++)
        {
            _enemyTransform.x = Random.Range(xFirst, xLast);
            Instantiate(_enemy, _enemyTransform, Quaternion.identity);
        }
    }




}
