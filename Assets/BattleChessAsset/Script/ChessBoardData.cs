using UnityEngine;
using System.Collections;


// for board data definition
public class ChessBoardData {
	
	public enum PlayerSide {
		e_NoneSide = 0,
		e_White,
		e_Black	
	};
	
	public enum PieceType {
		e_King = 0,
		e_Queen,
		e_Look,
		e_Bishop,
		e_Knight,
		e_Pawn,		
	};	
	
	// number of board square
	public static int nNumBoardSquare = 64;
	// number of pieces white = 16, black = 16
	public static int nNumWholePiece = 32;
	public static int nNumWhitePiece = 16;
	public static int nNumBlackPiece = 16;	
	// number of pile and rank
	public static int nNumPile = 8;
	public static int nNumRank = 8;	
	
	
	
	public static int nNone_Piece = -1;
	public static int nWhite_King = 0;
	public static int nWhite_Queen = 1;
	public static int nWhite_LookLeft = 2;
	public static int nWhite_LookRight = 3;
	public static int nWhite_BishopLeft = 4;
	public static int nWhite_BishopRight = 5;
	public static int nWhite_KnightLeft = 6;
	public static int nWhite_KnightRight = 7;
	public static int nWhite_Pawn1 = 8;
	public static int nWhite_Pawn2 = 9;
	public static int nWhite_Pawn3 = 10;
	public static int nWhite_Pawn4 = 11;
	public static int nWhite_Pawn5 = 12;
	public static int nWhite_Pawn6 = 13;
	public static int nWhite_Pawn7 = 14;
	public static int nWhite_Pawn8 = 15;
	
	public static int nBlack_King = 16;
	public static int nBlack_Queen = 17;	
	public static int nBlack_LookLeft = 18;
	public static int nBlack_LookRight = 19;
	public static int nBlack_BishopLeft = 20;
	public static int nBlack_BishopRight = 21;
	public static int nBlack_KnightLeft = 22;
	public static int nBlack_KnightRight = 23;
	public static int nBlack_Pawn1 = 24;
	public static int nBlack_Pawn2 = 25;
	public static int nBlack_Pawn3 = 26;
	public static int nBlack_Pawn4 = 27;
	public static int nBlack_Pawn5 = 28;
	public static int nBlack_Pawn6 = 29;
	public static int nBlack_Pawn7 = 30;
	public static int nBlack_Pawn8 = 31;
	
	
	public static int [,] aStartPiecePos = new int[8,8] {
		{nWhite_LookLeft, nWhite_KnightLeft, nWhite_BishopLeft, nWhite_Queen, nWhite_King, nWhite_BishopRight, nWhite_KnightRight, nWhite_LookRight},
		{nWhite_Pawn1, nWhite_Pawn2, nWhite_Pawn3, nWhite_Pawn4, nWhite_Pawn5, nWhite_Pawn6, nWhite_Pawn7, nWhite_Pawn8},
		
		{nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece},
		{nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece},
		{nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece},
		{nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece, nNone_Piece},
		
		{nBlack_Pawn1, nBlack_Pawn2, nBlack_Pawn3, nBlack_Pawn4, nBlack_Pawn5, nBlack_Pawn6, nBlack_Pawn7, nBlack_Pawn8},
		{nBlack_LookLeft, nBlack_KnightLeft, nBlack_BishopLeft, nBlack_Queen, nBlack_King, nBlack_BishopRight, nBlack_KnightRight, nBlack_LookRight}
	};
	
	
	public struct ChessPiece {
		
		public GameObject gameObject;
		public PlayerSide playerSide;	
		
		
		public void SetPiece( GameObject gameObject, PlayerSide playerSide ) {
			
			this.gameObject = gameObject;
			this.playerSide = playerSide;
		}
	}
	
	static ChessBoardData()	{	
		
	}
}
