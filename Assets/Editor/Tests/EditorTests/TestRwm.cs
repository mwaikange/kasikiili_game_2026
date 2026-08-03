using System.Collections;
using System.Collections.Generic;
using Commands;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestRwm
{
    private RwmProbability rwmProbability;
    private float totalUserBalance;

    [SetUp]
    public void SetUp()
    {
        totalUserBalance = 10000f;
        rwmProbability = new RwmProbability(totalUserBalance);
    }

    [Test]
    public void RwmProbability_CorrectlyCalculatesPLA()
    {
        Assert.AreEqual(7000f, rwmProbability._pla);
    }

    [Test]
    public void RwmProbability_CorrectlyCalculatesAFA()
    {
        Assert.AreEqual(3000f, rwmProbability._afa);
    }

    [Test]
    public void SelectRwm_CorrectlyReturnsRWMValue()
    {
        int rwm = rwmProbability.SelectRwm();
        Assert.IsTrue(rwm >= 10 && rwm <= 2000);
    }
    
    [Test]
    public void Test_SelectRwm_Tub999()
    {
        //  (a) is 0 - 999 /     (10) => 100%
        float tub = 999;
        RwmProbability rwmProbability = new RwmProbability(tub);
        int result = rwmProbability.SelectRwm();
        Assert.AreEqual(10, result);
    }

    [Test]
    public void Test_SelectRwm_Tub5000()
    {
        //  (b) is 1000 - 5000/  (10) => 65%  (25) => 35% 
        float tub = 5000;
        RwmProbability rwmProbability = new RwmProbability(tub);
        int result = rwmProbability.SelectRwm();

        // This test should pass as long as the result is either 10 or 25
        Assert.IsTrue(result == 10 || result == 25);
    }

    [Test]
    public void Test_SelectRwm_Tub20000()
    {
        // (c) is 5001 - 20000/ (10) => 55%  (25) => 30% (50) => 15%
        float tub = 20000; // 14000 = pla
        RwmProbability rwmProbability = new RwmProbability(tub);
        int result = rwmProbability.SelectRwm();
        // This test should pass as long as the result is either 10, 25, or 50
        Assert.IsTrue(result == 10 || result == 25 || result == 50);
    }

    [Test]
    public void Test_SelectRwm_Tub50000()
    {
        // (d) is 20001 - 50000/(10) => 50%   (25)=> 25% (50) => 10% (100) =>15%
        float tub = 50000;
        RwmProbability rwmProbability = new RwmProbability(tub);
        int result = rwmProbability.SelectRwm();

        // This test should pass as long as the result is either 10, 25, 50, or 100
        Assert.IsTrue(result == 10 || result == 25 || result == 50 || result == 100);
    }

    [Test]
    public void Test_SelectRwm_Tub100000()
    {
        //  (e) is 50001 - 100000/(10)=> 45%   (25)=> 20% (50) => 5% (100) =>20% (1000) =>10%
        float tub = 100000;
        RwmProbability rwmProbability = new RwmProbability(tub);
        int result = rwmProbability.SelectRwm();

        // This test should pass as long as the result is either 10, 25, 50, 100, or 1000
        Assert.IsTrue(result == 10 || result == 25 || result == 50 || result == 100 || result == 1000);
    }

    [Test]
    public void Test_SelectRwm_Tub180000()
    {
        // (f) is 100001 - 180000/(10) =>40%   (25)=> 15% (50) => 5 % (100) =>15% (1000) =>15% (2000) => 10%
        float tub = 180000;
        RwmProbability rwmProbability = new RwmProbability(tub);
        int result = rwmProbability.SelectRwm();

        // This test should pass as long as the result is either 10, 25, 50, 100, 1000, or 2000
        Assert.IsTrue(result == 10 || result == 25 || result == 50 || result == 100 || result == 1000 || result == 2000);
    }
    
    [Test]
    public void Test_SelectRwm_Tub180001()
    {
        // (g) is 180001+  / (10) =>35%   (25)=> 10% (50) => 10 % (100) =>15% (1000) =>15% (2000) => 15%
        float tub = 2000001;
        RwmProbability rwmProbability = new RwmProbability(tub);
        int result = rwmProbability.SelectRwm();
        Assert.GreaterOrEqual(result, 10);
        Assert.LessOrEqual(result, 2000);
    }
}