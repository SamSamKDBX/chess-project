using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Classe définissant un plateau d'échec
/// </summary>
public class Board : MonoBehaviour
{
    private readonly List<SquareV2> _allSquares = new List<SquareV2>();

    public Board()
    {
        Initialize();
    }

    /// <summary>
    /// Initialise le plateau avec toutes les cases
    /// </summary>
    public void Initialize(bool addPieces = false)
    {
        // Vider la liste
        _allSquares.Clear();
        // Ajouter au plateau une case pour chaque ligne et chaque colonne
        for (int line = 0; line < 8; line++)
            for (int col = 0; col < 8; col++)
                _allSquares.Add(new SquareV2(line, col));

        if (addPieces)
            AddPieces();
    }

    /// <summary>
    /// Permet d'ajouter les pièces sur le plateau à leurs position de départ
    /// </summary>
    private void AddPieces()
    {
        _allSquares.ForEach(s => s.AddStartingPiece());
    }

    /// <summary>
    /// Permet de récupérer la pièce sur le plateau à la case target
    /// </summary>
    /// <param name="line"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public PieceV2 GetPiece(SquareV2 target)
    {
        // Vérifier que les arguments ne sont pas null
        if (target == null) throw new ArgumentNullException($"Erreur {nameof(target)} est null");
        // Vérifier que la target n'est pas out
        if (IsOut(target))
            throw new InvalidOperationException($"Erreur {nameof(target)} n'est pas sur le plateau");

        return _allSquares.First(s => s.Equals(target)).ContainedPiece;
    }

    /// <summary>
    /// Permet de placer une pièce sur le plateau à la case target
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="target"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void PutPiece(PieceV2 piece, SquareV2 target)
    {
        // Vérifier que les arguments ne sont pas null
        if (piece == null) throw new ArgumentNullException($"Erreur {nameof(piece)} est null");
        if (target == null) throw new ArgumentNullException($"Erreur {nameof(target)} est null");
        // Vérifier que la target n'est pas out
        if (IsOut(target))
            throw new InvalidOperationException($"Erreur {nameof(target)} n'est pas sur le plateau");

        piece.VirtualMove(target);
    }

    /// <summary>
    /// Indique si la case est en dehors du plateau
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public bool IsOut(SquareV2 square)
    {
        // Vérifier que les arguments ne sont pas null
        if (square == null) throw new ArgumentNullException($"Erreur {nameof(square)} est null");
        // Retourner true si la case est en dehors du plateau
        return !_allSquares.Contains(square)
            || square.Col > 7
            || square.Col < 0
            || square.Line > 7
            || square.Line < 0;
    }

    /// <summary>
    /// Permet de récupérer la diagonale commune aux deux cases en paramètre
    /// </summary>
    /// <param name="square1"></param>
    /// <param name="square2"></param>
    /// <returns>
    /// Une liste contenant les cases de la diagonale commune aux deux cases
    /// </returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException">Si les deux cases sont égales ou n'ont pas de diagonale commune</exception>
    public List<SquareV2> GetCommonDiagonal(SquareV2 square1, SquareV2 square2)
    {
        // Vérifier que les arguments ne sont pas null
        if (square1 == null) throw new ArgumentNullException($"Erreur {nameof(square1)} est null");
        if (square2 == null) throw new ArgumentNullException($"Erreur {nameof(square2)} est null");
        // Vérifier que les cases sont dans les limites du plateau
        if (IsOut(square1)) throw new InvalidOperationException($"Erreur {nameof(square1)} est hors du plateau");
        if (IsOut(square2)) throw new InvalidOperationException($"Erreur {nameof(square2)} est hors du plateau");
        // Vérifier que les deux cases sont différentes
        if (square1.Equals(square2))
            throw new InvalidOperationException($"{nameof(square1)} est égale à {nameof(square2)}");
        // Vérifier que les deux cases sont sur la même diagonale
        if (!square1.IsOnSameDiag(square2))
            throw new InvalidOperationException($"{nameof(square1)} et {nameof(square2)} ne sont pas sur la même diagonale");

        return _allSquares.Where(s => s.IsOnSameDiag(square1) && s.IsOnSameDiag(square2))
                          .OrderBy(s => s.Col)
                          .ThenBy(s => s.Line)
                          .ToList();
    }

    /// <summary>
    /// Permet de récupérer la ligne ou colonne commune aux deux cases en paramètre
    /// </summary>
    /// <param name="square1"></param>
    /// <param name="square2"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public List<SquareV2> GetCommonRange(SquareV2 square1, SquareV2 square2)
    {
        // Vérifier que les arguments ne sont pas null
        if (square1 == null) throw new ArgumentNullException($"Erreur {nameof(square1)} est null");
        if (square2 == null) throw new ArgumentNullException($"Erreur {nameof(square2)} est null");
        // Vérifier que les cases sont dans les limites du plateau
        if (IsOut(square1)) throw new InvalidOperationException($"Erreur {nameof(square1)} est hors du plateau");
        if (IsOut(square2)) throw new InvalidOperationException($"Erreur {nameof(square2)} est hors du plateau");
        // Vérifier que les deux cases sont différentes
        if (square1.Equals(square2))
            throw new InvalidOperationException($"{nameof(square1)} est égale à {nameof(square2)}");

        // Si les deux cases sont sur la même ligne
        if (square1.IsOnSameLine(square2))
            return _allSquares.Where(s => s.IsOnSameLine(square1))
                              .OrderBy(s => s.Line)
                              .ToList();
        // Sinon si les deux cases sont sur la même colonne
        else if (square1.IsOnSameCol(square2))
            return _allSquares.Where(s => s.IsOnSameCol(square1))
                              .OrderBy(s => s.Col)
                              .ToList();
        else
            // Sinon si les deux cases ne sont ni sur la même ligne ni sur la même colonne
            throw new InvalidOperationException($"{nameof(square1)} et {nameof(square2)} ne sont pas sur la même ligne ni sur la même colonne");
    }

