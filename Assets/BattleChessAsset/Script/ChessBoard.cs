using UnityEngine;
using System.Collections;

using System.Collections.Generic;

// for moblie : do not support name space, android
//namespace BattleChess {	
	
	
public class ChessBoard {
	
	// board
	// 8 x 8, pile x rank
	public ChessBoardSquare[,] aBoardSqure;	
	
	public List<ChessPiece> listPiece;
	
	// board material
	Material matBoard1;	
	Material matBoard2;	
	
	// selected square
	ChessBoardSquare selectSquare;		
	ParticleSystem selectPiecePSystem;	
	
	// movable board pos
	List<ChessMoveManager.sMove> listCurrMovable;			
	
	// current move, en passant target move
	ChessMoveManager.sMove currMove;
	
	// half move
	int nCurrHalfMove;
	
	// total move
	int nCurrTotalMove;
	
	// user player side
	public PlayerSide UserPlayerSide { get; set; }
	
	// time
	public int ThinkingTime { get; set; }
	
	public bool Ready { get; set; }
	
	// player turn
	public PlayerSide CurrTurn { get; set; }
	
	// castling
	public ChessCastling currCastlingState;
	
	
	
	
	// constructor
	public ChessBoard() {			
		
	}
	
	
	// interface
	public void Init( BattleChessMain chessMain, Transform[] aPieceRef, ParticleSystem selectPSystemRef, ParticleSystem movablePSystemRef ) {
		
		// etc property		
		CurrTurn = PlayerSide.e_White;	
		UserPlayerSide = PlayerSide.e_White;
		ThinkingTime = 18000;
		nCurrHalfMove = 0;
		nCurrTotalMove = 0;
		
		Ready = false;
		
		// move
		currMove = new ChessMoveManager.sMove();
		currCastlingState = new ChessCastling() {
			
			CastlingWKSide = CastlingState.eCastling_Enable_State,
			CastlingWQSide = CastlingState.eCastling_Enable_State,
			CastlingBKSide = CastlingState.eCastling_Enable_State,
			CastlingBQSide = CastlingState.eCastling_Enable_State
		};	
		
		listCurrMovable = new List<ChessMoveManager.sMove>();
		
		// init board
		// piece list
		listPiece = new List<ChessPiece>();		
		aBoardSqure = new ChessBoardSquare[ChessData.nNumPile,ChessData.nNumRank];
		
		ChessPiece currPiece = null;
		for( int i=0; i<ChessData.nNumPile; i++ ){
			for( int j=0; j<ChessData.nNumRank; j++ ){				
				
				// movable square effect Particle System
				ParticleSystem movablePiecePSystem = MonoBehaviour.Instantiate( movablePSystemRef, Vector3.zero, Quaternion.identity ) as ParticleSystem;
				
				if( ChessData.aStartPiecePos[i,j] == PiecePlayerType.eNone_Piece ) {
					
					currPiece = new ChessPiece( null, PlayerSide.e_NoneSide, PiecePlayerType.eNone_Piece );
					listPiece.Add( currPiece );
					
					aBoardSqure[nCurrPile,nCurrRank] = new ChessBoardSquare( currPiece, movablePiecePSystem, i, j );							
				}
				else
				{
					Vector3 currPos = new Vector3( j - 3.5f, 0.0f, i - 3.5f );					
					
					Transform currTransform = aPieceRef[(int)ChessData.aStartPiecePos[i,j]];
					Transform currPieceObject = MonoBehaviour.Instantiate( currTransform, currPos, currTransform.rotation ) as Transform;
					
					
					if( i == 0 || i == 1 ) {																						
						
						currPiece = new ChessPiece( currPieceObject.gameObject, PlayerSide.e_White,	ChessData.aStartPiecePos[i,j] );
						listPiece.Add( currPiece );
						
						aBoardSqure[i,j] = new ChessBoardSquare( currPiece, movablePiecePSystem, i, j );
					}
					else if( i == 6 || i == 7 ) {						
						
						currPiece = new ChessPiece( currPieceObject.gameObject, PlayerSide.e_Black,	ChessData.aStartPiecePos[i,j] );
						listPiece.Add( currPiece );
						
						aBoardSqure[i,j] = new ChessBoardSquare( currPiece, movablePiecePSystem, i, j );
					}									
				}				
			}		
		}
		
		// piece coloar
		SetWhiteSidePieceColor( Color.white );
		SetBlackSidePieceColor( Color.white );
		
		
		// board material
		if( chessMain.renderer.materials.Length == 2 ) {
			
			matBoard1 = chessMain.renderer.materials[0];
			matBoard2 = chessMain.renderer.materials[1];			
			
			Color rgbaWhiteBoard, rgbaBlackBoard;
			rgbaWhiteBoard = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			rgbaBlackBoard = new Color( 0.039f, 0.34f, 0.22f, 1.0f );
			
			SetWhiteSideBoardColor( rgbaWhiteBoard );
			SetBlackSideBoardColor( rgbaBlackBoard );
		}	
		
		// particle effect
		selectSquare = null;
		selectPiecePSystem = MonoBehaviour.Instantiate( selectPSystemRef, Vector3.zero, Quaternion.identity ) as ParticleSystem;				
		selectPiecePSystem.Stop();
	}
	
