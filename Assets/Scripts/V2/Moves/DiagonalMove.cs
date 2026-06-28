using System;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// Classe définissant un mouvement en diagonale
/// </summary>
public class DiagonalMove : IMoveType
{
    /// <summary>
    /// Indique si le mouvement est valide pour pieceToMove vers target 
    /// (uniquement valide si une pièce est mangée lors du mouvement)
    /// </summary>
    /// <param name="pieceToMove"></param>
    /// <param name="target"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    public bool IsEatingValidMove(PieceV2 pieceToMove, SquareV2 target, Board board)
    {
        return IsValidMove(pieceToMove, target, board);
    }

    /// <summary>
    /// Bouge une pièce en diagonale
    /// </summary>
    /// <param name="target"></param>
    public bool IsValidMove(PieceV2 pieceToMove, SquareV2 target, Board board)
    {
        // Vérifier que les arguments ne sont pas null
        if (pieceToMove == null) throw new ArgumentNullException($"Erreur {nameof(pieceToMove)} est null");
        if (target == null) throw new ArgumentNullException($"Erreur {nameof(target)} est null");
        if (board == null) throw new ArgumentNullException($"Erreur {nameof(board)} est null");

        SquareV2 origin = pieceToMove.ActualSquare;
        try
        {
            // S'il n'y a aucune pièce sur la diagonale entre l'origin et la target
            return Board.AnyPieceBetween(origin, target, board.GetCommonDiagonal(origin, target));
        }
        catch (InvalidOperationException e)
        {
            // Si les deux cases ne sont égales ou pas sur la même diagonale
            throw new InvalidMoveException($"Mouvement invalide", e);
        }
    }
}