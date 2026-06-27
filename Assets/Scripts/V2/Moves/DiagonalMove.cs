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
        Ensure.That(nameof(pieceToMove)).IsNotNull(pieceToMove);
        Ensure.That(nameof(target)).IsNotNull(target);
        Ensure.That(nameof(board)).IsNotNull(board);

        SquareV2 origin = pieceToMove.ActualSquare;
        try
        {
            // Récupérer la diagonale commune aux deux cases parmi toutes les cases du plateau
            List<SquareV2> diagonal = board.GetCommonDiagonal(origin, target);
            // S'il n'y a aucune pièce sur la voie entre l'origin et la target
            if (board.AnyPieceBetween(origin, target, diagonal))
            {
                return true;
            }
        }
        catch (InvalidOperationException e)
        {
            // Si les deux cases ne sont pas sur la même diagonale
            throw new InvalidMoveException($"Mouvement invalide", e);
        }
        return true;
    }
}