using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsText : MonoBehaviour
{
    private TMP_Text text;

    private void Start() {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateText(string newText) {
        text.text = newText;
    }
}
