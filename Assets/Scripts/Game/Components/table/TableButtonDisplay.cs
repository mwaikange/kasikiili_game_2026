using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ViewModel;

namespace Components
{
    public class TableButtonDisplay : MonoBehaviour
    {
        public TableButton tableButton;

        public GameObject flareObject;
        public TextMeshProUGUI numberText;

        private Color _defaultColorText;

        void Awake()
        {
            if (tableButton == null)
            {
                Debug.LogWarning("Table Button Not Found!");
                return;
            }
            flareObject.SetActive(false);
            _defaultColorText = numberText.color;
            numberText.text = tableButton.betValue.ToString();
            tableButton.isSelected
                .Subscribe(OnButtonSelect)
                .AddTo(this);
            tableButton.Refresh
                .Subscribe(OnButtonSelect)
                .AddTo(this);
        }
        
        private void OnButtonSelect(bool selected)
        {
            if (selected)
            {
                Select();
            }
            else
            {
                UnSelect();
            }
        }
        
        private void Select()
        {
            flareObject.SetActive(true);
            numberText.color = Color.white;
        }
        private void UnSelect()
        {
            flareObject.SetActive(false);
            numberText.color = _defaultColorText;
        }
    }
}