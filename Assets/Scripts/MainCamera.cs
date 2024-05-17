using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private PlayerInput _player;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _targetPosition;


    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerInput>();
        _offset = transform.position - _player.transform.position;
        _offset.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _targetPosition = new Vector3(_player.transform.position.x, 0, 0) + _offset;
        transform.position = _targetPosition;
    }
}
