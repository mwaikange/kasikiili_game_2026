using System;
using System.Collections;
using System.Collections.Generic;
using Commands;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ViewModel;

namespace Components
{
    [RequireComponent (typeof(Button))]
    public class TableButtonInput : MonoBehaviour
    {
        public TableCmdFactory tableCmdFactory;
        public RouletteManager rouletteManager;
        public TableManager tableManager;
        public TableButton tableButton;


        void Start()
        {
            if (tableButton == null)
            {
                Debug.LogWarning("Table Button Not Found!");
                return;
            }
            tableButton.isSelected.Value = false;
            tableButton.Refresh
                .Subscribe(RefreshButton)
                .AddTo(this);
        }

        private void RefreshButton(bool isSelected)
        {
            tableCmdFactory.ClickButton(tableManager, tableButton).Execute();
        }

        public void Click()
        {
            if (tableButton == null) return;

            if (!rouletteManager.tableActive.Value || !rouletteManager.gameActive.Value)
                return;
            
            tableCmdFactory.ClickButton(tableManager, tableButton).Execute();
        }
    }
}