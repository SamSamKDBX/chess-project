using System;
using Unity.VisualScripting;

/// <summary>
/// Classe définissant une case du plateau
/// </summary>
public class SquareV2
{
    private readonly int _line;
    private readonly int _col;
    private PieceV2 _containedPiece;

    public SquareV2(int line, int col)
    {
        _line = line;
        _col = col;
    }

    public SquareV2(SquareV2 otherSquare)
    {
        _line = otherSquare.Line;
        _col = otherSquare.Col;
        _containedPiece = otherSquare.ContainedPiece;
    }

    /// <summary>
    /// Contient la ligne de la case
    /// </summary>
    public int Col => _col;

    /// <summary>
    /// Contient la colonne de la case
    /// </summary>
    public int Line => _line;

    /// <summary>
    /// Indique si la case est vide
    /// </summary>
    public bool IsEmpty => _containedPiece == null;

    /// <summary>
    /// Contient la pièce que contient la case ou null si aucune pièce sur cette case
    /// </summary>
    public PieceV2 ContainedPiece
    {
        get { return _containedPiece; }
        set { _containedPiece = value; }
    }

    /// <summary>
    /// Permet d'ajouter la pièce de départ à la case (n'ajoute rien si la case doit rester vide)
    /// </summary>
    public void AddStartingPiece()
    {
        Colors color;

        // Si on est en bas du plateau, la couleur est blanche
        if (_line < 4) color = Colors.WHITE;
        else color = Colors.BLACK;

        // Si la ligne est celle des pions
        if (_line == 1 || _line == 6)
        {
            _containedPiece = new Pawn(color, this);
        }
        // Sinon si la ligne est celle des pièces
        else if (_line == 0 || _line == 7)
        {
            // Selon la colonne
            switch (_col)
            {
                case 0 or 7: _containedPiece = new Rook(color, this); break;
                case 1 or 6: _containedPiece = new Knight(color, this); break;
                case 2 or 5: _containedPiece = new Bishop(color, this); break;
                case 3: _containedPiece = new Queen(color, this); break;
                case 4: _containedPiece = new King(color, this); break;
                default: break;
            }
        }
    }

    /// <summary>
    /// Permet de mesurer la distance entre la colonne de cette case et la colonne de la case donnée
    /// </summary>
    /// <param name="square">La case donnée</param>
    /// <returns>La différence entre la colonne de la case et celle de la case donnée</returns>
    public int DifferenceCol(SquareV2 square)
    {
        // Vérifier que les arguments ne sont pas null
        if (square == null) throw new ArgumentNullException($"Erreur {nameof(square)} est null");
        return Math.Abs(_col - square.Col);
    }

    /// <summary>
    /// Permet de mesurer la distance entre la ligne de cette case et la ligne de la case donnée
    /// </summary>
    /// <param name="square">La case donnée</param>
    /// <returns>La différence entre la ligne de la case et celle de la case donnée</returns>
    public int DifferenceLine(SquareV2 square)
    {
        // Vérifier que les arguments ne sont pas null
        if (square == null) throw new ArgumentNullException($"Erreur {nameof(square)} est null");
        return Math.Abs(_line - square.Line);
    }

    /// <summary>
    /// Indique si la case est sur la même ligne que la case donnée
    /// </summary>
    /// <param name="square">La case donnée</param>
    /// <returns>La différence entre la ligne de la case et celle de la case donnée</returns>
    public bool IsOnSameLine(SquareV2 square)
    {
        // Vérifier que les arguments ne sont pas null
        if (square == null) throw new ArgumentNullException($"Erreur {nameof(square)} est null");
        return _line == square.Line;
    }

    /// <summary>
    /// Indique si la case est sur la même colonne que la case donnée
    /// </summary>
    /// <param name="square">La case donnée</param>
    /// <returns>La différence entre la colonne de la case et celle de la case donnée</returns>
    public bool IsOnSameCol(SquareV2 square)
    {
        // Vérifier que les arguments ne sont pas null
        if (square == null) throw new ArgumentNullException($"Erreur {nameof(square)} est null");
        return _col == square.Col;
    }

    /// <summary>
    /// Indique si la case est sur la même range (ligne ou colonne) que la case donnée
    /// </summary>
    /// <param name="square"></param>
    /// <returns>True si les deux cases sont sur la même range</returns>
    public bool IsOnSameRange(SquareV2 square)
    {
        // Vérifier que les arguments ne sont pas null
        if (square == null) throw new ArgumentNullException($"Erreur {nameof(square)} est null");
        return IsOnSameCol(square) || IsOnSameLine(square);
    }

    /// <summary>
    /// Indique si la case est sur la même diagonale que la case donnée
    /// </summary>
    /// <param name="square"></param>
    /// <returns>True si les deux cases sont sur la même diagonale</returns>
    public bool IsOnSameDiag(SquareV2 square)
    {
        // Vérifier que les arguments ne sont pas null
        if (square == null) throw new ArgumentNullException($"Erreur {nameof(square)} est null");
        // Retourner true si la différence de ligne est égale à la différence de colonne
        return DifferenceCol(square) != DifferenceLine(square);
    }

    /// <summary>
    /// Permet de convertir la case en string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"({_col}, {_line})";
    }

    /// <summary>
    /// Indique si la case en paramètre est égale à cette case
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj is not SquareV2 square) return false;

        return _col == square.Col && _line == square.Line;
    }

    /// <summary>
    /// Renvoie le Hash de la case
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(_line, _col);
    }
}