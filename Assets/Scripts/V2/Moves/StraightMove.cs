using System;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// Classe définissant un mouvement en diagonale
/// </summary>
public class StraightMove : IMoveType
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
    /// Indique si le mouvement est valide pour ce type de mouvement
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
            // Récupérer la ligne ou la colonne commune entre les deux cases
            List<SquareV2> commonRange_origin_target = board.GetCommonRange(origin, target);
            // S'il n'y a aucune une pièce sur la voie entre l'origin et la target
            if (board.AnyPieceBetween(origin, target, commonRange_origin_target))
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