	public void SelectSquare( ChessBoardSquare selSquare ) {
		
		if( selSquare != null && selSquare.IsBlank() == false ) {									
						
			if( selSquare.piece.playerSide == UserPlayerSide ) {
				
				PlaySelectEffect( selSquare.piece.gameObject.transform.position, selSquare.piece.gameObject.transform.rotation );				
				selectSquare = selSquare;		
				return;
			}
						
		}		
			
		// movable pos	
		StopSelectEffect();
		selectSquare = null;			
	}
	
	public ChessPiece GetPiece( Vector3 vPos ) {	
		
		// movable pos	
		int nRank = 0, nPile = 0;
		bool bValidPos = ChessPosition.GetRankPilePos( vPos, ref nRank, ref nPile );
		if( bValidPos ) {				
					
			if( aBoardSquare[nPile,nRank].IsBlank() == false ) {
			
				return aBoardSquare[nPile,nRank].piece;				
			}
		}
		
		return null;
	}
	
	public ChessBoardSquare GetSquare( Vector3 vPos, bool bExistPiece ) {	
		
		// movable pos	
		int nRank = 0, nPile = 0;
		bool bValidPos = ChessPosition.GetRankPilePos( vPos, ref nRank, ref nPile );
		if( bValidPos ) {				
			
			if( bExistPiece ) {
				
				if( aBoardSquare[nPile,nRank].IsBlank() == false ) {
					
					return aBoardSquare[nPile,nRank];
				}
			}
			else
				return aBoardSquare[nPile,nRank];					
		}
		
		return null;
	}	
	
	public void UpdateMoveCount() {
		
		// increase half move and total move
		if( CurrTurn == PlayerSide.e_Black )
			nCurrTotalMove++;					
		
		if( currMove.IsResetHalfMove() )						
			nCurrHalfMove = 0;	
		else						
			nCurrHalfMove++;		
	}
	
	public void UpdateCastlingState( ChessPiece piece ) {
		
		// disable castling state
		switch( piece.piecePlayerType ) 						
		{
			case PiecePlayerType.eWhite_King:
			{
				currCastlingState.CastlingWKSide = CastlingState.eCastling_Disable_State;
				currCastlingState.CastlingWQSide = CastlingState.eCastling_Disable_State;
			}
			break;
			
			case PiecePlayerType.eWhite_LookLeft:
			{
				currCastlingState.CastlingWQSide = CastlingState.eCastling_Disable_State;
			}
			break;
				
			case PiecePlayerType.eWhite_LookRight:
			{
				currCastlingState.CastlingWKSide = CastlingState.eCastling_Disable_State;
			}
			break;
				
			case PiecePlayerType.eBlack_King:
			{
				currCastlingState.CastlingBKSide = CastlingState.eCastling_Disable_State;
				currCastlingState.CastlingBQSide = CastlingState.eCastling_Disable_State;
			}
			break;
				
			case PiecePlayerType.eBlack_LookLeft:
			{
				currCastlingState.CastlingBQSide = CastlingState.eCastling_Disable_State;
			}
			break;
				
			case PiecePlayerType.eBlack_LookRight:
			{
				currCastlingState.CastlingBKSide = CastlingState.eCastling_Disable_State;
			}
			break;									
		}		
	}
	
