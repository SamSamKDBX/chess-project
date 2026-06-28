using System;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// Classe définissant un mouvement en diagonale
/// </summary>
public class KingMove : IMoveType
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
        if (pieceToMove == null) throw new ArgumentNullException($"Erreur {nameof(pieceToMove)} est null");
        if (target == null) throw new ArgumentNullException($"Erreur {nameof(target)} est null");

        // Si la pièce ne bouge que d'une case
        SquareV2 origin = pieceToMove.ActualSquare;
        return origin.DifferenceCol(target) == 1
            && origin.DifferenceLine(target) == 1;
    }
}