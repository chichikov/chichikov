using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {	
	
	// chess engine management
	private ChessEngineManager ChessEngineMgr;	
	
	// chess piece prefab reference
	public Transform[] aWholePiece;		
	
	// board, 8 x 8, pile x rank
	private GameObject[,] aBoard;	
	
	
	
	// Use this for initialization
	void Start () {	
		
		// chess engine init/start
		ChessEngineMgr = new ChessEngineManager();
		ChessEngineMgr.processUI = new ChessEngineManager.ProcessUI( ProcessEngineCommand );
		
		StartCoroutine( ChessEngineMgr.Start() );		
		
		// init piece
		
		// init board
		aBoard = new GameObject[ChessBoardData.nNumPile,ChessBoardData.nNumRank];
		for( int i=0; i<ChessBoardData.nNumPile; i++ ){
			for( int j=0; j<ChessBoardData.nNumRank; j++ ){
				if( ChessBoardData.aStartPiecePos[i,j] == ChessBoardData.nNone_Piece )
					aBoard[i,j] = null;
				else
				{
					Vector3 currPos = new Vector3( 0.5f + j * 1.025f - 1.025f * 4.0f, 0.01f, 0.5f + i * 1.025f - 1.025f * 4.0f );					
					
					Transform currTransform = aWholePiece[ChessBoardData.aStartPiecePos[i,j]];
					Transform currPieceObject = Instantiate( currTransform, currPos, currTransform.rotation ) as Transform;
					aBoard[i,j] = currPieceObject.gameObject;
				}
			}		
		}	
	}
	
	// Update is called once per frame
	void Update () {		
		
		// process engine command respond
		ProcessReceivedQueue();		
	
	}
	
	void OnDestroy () {   
		
		// chess engine end
		ChessEngineMgr.End();
		
	}
	
	void ProcessEngineRespond( string strRespond ) {
		
		ChessEngineMgr.ProcessCommand( strRespond );		
	}
		
	
	// process engine command respond
	void ProcessReceivedQueue() {
		// read one line
		string strCurRespond = ChessEngineMgr.PopReceivedQueue();
		while( strCurRespond != null ) {
			
			// process one engine respond
			ProcessEngineRespond( strCurRespond );			
			
			strCurRespond = ChessEngineMgr.PopReceivedQueue();
		}
		
	}
	
	
	
	// 
	bool ProcessIdCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ProcessUciOkCommand( CommandBase.CommandData cmdData ) {
		
		// send setoption command!!!
		
		// send isready command	
		ChessEngineMgr.Send( "isready" );		
		
		return false;
	}
	
	bool ProcessReadyOkCommand( CommandBase.CommandData cmdData ) {
		
		// send isready command	
		//ChessEngineMgr.Send( "ucinewgame" );
		
		// test move
		//ChessEngineMgr.Send( "position startpos moves e2e4" );
		//ChessEngineMgr.Send( "go infinite" );	
		
		return false;
	}
	
	bool ProcessCopyProtectionCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ProcessRegistrationCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ProcessOptionCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ProcessInfoCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}
	
	bool ProcessBestMoveCommand( CommandBase.CommandData cmdData ) {
		
		return false;
	}	
	
	// delegate function for engine command
	public bool ProcessEngineCommand( EngineToGuiCommand cmd ) {
		
		/*
		//if( cmd.commandData.bInvalidCmd == false ) {
			switch( cmd.commandData.strCmd ) {
					
				case "id":		
					break;//return ProcessIdCommand( cmd.commandData );						
						
				case "uciok":
					//return ProcessUciOkCommand( cmd.commandData );						
					ChessEngineMgr.Send( "isready" );
				break;
					
				case "readyok":		
					break;//return ProcessReadyOkCommand( cmd.commandData );							
				
				case "copyprotection":
					break;//return ProcessCopyProtectionCommand( cmd.commandData );						
				
				case "registration":
					break;//return ProcessRegistrationCommand( cmd.commandData );							
					
				case "option":
					break;//return ProcessOptionCommand( cmd.commandData );							
				
				case "info":
					break;//return ProcessInfoCommand( cmd.commandData );											
				
				case "bestmove":
					break;//return ProcessBestMoveCommand( cmd.commandData );							
					
				default:								
					return false;					
			} // switch				
		//}		
		*/
		
		//if( cmd.commandData.bInvalidCmd == false ) {
			if( cmd.strCmdSrc == "uciok" ) {
				
				ChessEngineMgr.Send( "isready" );
			}
		//}
		
		return false;
	}
}
