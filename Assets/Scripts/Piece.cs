using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;
using System;

public class Piece : MonoBehaviour
{
    private string Name;
    private string color;
    private Position position;
    private List<Position> latestPositions;
    private List<GameObject> possibleSquares;
    private bool hasNeverMoved;
    private ChessBoard chessBoard;
    private bool isClickedVar;
    private int directionY;
    private Piece dangerousPiece;
    public SpriteRenderer capturedSR;
    private readonly string[] directions = {
            "Bottom",
            "Right",
            "Top",
            "Left",
            "TopRightCorner",
            "TopLeftCorner",
            "BottomRightCorner",
            "BottomLeftCorner"
        };

    public void setAttributes(string Name, string color, int x, int y, ChessBoard chessBoard)
    {
        this.Name = Name;
        this.color = color;
        this.directionY = color == "Black" ? 1 : -1;
        this.position = new Position(x, y);
        this.hasNeverMoved = true;
        this.chessBoard = chessBoard;
        this.latestPositions = new List<Position>();
        this.isClickedVar = false;
    }

    public string getName()
    {
        return this.Name;
    }

    public string getColor()
    {
        return this.color;
    }

    public Position getPosition()
    {
        return this.position;
    }

    public int getX()
    {
        return this.position.getX();
    }

    public int getY()
    {
        return this.position.getY();
    }

    public Position getLastPosition()
    {
        return this.latestPositions.Last();
    }

    public void setLastPosition(Position pos)
    {
        this.latestPositions.Add(pos);
    }

    public void setChessBoard(ChessBoard cb)
    {
        this.chessBoard = cb;
    }

    public void setPosition(Position p)
    {
        this.position = p;
    }

    public bool neverMadeMove()
    {
        return this.hasNeverMoved;
    }

    public void madeMove()
    {
        this.hasNeverMoved = false;
    }

    private string getDirectionBetween(Position from, Position to)
    {
        int dx = to.getX() - from.getX();
        int dy = to.getY() - from.getY();

        if (dx == 0 && dy > 0) return "Top";
        if (dx == 0 && dy < 0) return "Bottom";
        if (dy == 0 && dx > 0) return "Right";
        if (dy == 0 && dx < 0) return "Left";
        if (dx > 0 && dy > 0 && dx == dy) return "TopRightCorner";
        if (dx < 0 && dy > 0 && -dx == dy) return "TopLeftCorner";
        if (dx > 0 && dy < 0 && dx == -dy) return "BottomRightCorner";
        if (dx < 0 && dy < 0 && dx == dy) return "BottomLeftCorner";

        return null; // Pas une direction valide (pas aligné)
    }

    private (int stepX, int stepY) getStepFromDirection(string direction)
    {
        switch (direction)
        {
            case "Top": return (0, 1);
            case "Bottom": return (0, -1);
            case "Right": return (1, 0);
            case "Left": return (-1, 0);
            case "TopRightCorner": return (1, 1);
            case "TopLeftCorner": return (-1, 1);
            case "BottomRightCorner": return (1, -1);
            case "BottomLeftCorner": return (-1, -1);
            default: return (0, 0); // direction inconnue
        }
    }

    public bool moveTo(Move move, ChessBoard chessBoard)
    {
        Position target = move.getPosition();

        // si les conditions le coup est légal pour la pièce à déplacer
        if (isLegalMove(move))
        {
            // on bouge dans le tableau chessBoard
            chessBoard.movePieceChessBoard(target, this);
            chessBoard.addMoveToHistory(move);
            print($"Move piece {move.getPiece().name} to ({target.getX()}, {target.getY()}) is legal\n--------------------------------------------------------------------");
            return true;
        }
        print("move is not legal\n--------------------------------------------------------------------");
        return false;
    }

    public bool isLegalMove(Move move)
    {
        if (move.getPosition().equals(this.position)) { print("target equals position"); return false; }
        if (!chessBoard.isNotOut(move.getPosition())) { print("target is out"); return false; }
        if (!this.isWayClear(move, chessBoard)) { print("way is not clear"); return false; }
        if (willPutKingInCheck(move, chessBoard)) { print("Cannot put king in check"); return false; }
        if (this.Name != "King" && isCheck(chessBoard.getKing(this.color).getPosition(), this.chessBoard) && isNotSavingKing(move))
        {
            print("save the king before !");
            return false;
        }
        switch (this.Name)
        {
            case "King": return this.isKingLegalMove(move);
            case "Queen": return this.isQueenLegalMove(move);
            case "Bishop": return this.isBishopLegalMove(move);
            case "Knight": return this.isKnightLegalMove(move);
            case "Rook": return this.isRookLegalMove(move);
            case "Pawn": return this.isPawnLegalMove(move);
            default:
                print("Error in Piece.isLegalMove()");
                return false;
        }
    }