	public void UpdateMove( ChessPiece srcPiece, ChessPiece trgPiece, ref ChessMoveManager.sMove move ) {
		
		UpdateMoveCount();
		UpdateCastlingState( srcPiece );					
		
		// normal move
		if( ChessMoveManager.IsNormalMove( move.moveType ) ) {			
			
			// pawn move
			if( ChessMoveManager.IsPawnMove( move.moveType ) ) {
				
				// two square move
				if( ChessMoveManager.IsPawnTwoMove( move.moveType ) ) {					
					
					srcPiece.bEnPassantCapture = true;					
				}				
				
				// promote move
				if( ChessMoveManager.IsPromoteMove( move.moveType ) ) {
					
				}				
			}
		}
		
		// capture move
		if( ChessMoveManager.IsCaptureMove( move.moveType ) ) {
			
			// pawn move
			if( ChessMoveManager.IsPawnMove( move.moveType ) ) {
				
				// promote move
				if( ChessMoveManager.IsPromoteMove( move.moveType ) ) {
					
				}				
			}
			
			trgPiece.ClearPiece( true );
		}	
		
		
		// enpassantmove
		if( ChessMoveManager.IsEnpassantMove( move.moveType ) ) {
			
			// pawn move
			if( ChessMoveManager.IsPawnMove( move.moveType ) ) {				
							
			}			
		}
		
		// castling move
		if( ChessMoveManager.IsCastlingMove( move.moveType ) ) {
			
			int nLookRank = 0, nLookPile = 0;
			move.trgPiece.position.GetPositionIndex( ref nLookRank, ref nLookPile );
			// white king side castling
			if( ChessMoveManager.IsWhiteKingSideCastlingMove( move.moveType ) ) {
				
				ChessPiece lookPiece = aBoard[0,7];				
				nLookRank = nLookRank - 1;
				aBoard[nLookPile, nLookRank].SetPiece( lookPiece );			
				lookPiece.ClearPiece();
			}				
			
			// white Queen side castling
			if( ChessMoveManager.IsWhiteQueenSideCastlingMove( move.moveType ) ) {
				
				ChessPiece lookPiece = aBoard[0,0];				
				nLookRank = nLookRank + 1;
				aBoard[nLookPile, nLookRank].SetPiece( lookPiece );	
				lookPiece.ClearPiece();				
			}	
			
			// black king side castling
			if( ChessMoveManager.IsBlackKingSideCastlingMove( move.moveType ) ) {
				
				ChessPiece lookPiece = aBoard[7, 7];				
				nLookRank = nLookRank - 1;
				aBoard[nLookPile, nLookRank].SetPiece( lookPiece );	
				lookPiece.ClearPiece();
			}	
			
			// black queen side castling
			if( ChessMoveManager.IsBlackQueenSideCastlingMove( move.moveType ) ) {
				
				ChessPiece lookPiece = aBoard[7, 0];				
				nLookRank = nLookRank + 1;
				aBoard[nLookPile, nLookRank].SetPiece( lookPiece );	
				lookPiece.ClearPiece();
			}	
		}		
		
		
		// move real board piece
		trgPiece.SetPiece( srcPiece );			
		srcPiece.ClearPiece();		
	}
	
