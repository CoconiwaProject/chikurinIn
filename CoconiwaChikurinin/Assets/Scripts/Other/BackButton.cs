using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BackButton : MonoBehaviour
{
    Button m_button;

    void Awake()
    {
        m_button = GetComponent<Button>();
    }

    void Start()
    {
        m_button.onClick.AddListener(() => UnderBerMenu.I.LoadPreviousScene());
    }
}