    /// <summary>
    /// Indique si toutes s'il y a au moins une case non vide entre origin et target (exclus)
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="rangeOrDiag"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static bool AnyPieceBetween(SquareV2 origin, SquareV2 target, List<SquareV2> rangeOrDiag)
    {
        // Vérifier que les arguments ne sont pas null
        if (origin == null) throw new ArgumentNullException($"Erreur {nameof(origin)} est null");
        if (target == null) throw new ArgumentNullException($"Erreur {nameof(target)} est null");
        if (rangeOrDiag == null) throw new ArgumentNullException($"Erreur {nameof(rangeOrDiag)} est null");
        // Vérifier que les cases sont dans la range
        if (!rangeOrDiag.Contains(origin)) throw new InvalidOperationException($"Erreur {nameof(origin)} n'est pas dans {nameof(rangeOrDiag)}");
        if (!rangeOrDiag.Contains(target)) throw new InvalidOperationException($"Erreur {nameof(target)} n'est pas dans {nameof(rangeOrDiag)}");

        // Récupérer les index de origin et target
        int originIndex = rangeOrDiag.IndexOf(origin);
        int targetIndex = rangeOrDiag.IndexOf(target);
        int startIndex = Mathf.Min(targetIndex, originIndex);
        int endIndex = Mathf.Max(targetIndex, originIndex);

        // Parcourir les cases entre startIndex et endIndex 
        // et checker si la case est vide
        return rangeOrDiag.GetRange(startIndex + 1, endIndex - startIndex - 1)
                          .Any(s => !s.IsEmpty);
    }

    /// <summary>
    /// Permet de récupérer le roi de la couleur donné
    /// </summary>
    /// <param name="color"></param>
    /// <returns>Le roi de la couleur donnée ou null si pas trouvé</returns>
    private King GetKing(Colors color)
    {
        return _allSquares.FirstOrDefault(s => s.ContainedPiece is King
                                          && s.ContainedPiece?.Color == color)?.ContainedPiece as King;
    }

    /// <summary>
    /// Indique si le roi de la couleur donnée est en échec
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool IsKingInCheckNow(Colors color)
    {
        King king = GetKing(color);
        return IsAttacked(king.ActualSquare, king.Color);
    }

    /// <summary>
    /// Indique si le roi de la couleur de la pièce donnée sera en échec 
    /// après un déplacement de la pièce vers target
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsKingInCheckAfterMove(PieceV2 piece, SquareV2 target)
    {
        // Vérifier que les arguments ne sont pas null
        if (piece == null) throw new ArgumentNullException($"Erreur {nameof(piece)} est null");
        if (target == null) throw new ArgumentNullException($"Erreur {nameof(target)} est null");

        // Récupérer la pièce actuelle sur target et l'origine
        PieceV2 oldPiece = target.ContainedPiece;
        SquareV2 origin = piece.ActualSquare;
        // Déplacer piece vers target
        piece.VirtualMove(target);
        // Tester si le roi est en échec
        bool isKingCheck = IsKingInCheckNow(piece.Color);
        // Remettre les pièces à leur place
        piece.VirtualMove(origin);
        oldPiece.VirtualMove(target);
        return isKingCheck;
    }

    /// <summary>
    /// Permet de savoir si la target est attaquée par une pièce de la couleur donnée
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private bool IsAttacked(SquareV2 target, Colors strikerColor)
    {
        // Pour chaque case contenant une pièce
        // Checker si la pièce est de la couleur attaquante et si elle menace la target
        return _allSquares.Select(s => s.ContainedPiece)
                          .Any(p => p != null
                                && p.Color == strikerColor
                                && p.MoveType.IsEatingValidMove(p, target, this));
    }

    /// <summary>
    /// Permet d'afficher le plateau dans la console
    /// </summary>
    public void Display()
    {
        Debug.Log(ToString());
    }

    /// <summary>
    /// Permet de convertir le plateau en string
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string board = "    | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 |\n";
        foreach (SquareV2 square in _allSquares)
        {
            if (square.Col == 0)
            {
                board += $"| {square.Line} ";
            }
            board += "| " + square.ContainedPiece != null ? square.ContainedPiece : "◻" + " ";
            if (square.Col == 7)
            {
                board += $"|\n";
            }
        }
        board += "|";
        return board;
    }
}