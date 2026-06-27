using System;

/// <summary>
/// Classe définissant un mouvement en diagonale
/// </summary>
public class KnightMove : IMoveType
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
        // Si la case target est distante de 1 case en largeur et 2 en longueur ou l'inverse
        if (origin.DifferenceCol(target) == 2 && origin.DifferenceLine(target) == 1
            || origin.DifferenceCol(target) == 1 && origin.DifferenceLine(target) == 2)
        {
            return true;
        }
        throw new InvalidMoveException($"La case cible {origin} n'est pas une case valide pour le cavalier {pieceToMove}");
    }
}