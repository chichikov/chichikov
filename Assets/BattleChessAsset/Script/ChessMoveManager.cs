using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;


//namespace BattleChess {	

public class ChessMoveManager {	
	
	
	public enum MoveDirectionType : int {
	
		eDirection_Move_Up = 0,
		eDirection_Move_Down,
		eDirection_Move_Left,
		eDirection_Move_Right,
		eDirection_Move_LeftUp_Diagonal,
		eDirection_Move_LeftDown_Diagonal,
		eDirection_Move_RightUp_Diagonal,
		eDirection_Move_RightDown_Diagonal,
		eDirection_Move_LeftUp_Leap,
		eDirection_Move_LeftDown_Leap,
		eDirection_Move_RightUp_Leap,
		eDirection_Move_RightDown_Leap,	
	}
	
	[Flags]
	public enum MoveType {
		
		eNone_Move = 0x0,
		eNormal_Move = 0x1,
		eCapture_Move = 0x2,
		eEnPassan_Move = 0x04,
		ePromote_Move = 0x08,		
		ePawn_Move = 0x10,
		eCastling_Move = 0x20,
		eCastling_White_KingSide_Move = 0x40,
		eCastling_White_QueenSide_Move = 0x80,
		eCastling_Black_KingSide_Move = 0x100,
		eCastling_Black_QueenSide_Move = 0x200,
	}	
	
	public static bool IsNormalMove( MoveType moveType ) {
		
		if( (moveType & MoveType.eNormal_Move) > 0 )
			return true;
	
		return false;		
	}
	
	public static bool IsCaptureMove( MoveType moveType ) {
		
		if( (moveType & MoveType.eCapture_Move) > 0 )
			return true;
	
		return false;		
	}
	
	public static bool IsEnpassantMove( MoveType moveType ) {
		
		if( (moveType & MoveType.eEnPassan_Move) > 0 )
			return true;
	
		return false;		
	}
	
	public static bool IsPawnMove( MoveType moveType ) {
		
		if( (moveType & MoveType.ePawn_Move) > 0 )
			return true;
	
		return false;		
	}
	
	public static bool IsPromoteMove( MoveType moveType ) {
		
		if( (moveType & MoveType.ePromote_Move) > 0 )
			return true;
	
		return false;		
	}
	
	public static bool IsCastlingMove( MoveType moveType ) {
		
		if( (moveType & MoveType.eCastling_Move) > 0 )
			return true;
	
		return false;		
	}
	
	public static bool IsWhiteKingSideCastlingMove( MoveType moveType ) {
		
		if( (moveType & MoveType.eCastling_White_KingSide_Move) > 0 )
			return true;
	
		return false;		
	}
	
	public static bool IsWhiteQueenSideCastlingMove( MoveType moveType ) {
		
		if( (moveType & MoveType.eCastling_White_QueenSide_Move) > 0 )
			return true;
	
		return false;		
	}	
	
	public static bool IsBlackKingSideCastlingMove( MoveType moveType ) {
		
		if( (moveType & MoveType.eCastling_Black_KingSide_Move) > 0 )
			return true;
	
		return false;		
	}	
	
	public static bool IsBlackQueenSideCastlingMove( MoveType moveType ) {
		
		if( (moveType & MoveType.eCastling_Black_QueenSide_Move) > 0 )
			return true;
	
		return false;		
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
			
			this.movePiece.CopyFrom( movePiece );
			
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
			
			this.movePiece.CopyFrom( movePiece );
			
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
			
			this.movePiece.CopyFrom( movePiece );
			
			this.EnPassantTargetSquare = enPassantTrgSquare;
		}
		
		public void Set( sMove move ) {
			
			this.moveType = move.moveType;
			
			this.srcPos = move.srcPos;
			this.trgPos = move.trgPos;
			
			this.movePiece.CopyFrom( move.movePiece );
			
			this.EnPassantTargetSquare = move.EnPassantTargetSquare;
		}
		
