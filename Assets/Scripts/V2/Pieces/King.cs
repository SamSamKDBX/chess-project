/// <summary>
/// Classe définissant un roi
/// </summary>
public class King : PieceV2
{
    public King(Colors color, SquareV2 originSquare) : base(color, originSquare, new KingMove())
    {

    }

    /// <summary>
    /// Permet de convertir le roi en string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _color == Colors.WHITE ? "♚" : "♔";
    }
}