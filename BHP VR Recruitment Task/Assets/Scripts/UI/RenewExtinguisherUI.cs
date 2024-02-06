using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenewExtinguisherUI : MonoBehaviour
{
    [SerializeField] private Button renewExtinguisherButton;

    [Space(10)]

    [SerializeField] private GameObject extinguisherPrefab;


    private void Awake()
    {
        renewExtinguisherButton.onClick.AddListener(() =>
        {

        });
    }
}
