using UnityEngine;
using System.Collections;

// execute for chess engine process
using System.Diagnostics;
using System.IO; 

// chess engine manager class
public class ChessEngineManager {
	
	// chess engine executive file path
	private string strChessEnginePath = @"F:\WorkUnity\Unity Projects\Work\WorkU\BattleChessInfinity\ChessEngine\StockFish.exe";	
	
	// execute for chess engine process	
    private Process ChessEngineProc = null;	
	
	
	public ChessEngineManager() {
		
		Init();		
	}
	
	~ChessEngineManager() {
		
		Destroy();
	}	
	
	void Init() {
		
	}
	
	void Destroy() {
		
	}
	
		
	
	public IEnumerator Start() {
		
		// start chess engine(stockfish)
		ChessEngineProc = new Process();
		ChessEngineProc.StartInfo.FileName = strChessEnginePath;    	
		ChessEngineProc.StartInfo.CreateNoWindow = true;

    	ChessEngineProc.Start();
		
		// wait for 2.0 sec for process thread running
		yield return new WaitForSeconds(2.0f);
		
		
		
		// send command to chess engine
		// 1, uci
		//Send( "uci" );
		
		
	}
	
	public void End() {
		
		ChessEngineProc.Kill();	
		ChessEngineProc.Close();
		ChessEngineProc = null;
	}
	
	public void Send( string strCommand ) {
		
		ChessEngineProc.StandardInput.WriteLine( strCommand );
	}
	
	public string Receive() {
		
		string strRet = ChessEngineProc.StandardOutput.ReadLine();
		return strRet;
	}
		
}