		public void Clear() {
			
			this.moveType = MoveType.eNone_Move;
			
			this.srcPos.SetPosition( -1, -1 );
			this.trgPos.SetPosition( -1, -1 );	
			
			this.movePiece.ClearPiece();
			
			this.EnPassantTargetSquare = new ChessEnPassant() {
				
				Rank = -1,
				Pile = -1,
				Available = false				
			};
		}
		
		// is Increse half move
		public bool IsResetHalfMove() {
			
			if( (moveType & MoveType.ePawn_Move) > 0 || 
				(moveType & MoveType.eCapture_Move) > 0 || 
				(moveType & MoveType.eCastling_Move) > 0 )
				return true;
			return false;
		}
	}	
	
	// validate move position
	public static bool GetValidateMoveList( ChessBoard board, ref ChessPiece piece, List<sMove> listRetBoardPos ) {		
		
		bool bRet = false;
		
		if( piece.IsBlank() )
		{
			UnityEngine.Debug.LogError( "GetValidateMoveList - IsBlank()" );
			return bRet;	
		}
		
		if( piece.position.IsInvalidPos() )
		{
			UnityEngine.Debug.LogError( "GetValidateMoveList - IsInvalidPos()" );
			return bRet;	
		}
			
		switch( piece.pieceType ) {
			
			case PieceType.e_King:
			{
				bRet = GetKingMoveList( board, ref piece, listRetBoardPos );							
			}
			break;
			
			case PieceType.e_Queen:
			{
				bRet = GetQueenMoveList( board, ref piece, listRetBoardPos );			
			}	
			break;
			
			case PieceType.e_Look:
			{
				bRet = GetLookMoveList( board, ref piece, listRetBoardPos );			
			}
			break;
			
			case PieceType.e_Bishop:
			{
				bRet = GetBishopMoveList( board, ref piece, listRetBoardPos );
			}
			break;
			
			case PieceType.e_Knight:
			{
				bRet = GetKnightMoveList( board, ref piece, listRetBoardPos );		
			}
			break;
			
			case PieceType.e_Pawn:
			{
				bRet = GetPawnMoveList( board, ref piece, listRetBoardPos );						
			}				
			break;
			
			default:	
			{
			}
			break;
		}			
		
		
		return bRet;
	}	
	
	
	public static bool GetPawnMoveList( ChessBoard board, ref ChessPiece piece, List<sMove> listRetBoardPos ) {				
		
		//UnityEngine.Debug.LogError( "GetPawnMoveList - start" + " " + piece.position + " " + piece.playerSide );
		
		ChessPosition srcPos = piece.position;		
		PlayerSide srcPlayerSide = piece.playerSide;			
		
		int nMoveRankIdx = 0, nMovePileIdx = 0;
		
		ChessPosition movePos = new ChessPosition(srcPos.pos);			
		sMove move = new sMove();		
		
		// pure move
		// pure move - one pile move
		int nTempRank, nTempPile;
		nTempRank = 0;		
		nTempPile = srcPlayerSide == PlayerSide.e_White ? 1 : -1;
		
		bool bValidMove = movePos.MovePosition( nTempRank, nTempPile );
		if( bValidMove ) {					
			
			movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
			// check already existing piece				
			if( board.aBoard[nMovePileIdx, nMoveRankIdx].IsBlank() ) {																															
				
				// normal move
				move.moveType = MoveType.eNormal_Move | MoveType.ePawn_Move;
				// promote move	
				if( srcPlayerSide == PlayerSide.e_White ) {
					if( movePos.IsTopBoundary() )
						move.moveType |= MoveType.ePromote_Move;								
				}
				else {
					
					if( movePos.IsBottomBoundary() )
						move.moveType |= MoveType.ePromote_Move;
				}
			
				move.srcPos = srcPos;
				move.trgPos = movePos;
			
				move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
				
				listRetBoardPos.Add( move );
			}	
		}				
		
		// pure move - two pile move		
		if( ((int)srcPos.pos / ChessData.nNumRank) == 1 && srcPlayerSide == PlayerSide.e_White || 
			((int)srcPos.pos / ChessData.nNumRank) == 6 && srcPlayerSide == PlayerSide.e_Black ) {
			
			movePos.SetPosition( srcPos );
			
			nTempRank = 0;		
			nTempPile = srcPlayerSide == PlayerSide.e_White ? 2 : -2;
			
			bValidMove = movePos.MovePosition( nTempRank, nTempPile );
			if( bValidMove ) {							
					
				movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
				if(	board.aBoard[nMovePileIdx, nMoveRankIdx].IsBlank() ) {														
					
					move.moveType = MoveType.eNormal_Move | MoveType.ePawn_Move;
					move.srcPos = srcPos;
					move.trgPos = movePos;
				
					move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
				
					// en passant target move check					
					move.EnPassantTargetSquare = new ChessEnPassant() {
						Rank = nMoveRankIdx,
						Pile = nMovePileIdx,
						Available = true
					};	
					
					board.aBoard[nMovePileIdx, nMoveRankIdx].bEnPassantCapture = true;					
					
					listRetBoardPos.Add( move );													
				}				
			}	
		}
		
	
		// capture move
		// left diagonal capture	
		// check left boundary
		movePos.SetPosition( srcPos );
		
		nTempRank = -1;		
		nTempPile = srcPlayerSide == PlayerSide.e_White ? 1 : -1;
		
		bValidMove = movePos.MovePosition( nTempRank, nTempPile );					
		if( bValidMove ) {					
		
			movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
			if(	board.aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {														
				
				// capture move
				move.moveType = MoveType.eCapture_Move | MoveType.ePawn_Move;
			
				// promote move	
				if( srcPlayerSide == PlayerSide.e_White ) {
					
					if( movePos.IsTopBoundary() )
						move.moveType |= MoveType.ePromote_Move;
				}
				else {
					
					if( movePos.IsBottomBoundary() )
						move.moveType |= MoveType.ePromote_Move;
				}
			
				move.srcPos = srcPos;
				move.trgPos = movePos;
			
				move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
				
				listRetBoardPos.Add( move );
			}
		}	
	
		// right diagonal capture	
		// check right boundary
		movePos.SetPosition( srcPos );
		
		nTempRank = 1;		
		nTempPile = srcPlayerSide == PlayerSide.e_White ? 1 : -1;
		
		bValidMove = movePos.MovePosition( nTempRank, nTempPile );					
		if( bValidMove ) {					
		
			movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
			if(	board.aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {														
				
				// capture move
				move.moveType = MoveType.eCapture_Move | MoveType.ePawn_Move;
			
				// promote move	
				if( srcPlayerSide == PlayerSide.e_White ) {
					
					if( movePos.IsTopBoundary() )
						move.moveType |= MoveType.ePromote_Move;
				}
				else {
					
					if( movePos.IsBottomBoundary() )
						move.moveType |= MoveType.ePromote_Move;
				}
			
				move.srcPos = srcPos;
				move.trgPos = movePos;
			
				move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
				
				listRetBoardPos.Add( move );
			}
		}					
	
		// en-passant move
		// left en passant move check
		movePos.SetPosition( srcPos );
		
		nTempRank = -1;		
		nTempPile = 0;
		
		bValidMove = movePos.MovePosition( nTempRank, nTempPile );					
		if( bValidMove ) {					
		
			movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
			if(	board.aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) &&
				board.aBoard[nMovePileIdx, nMoveRankIdx].bEnPassantCapture ) {														
				
				bValidMove = movePos.MovePosition( 0, 1 );	
				if( bValidMove ) {				
				
					movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
					if(	board.aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {	
						// capture move
						move.moveType = MoveType.eEnPassan_Move | MoveType.ePawn_Move;								
					
						move.srcPos = srcPos;
						move.trgPos = movePos;
					
						move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
						
						listRetBoardPos.Add( move );
					}
				}
			}
		}
	
		// right en passant move check
		movePos.SetPosition( srcPos );
		
		nTempRank = 1;		
		nTempPile = 0;
		
		bValidMove = movePos.MovePosition( nTempRank, nTempPile );					
		if( bValidMove ) {					
		
			movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
			if(	board.aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) &&
				board.aBoard[nMovePileIdx, nMoveRankIdx].bEnPassantCapture ) {														
				
				bValidMove = movePos.MovePosition( 0, 1 );	
				if( bValidMove ) {				
				
					movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
					if(	board.aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {	
						// capture move
						move.moveType = MoveType.eEnPassan_Move | MoveType.ePawn_Move;								
					
						move.srcPos = srcPos;
						move.trgPos = movePos;
					
						move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
						
						listRetBoardPos.Add( move );
					}
				}
			}
		}				
		
		return true;
	}	
	
	
	public static bool GetKingMoveList( ChessBoard board, ref ChessPiece piece, List<sMove> listRetBoardPos ) {	
		
		ChessPosition srcPos = piece.position;		
		PlayerSide srcPlayerSide = piece.playerSide;			
		
		int nMoveRankIdx = 0, nMovePileIdx = 0;
		
		ChessPosition movePos = new ChessPosition(srcPos.pos);	
		sMove move = new sMove();
		
		// all(radial) direction one move		
		int nTempRank, nTempPile;
		
		for( int nMovePile=-1; nMovePile<=1; nMovePile++ ) {
			for( int nMoveRank=-1; nMoveRank<=1; nMoveRank++ ) {
			
				nTempRank = nMoveRank;		
				nTempPile = nMovePile;				
				
				movePos.SetPosition( srcPos );				
				bool bValidMove = movePos.MovePosition( nTempRank, nTempPile );
				if( bValidMove ) {					
					
					movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
					// normal move				
					if( board.aBoard[nMovePileIdx, nMoveRankIdx].IsBlank() ) {																																											
						
						// normal move
						move.moveType = MoveType.eNormal_Move;						
					
						move.srcPos = srcPos;
						move.trgPos = movePos;
					
						move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
						
						listRetBoardPos.Add( move );
					}
					// capture move
					else if( board.aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {
						
						// normal move
						move.moveType = MoveType.eCapture_Move;						
					
						move.srcPos = srcPos;
						move.trgPos = movePos;
					
						move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
						
						listRetBoardPos.Add( move );
					}
				}
			}
		}
		
		// castling move
		// king side castling			
		movePos.SetPosition( srcPos );
		
		nTempRank = 2;		
		nTempPile = 0;
		
		bool bValidCastlingMove = movePos.MovePosition( nTempRank, nTempPile );
		if( bValidCastlingMove ) {
		
			movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
			if(	board.aBoard[nMovePileIdx, nMoveRankIdx].IsBlank() ) {
				
				// position check castling
				bool bCalstling = false;
				if( srcPlayerSide == PlayerSide.e_White ) {
					
					if( board.currCastlingState.CastlingWKSide == CastlingState.eCastling_Enable_State ) {											
						bCalstling = true;
					}
				}
				else {
					if( board.currCastlingState.CastlingBKSide == CastlingState.eCastling_Enable_State ) {											
						bCalstling = true;
					}
				}
				
				if( bCalstling ) {
					
					// check look square blank
					nTempRank = -1;		
					nTempPile = 0;				
						
					ChessPosition moveLookPos = new ChessPosition(movePos.pos);						
					
					bValidCastlingMove = moveLookPos.MovePosition( nTempRank, nTempPile );
					if( bValidCastlingMove ) {
						
						int nLookMoveRank = 0, nLookMovePile = 0;
						moveLookPos.GetPositionIndex( ref nLookMoveRank, ref nLookMovePile );
						if(	board.aBoard[nLookMovePile, nLookMoveRank].IsBlank() ) {
							
							MoveType castlingSideType = srcPlayerSide == 
								PlayerSide.e_White ? MoveType.eCastling_White_KingSide_Move : MoveType.eCastling_Black_KingSide_Move;
							move.moveType = MoveType.eCastling_Move | castlingSideType;
							move.srcPos = srcPos;
							move.trgPos = movePos;
						
							move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );						
							
							listRetBoardPos.Add( move );
						}
					}					
				}																
			}
		}
		
		// queen side castling		
		movePos.SetPosition( srcPos );
		
		nTempRank = -2;		
		nTempPile = 0;
		
		bValidCastlingMove = movePos.MovePosition( nTempRank, nTempPile );
		if( bValidCastlingMove ) {
		
			movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
			if(	board.aBoard[nMovePileIdx, nMoveRankIdx].IsBlank() ) {
				
				// position check castling
				bool bCalstling = false;
				if( srcPlayerSide == PlayerSide.e_White ) {
					
					if( board.currCastlingState.CastlingWQSide == CastlingState.eCastling_Enable_State ) {											
						bCalstling = true;
					}
				}
				else {
					if( board.currCastlingState.CastlingBQSide == CastlingState.eCastling_Enable_State ) {											
						bCalstling = true;
					}
				}
				
				if( bCalstling ) {
					
					// check look square blank
					nTempRank = 1;		
					nTempPile = 0;				
						
					ChessPosition moveLookPos = new ChessPosition(movePos.pos);						
					bValidCastlingMove = moveLookPos.MovePosition( nTempRank, nTempPile );
					if( bValidCastlingMove ) {
						
						int nLookMoveRank = 0, nLookMovePile = 0;
						moveLookPos.GetPositionIndex( ref nLookMoveRank, ref nLookMovePile );
						if(	board.aBoard[nLookMovePile, nLookMoveRank].IsBlank() ) {
							
							MoveType castlingSideType = srcPlayerSide == PlayerSide.e_White ? 
								MoveType.eCastling_White_QueenSide_Move : MoveType.eCastling_Black_QueenSide_Move;
							move.moveType = MoveType.eCastling_Move | castlingSideType;
							move.srcPos = srcPos;
							move.trgPos = movePos;
						
							move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );						
							
							listRetBoardPos.Add( move );
						}
					}					
				}																
			}
		}		
		
		return true;
	}
	
	public static bool GetQueenMoveList( ChessBoard board, ref ChessPiece piece, List<sMove> listRetBoardPos ) {		
		
		// up
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_Up );
		// down
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_Down );
		// left
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_Left );
		// right
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_Right );
		// left-up - diagonal
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_LeftUp_Diagonal );
		// left-down - diagonal
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_LeftDown_Diagonal );
		// right-up - diagonal
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_RightUp_Diagonal );
		// right-down - diagonal
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_RightDown_Diagonal );
		
		if( listRetBoardPos.Count > 0 )
			return true;
		
		return false;
	}
	
	public static bool GetLookMoveList( ChessBoard board, ref ChessPiece piece, List<sMove> listRetBoardPos ) {		
		
		// up
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_Up );
		// down
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_Down );
		// left
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_Left );
		// right
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_Right );
		
		if( listRetBoardPos.Count > 0 )
			return true;
		
		return false;
	}
	
	public static bool GetBishopMoveList( ChessBoard board, ref ChessPiece piece, List<sMove> listRetBoardPos ) {		
		
		// left-up - diagonal
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_LeftUp_Diagonal );
		// left-down - diagonal
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_LeftDown_Diagonal );
		// right-up - diagonal
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_RightUp_Diagonal );
		// right-down - diagonal
		GetStraightMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_RightDown_Diagonal );
		
		if( listRetBoardPos.Count > 0 )
			return true;
		
		return false;
	}
	
	public static bool GetKnightMoveList( ChessBoard board, ref ChessPiece piece, List<sMove> listRetBoardPos ) {	
		
		// left-up - diagonal
		GetLeapMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_LeftUp_Leap );
		// left-down - diagonal
		GetLeapMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_LeftDown_Leap );
		// right-up - diagonal
		GetLeapMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_RightUp_Leap );
		// right-down - diagonal
		GetLeapMoveList( board, ref piece, listRetBoardPos, MoveDirectionType.eDirection_Move_RightDown_Leap );
		
		if( listRetBoardPos.Count > 0 )
			return true;
		
		return false;	
	}
	
	// sub move method
	// helper method
	public static int GetNumDirectionIterCount( int nCurrRank, int nCurrPile, MoveDirectionType moveDirection ) {
		
		int nNumRamnatSqure = 0, nNumRamnatRank, nNumRamnatPile;		
		switch( moveDirection ) {
			
			case MoveDirectionType.eDirection_Move_Left:
			{
				nNumRamnatSqure = nCurrRank;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_Right:
			{
				nNumRamnatSqure = ChessData.nNumRank - (nCurrRank + 1);
			}
			break;
				
			case MoveDirectionType.eDirection_Move_Up:
			{
				nNumRamnatSqure = ChessData.nNumPile - (nCurrPile + 1);
			}
			break;
				
			case MoveDirectionType.eDirection_Move_Down:
			{				
				nNumRamnatSqure = nCurrPile;
			}
			break;	
			
			case MoveDirectionType.eDirection_Move_LeftUp_Diagonal:
			{
				nNumRamnatRank = nCurrRank;
				nNumRamnatPile = ChessData.nNumPile - (nCurrPile + 1);
				nNumRamnatSqure = Math.Min( nNumRamnatRank, nNumRamnatPile );
			}
			break;
				
			case MoveDirectionType.eDirection_Move_LeftDown_Diagonal:
			{
				nNumRamnatRank = nCurrRank;
				nNumRamnatPile = nCurrPile;
				nNumRamnatSqure = Math.Min( nNumRamnatRank, nNumRamnatPile );
			}
			break;
				
			case MoveDirectionType.eDirection_Move_RightUp_Diagonal:
			{
				nNumRamnatRank = ChessData.nNumRank - (nCurrRank + 1);
				nNumRamnatPile = ChessData.nNumPile - (nCurrPile + 1);
				nNumRamnatSqure = Math.Min( nNumRamnatRank, nNumRamnatPile );
			}
			break;
				
			case MoveDirectionType.eDirection_Move_RightDown_Diagonal:
			{
				nNumRamnatRank = ChessData.nNumRank - (nCurrRank + 1);
				nNumRamnatPile = nCurrPile;
				nNumRamnatSqure = Math.Min( nNumRamnatRank, nNumRamnatPile );
			}
			break;		
		}
		
		return nNumRamnatSqure;
	}
	
	public static void GetNextDirectionRankPile( ref int nNextRank, ref int nNextPile, MoveDirectionType moveDirection, int nCurrIter ) {
			
		switch( moveDirection ) {
			
			case MoveDirectionType.eDirection_Move_Left:
			{
				nNextRank = -nCurrIter;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_Right:
			{
				nNextRank = nCurrIter;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_Up:
			{
				nNextPile = nCurrIter;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_Down:
			{				
				nNextPile = -nCurrIter;
			}
			break;	
			
			case MoveDirectionType.eDirection_Move_LeftUp_Diagonal:
			{
				nNextRank = -nCurrIter;
				nNextPile = nCurrIter;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_LeftDown_Diagonal:
			{
				nNextRank = -nCurrIter;
				nNextPile = -nCurrIter;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_RightUp_Diagonal:
			{
				nNextRank = nCurrIter;
				nNextPile = nCurrIter;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_RightDown_Diagonal:
			{
				nNextRank = nCurrIter;
				nNextPile = -nCurrIter;
			}
			break;
			
			case MoveDirectionType.eDirection_Move_LeftUp_Leap:
			{
				nNextRank = -1;
				nNextPile = 2;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_LeftDown_Leap:
			{
				nNextRank = -1;
				nNextPile = -2;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_RightUp_Leap:
			{
				nNextRank = 1;
				nNextPile = 2;
			}
			break;
				
			case MoveDirectionType.eDirection_Move_RightDown_Leap:
			{
				nNextRank = 1;
				nNextPile = -2;
			}
			break;
		}	
	}
	
	// stright line move
	public static bool GetStraightMoveList( ChessBoard board, ref ChessPiece piece, List<sMove> listRetBoardPos, MoveDirectionType moveDirection ) {	
		
		ChessPosition srcPos = piece.position;		
		PlayerSide srcPlayerSide = piece.playerSide;			
		
		int nMoveRankIdx = 0, nMovePileIdx = 0;
		
		ChessPosition movePos = new ChessPosition(srcPos.pos);	
		sMove move = new sMove();
		
		// all(radial) direction one move		
		int nTempRank, nTempPile;
		
		int nSrcRank = 0, nSrcPile = 0, nIterCount;
		movePos.GetPositionIndex( ref nSrcRank, ref nSrcPile );		
		nIterCount = GetNumDirectionIterCount( nSrcRank, nSrcPile, moveDirection );
		//UnityEngine.Debug.LogError( "GetStraightMoveList() - nIterCount = " + nIterCount );
		
		for( int nCurrIter=1; nCurrIter<=nIterCount; nCurrIter++ ) {
			
			nTempRank = 0;		
			nTempPile = 0;
			
			GetNextDirectionRankPile( ref nTempRank, ref nTempPile, moveDirection, nCurrIter );						
			//UnityEngine.Debug.LogError( "GetStraightMoveList() - nTempRank, nTempPile " + nTempRank + " " + nTempPile );
			
			movePos.SetPosition( srcPos );				
			bool bValidMove = movePos.MovePosition( nTempRank, nTempPile );
			if( bValidMove ) {					
				
				movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
				// normal move				
				if( board.aBoard[nMovePileIdx, nMoveRankIdx].IsBlank() ) {																																											
					
					// normal move
					move.moveType = MoveType.eNormal_Move;						
				
					move.srcPos = srcPos;
					move.trgPos = movePos;
				
					move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
					
					listRetBoardPos.Add( move );					
				}				
				// capture move
				else if( board.aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {
					
					// normal move
					move.moveType = MoveType.eCapture_Move;						
				
					move.srcPos = srcPos;
					move.trgPos = movePos;
				
					move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
					
					listRetBoardPos.Add( move );
					
					return true;
				}				
				// our piece
				else {
					
					if( nCurrIter > 1 )
						return true;
					return false;
				}
			}			
		}
		
		return false;
	}	
	
	
	// leap move
	public static bool GetLeapMoveList( ChessBoard board, ref ChessPiece piece, List<sMove> listRetBoardPos, MoveDirectionType moveDirection ) {	
		
		ChessPosition srcPos = piece.position;		
		PlayerSide srcPlayerSide = piece.playerSide;			
		
		int nMoveRankIdx = 0, nMovePileIdx = 0;
		
		ChessPosition movePos = new ChessPosition(srcPos.pos);	
		sMove move = new sMove();
		
		// all(radial) direction one move		
		int nTempRank = 0, nTempPile = 0;		
		int nSrcRank = 0, nSrcPile = 0;
		
		movePos.GetPositionIndex( ref nSrcRank, ref nSrcPile );		
		
		GetNextDirectionRankPile( ref nTempRank, ref nTempPile, moveDirection, 0 );						
		
		movePos.SetPosition( srcPos );				
		bool bValidMove = movePos.MovePosition( nTempRank, nTempPile );
		if( bValidMove ) {					
			
			movePos.GetPositionIndex( ref nMoveRankIdx, ref nMovePileIdx );
			// normal move				
			if( board.aBoard[nMovePileIdx, nMoveRankIdx].IsBlank() ) {																																											
				
				// normal move
				move.moveType = MoveType.eNormal_Move;						
			
				move.srcPos = srcPos;
				move.trgPos = movePos;
			
				move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
				
				listRetBoardPos.Add( move );
				
				return true;
			}				
			// capture move
			else if( board.aBoard[nMovePileIdx, nMoveRankIdx].IsEnemy( srcPlayerSide ) ) {
				
				// normal move
				move.moveType = MoveType.eCapture_Move;						
			
				move.srcPos = srcPos;
				move.trgPos = movePos;
			
				move.movePiece.CopyFrom( board.aBoard[nMovePileIdx, nMoveRankIdx] );
				
				listRetBoardPos.Add( move );
				
				return true;
			}				
			// our piece
			else {				
				
				return false;
			}
		}			
		
		return false;
	}	
	
	
	
	
	static ChessMoveManager() {
	}
}

//}
