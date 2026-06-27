using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Classe définissant un plateau d'échec
/// </summary>
public class Board
{
    private readonly List<SquareV2> _allSquares;

    public Board()
    {
        _allSquares = new List<SquareV2>();
        // Pour chaque ligne
        for (int line = 0; line < 8; line++)
        {
            // Pour chaque colonne
            for (int col = 0; col < 8; col++)
            {
                // Ajouter au tableau une case pour cette ligne et cette colonne (les pièces sont rajoutée dans le constructeur)
                _allSquares.Add(new SquareV2(line, col));
            }
        }
    }

    /// <summary>
    /// Indique si la case est en dehors du plateau
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static bool IsOut(SquareV2 s)
    {
        if (s == null) throw new ArgumentNullException($"{nameof(s)} a été null");
        return s.Col > 7 || s.Col < 0 || s.Line > 7 || s.Line < 0;
    }

    /// <summary>
    /// Retourne les deux diagonales de la case en paramètre
    /// </summary>
    /// <param name="square"></param>
    /// <returns></returns>
    public List<SquareV2> GetTwoDiagonals(SquareV2 square)
    {
        if (square == null) throw new ArgumentNullException($"{nameof(square)} a été null");

        return _allSquares.Where(s => s.DifferenceCol(square) == s.DifferenceLine(square))
                          .OrderBy(s => s.Col)
                          .ThenBy(s => s.Line)
                          .ToList();
    }

    /// <summary>
    /// Renvoie la diagonale commune aux deux cases en paramètre
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public List<SquareV2> GetCommonDiagonal(SquareV2 s1, SquareV2 s2)
    {
        if (s1 == null) throw new ArgumentNullException($"{nameof(s1)} a été null");
        if (s2 == null) throw new ArgumentNullException($"{nameof(s2)} a été null");
        if (IsOut(s1)) throw new InvalidOperationException($"{nameof(s1)} est en dehors du plateau");
        if (IsOut(s2)) throw new InvalidOperationException($"{nameof(s2)} est en dehors du plateau");

        // Si les deux cases ne sont pas sur la même diagonale
        if (s1.DifferenceCol(s2) != s1.DifferenceLine(s2)) throw new InvalidOperationException($"{nameof(s1)} et {nameof(s2)} ne sont pas sur la même diagonale");

        return _allSquares.Where(s => s.DifferenceCol(s1) == s.DifferenceLine(s1) && s.DifferenceCol(s2) == s.DifferenceLine(s2))
                          .OrderBy(s => s.Col)
                          .ThenBy(s => s.Line)
                          .ToList();
    }

    /// <summary>
    /// Renvoie la ligne ou colonne commune aux deux cases en paramètre
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public List<SquareV2> GetCommonRange(SquareV2 s1, SquareV2 s2)
    {
        if (s1 == null) throw new ArgumentNullException($"{nameof(s1)} a été null");
        if (s2 == null) throw new ArgumentNullException($"{nameof(s2)} a été null");
        if (IsOut(s1)) throw new InvalidOperationException($"{nameof(s1)} est en dehors du plateau");
        if (IsOut(s2)) throw new InvalidOperationException($"{nameof(s2)} est en dehors du plateau");

        // Si les deux cases sont sur la même ligne
        if (s1.IsOnSameLine(s2))
        {
            // Retourner la ligne
            return _allSquares.Where(s => s.IsOnSameLine(s1))
                              .OrderBy(s => s.Line)
                              .ToList();
        }
        // Sinon si les deux cases sont sur la même colonne
        else if (s1.IsOnSameCol(s2))
        {
            // Retourner la colonne
            return _allSquares.Where(s => s.IsOnSameCol(s1))
                              .OrderBy(s => s.Col)
                              .ToList();
        }
        else
        {
            // Sinon si les deux cases ne sont ni sur la même ligne ni sur la même colonne
            throw new InvalidOperationException($"{nameof(s1)} et {nameof(s2)} ne sont pas sur la même ligne ni sur la même colonne");
        }
    }

    /// <summary>
    /// Permet de parcourir le plateau depuis origin vers target
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="squares"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public bool IsWayClear(SquareV2 origin, SquareV2 target, List<SquareV2> squares = null)
    {
        if (origin == null) throw new ArgumentNullException($"{nameof(origin)} a été null");
        if (target == null) throw new ArgumentNullException($"{nameof(target)} a été null");
        if (IsOut(origin)) throw new InvalidOperationException($"{nameof(origin)} est en dehors du plateau");
        if (IsOut(target)) throw new InvalidOperationException($"{nameof(target)} est en dehors du plateau");

        int i = 0;
        // Parcourir les cases jusqu'à origin
        while (i < squares.Count && squares[i] != origin) i++;
        // Parcourir les cases jusqu'à target
        while (i < squares.Count)
        {
            // Si on tombe sur la target
            if (squares[i] == target)
            {
                return true;
            }
            // Si on tombe sur une case non vide
            else if (squares[i] != null)
            {
                throw new InvalidOperationException($"La case {squares[i]} n'est pas vide");
            }
        }
        throw new InvalidOperationException($"");
    }

    /// <summary>
    /// Permet de récupérer le roi de la couleur donné
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public King GetKing(Colors color)
    {
        return (King)_allSquares.FirstOrDefault(s => s.ContainedPiece is King && s.ContainedPiece?.Color == color)?.ContainedPiece;
    }

    /// <summary>
    /// Permet d'afficher le plateau dans la console
    /// </summary>
    public void Print()
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