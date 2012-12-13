using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;


//namespace BattleChess {	

public class ChessMoveManager {	
	
	
	[Flags]
	public enum MoveType {
		
		eNone_Move = 0x0,
		eNormal_Move = 0x1,
		eCapture_Move = 0x2,
		eEnPassan_Move = 0x04,
		ePromote_Move = 0x08,
		eCastling_Move = 0x10,
		ePawn_Move = 0x20
	}
	
	
	
	public struct sMove {
		
		public MoveType moveType;	
		
		public ChessPosition srcPos;
		public ChessPosition trgPos;
		
		public ChessPiece movePiece;
		
		public ChessEnPassant EnPassantTargetSquare { get; set; }
		
		//ChessPiece capturePiece;		
		public sMove( ChessPiece movePiece, MoveType moveType, ChessPosition srcPos, ChessPosition trgPos ) {
			
			this.moveType = moveType;
			
			this.srcPos = srcPos;
			this.trgPos = trgPos;	
			
			this.movePiece = movePiece;
			
			this.EnPassantTargetSquare = new ChessEnPassant() {
				
				Rank = -1,
				Pile = -1,
				Available = false				
			};
		}			
		
		public void Set( ChessPiece movePiece, MoveType moveType, ChessPosition srcPos, ChessPosition trgPos ) {
			
			this.moveType = moveType;
			
			this.srcPos = srcPos;
			this.trgPos = trgPos;	
			
			this.movePiece = movePiece;
			
			this.EnPassantTargetSquare = new ChessEnPassant() {
				
				Rank = -1,
				Pile = -1,
				Available = false				
			};
		}	
		
		public void Set( ChessPiece movePiece, MoveType moveType, ChessPosition srcPos, ChessPosition trgPos, ChessEnPassant enPassantTrgSquare ) {
			
			this.moveType = moveType;
			
			this.srcPos = srcPos;
			this.trgPos = trgPos;
			
			this.movePiece = movePiece;
			
			this.EnPassantTargetSquare = enPassantTrgSquare;
		}
		
		public void Clear() {
			
			this.moveType = MoveType.eNone_Move;
			
			this.srcPos.SetPosition( -1, -1 );
			this.trgPos.SetPosition( -1, -1 );	
			
			this.movePiece = null;
			
			this.EnPassantTargetSquare = new ChessEnPassant() {
				
				Rank = -1,
				Pile = -1,
				Available = false				
			};
		}
		
		// is Increse half move
		public bool IsResetHalfMove() {
			
			if( moveType & ePawn_Move || moveType & eCapture_Move || moveType & eCastling_Move )
				return true;
			return false;
		}
	}	
	
