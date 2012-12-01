using UnityEngine;
using System.Collections;

using System.Collections.Generic;



// for board data definition
public class ChessBoardData {
	
	public enum PlayerSide {
		e_NoneSide = 0,
		e_White,
		e_Black	
	};
	
	public enum PieceType {
		e_None = -1,
		e_King = 0,
		e_Queen,
		e_Look,
		e_Bishop,
		e_Knight,
		e_Pawn,		
	};	
	
	public enum PieceInstanceType : int {
		
		eNone_Piece = -1,
		eWhite_King = 0,
		eWhite_Queen,
		eWhite_LookLeft,
		eWhite_LookRight,
		eWhite_BishopLeft,
		eWhite_BishopRight,
		eWhite_KnightLeft,
		eWhite_KnightRight,
		eWhite_Pawn1,
		eWhite_Pawn2,
		eWhite_Pawn3,
		eWhite_Pawn4,
		eWhite_Pawn5,
		eWhite_Pawn6,
		eWhite_Pawn7,
		eWhite_Pawn8,
		
		eBlack_King,
		eBlack_Queen,
		eBlack_LookLeft,
		eBlack_LookRight,
		eBlack_BishopLeft,
		eBlack_BishopRight,
		eBlack_KnightLeft,
		eBlack_KnightRight,
		eBlack_Pawn1,
		eBlack_Pawn2,
		eBlack_Pawn3,
		eBlack_Pawn4,
		eBlack_Pawn5,
		eBlack_Pawn6,
		eBlack_Pawn7,
		eBlack_Pawn8
	};
	
	public enum BoardPosition {
    
        //BoardPositions   
	    A1 = 0x00, B1 = 0x01, C1 = 0x02, D1 = 0x03, E1 = 0x04, F1 = 0x05, G1 = 0x06, H1 = 0x07,
	    A2 = 0x08, B2 = 0x09, C2 = 0x0A, D2 = 0x0B, E2 = 0x0C, F2 = 0x0D, G2 = 0x0E, H2 = 0x0F,
	    A3 = 0x10, B3 = 0x11, C3 = 0x12, D3 = 0x13, E3 = 0x14, F3 = 0x15, G3 = 0x16, H3 = 0x17,
	    A4 = 0x18, B4 = 0x19, C4 = 0x1A, D4 = 0x1B, E4 = 0x1C, F4 = 0x1D, G4 = 0x1E, H4 = 0x1F,
	    A5 = 0x20, B5 = 0x21, C5 = 0x22, D5 = 0x23, E5 = 0x24, F5 = 0x25, G5 = 0x26, H5 = 0x27,
	    A6 = 0x28, B6 = 0x29, C6 = 0x2A, D6 = 0x2B, E6 = 0x2C, F6 = 0x2D, G6 = 0x2E, H6 = 0x2F,
	    A7 = 0x30, B7 = 0x31, C7 = 0x32, D7 = 0x33, E7 = 0x34, F7 = 0x35, G7 = 0x36, H7 = 0x37,
	    A8 = 0x38, B8 = 0x39, C8 = 0x3A, D8 = 0x3B, E8 = 0x3C, F8 = 0x3D, G8 = 0x3E, H8 = 0x3F,
	    BoardBits = 0x3F, InvalidPosition = 0x40
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
	