    private bool isNotSavingKing(Move move)
    {
        // TODO ca marche pas encore
        string dangerousPieceName = dangerousPiece.getName();
        Position dangerousPiecePos = dangerousPiece.getPosition();
        Position kingPos = chessBoard.getKing(this.color).getPosition();
        if (move.getPosition().equals(this.dangerousPiece.getPosition())) { print("eat it is a good idea"); return false; }
        if (dangerousPieceName == "Knight" ||
            dangerousPieceName == "Pawn" ||
            (dangerousPieceName == "Rook" || dangerousPieceName == "Bishop" || dangerousPieceName == "Queen") &&
            !isBlocking(move.getPosition()))
        {
            //print("Not Saving the King");
            return true;
        }
        //print("is Saving the king--------------------------------");
        return false;
    }

    private bool isBlocking(Position target)
    {
        Position tempPos = chessBoard.getKing(this.color).getPosition().copy();
        Position dangerousPiecePos = this.dangerousPiece.getPosition();
        (int stepX, int stepY) = getStepFromDirection(getDirectionBetween(tempPos, dangerousPiecePos));

        while (chessBoard.isNotOut(tempPos) && !tempPos.equals(dangerousPiecePos))
        {
            if (tempPos.equals(target))
            {
                //print("is blocking attack");
                return true;
            }
            tempPos.incrementXY(stepX, stepY);
        }
        //print("is not blocking attack");
        return false;
    }

    private bool willPutKingInCheck(Move move, ChessBoard chessBoard)
    {
        // TODO
        int indexDirections = 0;
        Position foundPiecePos;
        Piece foundPiece = null;
        while (indexDirections < 8)
        {
            foundPiecePos = this.position.copy();
            chessBoard.findNextPiece(this.directions[indexDirections], foundPiecePos);
            foundPiece = chessBoard.getPiece(foundPiecePos);
            if (foundPiece != null && foundPiece == chessBoard.getKing(this.color)) break;
            indexDirections++;
        }
        if (foundPiece != chessBoard.getKing(this.color)) { print("pas besoin de la suite"); return false; }
        string directionToVerify = findOpposite(this.directions[indexDirections]);
        foundPiecePos = this.position.copy();
        chessBoard.findNextPiece(directionToVerify, foundPiecePos);
        foundPiece = chessBoard.getPiece(foundPiecePos);
        if (foundPiece != null && foundPiece.getColor() != this.color && foundPiece.isDangerous(directionToVerify))
        {
            // le roi est en danger si on bouge "this"
            print("Cannot put the king in check");
            return true;
        }
        print("The king is fine");
        return false;
    }

    private bool isDangerous(string directionToVerify)
    {
        if (this.Name == "Queen") return true;
        if (this.Name == "Bishop" && directionToVerify.EndsWith("Corner")) return true;
        if (this.Name == "Rook" && !directionToVerify.EndsWith("Corner")) return true;
        return false;
    }

    private string findOpposite(string direction)
    {
        switch (direction)
        {
            case "Top": return "Bottom";
            case "Bottom": return "Top";
            case "Left": return "Right";
            case "Right": return "Left";
            case "TopRightCorner": return "BottomLeftCorner";
            case "BottomLeftCorner": return "TopRightCorner";
            case "TopLeftCorner": return "BottomRightCorner";
            case "BottomRightCorner": return "TopLeftCorner";
            default: return null;
        }
    }

