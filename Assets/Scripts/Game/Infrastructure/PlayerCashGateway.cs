using System;
using UniRx;
using UnityEngine;
using UnityEditor;
using ViewModel;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using ViewModel;

namespace Infrastructure
{
    public class PlayerCashGateway
    {
        public IObservable<Unit> GetPlayerCredits(CashManager cashManager)
        {
            return Observable.FromCoroutine<Unit>(observer => GetCredits(observer, cashManager));
        }
        
        IEnumerator GetCredits(IObserver<Unit> observer, CashManager cashManager)
        { 
            cashManager.ResetMoney();
            cashManager.PaymentSystem(1000);
            yield return new WaitForSeconds(0);
            
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }
        
        public IObservable<Unit> SetPlayerCredits(int credits, CashManager cashManager)
        {
            return Observable.FromCoroutine<Unit>(observer => SetCredits(observer, credits, cashManager));
        }
        
        IEnumerator SetCredits(IObserver<Unit> observer, int credits, CashManager cashManager)
        { 
            cashManager.ResetMoney();
            cashManager.PaymentSystem(credits);
            
            yield return new WaitForSeconds(0);
            
            observer.OnNext(Unit.Default); // push Unit or all buffer result.
            observer.OnCompleted();
        }
    }
}