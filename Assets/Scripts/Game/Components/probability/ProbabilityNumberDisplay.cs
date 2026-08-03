
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ViewModel;
using UniRx;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Components
{
    public class ProbabilityNumberDisplay : MonoBehaviour
    {
        [Header("Round")]
        public RoundManager roundManager;
        public GameObject[] lights;
        [Header("Duration")]
        public float stepQuick = 0.1f;
        
        private int _currentPos;
        
        private static readonly int[] ProbabilityNumbers = { 10, 50, 1000, 2000, 25, 100 };
        
        private void Awake()
        {
            TurnOffAll();
            roundManager.OnProbability
                .Subscribe(OnProbabilityNumber)
                .AddTo(this);
            roundManager.OnClear
                .Subscribe(OnClear)
                .AddTo(this);
        }

        private void OnClear(bool clear)
        {
            if(!clear) return;
            TurnOffAll();
        }

        private void OnProbabilityNumber(int probabilityNumber)
        {
            Debug.Log("Probability number is " + probabilityNumber);
            StopAllCoroutines();
            StartCoroutine(DecreaseTime(probabilityNumber, roundManager.probabilityDuration));
        }

        private IEnumerator DecreaseTime(int probabilityNumber, float totalTime)
        {
            TurnOffAll();
            var step = stepQuick;
            var timeLeft = totalTime;
            while (timeLeft >= 0.0f)
            {
                if (_currentPos >= ProbabilityNumbers.Length)
                    _currentPos = 0;
                lights[_currentPos].SetActive(true);
                timeLeft -= (step);
                yield return new WaitForSeconds(step);
                lights[_currentPos].SetActive(false);
                _currentPos += 1;
            }
            TurnOffAll();
            SetFlareFinalRotatorPosition(probabilityNumber);
        }

        private void TurnOffAll()
        {
            _currentPos = 0;
            foreach (var obj in lights)
            {
                obj.SetActive(false);
            }
        }
        
        private int GetArrayPosition(int probability)
        {
            return Array.FindIndex(ProbabilityNumbers, row => row == probability);
        }
        
        private void SetFlareFinalRotatorPosition(int numberWin)
        {
            var numberPosition = GetArrayPosition(numberWin);
            lights[numberPosition].SetActive(true);
        } 
    }
}
