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
    public class RouletteStartInput : MonoBehaviour
    {
        public RouletteCmdFactory rouletteCmdFactory;
        public RouletteManager rouletteManager;
        public AudioManager audioManager;
        public RoundManager roundManager;
        public TableManager tableManager;
        
        public void Awake()
        {
            rouletteCmdFactory.StartGame(rouletteManager, roundManager, tableManager).Execute();
            rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Game).Execute();
        }
    }
}