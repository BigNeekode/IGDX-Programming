using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyInputButton
{
  Jump = 0,
  Teleport = 1,
}

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;

    public NetworkButtons buttons;


}
