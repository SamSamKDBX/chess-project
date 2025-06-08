using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public ChessBoard chessBoard;

    void Start()
    {
        // faire un test de mouvement 
        Position pos = new Position(4, 6); // position du pion blanc devant le roi (e2);
        Piece piece = chessBoard.getPiece(pos); // le pion e2
        Move move_test = new Move(piece, 4, 4); // on créé un nouveau move avec la pièce et la position target
        bool validMove = piece.moveTo(move_test, chessBoard); // on essaye de déplacer la pièce
        if (!validMove) print("e4 invalide");

        piece = chessBoard.getPiece(new Position(4, 7));
        move_test = new Move(piece, 4, 6);
        piece.moveTo(move_test, chessBoard);

        piece = chessBoard.getPiece(new Position(4, 1));
        move_test = new Move(piece, 4, 2);
        piece.moveTo(move_test, chessBoard);

        piece = chessBoard.getPiece(new Position(3, 0));
        move_test = new Move(piece, 6, 3);
        piece.moveTo(move_test, chessBoard);

        piece = chessBoard.getPiece(new Position(6, 3));
        move_test = new Move(piece, 6, 4);
        piece.moveTo(move_test, chessBoard);
    }  
}
