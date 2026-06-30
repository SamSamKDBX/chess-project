using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class AnyPieceBetweenTests
{
    private Board _board = new GameObject().AddComponent<Board>();

    [SetUp]
    public void SetUpBoard()
    {
        _board.Initialize();
    }
}
