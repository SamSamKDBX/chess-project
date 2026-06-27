using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Classe définissant un fou
/// </summary>
public class Bishop : PieceV2
{
    public Bishop(Colors color, SquareV2 originSquare) : base(color, originSquare, new DiagonalMove())
    {

    }

    /// <summary>
    /// Permet de récupérer les cases possible du fou
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public override List<SquareV2> GetPossibleSquares(Board board)
    {
        if (board == null) throw new ArgumentNullException($"{nameof(board)} a été null");

        // Créer une liste
        List<SquareV2> possibleSquares = new List<SquareV2>();
        // Récupérer les deux diagonales du fou
        List<SquareV2> twoDiagonals = board.GetTwoDiagonals(_actualSquare);
        // Retourner les cases 
        return twoDiagonals.Where(s => s.ContainedPiece == null).ToList();
    }

    /// <summary>
    /// Permet de convertir le fou en string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _color == Colors.WHITE ? "♝" : "♗";
    }
}