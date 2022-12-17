using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Health : MonoBehaviour
{

    public Health Health;

    // Start is called before the first frame update
    void Start()
    {
        if(!Health) { return; }
        m_Text = GetComponent<TMP_Text>();
        m_IsDirty = true;
        Health.healthChange += SetDirty;
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_IsDirty) { return; }

        int hp = Health.HP;
        m_Text.text = hp.ToString();

        m_IsDirty = false;
    }

    public void SetDirty()
    {
        m_IsDirty = true;
    }

    bool m_IsDirty;
    TMP_Text m_Text;
}
