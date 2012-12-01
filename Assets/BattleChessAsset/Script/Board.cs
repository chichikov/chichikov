using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class Board : MonoBehaviour {	
	
	// chess engine management
	private ChessEngineManager chessEngineMgr;	
	
	// chess piece prefab reference
	public Transform[] aWholePiece;		
	
	// board, 8 x 8, pile x rank
	private ChessBoardData.ChessPiece[,] aBoard;
	
	// board material
	private Material matBoard1;	
	private Material matBoard2;		
	
	// particle effect
	public ParticleSystem selectPiecePSystemPref;	
	private ParticleSystem selectPiecePSystem;
	private GameObject selectPiecePSObj;	
	
	// selected piece
	private ChessBoardData.ChessPiece selectPiece;
	
	// movable board pos
	private List<ChessEnginMoveManager.sMove> listCurrMovable;
	
	// movable particle effect
	public ParticleSystem movablePiecePSystemRef;
	
	
	
	// Use this for initialization
	void Start () {				
		
		listCurrMovable = new List<ChessEnginMoveManager.sMove>();
		
		// init piece
		
		
		// init board
		aBoard = new ChessBoardData.ChessPiece[ChessBoardData.nNumPile,ChessBoardData.nNumRank];
		for( int i=0; i<ChessBoardData.nNumPile; i++ ){
			for( int j=0; j<ChessBoardData.nNumRank; j++ ){
				if( ChessBoardData.aStartPiecePos[i,j] == ChessBoardData.PieceInstanceType.eNone_Piece ) {
					
					aBoard[i,j].SetPiece( null, ChessBoardData.PlayerSide.e_NoneSide,
						ChessBoardData.PieceInstanceType.eNone_Piece, i, j );					
				}
				else
				{
					Vector3 currPos = new Vector3( j - 3.5f, 0.0f, i - 3.5f );					
					
					Transform currTransform = aWholePiece[(int)ChessBoardData.aStartPiecePos[i,j]];
					Transform currPieceObject = Instantiate( currTransform, currPos, currTransform.rotation ) as Transform;
					
					
					if( i == 0 || i == 1 ) {
						
						aBoard[i,j].SetPiece( currPieceObject.gameObject, ChessBoardData.PlayerSide.e_White,
							ChessBoardData.aStartPiecePos[i,j], i, j );												
					}
					else if( i == 6 || i == 7 ) {
						
						aBoard[i,j].SetPiece( currPieceObject.gameObject, ChessBoardData.PlayerSide.e_Black,
							ChessBoardData.aStartPiecePos[i,j], i, j );						
					}									
				}
				
				ParticleSystem movablePiecePSystem = Instantiate( movablePiecePSystemRef, Vector3.zero, Quaternion.identity ) as ParticleSystem;
				aBoard[i,j].SetMovableEffect( movablePiecePSystem );
			}		
		}
		
		
		// piece coloar
		SetWhiteSidePieceColor( Color.white );
		SetBlackSidePieceColor( Color.white );
		
		
		// board material
		if( renderer.materials.Length == 2 ) {
			
			matBoard1 = renderer.materials[0];
			matBoard2 = renderer.materials[1];			
			
			Color rgbaWhiteBoard, rgbaBlackBoard;
			rgbaWhiteBoard = new Color( 1.0f, 1.0f, 1.0f, 1.0f );
			rgbaBlackBoard = new Color( 0.039f, 0.34f, 0.22f, 1.0f );
			
			SetWhiteSideBoardColor( rgbaWhiteBoard );
			SetBlackSideBoardColor( rgbaBlackBoard );
		}	
		
		// particle effect
		selectPiecePSystem = Instantiate( selectPiecePSystemPref, Vector3.zero, Quaternion.identity ) as ParticleSystem;		
		selectPiecePSObj = selectPiecePSystem.gameObject;
		selectPiecePSystem.Stop();
		
		// chess engine init/start
		chessEngineMgr = new ChessEngineManager();	
		
		StartCoroutine( chessEngineMgr.Start() );		
	}
	
	// Update is called once per frame
	void Update () {		
		
		// process engine command respond
		ProcessEngineCommand();	
		
		// input
		// piece selection
		if( Input.GetMouseButton(0) ) {
			
			// collision check
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        
			if( Physics.Raycast( ray, out hitInfo, 1000 ) ) {
				
				if( hitInfo.collider.gameObject.tag != "Board" ) {
					//hitInfo.collider.gameObject.
					Vector3 vPos = hitInfo.collider.gameObject.transform.position;
					Quaternion rot = hitInfo.collider.gameObject.transform.rotation;
					
					SelectPiece( hitInfo.collider.gameObject, vPos, rot );													
				}
				else {
					
					SelectPiece( null, Vector3.zero, Quaternion.identity );										
				}
			}
			else {
				
				SelectPiece( null, Vector3.zero, Quaternion.identity );								
			}
			
			UpdateCurrMoveable();
		}	
	}
	
	void OnDestroy () {   
		
		// chess engine end
		chessEngineMgr.End();
		
	}	
	
	
	
	
	
	
	//
	void GetRankPilePos( Vector3 pos ) {		
		
		float fHalfBoardWidth = ChessBoardData.fBoardWidth / 2.0f;
		if( pos.x >= -fHalfBoardWidth && pos.x <= fHalfBoardWidth && 
			pos.z >= -fHalfBoardWidth && pos.z <= fHalfBoardWidth ) {
			
			int nPosX, nPosZ;
			if( pos.x < 0 )
			{
				nPosX = (int)(pos.x - 0.5f);
				nPosX += 4;
			}
			else	
			{
				nPosX = (int)(pos.x + 1.0f);
				nPosX += 3;
			}			
			
			if( pos.z < 0 )	
			{
				nPosZ = (int)(pos.z - 0.5f);
				nPosZ += 4;
			}
			else	
			{
				nPosZ = (int)(pos.z + 1.0f);
				nPosZ += 3;
			}				
			
			selectPiece.SetPiece( ref aBoard[nPosZ, nPosX] );			
		}
		else {
			
			selectPiece.Clear();			
		}
	}
	
	void SelectPiece( GameObject gameObject, Vector3 pos, Quaternion rot ) {
		
		if( gameObject != null ) {
			
			selectPiecePSObj.transform.position = pos;
			selectPiecePSObj.transform.rotation = rot;
			selectPiecePSystem.Play();		
			// movable pos	
			GetRankPilePos( pos );
		}
		else {
			
			selectPiecePSystem.Stop();
			// movable pos	
			selectPiece.Clear();
		}
	}
	
	void UpdateCurrMoveable() {
		
		// previous movable effect stop
		foreach( ChessBoardData.ChessPiece piece in aBoard ) {		
			
			int nRank = 0, nPile = 0;
			ChessBoardData.ChessPosition.GetPositionIndex( piece.position.pos, ref nRank, ref nPile );
			aBoard[nPile, nRank].ShowMovableEffect(false);
		}
		
		listCurrMovable.Clear();
		ChessEnginMoveManager.GetValidateMoveList( aBoard, selectPiece, listCurrMovable );
		
		// movable effect start
		foreach( ChessEnginMoveManager.sMove move in listCurrMovable ) {
			
			int nRank = 0, nPile = 0;
			ChessBoardData.ChessPosition.GetPositionIndex( move.trgPos, ref nRank, ref nPile );
			
			if( move.trgPos == ChessBoardData.BoardPosition.InvalidPosition )
				continue;
			
			aBoard[nPile, nRank].ShowMovableEffect(true);
		}		
	}

	
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
		
		for( int i=0; i<ChessBoardData.nNumPile; i++ ) {
			for( int j=0; j<ChessBoardData.nNumRank; j++ ) {
				
				if( aBoard[i,j].gameObject != null ) {
					
					if( aBoard[i,j].playerSide == ChessBoardData.PlayerSide.e_White )
						aBoard[i,j].gameObject.renderer.material.SetColor( "_Color", rgbaWhite );											
				}
			}
		}		
	}
	
	void SetBlackSidePieceColor( Color rgbaBlack ) {
		
		for( int i=0; i<ChessBoardData.nNumPile; i++ ) {
			for( int j=0; j<ChessBoardData.nNumRank; j++ ) {
				
				if( aBoard[i,j].gameObject != null ) {
					
					if( aBoard[i,j].playerSide == ChessBoardData.PlayerSide.e_Black )
						aBoard[i,j].gameObject.renderer.material.SetColor( "_Color", rgbaBlack );	
				}
			}
		}	
	}	
	
	
	
	
	
	
	// process engine command respond
	void ProcessEngineCommand() {
		// read one line
		string strCurCommandLine = chessEngineMgr.PopReceivedQueue();
		while( strCurCommandLine != null ) {
			
			// process one engine respond
			EngineToGuiCommand command = chessEngineMgr.ParseCommand( strCurCommandLine );
			if( command != null ) {				
				
				//command.PrintCommand();
				chessEngineMgr.SetConfigCommand( command.commandData );
				
				ExcuteEngineCommand( command );								
			}
			
			strCurCommandLine = chessEngineMgr.PopReceivedQueue();
		}
		
	}
	
	
	
	// 
	bool ExcuteIdCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteUciOkCommand( CommandBase.CommandData cmdData ) {
		
		// send setoption command!!!
		
		// send isready command	
		chessEngineMgr.Send( "isready" );		
		
		return false;
	}
	
	bool ExcuteReadyOkCommand( CommandBase.CommandData cmdData ) {
		
		// send isready command	
		chessEngineMgr.Send( "ucinewgame" );
		
		// test move
		chessEngineMgr.Send( "position startpos moves e2e4" );
		
		/*
		if( aBoard[1,4] != null ) {
			Vector3 vNewPos = new Vector3( aBoard[1,4].transform.position.x, 
				aBoard[1,4].transform.position.y, 0.5f );
			
			aBoard[1,4].transform.position = vNewPos;
		}
		*/
		
		chessEngineMgr.Send( "go" );	
		
		return false;
	}
	
	bool ExcuteCopyProtectionCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteRegistrationCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteOptionCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteInfoCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ExcuteBestMoveCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}	
	
	// delegate function for engine command
	public bool ExcuteEngineCommand( EngineToGuiCommand cmd ) {		
		
		if( cmd == null )
			return false;
		
		bool bValidCmd = !cmd.commandData.bInvalidCmd;		
		if( bValidCmd ) {
			
			string strCmd = cmd.commandData.strCmd;
			
			switch( strCmd ) {
					
				case "id":		
					return ExcuteIdCommand( cmd.commandData );						
						
				case "uciok":	
					return ExcuteUciOkCommand( cmd.commandData );				
					
				case "readyok":		
					return ExcuteReadyOkCommand( cmd.commandData );							
				
				case "copyprotection":
					return ExcuteCopyProtectionCommand( cmd.commandData );						
				
				case "registration":
					return ExcuteRegistrationCommand( cmd.commandData );							
					
				case "option":
					return ExcuteOptionCommand( cmd.commandData );							
				
				case "info":
					return ExcuteInfoCommand( cmd.commandData );											
				
				case "bestmove":
					return ExcuteBestMoveCommand( cmd.commandData );							
					
				default:								
					return false;					
			} // switch	
			
			//return true;
		}	
		
		return false;
	}	
	
}
