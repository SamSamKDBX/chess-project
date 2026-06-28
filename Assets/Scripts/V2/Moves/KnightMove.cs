using System;
using Unity.VisualScripting;

/// <summary>
/// Classe définissant un mouvement en diagonale
/// </summary>
public class KnightMove : IMoveType
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

        // Si la case target est distante de 1 case en largeur et 2 en longueur ou l'inverse
        SquareV2 origin = pieceToMove.ActualSquare;
        return origin.DifferenceCol(target) == 2 && origin.DifferenceLine(target) == 1
            || origin.DifferenceCol(target) == 1 && origin.DifferenceLine(target) == 2;
    }
}