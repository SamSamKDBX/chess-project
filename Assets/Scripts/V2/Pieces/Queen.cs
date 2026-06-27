
/// <summary>
/// Classe définissant une reine
/// </summary>
public class Queen : PieceV2
{
    public Queen(Colors color, SquareV2 originSquare) :
    base(color, originSquare, new QueenMove())
    {

    }

    /// <summary>
    /// Permet de convertir la reine en string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _color == Colors.WHITE ? "♛" : "♕";
    }
}