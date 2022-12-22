using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class UI_FunFacts : MonoBehaviour
{

    public string[] FunFacts;

    void Start()
    {
        m_Text = GetComponent<TMP_Text>();
        int maxFacts = FunFacts.Length;
        int randFact = Random.Range(0, maxFacts);
        m_Text.text = FunFacts[randFact];
    }

    TMP_Text m_Text;
}
