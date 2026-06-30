using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class IsOutTests
{
    private Board _board;

    [SetUp]
    public void SetUpBoard()
    {
        _board = new GameObject().AddComponent<Board>();
        _board.Initialize();
    }

    [Test]
    public void ShouldTargetBeOutIfLineGreaterThanSeven()
    {
        // GIVEN
        SquareV2 target = new SquareV2(10, 0);
        // WHEN

        // THEN
        Assert.IsTrue(_board.IsOut(target));
    }

    [Test]
    public void ShouldTargetBeOutIfColGreaterThanSeven()
    {
        // GIVEN
        SquareV2 target = new SquareV2(0, 10);
        // WHEN

        // THEN
        Assert.IsTrue(_board.IsOut(target));
    }

    [Test]
    public void ShouldTargetBeOutIfLineSmallerThanZero()
    {
        // GIVEN
        SquareV2 target = new SquareV2(-1, 0);
        // WHEN

        // THEN
        Assert.IsTrue(_board.IsOut(target));
    }

    [Test]
    public void ShouldTargetBeOutIfColSmallerThanZero()
    {
        // GIVEN
        SquareV2 target = new SquareV2(0, -1);
        // WHEN

        // THEN
        Assert.IsTrue(_board.IsOut(target));
    }

    [Test]
    public void ShouldValidTargetNotBeOut()
    {
        // GIVEN
        SquareV2 target = new SquareV2(0, 0);
        // WHEN

        // THEN
        Assert.IsFalse(_board.IsOut(target));
    }

    [Test]
    public void ShouldThrowExceptionIfTargetNull()
    {
        // GIVEN
        SquareV2 target = null;
        // WHEN
        
        // THEN
        Assert.Throws<ArgumentNullException>(() => _board.IsOut(target));
    }
}
