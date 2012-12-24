using UnityEngine;
using System.Collections;

using System.Collections.Generic;

// for moblie : do not support name space, android
//namespace BattleChess {	
	
	
public class ChessBoard {
	
	// board
	// 8 x 8, pile x rank
	ChessPiece[,] aBoard;	
	
	// board material
	Material matBoard1;	
	Material matBoard2;	
	
	
	// selected piece
	ChessPiece selectPiece;			
	
	ParticleSystem selectPiecePSystem;
	//private GameObject selectPiecePSObj;	
	
	// movable board pos
	List<ChessMoveManager.sMove> listCurrMovable;	
	
	// player turn
	PlayerSide currTurn;
	
	// castling
	ChessCastling currCastlingState;
	
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
	
	
	
	
	// constructor
	public ChessBoard() {			
		
	}
	
	
	// interface
	public void Init( BattleChessMain chessMain, Transform[] aPieceRef, ParticleSystem selectPSystemRef, ParticleSystem movablePSystemRef ) {
		
		currTurn = PlayerSide.e_White;	
		UserPlayerSide = PlayerSide.e_White;
		ThinkingTime = 18000;
		nCurrHalfMove = 0;
		nCurrTotalMove = 0;
		
		currMove = new ChessMoveManager.sMove();
		currCastlingState = new ChessCastling() {
			
			CastlingWKSide = CastlingState.eCastling_Enable_State,
			CastlingWQSide = CastlingState.eCastling_Enable_State,
			CastlingBKSide = CastlingState.eCastling_Enable_State,
			CastlingBQSide = CastlingState.eCastling_Enable_State
		};	
		
		listCurrMovable = new List<ChessMoveManager.sMove>();
		
		// init board
		aBoard = new ChessPiece[ChessData.nNumPile,ChessData.nNumRank];
		for( int i=0; i<ChessData.nNumPile; i++ ){
			for( int j=0; j<ChessData.nNumRank; j++ ){
				if( ChessData.aStartPiecePos[i,j] == PiecePlayerType.eNone_Piece ) {
					
					aBoard[i,j].Init( null, PlayerSide.e_NoneSide,
						PiecePlayerType.eNone_Piece, i, j );					
				}
				else
				{
					Vector3 currPos = new Vector3( j - 3.5f, 0.0f, i - 3.5f );					
					
					Transform currTransform = aPieceRef[(int)ChessData.aStartPiecePos[i,j]];
					Transform currPieceObject = MonoBehaviour.Instantiate( currTransform, currPos, currTransform.rotation ) as Transform;
					
					
					if( i == 0 || i == 1 ) {
						
						aBoard[i,j].Init( currPieceObject.gameObject, PlayerSide.e_White,
							ChessData.aStartPiecePos[i,j], i, j );												
					}
					else if( i == 6 || i == 7 ) {
						
						aBoard[i,j].Init( currPieceObject.gameObject, PlayerSide.e_Black,
							ChessData.aStartPiecePos[i,j], i, j );						
					}									
				}
				
				ParticleSystem movablePiecePSystem = MonoBehaviour.Instantiate( movablePSystemRef, Vector3.zero, Quaternion.identity ) as ParticleSystem;
				aBoard[i,j].SetMovableEffect( movablePiecePSystem );
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
		selectPiecePSystem = MonoBehaviour.Instantiate( selectPSystemRef, Vector3.zero, Quaternion.identity ) as ParticleSystem;				
		selectPiecePSystem.Stop();
	}
	
	public void SelectPiece( GameObject gameObject, Vector3 vPos, Quaternion rot ) {
		
		if( gameObject != null ) {			
						
			// movable pos	
			int nRank = 0, nPile = 0;
			bool bValidPos = ChessPosition.GetRankPilePos( vPos, ref nRank, ref nPile );
			if( bValidPos ) {
				
				if( aBoard[nPile, nRank].playerSide == UserPlayerSide ) {
					
					PlaySelectEffect( vPos, rot );				
					selectPiece.CopyFrom( aBoard[nPile, nRank] );			
					return;
				}
			}			
		}		
			
		// movable pos	
		StopSelectEffect();
		selectPiece.Init();			
	}
	
	public bool MoveTo( Vector3 vPos ) {		
		
		if( selectPiece.playerSide == UserPlayerSide ) {
			int nRank = 0, nPile = 0;
			bool bValidPos = ChessPosition.GetRankPilePos( vPos, ref nRank, ref nPile );				
			if( bValidPos ) {			
				
				int nSelRank = 0, nSelPile = 0;
				selectPiece.position.GetPositionIndex( ref nSelRank, ref nSelPile );			
				
				if( IsValidMove( aBoard[nPile, nRank].position, ref currMove ) ) {					
					
					// increase half move and total move					
					if( currMove.movePiece.IsBlank() ) {
						
						if( selectPiece.playerSide == PlayerSide.e_Black )
							nCurrTotalMove++;												
					}
					else {
						
						UnityEngine.Debug.LogError( "MoveTo() Not set Move ChessPiece" );
						return false;												
					}
					
					if( currMove.IsResetHalfMove() )						
						nCurrHalfMove = 0;	
					else						
						nCurrHalfMove++;					
					
					aBoard[nPile, nRank].SetPiece( selectPiece );			
					aBoard[nSelPile, nSelRank].ClearPiece();
					
					return true;
				}					
			}	
		}
		
		return false;
	}
	
	public void UpdateCurrMoveable() {
		
		StopMovableEffect();
		
		listCurrMovable.Clear();
		ChessMoveManager.GetValidateMoveList( aBoard, selectPiece, listCurrMovable );
		
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
		if( currTurn == PlayerSide.e_White )
			strTurn = "w";
		else if( currTurn == PlayerSide.e_Black )
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
				
				if( aBoard[i,j].gameObject != null ) {
					
					if( aBoard[i,j].playerSide == PlayerSide.e_White )
						aBoard[i,j].gameObject.renderer.material.SetColor( "_Color", rgbaWhite );											
				}
			}
		}		
	}
	
	void SetBlackSidePieceColor( Color rgbaBlack ) {
		
		for( int i=0; i<ChessData.nNumPile; i++ ) {
			for( int j=0; j<ChessData.nNumRank; j++ ) {
				
				if( aBoard[i,j].gameObject != null ) {
					
					if( aBoard[i,j].playerSide == PlayerSide.e_Black )
						aBoard[i,j].gameObject.renderer.material.SetColor( "_Color", rgbaBlack );	
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
		foreach( ChessPiece piece in aBoard ) {		
			
			int nRank = 0, nPile = 0;
			piece.position.GetPositionIndex( ref nRank, ref nPile );
			aBoard[nPile, nRank].ShowMovableEffect(false);
		}		
	}
	
	void PlayMovableEffect() {
		
		foreach( ChessMoveManager.sMove move in listCurrMovable ) {
			
			int nRank = 0, nPile = 0;
			move.trgPos.GetPositionIndex( ref nRank, ref nPile );			
			
			aBoard[nPile, nRank].ShowMovableEffect(true);
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
}
	
//}
