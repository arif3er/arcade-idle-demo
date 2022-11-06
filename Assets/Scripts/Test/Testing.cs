using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    /*public delegate void TestDelegate();
    public delegate bool TestBoolDelegate(int i);

    private TestDelegate testDelegateFunction;
    private TestBoolDelegate testBoolDelegateFunction;

    private Action testAction;
    private Action<int, float> testIntFloatAction;
    private Func<bool> testFunc;
    private Func<int, bool> testIntBoolFunc;*/

    [SerializeField] private ActionOnTimer actionOnTimer;


    void Start()
    {
        actionOnTimer.SetTimer(1f, () => { Debug.Log("Timer complete!"); });




        /*testDelegateFunction += () => { Debug.Log("Lambda expression"); };
        testDelegateFunction += () => { Debug.Log("Second Lambda expression"); };
        testDelegateFunction();


        testBoolDelegateFunction = MyTestBoolDelegateFunction;
        testBoolDelegateFunction = (int i) => i < 5;
        Debug.Log(testBoolDelegateFunction(1));

        testIntFloatAction = (int i, float f) => { Debug.Log("Test int float action!"); };

        testFunc = () => false;

        testIntBoolFunc = (int i) => { return i < 5; };*/
    }

    private void MyTestDelegateFunction()
    {
        Debug.Log("MyTestDelegateFunction");
    }

    private void MySecondTestDelegateFunction()
    {
        Debug.Log("MySecondTestDelegateFunction");
    }

    private bool MyTestBoolDelegateFunction(int i)
    {
        return i < 5;
    }
}
