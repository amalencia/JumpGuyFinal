using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsCollected : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsCollected;

    public void UpdateTotalCoins(int _counter)
    {
        _coinsCollected.text = _counter.ToString();
    }
}
