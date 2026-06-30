using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GetPieceTests
{
    private Board _board = new GameObject().AddComponent<Board>();
    private King _piece = new GameObject().AddComponent<King>();

    [SetUp]
    public void SetUpBoard()
    {
        _board.Initialize();
        _piece.Initialize(Colors.WHITE, new SquareV2(0, 0));
    }

    [Test]
    public void ShouldGetPiece()
    {
        // GIVEN
        SquareV2 target = new SquareV2(1, 0);
        _board.PutPiece(_piece, target);
        // WHEN
        PieceV2 actualPiece = _board.GetPiece(target);
        // THEN
        Assert.AreEqual(_piece, actualPiece);
    }

    [Test]
    public void ShouldNotGetPieceAtNullTarget()
    {
        // GIVEN
        SquareV2 target = new SquareV2(1, 0);
        _board.PutPiece(_piece, target);
        // WHEN
        target = null;
        // THEN
        Assert.Throws<ArgumentNullException>(() => _board.GetPiece(target));
    }

    [Test]
    public void ShouldNotGetPieceAtOutTarget()
    {
        // GIVEN
        SquareV2 target = new SquareV2(1, 0);
        _board.PutPiece(_piece, target);
        // WHEN
        target = new SquareV2(100, 100);
        // THEN
        Assert.Throws<InvalidOperationException>(() => _board.GetPiece(target));
    }

    [Test]
    public void ShouldNotGetUnplacedPiece()
    {
        // GIVEN
        SquareV2 target = new SquareV2(1, 0);
        // WHEN
        // THEN
        Assert.Throws<InvalidOperationException>(() => _board.GetPiece(target));
    }
}
