using System;
using Interface;
using UnityEngine;

public class InjectHandler : IInjectable
{
    public ILogic CurrentLogic { get; private set; }

    public void SetLogic(ILogic logic)
    {
        if ( logic == null )
            throw new ArgumentException(nameof(logic));

        CurrentLogic = logic;
    }
}