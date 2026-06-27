using System;
using System.Collections.Generic;

/// <summary>
/// Classe définissant un mouvement en diagonale
/// </summary>
public class KingMove : IMoveType
{
    /// <summary>
    /// Indique si le mouvement est valide pour ce type de mouvement
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
            // Si la pièce ne bouge que d'une case
            if (origin.DifferenceCol(target) == 1
            && origin.DifferenceLine(target) == 1)
            {
                return true;
            }
            throw new InvalidMoveException();
        }
        catch (InvalidOperationException e)
        {
            // Si les deux cases ne sont pas sur la même diagonale
            throw new InvalidMoveException($"La case cible ({origin}) n'est pas valide pour le {pieceToMove}", e);
        }
    }
}