	public static float fTileWidth = 1.0f;
	public static float fBoardWidth = nNumRank * fTileWidth;
	
	
	public static PieceInstanceType [,] aStartPiecePos = new PieceInstanceType[8,8] {
		
		{PieceInstanceType.eWhite_LookLeft, PieceInstanceType.eWhite_KnightLeft, PieceInstanceType.eWhite_BishopLeft, PieceInstanceType.eWhite_Queen, PieceInstanceType.eWhite_King, PieceInstanceType.eWhite_BishopRight, PieceInstanceType.eWhite_KnightRight, PieceInstanceType.eWhite_LookRight},
		{PieceInstanceType.eWhite_Pawn1, PieceInstanceType.eWhite_Pawn2, PieceInstanceType.eWhite_Pawn3, PieceInstanceType.eWhite_Pawn4, PieceInstanceType.eWhite_Pawn5, PieceInstanceType.eWhite_Pawn6, PieceInstanceType.eWhite_Pawn7, PieceInstanceType.eWhite_Pawn8},
		
		{PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece},
		{PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece},
		{PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece},
		{PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece, PieceInstanceType.eNone_Piece},
		
		{PieceInstanceType.eBlack_Pawn1, PieceInstanceType.eBlack_Pawn2, PieceInstanceType.eBlack_Pawn3, PieceInstanceType.eBlack_Pawn4, PieceInstanceType.eBlack_Pawn5, PieceInstanceType.eBlack_Pawn6, PieceInstanceType.eBlack_Pawn7, PieceInstanceType.eBlack_Pawn8},
		{PieceInstanceType.eBlack_LookLeft, PieceInstanceType.eBlack_KnightLeft, PieceInstanceType.eBlack_BishopLeft, PieceInstanceType.eBlack_Queen, PieceInstanceType.eBlack_King, PieceInstanceType.eBlack_BishopRight, PieceInstanceType.eBlack_KnightRight, PieceInstanceType.eBlack_LookRight}
	};
	
	public static PieceType GetPieceType( PieceInstanceType pieceInstType ) {
		
		if( pieceInstType >= PieceInstanceType.eWhite_King && pieceInstType <= PieceInstanceType.eWhite_Pawn8 ) {
			
			switch( pieceInstType )
			{
				case PieceInstanceType.eWhite_King:
					return PieceType.e_King;				
				
				case PieceInstanceType.eWhite_Queen:
					return PieceType.e_Queen;				
				
				case PieceInstanceType.eWhite_LookLeft:				
				case PieceInstanceType.eWhite_LookRight:
					return PieceType.e_Look;
								
				case PieceInstanceType.eWhite_BishopLeft:
				case PieceInstanceType.eWhite_BishopRight:
					return PieceType.e_Bishop;				
				
				case PieceInstanceType.eWhite_KnightLeft:
				case PieceInstanceType.eWhite_KnightRight:
					return PieceType.e_Knight;				
				
				case PieceInstanceType.eWhite_Pawn1:
				case PieceInstanceType.eWhite_Pawn2:
				case PieceInstanceType.eWhite_Pawn3:
				case PieceInstanceType.eWhite_Pawn4:
				case PieceInstanceType.eWhite_Pawn5:
				case PieceInstanceType.eWhite_Pawn6:
				case PieceInstanceType.eWhite_Pawn7:
				case PieceInstanceType.eWhite_Pawn8:
					return PieceType.e_Pawn;						
			}
		}
		else if( pieceInstType >= PieceInstanceType.eWhite_King && pieceInstType <= PieceInstanceType.eWhite_Pawn8 ) {
			
			switch( pieceInstType )
			{
				case PieceInstanceType.eBlack_King:
					return PieceType.e_King;				
				
				case PieceInstanceType.eBlack_Queen:
					return PieceType.e_Queen;				
				
				case PieceInstanceType.eBlack_LookLeft:				
				case PieceInstanceType.eBlack_LookRight:
					return PieceType.e_Look;				
				
				case PieceInstanceType.eBlack_BishopLeft:
				case PieceInstanceType.eBlack_BishopRight:
					return PieceType.e_Bishop;				
				
				case PieceInstanceType.eBlack_KnightLeft:
				case PieceInstanceType.eBlack_KnightRight:
					return PieceType.e_Knight;				
				
				case PieceInstanceType.eBlack_Pawn1:
				case PieceInstanceType.eBlack_Pawn2:
				case PieceInstanceType.eBlack_Pawn3:
				case PieceInstanceType.eBlack_Pawn4:
				case PieceInstanceType.eBlack_Pawn5:
				case PieceInstanceType.eBlack_Pawn6:
				case PieceInstanceType.eBlack_Pawn7:
				case PieceInstanceType.eBlack_Pawn8:
					return PieceType.e_Pawn;				
			}
		}
		else {
			
			return PieceType.e_None;	
		}
		
		return PieceType.e_None;
	}
	
