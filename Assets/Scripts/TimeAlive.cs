using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class TimeAlive : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeAlive;
    [SerializeField] private float _floatTimer;
    [SerializeField] private int _intTimer;
    // Start is called before the first frame update
    void Start()
    {
        _floatTimer = 0;
        _intTimer = 0;
        _timeAlive.text = _intTimer.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        _floatTimer += Time.deltaTime;
        if (_floatTimer > _intTimer)
        {
            _intTimer++;
            _timeAlive.text = _intTimer.ToString();
        }
    }
}
