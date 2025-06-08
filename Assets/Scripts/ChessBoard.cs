using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class ChessBoard : MonoBehaviour
{
    private List<Piece> pieces;
    public GameObject Pieces;
    private Piece[,] chessBoard;
    private List<Move> movesHistory;
    private List<Piece> capturedBlackPieces;
    private List<Piece> capturedWhitePieces;
    public GameObject capturedPieceCountTextPrefab;

    void Awake()
    {
        this.pieces = new List<Piece>();
        this.movesHistory = new List<Move>();
        this.capturedBlackPieces = new List<Piece>();
        this.capturedWhitePieces = new List<Piece>();
        Piece tempPiece;

        // Remplissage du plateau
        /*
            7| ♖  ♘  ♗  ♕  ♔  ♗  ♘  ♖  Black
            6| ♙  ♙  ♙  ♙  ♙  ♙  ♙  ♙
            5| ◼  ◻  ◼  ◻  ◼  ◻  ◼  ◻
            4| ◻  ◼  ◻  ◼  ◻  ◼  ◻  ◼
            3| ◼  ◻  ◼  ◻  ◼  ◻  ◼  ◻
            2| ◻  ◼  ◻  ◼  ◻  ◼  ◻  ◼
            1| ♟  ♟  ♟  ♟  ♟  ♟  ♟  ♟
            0| ♜  ♞  ♝  ♛  ♚  ♝  ♞  ♜  White
              -----------------------
               0  1  2  3  4  5  6  7 
        */
        foreach (Transform child in Pieces.transform)
        {
            pieces.Add(child.GetComponent<Piece>());
            // Debug.Log("add " + pieces.Last().name);
        }
        this.chessBoard = new Piece[8, 8];
        // on rempli d'abord de cases vides
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                this.chessBoard[y, x] = null;
            }
        }
        // puis on rajoute les pièces
        foreach (Piece piece in pieces)
        {
            int x = (int)piece.transform.position.x;
            int y = (int)piece.transform.position.y;
            this.chessBoard[-y, x] = piece;
        }
        // on remplie les attributs des pièces
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                tempPiece = this.chessBoard[y, x];
                if (y == 1)
                {
                    tempPiece.setAttributes("Pawn", "Black", x, y, this);
                }
                else if (y == 6)
                {
                    tempPiece.setAttributes("Pawn", "White", x, y, this);
                }
                else if (y == 0)
                {
                    tempPiece.setAttributes(getPieceName(x), "Black", x, y, this);
                }
                else if (y == 7)
                {
                    tempPiece.setAttributes(getPieceName(x), "White", x, y, this);
                }
                else
                {
                    continue;
                }
            }
        }
    }

    private string getPieceName(int columnIndex)
    {
        switch (columnIndex)
        {
            case 0: return "Rook";
            case 1: return "Knight";
            case 2: return "Bishop";
            case 3: return "Queen";
            case 4: return "King";
            case 5: return "Bishop";
            case 6: return "Knight";
            case 7: return "Rook";
            default: return "Error";
        }
    }
    /* public ChessBoard()
    {
        this.movesHistory = new List<Move>();
        this.capturedBlackPieces = new List<Piece>();
        this.capturedWhitePieces = new List<Piece>();

        this.chessBoard = new Piece[8, 8];
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                this.chessBoard[y, x] = null;
            }
        }
    } */

    public void addPiece(Piece piece)
    {
        int x = piece.getX();
        int y = piece.getY();
        this.chessBoard[y, x] = piece;
        piece.setChessBoard(this);
    }

    /*
        Fonction qui déplace une pièce donnée à une position données 
        dans le tableau chessBoard

        et modifie le champ position de la pièce
    */
    // précondition : le coup doit etre valide
    public void movePieceChessBoard(Position p, Piece piece)
    {
        // on ajoute la position avant mouvement à la liste des dernieres positions
        piece.setLastPosition(piece.getPosition());

        // on récupère la pièce située à la position donnée
        Piece pieceInP = this.getPiece(p);

        // si la case est occupée, on capture la pièce
        if (pieceInP != null)
        {
            this.capturePiece(pieceInP);
        }
        // on ajoute la piece sur la nouvelle position
        this.chessBoard[p.getY(), p.getX()] = piece;
        // on vide l'ancienne position
        this.chessBoard[piece.getY(), piece.getX()] = null;
        // on met à jour la position de la pièce déplacée
        piece.setPosition(p);

        // on bouge la pièce dans la vue du jeu
        piece.transform.position = new Vector3(piece.getX(), -piece.getY(), -1);
        // on indique à la pièce qu'elle a bougée
        piece.madeMove();
        // debug affichage
        print($"{piece.name} moved to ({piece.getX()}, {piece.getY()})");
    }

    public void capturePiece(Piece p)
    {
        if (p.getColor() == "Black")
        {
            this.capturedBlackPieces.Add(p);
            this.sort(this.capturedBlackPieces);
        }
        else
        {
            this.capturedWhitePieces.Add(p);
            this.sort(this.capturedWhitePieces);
        }
    }

    private void sort(List<Piece> capturedPieces)
    {
        Dictionary<string, int> pieceValue = new Dictionary<string, int>
        {
            {"Queen", 9},
            {"Rook", 5},
            {"Bishop", 3},
            {"Knight", 3}, // meme valeur que bishop normalement mais ici non pour le tri
            {"Pawn", 1}
        };

        capturedPieces.Sort((a, b) =>
        {
            int valueA = pieceValue.ContainsKey(a.getName()) ? pieceValue[a.getName()] : 0;
            int valueB = pieceValue.ContainsKey(b.getName()) ? pieceValue[b.getName()] : 0;
            // On trie de la plus forte à la plus faible
            return valueB.CompareTo(valueA);
        });
    }

    public bool isNotOut(Position p)
    {
        if (p.getX() >= 0 && p.getX() <= 7 && p.getY() <= 7 && p.getY() >= 0)
        {
            return true;
        }
        //print("position out of board");
        return false;
    }


    public Piece getPiece(Position p)
    {
        if (isNotOut(p))
        {
            return this.chessBoard[p.getY(), p.getX()];
        }
        return null;
    }


    public Piece getKing(string color)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece p = this.chessBoard[j, i];
                if (p != null && p.getName() == "King" && p.getColor() == color)
                {
                    return p;
                }
            }
        }
        return null;
    }

    public void addMoveToHistory(Move m)
    {
        this.movesHistory.Add(m);
    }

    /*
    public Move getMoveFromHistory(int index)
    {
        return this.movesHistory[index];
    }
    */

    public Move getLastMoveFromHistory()
    {
        return this.movesHistory.Last();
    }

    // scan dans la direction donnée depuis la position donnée jusqu'à tomber sur une pièce ou sortir du plateau
    public void findNextPiece(string direction, Position position)
    {
        int posX;
        int posY;
        // Déterminer la direction de mouvement selon les axes lignes et colonnes

        // Si la direction est "Bottom", "BottomRightCorner" ou "BottomLeftCorner", 
        // on se déplace vers le bas (ligne négative : -1), sinon vers le haut (+1).
        int stepY = direction == "Bottom" || direction == "BottomRightCorner" || direction == "BottomLeftCorner" ? 1 : -1;

        // Si la direction est "Right", "BottomRightCorner" ou "TopRightCorner",
        // on se déplace vers la droite (colonne positive : +1), sinon vers la gauche (-1).
        int stepX = direction == "Right" || direction == "BottomRightCorner" || direction == "TopRightCorner" ? 1 : -1;

        // Si la direction est strictement horizontale ("Left" ou "Right"),
        // il n'y a pas de mouvement vertical, donc on met stepY à 0.
        if (direction == "Left" || direction == "Right")
        {
            stepY = 0;
        }
        // Si la direction est strictement verticale ("Bottom" ou "Top"),
        // il n'y a pas de mouvement horizontal, donc on met stepX à 0.
        else if (direction == "Bottom" || direction == "Top")
        {
            stepX = 0;
        }
        //print($"findNextPiece({direction},({position.getX()}, {position.getY()}))");

        // puis on déplace la position dans la direction donnée jusqu'à trouver une case non vide
        // ou atteindre la position target du mouvement étudié
        bool firstIteration = true;
        while (isNotOut(position))
        {
            posX = position.getX();
            posY = position.getY();
            if (firstIteration == true || this.chessBoard[posY, posX] == null)
            {
                position.incrementXY(stepX, stepY);
                firstIteration = false;
                continue;
            }
            break;
        }
        if (this.getPiece(position) != null)
        {
            //print($"piece found at ({position.getX()}, {position.getY()}) : {this.getPiece(position).name}");
        }
    }

    public void print()
    {
        Debug.Log(this.toString());
    }

    public string toString()
    {
        /*
            7| ♖  ♘  ♗  ♕  ♔  ♗  ♘  ♖  Black
            6| ♙  ♙  ♙  ♙  ♙  ♙  ♙  ♙
            5| ◼  ◻  ◼  ◻  ◼  ◻  ◼  ◻
            4| ◻  ◼  ◻  ◼  ◻  ◼  ◻  ◼
            3| ◼  ◻  ◼  ◻  ◼  ◻  ◼  ◻
            2| ◻  ◼  ◻  ◼  ◻  ◼  ◻  ◼
            1| ♟  ♟  ♟  ♟  ♟  ♟  ♟  ♟
            0| ♜  ♞  ♝  ♛  ♚  ♝  ♞  ♜  White
              -----------------------
               0  1  2  3  4  5  6  7 
        */
        Position p = new Position(0, 0);
        Piece piece;
        string str = "";
        for (int y = 0; y < 8; y++)
        {
            str += 8 - y + "|";
            for (int x = 0; x < 8; x++)
            {
                p.setPosition(x, y);
                piece = this.getPiece(p);
                if (piece == null)
                {
                    str += " ◻ ";
                    continue;
                }
                if (piece.getColor() == "Black")
                {
                    switch (piece.getName())
                    {
                        case "King":
                            str += " ♔ ";
                            break;
                        case "Queen":
                            str += " ♕ ";
                            break;
                        case "Bishop":
                            str += " ♗ ";
                            break;
                        case "Knight":
                            str += " ♘ ";
                            break;
                        case "Rook":
                            str += " ♖ ";
                            break;
                        case "Pawn":
                            str += " ♙ ";
                            break;
                    }
                }
                else
                {
                    switch (piece.getName())
                    {
                        case "King":
                            str += " ♚ ";
                            break;
                        case "Queen":
                            str += " ♛ ";
                            break;
                        case "Bishop":
                            str += " ♝ ";
                            break;
                        case "Knight":
                            str += " ♞ ";
                            break;
                        case "Rook":
                            str += " ♜ ";
                            break;
                        case "Pawn":
                            str += " ♟ ";
                            break;
                    }
                }
            }
            str += "\n";
        }
        str += "    -----------------------\n     a   b   c   d   e   f   g   h";
        return str;
    }

    private void displayCapturedPieces(List<Piece> capturedPieces, int xDisplay)
    {
        int i = 0;
        string[] names = {
            "Queen",
            "Rook",
            "Bishop",
            "Knight",
            "Pawn"
        };

        foreach (string name in names)
        {
            List<Piece> piecesToDisplay = capturedPieces.FindAll(p => p.getName() == name);
            if (piecesToDisplay.Count > 0)
            {
                piecesToDisplay.ForEach(p => p.transform.position = new Vector3(xDisplay, -i, -1));

                Piece p = piecesToDisplay[0];

                // Affiche le nombre à côté
                if (piecesToDisplay.Count > 1)
                {
                    // Instancie ou met à jour un texte à côté de la pièce
                    GameObject textObj = Instantiate(this.capturedPieceCountTextPrefab, p.transform.position + new Vector3(0.1f, -0.1f, 0), Quaternion.identity, p.transform);
                    var textMesh = textObj.GetComponent<TextMesh>();
                    if (textMesh != null)
                        textMesh.text = "x" + piecesToDisplay.Count;
                }
                i++;
            }
        }
    }

    void Update()
    {
        displayCapturedPieces(this.capturedBlackPieces, 10);
        displayCapturedPieces(this.capturedWhitePieces, 11);
    }
}
