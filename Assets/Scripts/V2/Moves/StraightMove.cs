using System;
using System.Collections.Generic;

/// <summary>
/// Classe définissant un mouvement en diagonale
/// </summary>
public class StraightMove : IMoveType
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
            // Récupérer la ligne ou la colonne commune entre les deux cases
            List<SquareV2> commonRange_origin_target = board.GetCommonRange(origin, target);
            // S'il n'y a aucune une pièce sur la voie entre l'origin et la target
            if (board.IsWayClear(origin, target, commonRange_origin_target))
            {
                return true;
            }
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidMoveException($"La target {target} n'est pas sur la même ligne ni sur la même colonne que la case d'origine", e);
        }
        return true;
    }
}