using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubscriber : MonoBehaviour
{
    void Start()
    {
        TestEvent testEvent = GetComponent<TestEvent>();
        testEvent.OnSpacePressed += TestEvent_OnSpacePressed;
        testEvent.OnFloatEvent += TestEvent_OnFloatEvent;
        testEvent.OnActionEvent += TestEvent_OnActionEvent;
    }

    private void TestEvent_OnActionEvent(bool arg1, int arg2)
    {
        Debug.Log(arg1 + " " + arg2);
    }

    private void TestEvent_OnFloatEvent(float f)
    {
        Debug.Log("Float: " + f);
    }

    private void TestEvent_OnSpacePressed(object sender, TestEvent.OnSpacePressedEventArgs e)
    {
        Debug.Log("Space!" + e.spaceCount);
    }
}
