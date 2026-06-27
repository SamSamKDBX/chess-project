using System;
using Unity.VisualScripting;

public class QueenMove : IMoveType
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
    /// <param name="pieceToMove"></param>
    /// <param name="target"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidMoveException"></exception>
    public bool IsValidMove(PieceV2 pieceToMove, SquareV2 target, Board board)
    {
        // Vérifier que les arguments ne sont pas null
        Ensure.That(nameof(pieceToMove)).IsNotNull(pieceToMove);
        Ensure.That(nameof(target)).IsNotNull(target);
        Ensure.That(nameof(board)).IsNotNull(board);

        SquareV2 origin = pieceToMove.ActualSquare;
        try
        {
            // Créer des mouvement de type diagonale et tout droit
            DiagonalMove diagonalMove = new DiagonalMove();
            StraightMove straightMove = new StraightMove();

            // Si le mouvement est en diagonale ou tout droit alors il est valide
            if (diagonalMove.IsValidMove(pieceToMove, target, board)
            || straightMove.IsValidMove(pieceToMove, target, board))
            {
                return true;
            }
            throw new InvalidMoveException();
        }
        catch (InvalidMoveException e)
        {
            // Si le mouvment n'est pas valide
            throw new InvalidMoveException($"La case cible ({origin}) n'est pas valide pour {pieceToMove}", e);
        }
    }
}