    private bool isWayClear(Move move, ChessBoard chessBoard)
    {
        if (this.Name == "Knight")
        {
            Piece targetPiece = chessBoard.getPiece(move.getPosition());
            if (targetPiece == null || targetPiece.getColor() != this.color)
                return true;
        }

        // on récupère la position target du mouvement et ses coordonnées
        Position target = move.getPosition();
        int targetX = target.getX();
        int targetY = target.getY();

        // les coordonnées de la pièce actuelle (this)
        int posX = this.getX();
        int posY = this.getY();

        int stepX; // direction du pas en x
        int stepY; // direction du pas en Y

        Position scanPos;

        // Si la direction est "Bottom", "BottomRightCorner" ou "BottomLeftCorner", 
        // on se déplace vers le bas (ligne négative : -1), sinon vers le haut (+1).
        // (targetY > posY && targetX == posX) || (targetY > posY && targetX < posX) || (targetY > posY && targetX > posX)
        stepY = targetY > posY ? 1 : -1;

        // Si la direction est "Right", "BottomRightCorner" ou "TopRightCorner",
        // on se déplace vers la droite (colonne positive : +1), sinon vers la gauche (-1).
        // (targetY == posY && targetX > posX) || (targetY < posY && targetX > posX) || (targetY > posY && targetX > posX)
        stepX = targetX > posX ? 1 : -1;

        // Si la direction est strictement horizontale ("Left" ou "Right"),
        // il n'y a pas de mouvement vertical, donc on met stepY à 0.
        if (targetY == posY)
        {
            stepY = 0;
        }
        // Si la direction est strictement verticale ("Bottom" ou "Top"),
        // il n'y a pas de mouvement horizontal, donc on met stepX à 0.
        else if (targetX == posX)
        {
            stepX = 0;
        }

        // on initialise une position (qui va changer) pour la prochaine pièce trouvée dans la direction du mouvement
        // et qui démarre à la position de la pièce actuelle (this)
        scanPos = this.position.copy();

        // puis on déplace la position dans la direction donnée jusqu'à trouver une case non vide
        // ou atteindre la position target du mouvement étudié
        while (chessBoard.isNotOut(scanPos)
            && !scanPos.equals(target))
        {
            scanPos.incrementXY(stepX, stepY);
            if (chessBoard.getPiece(scanPos) != null)
            {
                print($"scan stoped at ({scanPos.getX()}, {scanPos.getY()}) with piece : {chessBoard.getPiece(scanPos).getName()}");
                break;
            }
            else if (scanPos.equals(target))
            {
                print($"scan stoped at ({scanPos.getX()}, {scanPos.getY()}) because the target is here");
            }
        }

        // Si la case sur laquelle on s'est arrêté est différente de la target du move
        // ou que la case comporte une pièce de même couleur, alors le move n'est pas valide (passage à travers une pièce)
        Piece foundPiece = chessBoard.getPiece(scanPos);
        if (!scanPos.equals(target) || foundPiece != null && foundPiece.getColor() == this.color)
        {
            print("Way is not clear\nscanPos.equals(target) = " + scanPos.equals(target) + "\nfoundPiece null ? " + foundPiece == null);
            return false;
        }

        // on ne passe jamais au dessus d'une pièce et on ne s'arrête pas sur une case remplie par une pièce
        // de la même couleur, donc tout va bien
        print("way is clear");
        return true;
    }

    // King
    private bool isKingLegalMove(Move move)
    {
        // on ne se deplace que d'une case
        if (move.getPosition().distanceX(this.position) <= 1
            && move.getPosition().distanceY(this.position) <= 1
            && !this.isCheck(move.getPosition(), chessBoard))
        {
            // faire le roque et isCheck ////////////////////////////////////////////////////////////////////
            return true;
        }
        print($"move to ({move.getPosition().getY()}, {move.getPosition().getY()}) is not king legal move");
        return false;
    }

    // Queen
    private bool isQueenLegalMove(Move move)
    {
        // ne se deplace qu'horizontalement, verticalement ou en diagonale
        if (isRookLegalMove(move) || isBishopLegalMove(move))
        {
            return true;
        }
        print($"move to ({move.getPosition().getY()}, {move.getPosition().getY()}) is not queen legal move");
        return false;
    }

    // Rook
    private bool isRookLegalMove(Move move)
    {
        // ne se deplace que horizontalement ou verticalement 
        if (move.getPosition().getY() == this.position.getY() || move.getPosition().getX() == this.position.getX())
        {
            return true;
        }
        print($"move to ({move.getPosition().getY()}, {move.getPosition().getY()}) is not rook legal move");
        return false;
    }

    // Bishop
    private bool isBishopLegalMove(Move move)
    {
        // ne se deplace que horizontalement ou verticalement 
        if (move.getPosition().distanceY(this.position) == move.getPosition().distanceX(this.position))
        {
            return true;
        }
        print($"move to ({move.getPosition().getY()}, {move.getPosition().getY()}) is not bishop legal move");
        return false;
    }

