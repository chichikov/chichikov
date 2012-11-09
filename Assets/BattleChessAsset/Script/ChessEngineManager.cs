using UnityEngine;
using System.Collections;

// execute for chess engine process
using System.Diagnostics;
using System.IO; 

// chess engine manager class
public class ChessEngineManager {	
	
	// chess engine executive file path
	// Application.dataPath < == asset folder
	string strProcPath = @"ChessEngine\StockFish.exe"; 	
	
	// execute for chess engine process	
    Process procEngine;	
	
	// stream input/output
	StreamWriter swWRiter;
	// 비동기 로딩 스트림은 레퍼런스를 가질 수 없다!!!
	//StreamReader srReader;
	//StreamReader srErrReader;	
	
	// received command respond queue
	Queue queReceived;	
	
	// engine command parser
	ChessEngineCmdParser cmdParser;
	
	// command processor delegate
	public delegate bool ProcessUI( EngineToGuiCommand cmd );	
	public ProcessUI processUI;
	
	
	
	
	
	public ChessEngineManager() {		
		
		procEngine = null;
		queReceived = null;
		swWRiter = null;
		//srReader = null;
		//srErrReader = null;
		
		cmdParser = null;
		processUI = null;
	}	
	
	// interface
	public IEnumerator Start() {	
		
		// clear received command respond que
		queReceived = new Queue();	
		
		procEngine = new Process();
		procEngine.StartInfo.FileName = strProcPath;				
		//procEngine.StartInfo.Arguments = "uci";
		procEngine.StartInfo.CreateNoWindow = true;
		procEngine.StartInfo.UseShellExecute = false;
		procEngine.StartInfo.ErrorDialog = false;
		procEngine.StartInfo.RedirectStandardOutput = true;
		procEngine.StartInfo.RedirectStandardInput = true;
		//procEngine.StartInfo.RedirectStandardError = true;		
		
		// Set our event handler to asynchronously read the sort output/err.
        procEngine.OutputDataReceived += new DataReceivedEventHandler(StandardOutputHandler);
		//procEngine.ErrorDataReceived += new DataReceivedEventHandler(StandardErrorHandler);
		
		// start chess engine(stockfish)
		procEngine.Start();	
		
		// Start the asynchronous read of the output stream.
        procEngine.BeginOutputReadLine();
		//procEngine.BeginErrorReadLine();
		
		swWRiter = procEngine.StandardInput;		
		//srReader = procEngine.StandardOutput;
		//srErrReader = procEngine.StandardError;
		
		cmdParser = new ChessEngineCmdParser();
		
		// wait for 2.0 sec for process thread running
		yield return new WaitForSeconds(2.0f);				
		
		// send command to chess engine
		// 첫번째 명령이 안먹는 이유는?????
		// 실행 파라미터로 "uci"를 주어야 하는데 스트림 리다이렉션때문에 이게 안먹힘...
		// 
		Send( "Ping Test" );
		Send( "uci" );	
		
		//Send( "isready" );
	}
	
	public void End() {
			
		queReceived.Clear();		
		
		swWRiter.Close();		
		// 비동기 로딩 스트림은 클로즈 하면 안된다!!!
		//srReader.Close();
		//srErrReader.Close();
		
		procEngine.Kill();
		procEngine.Close();				
		procEngine = null;	
		
		cmdParser = null;
		processUI = null;
	}
	
	public void Send( string strUciCmd ) {
		
		if (!string.IsNullOrEmpty( strUciCmd ))
        {		
			swWRiter.WriteLine( strUciCmd );		
		}
	}	
	
	public string PopReceivedQueue() {
		
		if( queReceived.Count > 0 )
		{
			string strRet = queReceived.Dequeue() as string;
			
			//UnityEngine.Debug.Log(strRet);
			
			return strRet;
		}
		
		return null;
	}	
	
	public bool ProcessCommand( string strCmdLine ) {
		
		if( cmdParser != null ) {			
			
			bool bParseSuccess = cmdParser.Parse( strCmdLine );			
			if( bParseSuccess ) {
				
				return processUI( cmdParser.cmd );
			}
		}
		
		return false;
	}

	
	// internal	
	
	// async std output/err read
	private void StandardOutputHandler( object objProcess, DataReceivedEventArgs outLine )
    {
        // Collect the command output. 
        if (!string.IsNullOrEmpty(outLine.Data))
        {
			UnityEngine.Debug.Log(outLine.Data);            
			
            queReceived.Enqueue( outLine.Data );
        }
    }
	
	/*
	private void StandardErrorHandler( object objProcess, DataReceivedEventArgs outLine )
    {
        // Collect the command output.         
    }
    */	
}
