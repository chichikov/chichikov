using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandBase {
	
	// command parse exception
	class CmdException : System.Exception
	{
	    public CmdException( string message ) : base( message )
	    {
	
	    }
	}
	
	class CmdParseException : CmdException
	{
	    public CmdParseException( string message ) : base( message )
	    {
	
	    }
	}
	
	
		
	// inner struct
	public class CommandData {
		
		// command/value pair
		public string strCmd;		
		
		public Queue<string> queueStrValue;		
		
		// sub command
		//public CommandData [] aSubCmdData;
		public Queue<CommandData> queueSubCmdData;
		
		// whether invalid command or not
		public bool bInvalidCmd;	
		
		public CommandData() {
			
			strCmd = null;			
			queueStrValue = new Queue<string>();
		
			bInvalidCmd = true;
			
			//aSubCmdData = null;
			queueSubCmdData = new Queue<CommandData>();
		}
	}
	
	
	
	// command source line
	public string strCmdSrc;		
	
	// command data
	public CommandData commandData;
	
	
	public CommandBase( string strCommandSrc ) {		
		
		strCmdSrc = strCommandSrc;		
		
		commandData = new CommandData();	
	}
	
	
	
	
	
	// helper function
	string[] GetNextTokens( string [] str_tokens ) {
		
		if( str_tokens != null && str_tokens.Length > 1 ) {
			
			string [] str_next_tokens = new string[str_tokens.Length - 1];
			int nCnt = 0;
			foreach( string currToken in str_tokens ) {
				
				if( nCnt >= 1 ) {
					str_next_tokens[nCnt-1] = currToken;
				}
				
				nCnt++;
			}
			return str_next_tokens;
		}
		
		throw new CmdParseException( "GetNextTokens() - Invalid Token Exception Throw!!!" );
		//return null;
	}	
	
	string[] GetNextCommandTokens( string [] str_tokens, string strNextCmd, out string strCurrValueTokens ) {
		
		string [] str_next_tokens = null;
		if( str_tokens != null && str_tokens.Length > 1 ) {		
			
			// extract current's command value
			bool bEndExtracted = false;
			bool bCreatedExtra = false;
			
			int nCurrToken = 0;
			int nCurrNextToken = 0;
			
			strCurrValueTokens = "";
			
			foreach( string strToken in str_tokens ) {
				
				if( strToken == strNextCmd )			
					bEndExtracted = true;
				
				// 
				if( bEndExtracted ) {
					
					if( bCreatedExtra == false ) {
						
						str_next_tokens = new string[str_tokens.Length - nCurrToken];	
						bCreatedExtra = true;
					}
					
					if( bCreatedExtra )
						str_next_tokens[nCurrNextToken++] = str_tokens[nCurrToken];		
				}
				else {
					
					strCurrValueTokens += " " + strToken;				
				}	
				
				nCurrToken++;
			}
			
			if( str_next_tokens != null && str_next_tokens.Length > 0 )
				return str_next_tokens;
			
			throw new CmdParseException( "GetNextCommandTokens() - Invalid parameter Exception Throw!!!" );
			
			//return null;
		}
		else {
			
			throw new CmdParseException( "GetNextCommandTokens() - Invalid parameter Exception Throw!!!" );			
		}
		
		//return null;
	}	
	
	string GetTokensToString( string [] str_tokens ) {
		
		if( str_tokens != null && str_tokens.Length > 0 ) {
			
			string strRet = "";
			foreach( string strToken in str_tokens ) {
				strRet += " " + strToken;				
			}
			
			return strRet;
		}
		
		throw new CmdParseException( "GetTokensToString() - No Ecist Token Exception Throw!!!" );		
	}
	
	
	// parse
	// parse sub command
	void ParseIdCommand( CommandData cmdData, string [] str_tokens ) {	
		
		// sub command 1, name <x>, 2, author <x>
		string [] str_next_tokens = GetNextTokens( str_tokens );	
		//str_next_tokens = GetNextTokens( str_next_tokens );
		string strSubCmd = str_next_tokens[0];
		
		// should have sub command!!!
		if( strSubCmd == "name" )
		{
			// sub command
			CommandData subCmdData = new CommandData();
			subCmdData.strCmd = strSubCmd;			
			string strValue = GetTokensToString( str_next_tokens );
			subCmdData.queueStrValue.Enqueue( strValue );
			subCmdData.bInvalidCmd = false;			
			
			cmdData.queueSubCmdData.Enqueue(subCmdData);
						
			cmdData.bInvalidCmd = false;
		}		
		else if( strSubCmd == "author" )
		{
			// sub command
			CommandData subCmdData = new CommandData();
			subCmdData.strCmd = strSubCmd;
			string strValue = GetTokensToString( str_next_tokens );
			subCmdData.queueStrValue.Enqueue( strValue );
			subCmdData.bInvalidCmd = false;			
			
			cmdData.queueSubCmdData.Enqueue(subCmdData);
						
			cmdData.bInvalidCmd = false;
		}
		else {
			
			throw new CmdParseException( "ParseIdCommand() - Invalid Sub Command Exception Throw!!!" );
		}
	}	
	
	void ParseUciOkCommand( CommandData cmdData, string [] str_tokens ) {		
		
		cmdData.bInvalidCmd = false;
	}
	
	void ParseReadyOkCommand( CommandData cmdData, string [] str_tokens ) {		
		
		cmdData.bInvalidCmd = false;
	}
	
	void ParseCopyProtectionCommand( CommandData cmdData, string [] str_tokens ) {		
		
		if( str_tokens != null && str_tokens.Length > 1 ) {
			
			string [] str_next_tokens = GetNextTokens( str_tokens );
			
			// value 1, checking, 2, ok, 3, error
			cmdData.queueStrValue.Clear();				
			cmdData.queueStrValue.Enqueue( str_next_tokens[0] );
			cmdData.queueSubCmdData.Clear();
			
			cmdData.bInvalidCmd = false;
		}
		else {
			
			throw new CmdParseException( "ParseCopyProtectionCommand() - Invalid Command Value Exception Throw!!!" );			
		}
	}
	
	void ParseRegistrationCommand( CommandData cmdData, string [] str_tokens ) {
		
		if( str_tokens != null && str_tokens.Length > 1 ) {
			
			string [] str_next_tokens = GetNextTokens( str_tokens );
			
			// value 1, checking, 2, ok, 3, error
			cmdData.queueStrValue.Clear();				
			cmdData.queueStrValue.Enqueue( str_next_tokens[0] );
			cmdData.queueSubCmdData.Clear();
			
			cmdData.bInvalidCmd = false;
		}
		else {
			
			throw new CmdParseException( "ParseRegistrationCommand() - Invalid Command Value Exception Throw!!!" );			
		}
	}
	
	void ParseOptionCommand( CommandData cmdData, string [] str_tokens ) {					
		
		if( str_tokens != null && str_tokens.Length > 1 ) {
		
			string [] str_next_tokens = GetNextTokens( str_tokens );			
			//str_next_tokens = GetNextTokens( str_next_tokens );
			string strSubCmd = str_next_tokens[0];		
			
			// should have sub command!!!		
			if( strSubCmd == "name" ) {	
				
				CommandData subCmdData = new CommandData();			
				subCmdData.strCmd = strSubCmd;			
				string strCmdValue;
				str_next_tokens = GetNextCommandTokens( str_next_tokens, "type", out strCmdValue );
				subCmdData.queueStrValue.Enqueue( strCmdValue );
				subCmdData.bInvalidCmd = false;			
				
				cmdData.queueSubCmdData.Enqueue( subCmdData );
				
				strSubCmd = str_next_tokens[0];
				if( strSubCmd == "type" ) {
					
					subCmdData = new CommandData();	
					subCmdData.strCmd = strSubCmd;				
					str_next_tokens = GetNextTokens( str_next_tokens );				
					strCmdValue = str_next_tokens[0];				
					subCmdData.queueStrValue.Enqueue( strCmdValue );
					subCmdData.bInvalidCmd = false;	
					
					cmdData.queueSubCmdData.Enqueue( subCmdData );					
				}			
				
				if( str_next_tokens != null && str_next_tokens.Length > 1 ) {
					
					str_next_tokens = GetNextTokens( str_next_tokens );
					strSubCmd = str_next_tokens[0];
					if( strSubCmd == "default" ) {
						
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;				
						str_next_tokens = GetNextTokens( str_next_tokens );
						strCmdValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strCmdValue );
						subCmdData.bInvalidCmd = false;	
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );							
					}			
					
					if( str_next_tokens != null && str_next_tokens.Length > 1 ) {
						str_next_tokens = GetNextTokens( str_next_tokens );	
						strSubCmd = str_next_tokens[0];
						
						if( strSubCmd == "var" ) {
							
							subCmdData = new CommandData();
							subCmdData.strCmd = strSubCmd;				
							str_next_tokens = GetNextTokens( str_next_tokens );
							strCmdValue = str_next_tokens[0];
							subCmdData.queueStrValue.Enqueue( strCmdValue );				
							subCmdData.bInvalidCmd = false;	
							
							while( str_next_tokens.Length > 1 ) {
								
								str_next_tokens = GetNextTokens( str_next_tokens );
								str_next_tokens = GetNextTokens( str_next_tokens );	
								strCmdValue = str_next_tokens[0];
								subCmdData.queueStrValue.Enqueue( strCmdValue );						
							}
							
							cmdData.queueSubCmdData.Enqueue( subCmdData );						
						}
						else {
							
							if( strSubCmd == "min" ) {
								
								subCmdData = new CommandData();
								subCmdData.strCmd = strSubCmd;				
								str_next_tokens = GetNextTokens( str_next_tokens );
								strCmdValue = str_next_tokens[0];
								subCmdData.queueStrValue.Enqueue( strCmdValue );				
								subCmdData.bInvalidCmd = false;	
								
								cmdData.queueSubCmdData.Enqueue( subCmdData );
								
								str_next_tokens = GetNextTokens( str_next_tokens );				
							}
										
							strSubCmd = str_next_tokens[0];
							if( strSubCmd == "max" ) {
								
								subCmdData = new CommandData();
								subCmdData.strCmd = strSubCmd;
								str_next_tokens = GetNextTokens( str_next_tokens );
								strCmdValue = str_next_tokens[0];				
								subCmdData.queueStrValue.Enqueue( strCmdValue );			
								subCmdData.bInvalidCmd = false;	
								
								cmdData.queueSubCmdData.Enqueue( subCmdData );									
							}
						}
					}
				}
				
				cmdData.bInvalidCmd = false;
			}
			else {
				
				throw new CmdParseException( "ParseOptionCommand() - Invalid Command Value Exception Throw!!!" );			
			}
		}
		else {
			
			throw new CmdParseException( "ParseOptionCommand() - Invalid Parameter Exception Throw!!!" );			
		}
	}
		
		
	void ParseInfoCommand( CommandData cmdData, string [] str_tokens ) {																	
		
		if( str_tokens != null && str_tokens.Length > 1 ) {
			
			string [] str_next_tokens = GetNextTokens( str_tokens );
			
			string strSubCmd = str_next_tokens[0];
			str_next_tokens = GetNextTokens( str_next_tokens );			
			
			CommandData subCmdData = null;
			string strValue = null;
			
			while( strSubCmd != null ) {
				
				// should have sub command!!!		
				switch( strSubCmd ) {
					
					case "depth":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );						
					}
					break;
					
					case "seldepth":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );								
					}
					break;
					
					case "time":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );
					}
					break;
					
					case "nodes":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );								
					}
					break;
					
					case "pv":
					{
						
					}
					break;
					
					case "multipv":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );					
					}
					break;
					
					case "score":
					{
						
					}
					break;
					
					case "currmove":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );			
					}
					break;
					
					case "currmovenumber":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );					
					}
					break;
					
					case "hashfull":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );			
					}
					break;
					
					case "nps":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );					
					}
					break;
					
					case "tbhits":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );					
					}
					break;
					
					case "sbhits":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );				
					}
					break;
					
					case "cpuload":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );				
					}
					break;
					
					case "string":
					{
						subCmdData = new CommandData();
						subCmdData.strCmd = strSubCmd;
						strValue = str_next_tokens[0];
						subCmdData.queueStrValue.Enqueue( strValue );																
						subCmdData.bInvalidCmd = false;				
						
						cmdData.queueSubCmdData.Enqueue( subCmdData );				
					}
					break;
					
					case "refutation":
					{
						
					}
					break;
					
					case "currline":
					{
						
					}
					break;	
				}	
				
				if( str_next_tokens != null && str_next_tokens.Length > 1 ) {
					
					str_next_tokens = GetNextTokens( str_next_tokens );
					strSubCmd = str_next_tokens[0];
				}
				else {
					
					strSubCmd = null;				
				}
			}		
			
			cmdData.bInvalidCmd = false;
		}
		else {
			
			throw new CmdParseException( "ParseInfoCommand() - Invalid Parameter Exception Throw!!!" );			
		}
	}	

	void ParseBestMoveCommand( CommandData cmdData, string [] str_tokens ) {
		
		if( str_tokens != null && str_tokens.Length > 1 ) {
			
			string [] str_next_tokens = GetNextTokens( str_tokens );
			
			// bestmove <move1> [ ponder <move2> ]
			string strValue = str_next_tokens[0];
			cmdData.queueStrValue.Enqueue( strValue );														
			
			if( str_next_tokens.Length > 1 ) {
				// sub command - ponder <move2>		
				str_next_tokens = GetNextTokens( str_next_tokens );
				string strSubCmd = str_next_tokens[0];
				
				if( strSubCmd == "ponder" )
				{
					// subsub command
					CommandData subCmdData = new CommandData();			
					subCmdData.strCmd = strSubCmd;			
					strValue = str_next_tokens[0];
					subCmdData.queueStrValue.Enqueue( strValue );	
					subCmdData.bInvalidCmd = false;
					
					cmdData.queueSubCmdData.Enqueue( subCmdData );
				}
			}
			
			cmdData.bInvalidCmd = false;			
		}
		else {
			
			throw new CmdParseException( "ParseBestMoveCommand() - Invalid Parameter Exception Throw!!!" );						
		}
	}
	
	// parse command line
	public bool ParseCommand() {		
		
		try {
			// split token
			char[] delimiterChars = { ' ' };
	        string[] str_tokens = strCmdSrc.Split(delimiterChars);					
			
			// get command token	
			commandData.strCmd = str_tokens[0];				
			
			switch( commandData.strCmd ) {
				
			case "id":		
				ParseIdCommand( commandData, str_tokens );		
			break;
					
			case "uciok":
				ParseUciOkCommand( commandData, str_tokens );		
			break;
				
			case "readyok":		
				ParseReadyOkCommand( commandData, str_tokens );		
			break;		
			
			case "copyprotection":
				ParseCopyProtectionCommand( commandData, str_tokens );		
			break;
			
			case "registration":
				ParseRegistrationCommand( commandData, str_tokens );			
			break;
				
			case "option":
				ParseOptionCommand( commandData, str_tokens );			
			break;
			
			case "info":
				ParseInfoCommand( commandData, str_tokens );		
			break;
				
			case "bestmove":
				ParseBestMoveCommand( commandData, str_tokens );			
			break;
			
				
			default:												
				return false;						
				
			} // switch		
			
			if( commandData.bInvalidCmd ) {
				
				UnityEngine.Debug.Log( "Parsed Unknown Command or Sub Command Error" + " " + commandData.strCmd );				
				return false;
			}
			
			return true;
			
		} // switch		
		catch ( CmdException ex ) {
		
			UnityEngine.Debug.Log( ex.ToString() );			
		}
		finally {
			
						
		}
		
		return false;
	}	
}








public class EngineToGuiCommand : CommandBase {
	
	public EngineToGuiCommand( string strCommandSrc ) : base( strCommandSrc ) {	
	}
}







// engine to gui command
public class ChessEngineCmdParser {
	
	public EngineToGuiCommand cmd;
	
	public ChessEngineCmdParser() {	
		
		cmd = null;		
	}
	
	
	public bool Parse( string strCommandLine ) {
		
		cmd = null;
		cmd = new EngineToGuiCommand( strCommandLine );
		
		return cmd.ParseCommand();		
	}
}

