using System;
using NUnit.Framework;
using UnityEngine;

public class BoardTests : ScriptableObject
{
    private Board _board;
    private King _piece;
    private SquareV2 _target;

    [SetUp]
    public void SetUpBoard()
    {
        _board = new GameObject().AddComponent<Board>();
        _board.Initialize();
        
        _piece = new GameObject().AddComponent<King>();
        _piece.Initialize(Colors.WHITE, new SquareV2(0, 0));
        
        _target = new SquareV2(1, 0);
    }

    [Test]
    public void ShouldPutPiece()
    {
        // GIVEN
        // WHEN
        _board.PutPiece(_piece, _target);
        // THEN
        Assert.AreEqual(_board.GetPiece(_target), _piece);
    }

    [Test]
    public void ShouldNotPutNullPiece()
    {
        // GIVEN
        // WHEN
        _piece = null;
        // THEN
        Assert.Throws<ArgumentNullException>(() => _board.PutPiece(_piece, _target));
    }

    [Test]
    public void ShouldNotPutPieceToNullTarget()
    {
        // GIVEN
        // WHEN
        _target = null;
        // THEN
        Assert.Throws<ArgumentNullException>(() => _board.PutPiece(_piece, _target));
    }

    [Test]
    public void ShouldNotPutPieceToTargetOutOfBoard()
    {
        // GIVEN
        // WHEN
        _target = new SquareV2(100, 100);
        // THEN
        Assert.Throws<InvalidOperationException>(() => _board.PutPiece(_piece, _target));
    }
}