using System;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// Classe définissant un fou
/// </summary>
public class Bishop : PieceV2
{
    public Bishop(Colors color, SquareV2 originSquare) : base(color, originSquare, new DiagonalMove())
    {

    }

    /// <summary>
    /// Permet de convertir le fou en string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return _color == Colors.WHITE ? "♝" : "♗";
    }
}