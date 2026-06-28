using System.Linq;
using NUnit.Framework;

public class DiagonalMoveTests
{
    [Test]
    public void IsValidBishopMove()
    {
        Board board = new Board();
        SquareV2 square = new SquareV2(0, 0);
        SquareV2 target = new SquareV2(1, 1);
        Bishop bishop = new Bishop(Colors.BLACK, square);
        bishop.MoveType.IsValidMove(bishop, target, board);
    }
}