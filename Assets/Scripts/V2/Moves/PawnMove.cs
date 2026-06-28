using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Classe définissant un mouvement en diagonale
/// </summary>
public class PawnMove : IMoveType
{
    /// <summary>
    /// Indique si le mouvement est valide pour ce type de mouvement
    /// </summary>
    /// <param name="target"></param>
    public bool IsValidMove(PieceV2 pieceToMove, SquareV2 target, Board board)
    {
        // Vérifier que les arguments ne sont pas null
        if (pieceToMove == null) throw new ArgumentNullException($"Erreur {nameof(pieceToMove)} est null");
        if (target == null) throw new ArgumentNullException($"Erreur {nameof(target)} est null");
        if (board == null) throw new ArgumentNullException($"Erreur {nameof(board)} est null");

        SquareV2 origin = pieceToMove.ActualSquare;
        return target.Line - origin.Line == GetStep(pieceToMove.Color)
            && (origin.IsOnSameCol(target) || IsEatingValidMove(pieceToMove, target, board));
    }

    /// <summary>
    /// Indique si le mouvement est valide pour un pion qui mange une pièce adverse
    /// </summary>
    /// <param name="pieceToMove"></param>
    /// <param name="target"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    public bool IsEatingValidMove(PieceV2 pieceToMove, SquareV2 target, Board board)
    {
        // Vérifier que les arguments ne sont pas null
        if (pieceToMove == null) throw new ArgumentNullException($"Erreur {nameof(pieceToMove)} est null");
        if (target == null) throw new ArgumentNullException($"Erreur {nameof(target)} est null");

        SquareV2 origin = pieceToMove.ActualSquare;
        return !target.IsEmpty
            && target.ContainedPiece.Color == pieceToMove.OpponentColor
            && origin.DifferenceCol(target) == 1
            && target.Line - origin.Line == GetStep(pieceToMove.Color);
    }
    
    /// <summary>
    /// Permet de récupérer la direction d'un pas en fonction de la couleur du pion
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private int GetStep(Colors color)
    {
        return color == Colors.WHITE ? 1 : -1;
    }
}