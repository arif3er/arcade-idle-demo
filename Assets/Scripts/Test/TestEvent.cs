using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvent : MonoBehaviour
{

    public event EventHandler<OnSpacePressedEventArgs> OnSpacePressed;

    public class OnSpacePressedEventArgs : EventArgs
    {
        public int spaceCount;
    }

    public delegate void TestEventDelegate(float f);
    public event TestEventDelegate OnFloatEvent;

    public event Action<bool,int> OnActionEvent;

    private int spaceCount;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Space pressed
            spaceCount++;
            if (OnSpacePressed != null)
            {
                OnSpacePressed(this, new OnSpacePressedEventArgs { spaceCount =  spaceCount});
            }

            if (OnFloatEvent != null)
            {
                OnFloatEvent(5.5f);
            }

            if (OnActionEvent != null)
            {
                OnActionEvent(true, 11);
            }
        }
    }
}
