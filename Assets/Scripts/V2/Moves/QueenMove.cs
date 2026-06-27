using System;

public class QueenMove : IMoveType
{
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
        if (pieceToMove == null) throw new ArgumentNullException($"{nameof(pieceToMove)} a été null.");
        if (target == null) throw new ArgumentNullException($"{nameof(target)} a été null.");
        if (board == null) throw new ArgumentNullException($"{nameof(board)} a été null.");

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