using System;
using System.Collections.Generic;

/// <summary>
/// Classe définissant un mouvement en diagonale
/// </summary>
public class DiagonalMove : IMoveType
{
    /// <summary>
    /// Bouge une pièce en diagonale
    /// </summary>
    /// <param name="target"></param>
    public bool IsValidMove(PieceV2 pieceToMove, SquareV2 target, Board board)
    {
        // Vérifier que les arguments ne sont pas null
        if (pieceToMove == null) throw new ArgumentNullException($"{nameof(pieceToMove)} a été null.");
        if (target == null) throw new ArgumentNullException($"{nameof(target)} a été null.");
        if (board == null) throw new ArgumentNullException($"{nameof(board)} a été null.");

        SquareV2 origin = pieceToMove.ActualSquare;
        try
        {
            // Récupérer la diagonale commune aux deux cases parmi toutes les cases du plateau
            List<SquareV2> diagonal = board.GetCommonDiagonal(origin, target);
            // S'il n'y a aucune une pièce sur la voie entre l'origin et la target
            if (board.IsWayClear(origin, target, diagonal))
            {
                return true;
            }
        }
        catch (InvalidOperationException e)
        {
            // Si les deux cases ne sont pas sur la même diagonale
            throw new InvalidMoveException($"La case cible ({origin}) n'est pas sur la même diagonale que la pièce", e);
        }
        return true;
    }
}