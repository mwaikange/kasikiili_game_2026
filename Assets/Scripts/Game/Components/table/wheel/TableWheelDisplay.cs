using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ViewModel;
using UniRx;
using UnityEngine.Rendering;

namespace Components
{
    public class TableWheelDisplay : MonoBehaviour
    {
        [Header("Manager")]
        public RoundManager roundManager;
        
        [Header("Audio")]
        public AudioManager audioManager;
        public AudioSource audioSource;
        public AbstractAudio wheelFx;

        [Header("Wheel")] 
        public GameObject rotatorFlare;
        public float stepQuick = 0.1f;
        public float stepDecreaseHigh = 0.02f;
        public float stepDecreaseLow = 0.05f;
        public float stepDecreaseSlow = 0.01f;

        private const float RotatorAngle = 27.692307f;
        private static readonly int[] WheelNumbers = { 0, 4, 11, 6, 7, 2, 9, 8, 1, 10, 3, 12, 5 };
        
        private void Awake()
        {
            rotatorFlare.SetActive(false);
            roundManager.OnRound
                .Subscribe(OnWinNumberReceived)
                .AddTo(this);
            roundManager.OnClear
                .Subscribe(OnClearRound)
                .AddTo(this);
        }

        private void OnClearRound(bool clear)
        {
            if(!clear) return;
            rotatorFlare.SetActive(false);
        }

        private void OnWinNumberReceived(int winNumber)
        {
            rotatorFlare.SetActive(true);
            Debug.Log("Winning number is " + winNumber);
            audioManager.Play(wheelFx, audioSource);
            StopAllCoroutines();
            StartCoroutine(DecreaseTime(winNumber, roundManager.wheelDuration));
        }
        
        private IEnumerator DecreaseTime(int numberWin, float totalTime)
        {
            SetFlareRotatorPosition(numberWin);
            var step = stepQuick;
            var middleTime = totalTime / 2;
            var thirdTime = totalTime / 3;
            var timeLeft = totalTime;
            Debug.Log("START WHEEL!");
            while (timeLeft >= 0.0f)
            {
                step += stepDecreaseSlow;
                if (timeLeft <= middleTime && timeLeft > thirdTime) 
                    step += stepDecreaseLow;
                else if (timeLeft <= thirdTime)
                    step += stepDecreaseHigh;
                timeLeft -= (step);
                rotatorFlare.transform.Rotate(new Vector3(0, 0, RotatorAngle));
                yield return new WaitForSeconds(step);
            }
            SetFlareFinalRotatorPosition(numberWin);
            Debug.Log("END WHEEL!");
        }

        private void SetFlareRotatorPosition(int numberWin)
        {
            var numberPosition = GetArrayPosition(numberWin);
            var degreesPosition = (numberPosition * RotatorAngle) - RotatorAngle;
            if (degreesPosition < 0)
                degreesPosition = RotatorAngle * 12;
            rotatorFlare.transform.eulerAngles = new Vector3(0, 0, degreesPosition);
        }

        private void SetFlareFinalRotatorPosition(int numberWin)
        {
            var numberPosition = GetArrayPosition(numberWin);
            var degreesPosition = (numberPosition * RotatorAngle);
            rotatorFlare.transform.eulerAngles = new Vector3(0, 0, degreesPosition);
        }

        private int GetArrayPosition(int numberWin)
        {
            return Array.FindIndex(WheelNumbers, row => row == numberWin);
        }

    }
}
