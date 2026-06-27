using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    /// Contient la couleur de l'opposant
    /// </summary>
    public Colors OpponentColor => _color == Colors.BLACK ? Colors.WHITE : Colors.BLACK;

    /// <summary>
    /// Contient le type de mouvement de la pièce
    /// </summary>
    public IMoveType MoveType => _moveType;

    /// <summary>
    /// Permet de récupérer la liste des cases où peut actuellement se déplacer la pièce
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public List<SquareV2> GetValidSquares(Board board)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Indique si la pièce peut se déplacer à la case donnée
    /// <para>
    /// Vérifie que :
    /// <list type="bullet">
    ///     <item>La case d'arrivée est sur le plateau</item>
    ///     <item>La case d'arrivée ne contient pas une pièce de la même couleur</item>
    ///     <item>Le déplacement est valide pour le type de déplacement que peut effectuer la pièce</item>
    ///     <item>Le roi de la même couleur n'est pas en échec et ne le sera pas après le déplacement</item>
    /// </list>
    /// </para>
    /// </summary>
    /// <param name="context"></param>
    /// <returns>True si la pièce peut se déplacer vers target, false sinon</returns>
    protected virtual bool CanMoveTo(SquareV2 target, Board board)
    {
        return !board.IsOut(target)
                && (target.IsEmpty || target.ContainedPiece.Color != _color)
                && _moveType.IsValidMove(this, target, board)
                && !board.IsKingInCheckNow(Color)
                && !board.IsKingInCheckAfterMove(this, target);
    }

    /// <summary>
    /// Permet de déplacer la pièce à la case donnée si le mouvement est valide
    /// </summary>
    /// <param name="target"></param>
    /// <param name="board"></param>
    public void TryMove(SquareV2 target, Board board)
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

    /// <summary>
    /// Permet de déplacer la pièce vers target temporairement sans aucun check
    /// </summary>
    /// <param name="target"></param>
    /// <param name="board"></param>
    /// <returns></returns>
    public void VirtualMove(SquareV2 target)
    {
        // Vérifier que les arguments ne sont pas null
        Ensure.That(nameof(target)).IsNotNull(target);

        target.ContainedPiece = this;
        _actualSquare = target;
    }
}