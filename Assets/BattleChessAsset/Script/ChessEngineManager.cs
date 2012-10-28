using UnityEngine;
using System.Collections;

// execute for chess engine process
using System.Diagnostics;
using System.IO; 

// chess engine manager class
public class ChessEngineManager {
	
	bool bInitProc;
	
	// chess engine executive file path
	string strProcPath = @"C:\Users\darak\Documents\GitHub\chichikov\ChessEngine\StockFish";	
	
	// execute for chess engine process	
    Process procEngine;	
	
	// received command respond queue
	Queue queReceived;	
	
	
	
	
	
	public ChessEngineManager() {		
		
		queReceived = new Queue();		
		procEngine = null;		
		bInitProc = false;
	}	
	
	// interface
	public IEnumerator Start() {
		
		// if engine process is not already running, wait for 2 sec for engine process init
		if( bInitProc == false ) {			
		
			procEngine = new Process();
			procEngine.StartInfo.FileName = strProcPath;    	
			procEngine.StartInfo.CreateNoWindow = true;		
			
			// start chess engine(stockfish)
    		procEngine.Start();
			
			// wait for 2.0 sec for process thread running
			yield return new WaitForSeconds(2.0f);		
			
			bInitProc = true;
			
			// send command to chess engine
			// 1, uci
			Send( "uci" );
		}	
		
		// clear received command respond que
		queReceived.Clear();		
	}
	
	public void End() {
		
		if( bInitProc ) {
			
			queReceived.Clear();
			
			procEngine.Kill();
			procEngine.Close();
			procEngine = null;			
			
			bInitProc = false;			
		}		
	}
	
	public void Send( string strCommand ) {
		
		procEngine.StandardInput.WriteLine( strCommand );
	}	
	
	public string PopReceivedQueue() {
		
		if( queReceived.Count > 0 )
		{
			string strRet = queReceived.Dequeue() as string;
			return strRet;
		}
		
		return null;
	}
	
	public void UpdateReceivedQueue() {
		
		if( bInitProc )
		{
			while( procEngine.StandardOutput.Peek() >= 0 ) { 			

				string strRecieved = procEngine.StandardOutput.ReadLine();
				queReceived.Enqueue( strRecieved );
			}
		}
	}
	
	// internal	
}
