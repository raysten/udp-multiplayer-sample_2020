using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    public class Factory : PlaceholderFactory<Player>
    {
    }
}
