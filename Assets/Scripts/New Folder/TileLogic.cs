using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogic
{
    public int State { get; private set; } // 0=empty 1=filled 2=trail  -1=enemy area

    public void SomeMethod()
    {
        
    }

    public void SetFilled()
    {
        State = 1;
    }
    public void SetState(int stateNum)
    {
        State = stateNum;
    }
}
