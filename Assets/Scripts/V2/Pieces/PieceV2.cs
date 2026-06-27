using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe définissant une pièce
/// </summary>
public class PieceV2 : MonoBehaviour
{
    protected readonly Colors _color;
    protected SquareV2 _actualSquare;
    private List<SquareV2> _possibleSquares;
    private readonly IMoveType _moveType;
    private bool _hasNeverMoved;

    public PieceV2(Colors color, SquareV2 originSquare, IMoveType moveType)
    {
        _color = color;
        _actualSquare = originSquare;
        _moveType = moveType;
        _hasNeverMoved = true;
    }

    /// <summary>
    /// Indique si la pièce à effectué au moins 1 mouvement
    /// </summary>
    public bool HasNeverMoved => _hasNeverMoved;

    /// <summary>
    /// Contient la case actuelle de la pièce
    /// </summary>
    public SquareV2 ActualSquare => _actualSquare;

    /// <summary>
    /// Contient la couleur de la pièce
    /// </summary>
    public Colors Color => _color;

    /// <summary>
    /// Permet de récupérer la liste des cases où peut actuellement se déplacer la pièce
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public virtual List<SquareV2> GetPossibleSquares(Board board)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Indique si la pièce peut se déplacer à la case donnée
    /// </summary>
    /// <param name="context"></param>
    protected virtual bool CanMoveTo(SquareV2 target, Board board)
    {
        return _moveType.IsValidMove(this, target, board);
    }

    /// <summary>
    /// Permet de déplacer la pièce à la case donnée si le mouvement est valide
    /// </summary>
    /// <param name="target"></param>
    /// <param name="board"></param>
    public void MoveTo(SquareV2 target, Board board)
    {
        // Si le mouvement est valide pour cette pièce
        if (CanMoveTo(target, board))
        {
            // Déplacer la pièce
            _actualSquare = target;
            target.ContainedPiece = this;
            _hasNeverMoved = false;
        }
    }
}