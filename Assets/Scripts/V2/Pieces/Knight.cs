/// <summary>
/// Classe définissant un cavalier
/// </summary>
public class Knight : PieceV2
{
    public Knight(Colors color, SquareV2 originSquare) : base(color, originSquare, new KnightMove())
    {

    }

    /// <summary>
    /// Permet de convertir le cavalier en string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _color == Colors.WHITE ? "♞" : "♘";
    }
}