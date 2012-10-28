using UnityEngine;
using System.Collections;

public abstract class ChessEngineCommand {
	
	string strSrcCmd;	
	string strCmd;
	
	ArrayList alSubCommand;
	
	public ChessEngineCommand( string strCommand ) {
		
		strSrcCmd = strCommand;
		alSubCommand.Clear();		
	}
	
	public abstract bool Parse();
}


// engine to gui command
public class EngineToGuiCommand : ChessEngineCommand {	
	
	public static Hashtable htCmdDictionary;
	
	// static constructor
	static EngineToGuiCommand() {
		
		htCmdDictionary = new Hashtable();
		
		// command		
		// id
		/*
		htCmdDictionary["id"] = new Hashtable();		
		(htCmdDictionary["id"])["name"] = null;
		(htCmdDictionary["id"])["author"] = null;
		
		// uciok
		htCmdDictionary["uciok"] = null;
		
		// readyok
		htCmdDictionary["readyok"] = null;
		
		// bestmove <move1> [ponder <move2>]
		htCmdDictionary["bestmove"] = new Hashtable();
		(htCmdDictionary["bestmove"])["ponder"] = null;
		
		// copyprotection
		htCmdDictionary["copyprotection"] = null;
		
		// registration
		htCmdDictionary["registration"] = null;
		
		// info
		htCmdDictionary["info"] = new Hashtable();
		(htCmdDictionary["info"])["depth"] = null;
		(htCmdDictionary["info"])["seldepth"] = null;
		(htCmdDictionary["info"])["time"] = null;
		(htCmdDictionary["info"])["nodes"] = null;
		(htCmdDictionary["info"])["seldepth"] = null;
		(htCmdDictionary["info"])["seldepth"] = null;
		(htCmdDictionary["info"])["seldepth"] = null;
		
		// info
		htCmdDictionary["registration"] = null;
		*/
		
				
	}
	
	public EngineToGuiCommand( string strCommand ) : base(strCommand) {
		
		
		
	}
	
	public override bool Parse() {
		
		return true;
	}		
}

