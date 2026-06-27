


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


    /// <summary>
    /// Indique si le mouvement est valide pour pieceToMove vers target 
    /// (uniquement valide si une pièce est mangée lors du mouvement)
    /// </summary>
    /// <param name="pieceToMove"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsEatingValidMove(PieceV2 pieceToMove, SquareV2 target, Board board);
}