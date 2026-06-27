


/// <summary>
/// Interface définissant un mouvement
/// </summary>
public interface IMoveType
{
    /// <summary>
    /// Indique si le mouvement est valide pour ce type de mouvement
    /// </summary>
    /// <param name="pieceToMove"></param>
    /// <param name="target"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    public bool IsValidMove(PieceV2 pieceToMove, SquareV2 target, Board board);
}