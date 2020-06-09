//////////////////////////////////////////////////////////////////////////////////////////////////////////
// ITokenInterface.cs - Project #2																		//
// ver 1.0																								//
//Language: C# VS2017																					//
//Platform : Dell Inspiron 15 3543 Windows 10															//
// Application: Lexical Scanner Project- Software Modelling Analysis									//
// Author: Harsh Doshi SUID 980107176																	//
//////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package provides an interface for handling the source of tokens and also an interface for handling the token gathering state.
  

 * Maintenance History
 * -------------------
 * ver 1.0 : 7th October 2018
 * - first release
 */


///////////////////////////////////////////////////////////////////
// ITokenSource interface
// - Declares operations expected of any source of tokens.
//   provides a source only for Files, e.g., TokenFileSource, below.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ITokenizer
{

	using Token = StringBuilder;
	public interface ITokenSource
	{
		bool open(string path);
		void close();
		int next();
		int peek(int n = 0);
		bool end();
		int lineCount { get; set; }
	}

	///////////////////////////////////////////////////////////////////
	// ITokenState interface
	// - Declares operations expected of any token gathering state

	public interface ITokenState
	{
		Token getTok();
		bool isDone();
	}

}