	public bool MoveTo( Vector3 vPos ) {		
		
		if( CurrTurn == UserPlayerSide ) {
			int nTrgRank = 0, nTrgPile = 0;
			bool bValidPos = ChessPosition.GetRankPilePos( vPos, ref nTrgRank, ref nTrgPile );				
			if( bValidPos ) {			
				
				int nSelRank = 0, nSelPile = 0;
				selectPiece.position.GetPositionIndex( ref nSelRank, ref nSelPile );			
				
				if( IsValidMove( aBoard[nTrgPile, nTrgRank].position, ref currMove ) ) {									
					
					UpdateMove( aBoard[nSelPile, nSelRank], aBoard[nTrgPile, nTrgRank], ref currMove );
					
					return true;
				}					
			}	
		}
		
		return false;
	}
	
	// AI Move
	public bool AIMoveTo( ChessPosition srcPos, ChessPosition trgPos ) {		
		
		if( CurrTurn != UserPlayerSide ) {
			
			//UnityEngine.Debug.LogError( "AIMoveTo() - current turn = " + CurrTurn );
			
			int nSrcRank = 0, nSrcPile = 0;
			int nTrgRank = 0, nTrgPile = 0;
			
			BoardPositionType srcPosType = srcPos.GetPositionIndex( ref nSrcRank, ref nSrcPile );
			BoardPositionType trgPosType = trgPos.GetPositionIndex( ref nTrgRank, ref nTrgPile );				
						
			if( srcPosType > BoardPositionType.eNone && trgPosType > BoardPositionType.eNone &&
				aBoard[nSrcPile, nSrcRank].IsBlank() == false ) {	
				
				//UnityEngine.Debug.LogError( "AIMoveTo() - no blank" );
				
				List<ChessMoveManager.sMove> listAiMovable = new List<ChessMoveManager.sMove>();
				bool bMoveList = ChessMoveManager.GetValidateMoveList( this, aBoard[nSrcPile, nSrcRank], listAiMovable );
				if( bMoveList ) {
					//UnityEngine.Debug.LogError( "AIMoveTo() - no blank" + " " + bMoveList );
					ChessMoveManager.sMove aIMove = new ChessMoveManager.sMove();
					if( IsValidAIMove( trgPos, listAiMovable, ref aIMove ) ) {
						
						//UnityEngine.Debug.LogError( "AIMoveTo() - IsValidAIMove()" );					
						
						UpdateMove( aBoard[nSrcPile, nSrcRank], aBoard[nTrgPile, nTrgRank], ref aIMove );
							
						return true;					
					}
				}
			}	
		}
		
		UnityEngine.Debug.LogError( "AIMoveTo() - !!!!" );
		
		return false;
	}
	
	public void UpdateCurrMoveable() {
		
		StopMovableEffect();
		
		listCurrMovable.Clear();
		ChessMoveManager.GetValidateMoveList( this, selectSquare, listCurrMovable );
		
		// movable effect start
		PlayMovableEffect();			
	}
	
	public string GetCurrMoveCommand() {
		
		// position fen string
		string strPosFen = "position fen ", strResFen;
		int nNumBlank = 0;
		
		for( int i=ChessData.nNumPile-1; i>=0; i-- ){			
			for( int j=0; j<ChessData.nNumRank; j++ ){
				
				if( aBoard[i,j].IsBlank() ) {					
					
					nNumBlank++;
				}	
				else {
					
					if( nNumBlank > 0 ) {
						
						strPosFen += nNumBlank;
					}
						
					strPosFen += ChessData.GetPieceFenString( aBoard[i,j].piecePlayerType );
					nNumBlank = 0;
				}
			}
			
			if( nNumBlank > 0 )			
				strPosFen += nNumBlank;			
			
			if( i == 0 )
				strPosFen += " ";
			else
				strPosFen += "/";
			
			nNumBlank = 0;
		}
		
		// player turn
		string strTurn;
		if( CurrTurn == PlayerSide.e_White )
			strTurn = "w";
		else if( CurrTurn == PlayerSide.e_Black )
			strTurn = "b";
		else {
			
			strTurn = "w";
			UnityEngine.Debug.LogError( "Fen String Error - Player Turn" );
		}
		
		strResFen = strPosFen + strTurn;
		
		// cstling
		string strCastling = currCastlingState.GetFenString();		
		
		// en passant target square
		string strEnPassant = currMove.EnPassantTargetSquare.GetFenString();
		
		strResFen = strPosFen + strCastling + strEnPassant;
		
		// curr half move count for 50 move rule
		strResFen += " " + nCurrHalfMove;
		
		// total move - if black piece move completed, increse move
		strResFen += " " + nCurrTotalMove;
		
		return strResFen;
	}
	
