using System;
using Components;
using ViewModel;
using UnityEngine;
using UnityEngine.AI;

namespace Commands
{

    [CreateAssetMenu(fileName = "TableCmdFactory", menuName = "Factory/TableCmdFactory", order = 0)]
    public class TableCmdFactory : ScriptableObject 
    {
        public ButtonClickCmd ClickButton(TableManager tableManager, TableButton tableButton)
        {
            return new ButtonClickCmd(tableManager, tableButton);
        }
    }
}