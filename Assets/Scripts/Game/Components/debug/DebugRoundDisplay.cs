using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ViewModel;
using UniRx;

namespace Components
{
    public class DebugRoundDisplay : MonoBehaviour
    {
        public RoundManager roundManager;
        public TextMeshProUGUI tub;
        public TextMeshProUGUI pla;
        public TextMeshProUGUI afa;
        public TextMeshProUGUI multiplier;
        public TextMeshProUGUI letter;
        public TextMeshProUGUI win;

        private void Awake()
        {
            roundManager.probabilityPla
                .Subscribe(OnPla)
                .AddTo(this);
            roundManager.probabilityAfa
                .Subscribe(OnAfa)
                .AddTo(this);
            roundManager.probabilityTub
                .Subscribe(OnTub)
                .AddTo(this);
            roundManager.probabilityNumber
                .Subscribe(OnNumber)
                .AddTo(this);
            roundManager.probabilityLetter
                .Subscribe(OnLetter)
                .AddTo(this);
            roundManager.winNumber
                .Subscribe(OnWin)
                .AddTo(this);
        }

        void OnLetter(string letterString)
        {
            letter.text = "(A to G) SELECT = " + letterString;
        }

        void OnPla(float plaNumber)
        {
            pla.text = "(70%) PLA = " + plaNumber;
        }

        void OnAfa(float afaNumber)
        {
            afa.text = "(30%) AFA = " + afaNumber;
        }

        void OnTub(float tubNumber)
        {
            tub.text = "(100%) TUB = " + tubNumber;
        }

        void OnNumber(int probability)
        {
            multiplier.text = "MULT = " + probability;
        }

        void OnWin(int winNumber)
        {
            win.text = "ROULETTE = " + winNumber;
        }
    }
}