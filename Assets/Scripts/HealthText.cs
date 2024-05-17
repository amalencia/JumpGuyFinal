using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _health;
    // Start is called before the first frame update

    public void UpdateTotalHealth(int _counter)
    {
        _health.text = _counter.ToString();
    }
}
