/// <summary>
/// Classe définissant un roi
/// </summary>
public class King : PieceV2
{
    public King(Colors color, SquareV2 originSquare) : base(color, originSquare, new KingMove())
    {

    }

    /// <summary>
    /// Permet d'initialiser les champs d'une pièce
    /// </summary>
    public void Initialize(Colors color, SquareV2 originSquare)
    {
        base.Initialize(color, originSquare, new KingMove());
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