	public struct ChessPosition {
		
		public BoardPosition pos;		
		
		public ChessPosition( int nRank, int nPile ) {
			
			if( nRank >= 0 && nRank <= nNumRank && nPile >= 0 && nPile <= nNumPile ) {
			
				pos = (BoardPosition)(nPile * nNumRank + nRank);
			}
			else{
				
				pos = BoardPosition.InvalidPosition;	
			}	
		}
		
		public ChessPosition( BoardPosition boardPos ) {
			
			if( boardPos >= BoardPosition.A1 && boardPos < BoardPosition.InvalidPosition ) {
			
				pos = boardPos;
			}
			else{
				
				pos = BoardPosition.InvalidPosition;	
			}	
		}
		
		public void SetPosition( int nPile, int nRank ) {
			
			if( nRank >= 0 && nRank <= nNumRank && nPile >= 0 && nPile <= nNumPile ) {
			
				pos = (BoardPosition)(nPile * nNumRank + nRank);
			}
			else{
				
				pos = BoardPosition.InvalidPosition;		
			}			
		}
		
		public static bool GetPositionIndex( BoardPosition pos, ref int nRank, ref int nPile ) {
			
			if( pos >= BoardPosition.InvalidPosition )
				return false;
			
			int nPos = (int)pos;
			nRank = nPos % nNumRank;
			nPile = nPos / nNumRank;
			
			return true;
		}
	}
	
	public struct ChessPiece {
		
		public GameObject gameObject;
		
		public PlayerSide playerSide;
		public PieceType pieceType;
		public PieceInstanceType pieceInstType;
		
		public ChessPosition position;		
		
		public ParticleSystem movablePSystem;
		
				
		public void SetPiece( GameObject gameObject, PlayerSide playerSide, 
			PieceInstanceType pieceInstType, int nPile, int nRank ) {
			
			this.gameObject = gameObject;
			this.playerSide = playerSide;
			this.pieceType = ChessBoardData.GetPieceType( pieceInstType );
			this.pieceInstType = pieceInstType;
			this.position.SetPosition( nPile, nRank );			
		}
		
		public void SetPiece( ref ChessPiece chessPiece ) {
			
			this.gameObject = chessPiece.gameObject;
			this.playerSide = chessPiece.playerSide;
			this.pieceType = chessPiece.pieceType;
			this.pieceInstType = chessPiece.pieceInstType;
			this.position = chessPiece.position;			
		}
		
		public void Clear() {
			
			gameObject = null;		
			playerSide = PlayerSide.e_NoneSide;
			pieceType = PieceType.e_None;
			pieceInstType = PieceInstanceType.eNone_Piece;			
			position.pos = BoardPosition.InvalidPosition;			
		}
		
		public void SetMovableEffect( ParticleSystem movablePSystem ) {
			
			if( position.pos == BoardPosition.InvalidPosition )
				return;					
			
			this.movablePSystem = movablePSystem;							
			this.movablePSystem.Stop();
			
			int nRank = 0, nPile = 0;
			ChessPosition.GetPositionIndex( position.pos, ref nRank, ref nPile );			
			
			Vector3 pos = Vector3.zero;
			pos.x = nRank - 3.5f;
			pos.y = 0.01f;
			pos.z = nPile - 3.5f;
			Quaternion rot = Quaternion.identity;
			
			this.movablePSystem.gameObject.transform.position = pos;
			this.movablePSystem.gameObject.transform.rotation = rot;
			
			this.movablePSystem.gameObject.renderer.material.SetColor( "_Color", Color.blue );
		}
		
		public void ShowMovableEffect( bool bShow ) {
			
			if( movablePSystem == null )
				return;
			
			if( bShow ) {
				
				movablePSystem.Play();				
			}
			else{
				
				movablePSystem.Stop();
			}			
		}		
	}	
	
	
	
	
	static ChessBoardData()	{	
		
	}
}