    // Knight
    private bool isKnightLegalMove(Move move)
    {
        // ne se deplace que en L 
        if (move.getPosition().distanceX(this.position) == 2
            && move.getPosition().distanceY(this.position) == 1
            || move.getPosition().distanceX(this.position) == 1
            && move.getPosition().distanceY(this.position) == 2)
        {
            return true;
        }
        print($"move to ({move.getPosition().getY()}, {move.getPosition().getY()}) is not knight legal move");
        return false;
    }

    // Pawn
    private bool isPawnLegalMove(Move move)
    {
        // S'il est blanc sa ligne de départ est la 1 sinon la 6
        int startLine = this.color == "White" ? 6 : 1; // TODO ici aussi

        Position target = move.getPosition();
        int targetX = target.getX();
        int targetY = target.getY();

        int posX = this.getX();
        int posY = this.getY();

        // si le pion n'a pas bougé et qu'il avance de deux cases verticalement
        // ou
        // si le pion avance d'une case verticalement
        //print($"double : {targetY} == {posY + 2 * this.directionY} && {posY} == {startLine} && {targetX} == {posX}");
        //print($"eat : {targetY} == {posY + this.directionY} && {target.distanceX(this.position)} == 1");
        if ((targetY == posY + 2 * this.directionY && posY == startLine && targetX == posX)
            || (targetY == posY + this.directionY && targetX == posX))
        {
            return true;
        }
        else if (targetY == posY + this.directionY && target.distanceX(this.position) == 1)
        {
            Piece lastMovedPiece = this.chessBoard.getLastMoveFromHistory().getPiece();

            // si le pion se déplace d'une case en diagonale sur une case occuppée par un pion adverse
            //print($" eat 2 : {this.chessBoard.getPiece(target)} != null && {this.color} != {this.chessBoard.getPiece(this.position).getColor()}");
            if (this.chessBoard.getPiece(target) != null
                && this.color != this.chessBoard.getPiece(target).getColor())
            {
                return true;
            }
            // si le pion fait une prise en passant
            else if (lastMovedPiece.getName() == "Pawn"
                && lastMovedPiece.getColor() != this.color
                && target.getX() == lastMovedPiece.getPosition().getX()
                && targetY == 2 && posY == 3 || targetY == 5 && posY == 4
                && lastMovedPiece.getPosition().distanceX(this.position) == 1)
            {
                return true;
            }
        }
        print($"move to ({targetX}, {targetY}) is not pawn legal move");
        return false;
    }

