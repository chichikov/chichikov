using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class ChessEnginMoveManager {

	
	public enum MoveType {
		
		eNone_Move,
		eNormal_Move,
		eCapture_Move,
		eEnPassan_Move,
		ePromote_Move,
		eCastling_Move
	}
	
	
	
	public struct sMove {
		
		public MoveType moveType;	
		
		public ChessBoardData.BoardPosition srcPos;
		public ChessBoardData.BoardPosition trgPos;
		
		//ChessPiece capturePiece;	
		/*
		public sMove( MoveType moveType, ChessBoardData.BoardPosition srcPos, ChessBoardData.BoardPosition trgPos ) {
			
			this.moveType = moveType;
			
			this.srcPos = srcPos;
			this.trgPos = trgPos;
		}			
		
		public void Set( MoveType moveType, ChessBoardData.BoardPosition srcPos, ChessBoardData.BoardPosition trgPos ) {
			
			this.moveType = moveType;
			
			this.srcPos = srcPos;
			this.trgPos = trgPos;			
		}
		*/		
	}
	
	// validate move position
	public static void GetValidateMoveList( ChessBoardData.ChessPiece[,] aBoard, ChessBoardData.ChessPiece piece, 
											List<sMove> listRetBoardPos ) {			
		
		ChessBoardData.BoardPosition srcPos = piece.position.pos;
		ChessBoardData.PlayerSide playerSide = piece.playerSide;
		ChessBoardData.PieceType pPieceType = piece.pieceType;		
		
		int nMove, nMoveRankIdx = 0, nMovePileIdx = 0;
		ChessBoardData.BoardPosition movePos;
		
		sMove move;		
		
		if( playerSide == ChessBoardData.PlayerSide.e_White ) {
			
			switch( pPieceType ) {
				
				case ChessBoardData.PieceType.e_King:
				{
				
				}
				break;
				
				case ChessBoardData.PieceType.e_Queen:
				{
				
				}	
				break;
				
				case ChessBoardData.PieceType.e_Look:
				{
				
				}
				break;
				
				case ChessBoardData.PieceType.e_Bishop:
				{
				
				}
				break;
				
				case ChessBoardData.PieceType.e_Knight:
				{
				
				}
				break;
				
				case ChessBoardData.PieceType.e_Pawn:
				{
					// pure move
					nMove = (int)srcPos + ChessBoardData.nNumRank;
					if( nMove < (int)ChessBoardData.BoardPosition.InvalidPosition  )
					{
						// check already existing piece					
						movePos = (ChessBoardData.BoardPosition)nMove;						
						bool bValidIdx = ChessBoardData.ChessPosition.GetPositionIndex( movePos, ref nMoveRankIdx, ref nMovePileIdx );
						if( bValidIdx ) {
						
							if( aBoard[nMovePileIdx, nMoveRankIdx].gameObject == null ) {															
								
								move.moveType = MoveType.eNormal_Move;
								move.srcPos = srcPos;
								move.trgPos = movePos;
								
								listRetBoardPos.Add( move );
							}							
						}
					}	
				
					nMove = (int)srcPos + ChessBoardData.nNumRank * 2;
					if( nMove < (int)ChessBoardData.BoardPosition.InvalidPosition  )
					{
						// check already existing piece					
						movePos = (ChessBoardData.BoardPosition)nMove;						
						bool bValidIdx = ChessBoardData.ChessPosition.GetPositionIndex( movePos, ref nMoveRankIdx, ref nMovePileIdx );
						if( bValidIdx ) {
						
							if(	aBoard[nMovePileIdx, nMoveRankIdx].gameObject == null ) {														
								
								move.moveType = MoveType.eNormal_Move;
								move.srcPos = srcPos;
								move.trgPos = movePos;
								
								listRetBoardPos.Add( move );
							}							
						}
					}
				
					// capture move
				
					// en-passant move
				}				
				break;				
			}			
		}
		else if( playerSide == ChessBoardData.PlayerSide.e_Black ) {
			
		}
		
		return;
	}
	
	
	static ChessEnginMoveManager() {
	}
}