	public string GetCurrGoCommand() {
		
		string strSide = "wtime";
		if( UserPlayerSide == PlayerSide.e_White )
			strSide = "btime";
		
		string strRetGoCmd = "go " + strSide + " " + ThinkingTime;
		return strRetGoCmd;
	}
	
	
	
	
	
	
	// private method
	// SetBoardColor
	void SetWhiteSideBoardColor( Color rgbaWhite ) {
		
		if( matBoard1 != null ) {
			
			matBoard1.SetColor( "_Color", rgbaWhite );					
		}
	}
	
	void SetBlackSideBoardColor( Color rgbaBlack ) {
		
		if( matBoard2 != null ) {			
			
			matBoard2.SetColor( "_Color", rgbaBlack );			
		}
	}
	
	void SetWhiteSidePieceColor( Color rgbaWhite ) {
		
		for( int i=0; i<ChessData.nNumPile; i++ ) {
			for( int j=0; j<ChessData.nNumRank; j++ ) {
				
				if( aBoardSqure[i,j].IsBlank() == false ) {
					
					if( aBoardSqure[i,j].piece.playerSide == PlayerSide.e_White )
						aBoardSqure[i,j].piece.gameObject.renderer.material.SetColor( "_Color", rgbaWhite );											
				}
			}
		}		
	}
	
	void SetBlackSidePieceColor( Color rgbaBlack ) {
		
		for( int i=0; i<ChessData.nNumPile; i++ ) {
			for( int j=0; j<ChessData.nNumRank; j++ ) {
				
				if( aBoardSqure[i,j].IsBlank() == false ) {
					
					if( aBoardSqure[i,j].piece.playerSide == PlayerSide.e_Black )
						aBoardSqure[i,j].piece.gameObject.renderer.material.SetColor( "_Color", rgbaBlack );	
				}
			}
		}	
	}
	
	
	//
	void StopSelectEffect() {
		
		if( selectPiecePSystem != null )
			//UnityEngine.Debug.Log( "de select" );				
			selectPiecePSystem.Stop();	
			selectPiecePSystem.renderer.enabled = false;
	}
	
	void PlaySelectEffect( Vector3 vPos, Quaternion rot ) {
		
		if( selectPiecePSystem != null && selectPiecePSystem.gameObject != null ) {
			
			selectPiecePSystem.renderer.enabled = true;
			
			selectPiecePSystem.gameObject.transform.position = vPos;
			selectPiecePSystem.gameObject.transform.rotation = rot;
			selectPiecePSystem.Play();	
		}
	}	
	
	void StopMovableEffect() {
		
		// previous movable effect stop
		foreach( ChessBoardSquare square in aBoardSquare ) {					
			
			square.ShowMovableEffect(false);
		}		
	}
	
	void PlayMovableEffect() {
		
		foreach( ChessMoveManager.sMove move in listCurrMovable ) {					
			
			move.trgSquare.ShowMovableEffect(true);
		}	
	}
	
	bool IsValidMove( ChessPosition chessPosition, ref ChessMoveManager.sMove targetMove ) {
		
		foreach( ChessMoveManager.sMove move in listCurrMovable ) {
			
			if( move.trgPos == chessPosition ) {
				
				targetMove.Set( move );
				return true;
			}
		}
		
		targetMove.Clear();
		return false;
	}
		
	bool IsValidAIMove( ChessPosition chessPosition, List<ChessMoveManager.sMove> listMove, ref ChessMoveManager.sMove targetMove ) {
		
		foreach( ChessMoveManager.sMove move in listMove ) {			
			
			if( move.trgPos == chessPosition ) {
				
				targetMove.Set( move );
				return true;
			}
		}
		
		targetMove.Clear();
		return false;
	}
}
	
//}