    // vérifie si la case move.position est attaquée
    public bool isCheck(Position target, ChessBoard chessBoard)
    {
        // on créé une copie de la position à vérifier
        Position pos = target.copy();
        // une piece temporaire
        Piece tempPiece;

        // on regarde si un cavalier, le roi ou la reine adverse est a proximité
        // dans un carré de 16 cases autour de la case à vérifier
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                pos.setPosition(pos.getX() + i, pos.getY() + j);
                tempPiece = chessBoard.getPiece(pos);
                if (tempPiece != null && tempPiece.getColor() != this.color)
                {
                    // on vérifie les cavaliers adverse à ces cases là :
                    /*
                        . X . X .
                        X . . . X
                        . . O . .
                        X . . . X
                        . X . X .
                    */
                    if ((Mathf.Abs(i) == 1 && Mathf.Abs(j) == 2 || Mathf.Abs(i) == 2 && Mathf.Abs(j) == 1)
                        && (tempPiece.getName() == "Knight"))
                    {
                        print($"Check with kinght at ({tempPiece.getX()}, {tempPiece.getY()})");
                        this.dangerousPiece = tempPiece;
                        return true;
                    }
                    else if (Mathf.Abs(i) == 1 && Mathf.Abs(j) == 1
                        && (tempPiece.getName() == "King"
                        || tempPiece.getName() == "Queen"))
                    {
                        print($"Check with {tempPiece.name} at ({tempPiece.getX()}, {tempPiece.getY()})");
                        this.dangerousPiece = tempPiece;
                        return true;
                    }
                }
            }
        }

        // une autre pièce temporaire et son nom
        Piece pieceFound;
        string pieceFoundName;

        // on trace une ligne dans chaque direction pour vérifier si une pièce n'attaque pas la case
        for (int i = 0; i < 8; i++)
        {
            // on réinitialise la position temporaire
            pos.setPosition(target.getX(), target.getY());

            // on regarde la permière piece touvée dans une direction donnée
            chessBoard.findNextPiece(directions[i], pos);
            pieceFound = chessBoard.getPiece(pos);
            // si c'est une pièce adverse,
            if (pieceFound != null && pieceFound.getColor() != this.color)
            {
                pieceFoundName = pieceFound.getName();
                // s'il y a une reine,
                if (pieceFoundName == "Queen")
                {
                    print($"Check with {pieceFound.name} at ({pieceFound.getX()}, {pieceFound.getY()})");
                    this.dangerousPiece = pieceFound;
                    return true;
                }
                // une tour adverse qui attaque la case en ligne droite,
                else if (i < 4 && pieceFoundName == "Rook")
                {
                    print($"Check with {pieceFound.name} at ({pieceFound.getX()}, {pieceFound.getY()})");
                    this.dangerousPiece = pieceFound;
                    return true;
                }
                else if (i >= 4)
                {
                    // un fou en diagonale
                    if (pieceFoundName == "Bishop")
                    {
                        print($"Check with {pieceFound.name} at ({pieceFound.getX()}, {pieceFound.getY()})");
                        this.dangerousPiece = pieceFound;
                        return true;
                    }
                    // ou un pion proche en diagonale
                    else if (pieceFoundName == "Pawn"
                        && (pieceFound.getColor() == "Black"
                        && pieceFound.getY() == target.getY() - 1
                        || pieceFound.getColor() == "White"
                        && pieceFound.getY() == target.getY() + 1)
                        && Mathf.Abs(pos.getX() - target.getX()) == 1) // TODO distanceX
                    {
                        print($"Check with {pieceFound.name} at ({pieceFound.getX()}, {pieceFound.getY()})");
                        this.dangerousPiece = pieceFound;
                        return true;
                    }
                }
            }
        }

        // sinon c'est que la case n'est pas attaquée
        return false;
    }

    /* public void promote()
    {
        string choosedPiece;
        // choosedPiece = saisie parmi {"Queen", "Bishop", "Rook", "Knight"}
        // this.Name = choosedPiece;
    } */

    private void kingPossibleMoves(List<Move> moves)
    {
        // opstimisation possible
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Move move = new Move(this, this.getX() + i, this.getY() + j);
                if (isLegalMove(move))
                {
                    moves.Add(move);
                }
            }
        }
    }
    private void rookPossibleMoves(List<Move> moves)
    {
        // optimisation possible
        for (int i = 0; i < 8; i++)
        {
            Move move = new Move(this, i, this.getY());
            if (isLegalMove(move))
            {
                moves.Add(move);
            }
        }
        for (int i = 0; i < 8; i++)
        {
            Move move = new Move(this, this.getX(), i);
            if (isLegalMove(move))
            {
                moves.Add(move);
            }
        }
    }

    private void bishopPossibleMoves(List<Move> moves)
    {
        // optimisation possible
        Position pos;
        int stepX;
        int stepY;
        for (int i = 0; i < 4; i++)
        {
            // chaque case possible pour un fou
            switch (i)
            {
                case 0: stepX = 1; stepY = 1; break;
                case 1: stepX = 1; stepY = -1; break;
                case 2: stepX = -1; stepY = 1; break;
                case 3: stepX = -1; stepY = -1; break;
                default: return;
            }
            pos = new Position(this.getX(), this.getY());
            while (chessBoard.isNotOut(pos))
            {
                Move move = new Move(this, pos.getX(), pos.getY());
                if (isLegalMove(move))
                {
                    moves.Add(move);
                }
                pos.incrementXY(stepX, stepY);
            }
        }
    }

    private void knightPossibleMoves(List<Move> moves)
    {
        // optimisation possible
        int x;
        int y;
        for (int i = 0; i < 8; i++)
        {
            // chaque case possible pour un cavalier
            switch (i)
            {
                case 0: x = this.getX() + 2; y = this.getY() - 1; break;
                case 1: x = this.getX() + 2; y = this.getY() + 1; break;
                case 2: x = this.getX() + 1; y = this.getY() - 2; break;
                case 3: x = this.getX() + 1; y = this.getY() + 2; break;
                case 4: x = this.getX() - 2; y = this.getY() + 1; break;
                case 5: x = this.getX() - 2; y = this.getY() - 1; break;
                case 6: x = this.getX() - 1; y = this.getY() + 2; break;
                case 7: x = this.getX() - 1; y = this.getY() - 2; break;
                default: return;
            }
            Move move = new Move(this, x, y);
            if (isLegalMove(move))
            {
                moves.Add(move);
            }
        }
    }

    private void pawnPossibleMoves(List<Move> moves)
    {
        // optimisation possible
        int x;
        int y;
        for (int i = 0; i < 4; i++)
        {
            // chaque case possible pour un pion
            switch (i)
            {
                case 0: x = this.getX(); y = this.getY() + 1 * this.directionY; break;
                case 1: x = this.getX(); y = this.getY() + 2 * this.directionY; break;
                case 2: x = this.getX() + 1; y = this.getY() + 1 * this.directionY; break;
                case 3: x = this.getX() - 1; y = this.getY() + 1 * this.directionY; break;
                default: return;
            }
            Move move = new Move(this, x, y);
            if (isLegalMove(move))
            {
                moves.Add(move);
            }
        }
    }

    private void printPossibleMoves()
    {
        List<Move> possibleMoves = new List<Move>();
        List<GameObject> possibleSquares = new List<GameObject>();
        switch (this.Name)
        {
            case "King":
                this.kingPossibleMoves(possibleMoves);
                break;
            case "Queen":
                this.rookPossibleMoves(possibleMoves);
                this.bishopPossibleMoves(possibleMoves);
                break;
            case "Bishop":
                this.bishopPossibleMoves(possibleMoves);
                break;
            case "Knight":
                this.knightPossibleMoves(possibleMoves);
                break;
            case "Rook":
                this.rookPossibleMoves(possibleMoves);
                break;
            case "Pawn":
                this.pawnPossibleMoves(possibleMoves);
                break;
        }
        string forPrint = "";
        SpriteRenderer[] onlyInactive = FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(sr => !sr.gameObject.activeInHierarchy).ToArray();
        foreach (Move move in possibleMoves)
        {
            // TODO
            // afficher au joueur les coups contenus dans la liste
            int x = move.getPosition().getX();
            int y = move.getPosition().getY();
            foreach (SpriteRenderer sr in onlyInactive)
            {
                if (sr.name == $"square_{x}_{y}_possible")
                {
                    GameObject possibleSquare = sr.gameObject;
                    // print(possibleSquare.name + " is a possible square");
                    possibleSquare.SetActive(true);
                    possibleSquares.Add(possibleSquare);
                    forPrint += possibleSquare.name + ", ";
                }
            }
        }
        print($"List of possible squares ({forPrint})");
        this.possibleSquares = possibleSquares;
    }

    private bool isClicked()
    {
        // Vérifie si le bouton gauche de la souris est enfoncé
        if (Input.GetMouseButtonDown(0))
        {
            // Crée un rayon depuis la position de la souris
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            // Vérifie si un objet a été touché par le rayon
            if (hit.collider != null)
            {
                // Si l'objet cliqué est celui-ci
                if (hit.collider.gameObject == this.gameObject)
                {
                    // Code à exécuter lorsqu'on clique sur l'objet
                    // attendre que le joueur clique sur une autre case

                    Debug.Log("---------------------------------------------------------------\nL'objet " + this.name + " a été cliqué");
                    return true;
                }
            }
        }
        return false;
    }

    void Update()
    {
        if (this.isClickedVar)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                // Vérifie si un objet a été touché par le rayon
                if (hit.collider != null)
                {
                    // TODO
                    // boucle foreach pour les coups possible et vérification si la case cliquée est l'un d'eux
                    foreach (GameObject possibleSquare in possibleSquares)
                    {
                        if (hit.collider.gameObject == possibleSquare)
                        {
                            int x = (int)possibleSquare.transform.position.x;
                            int y = -(int)possibleSquare.transform.position.y;
                            print($"normalement le move ({x},{y}) est legal");
                            this.moveTo(new Move(this, x, y), this.chessBoard);
                        }
                        possibleSquare.SetActive(false);
                    }
                }
                this.isClickedVar = false;
            }
        }
        else if (this.isClicked())
        {
            this.printPossibleMoves();
            this.isClickedVar = true;
        }
    }
}