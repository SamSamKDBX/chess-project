/// <summary>
/// Classe définissant un pion
/// </summary>
public class Pawn : PieceV2
{
    public Pawn(Colors color, SquareV2 originSquare) : base(color, originSquare, new PawnMove())
    {

    }

    /// <summary>
    /// Permet de convertir le pion en string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _color == Colors.WHITE ? "♟" : "♙";
    }
}