	// validate move position
	public static bool GetValidateMoveList( ChessPiece[,] aBoard, ChessPiece piece, 
											List<sMove> listRetBoardPos ) {		
		
		if( piece.IsBlank() )
			return false;
		
		ChessPosition srcPos = piece.position;		
		PlayerSide srcPlayerSide = piece.playerSide;
		PieceType srcPieceType = piece.pieceType;			
		
		if( srcPos.IsInvalidPos() )
			return false;			
		
		
		int nMoveRankIdx = 0, nMovePileIdx = 0;
		
		ChessPosition movePos = new ChessPosition(srcPos.pos);			
		sMove move = new sMove();		
		
		if( srcPlayerSide == PlayerSide.e_White ) {
			
			switch( srcPieceType ) {
				
				case PieceType.e_King:
				{
				
				}
				break;
				
				case PieceType.e_Queen:
				{
				
				}	
				break;
				
				case PieceType.e_Look:
				{
				
				}
				break;
				
				case PieceType.e_Bishop:
				{
				
				}
				break;
				
				case PieceType.e_Knight:
				{
				
				}
				break;
				
				case PieceType.e_Pawn:
				{
					// pure move
					// pure move - one pile move				
					bool bValidMove = movePos.MovePosition( 0, 1 );
					if( bValidMove ) {					
						
						movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
						// check already existing piece				
						if( aBoard[nMovePileIdx, nMoveRankIdx].IsBlank() ) {																															
							
							// normal move
							move.moveType = MoveType.eNormal_Move | MoveType.ePawn_Move;
							// promote move								
							if( movePos.IsTopBoundary() )
								move.moveType |= MoveType.ePromote_Move;								
						
							move.srcPos = srcPos;
							move.trgPos = movePos;
						
							move.movePiece = aBoard[nMovePileIdx, nMoveRankIdx];
							
							listRetBoardPos.Add( move );
						}	
					}				
					
					// pure move - two pile move
					movePos.SetPosition( srcPos );
					bValidMove = movePos.MovePosition( 0, 2 );
					if( bValidMove ) {
					
						movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
						if(	aBoard[nMovePileIdx, nMoveRankIdx].IsBlank() ) {														
							
							move.moveType = MoveType.eNormal_Move | MoveType.ePawn_Move;
							move.srcPos = srcPos;
							move.trgPos = movePos;
						
							move.movePiece = aBoard[nMovePileIdx, nMoveRankIdx];
						
							// en passant target move check
							if( ((int)srcPos.pos / ChessData.nNumRank) == 1  ) {
								
								move.EnPassantTargetSquare = new ChessEnPassant() {
									Rank = nMoveRankIdx,
									Pile = nMovePileIdx,
									Available = true
								};							
							}	
							
							listRetBoardPos.Add( move );													
						}
					}					
					
				
					// capture move
					// left diagonal capture	
					// check left boundary
					movePos.SetPosition( srcPos );
					bValidMove = movePos.MovePosition( -1, 1 );					
					if( bValidMove ) {					
					
						movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
						if(	aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {														
							
							// capture move
							move.moveType = MoveType.eCapture_Move | MoveType.ePawn_Move;
						
							// promote move								
							if( movePos.IsTopBoundary() )
								move.moveType |= MoveType.ePromote_Move;
						
							move.srcPos = srcPos;
							move.trgPos = movePos;
						
							move.movePiece = aBoard[nMovePileIdx, nMoveRankIdx];
							
							listRetBoardPos.Add( move );
						}
					}	
				
					// right diagonal capture	
					// check right boundary
					movePos.SetPosition( srcPos );
					bValidMove = movePos.MovePosition( 1, 1 );					
					if( bValidMove ) {					
					
						movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
						if(	aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {														
							
							// capture move
							move.moveType = MoveType.eCapture_Move | MoveType.ePawn_Move;
						
							// promote move								
							if( movePos.IsTopBoundary() )
								move.moveType |= MoveType.ePromote_Move;
						
							move.srcPos = srcPos;
							move.trgPos = movePos;
						
							move.movePiece = aBoard[nMovePileIdx, nMoveRankIdx];
							
							listRetBoardPos.Add( move );
						}
					}					
				
					// en-passant move
					// left en passant move check
					movePos.SetPosition( srcPos );
					bValidMove = movePos.MovePosition( -1, 0 );					
					if( bValidMove ) {					
					
						movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
						if(	aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) &&
							aBoard[nMovePileIdx, nMoveRankIdx].bEnPassantCapture ) {														
							
							bValidMove = movePos.MovePosition( 0, 1 );	
							if( bValidMove ) {				
							
								movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
								if(	aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {	
									// capture move
									move.moveType = MoveType.eEnPassan_Move | MoveType.ePawn_Move;								
								
									move.srcPos = srcPos;
									move.trgPos = movePos;
								
									move.movePiece = aBoard[nMovePileIdx, nMoveRankIdx];
									
									listRetBoardPos.Add( move );
								}
							}
						}
					}
				
					// right en passant move check
					movePos.SetPosition( srcPos );
					bValidMove = movePos.MovePosition( 1, 0 );					
					if( bValidMove ) {					
					
						movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
						if(	aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) &&
							aBoard[nMovePileIdx, nMoveRankIdx].bEnPassantCapture ) {														
							
							bValidMove = movePos.MovePosition( 0, 1 );	
							if( bValidMove ) {				
							
								movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
								if(	aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {	
									// capture move
									move.moveType = MoveType.eEnPassan_Move | MoveType.ePawn_Move;								
								
									move.srcPos = srcPos;
									move.trgPos = movePos;
								
									move.movePiece = aBoard[nMovePileIdx, nMoveRankIdx];
									
									listRetBoardPos.Add( move );
								}
							}
						}
					}				
				}				
				break;				
			}			
		}
		else if( srcPlayerSide == PlayerSide.e_Black ) {
			
		}
		
		return true;
	}
	
	
	static ChessMoveManager() {
	}
}

//}
