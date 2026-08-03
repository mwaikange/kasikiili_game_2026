using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Table", menuName = "Scriptable/Manager/Table Manager")]
    public class TableManager : ScriptableObject
    {
        public CashManager cashManager;
        [Header("Runtime")]
        public List<TableButton> buttonsSelected;
        public List<TableButton> buttonsLast = new List<TableButton>();

        public bool CheckIfPlayerWin(int numberWin)
        {
            return buttonsSelected.ToArray().Any(button => button.numberParent == numberWin);
        }
        public TableButton[] GetWinButtons(int numberWin)
        {
            var win = buttonsSelected.ToArray().Where(button => button.numberParent == numberWin);
            return win.ToArray();
        }
    }
}