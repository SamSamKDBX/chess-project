/// <summary>
/// Classe définissant une tour
/// </summary>
public class Rook : PieceV2
{
    public Rook(Colors color, SquareV2 originSquare) : base(color, originSquare, new StraightMove())
    {

    }

    /// <summary>
    /// Permet de convertir la tour en string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _color == Colors.WHITE ? "♜" : "♖";
    }
}