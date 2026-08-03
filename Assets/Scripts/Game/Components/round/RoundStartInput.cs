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
    public class RoundStartInput : MonoBehaviour
    {
        public RoundCmdFactory gameCmdFactory;
        public RouletteManager rouletteManager;
        public RoundManager roundManager;
        public TableManager tableManager;
        
        public void Click()
        {
            gameCmdFactory.StartRound(rouletteManager, roundManager, tableManager).Execute();
        }
    }
}