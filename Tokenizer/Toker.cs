//////////////////////////////////////////////////////////////////////////////////////////////////////////
// Toker.cs - Project #2																				//
// ver 1.2																								//
//Language: C# VS2017																					//
//Platform : Dell Inspiron 15 3543 Windows 10															//
// Application: Lexical Scanner Project- Software Modelling Analysis									//
// Author: Harsh Doshi SUID 980107176																	//
// References:Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2018							//
//https://ecs.syr.edu/faculty/fawcett/handouts/CSE681/code/Project1HelpF2018/StatePattern_Toker_Demo/   //
//////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements a state based tokenizer and demonstrates 
 * formation of different tokens based on the state pattern 
  
 
 * Build Process
 * --------------------
 * Required Files:Toker.cs
                 :ITokenInterface.cs

* Public Interfaces
* Toker toker = new Toker();
* toker.getToken();
* toker.open(fullpath);
* toker.lineCount();
* toker.isDone()
* toker.getTok()
 

 * Maintenance History
 * -------------------
 * ver 1.2 : 08 October 2018
 * - added comments just above the definition of derived states
 * ver 1.1 : 07 October 2018
 * - Added DoublePunct State DoubleQuote State, CppComment State, SingleLineComment State and Cpp Comment State
 * ver 1.0 : 30 Aug 2018
 * - first release
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITokenizer;
using Tokenizer;

namespace Tokenizer
{
	using Token = StringBuilder;

	///////////////////////////////////////////////////////////////////
	// Toker class
	// - applications  use this class to collect tokens

	public class Toker
	{

		private TokenContext context_;       // holds single instance of all states and token source

		//----< initialize state machine >-------------------------------

		public Toker()
		{
			context_ = new TokenContext();      // context is the glue that holds all of the state machine parts 
		}
		//----< attempt to open source of tokens >-----------------------
		/*
         * If src is successfully opened, it uses TokenState.nextState(context_)
         * to set the initial state, based on the source content.
         */
		public bool open(string path)
		{
			TokenSourceFile src = new TokenSourceFile(context_);
			context_.src = src;
			return src.open(path);
		}
		//----< close source of tokens >---------------------------------

		public void close()
		{
			context_.src.close();
		}
		//----< extract a token from source >----------------------------

		private bool isWhiteSpaceToken(Token tok)
		{
			return (tok.Length > 0 && Char.IsWhiteSpace(tok[0]));
		}

		public Token getTok()
		{
			Token tok = null;
			while (!isDone())
			{
				tok = context_.currentState_.getTok();
				context_.currentState_ = TokenState.nextState(context_);
				if (!isWhiteSpaceToken(tok))
					break;
			}
			return tok;
		}
		//----< has Toker reached end of its source? >-------------------

		public bool isDone()
		{
			if (context_.currentState_ == null)
				return true;
			return context_.currentState_.isDone();
		}
		public int lineCount() { return context_.src.lineCount; }

		public List<Token> getToken()
		{
			List<Token> lst = new List<Token>();
			while (!isDone())
			{
				Token tok = getTok();
				Console.Write("\n -- line#{0, 4} : {1}", lineCount(), tok);
				lst.Add(tok);
			}
			//close();
			return lst;
		}
	}
	///////////////////////////////////////////////////////////////////
	// TokenContext class
	// - holds all the tokenizer states
	// - holds source of tokens
	// - internal qualification limits access to this assembly

	public class TokenContext
	{

		internal TokenContext()
		{
			ws_ = new WhiteSpaceState(this);
			ps_ = new PunctState(this);
			as_ = new AlphaState(this);
			dq_ = new DoubleQuote(this);
			dp_ = new DoublePunct(this);
			cp_ = new CppComment(this);
			cc_ = new SingleLineComment(this);
			sc_ = new SingleSpecial(this);
			sq_ = new SpecialPound(this);

			currentState_ = ws_;
		}
		internal WhiteSpaceState ws_ { get; set; }
		internal PunctState ps_ { get; set; }
		internal AlphaState as_ { get; set; }
		internal DoubleQuote dq_ { get; set; }
		internal DoublePunct dp_ { get; set; }
		internal CppComment cp_ { get; set; }
		internal SingleLineComment cc_ { get; set; }
		internal SingleSpecial sc_ { get; set; }



		internal SpecialPound sq_ { get; set; }
		// more states here

		internal TokenState currentState_ { get; set; }

		internal ITokenSource src { get; set; }  // can hold any derived class
		String temptoken = "";
		public string gettemptoken()
		{
			return temptoken;
		}
		public void settemptoken(string temptok)
		{
			this.temptoken = temptok;
		}
	}

	///////////////////////////////////////////////////////////////////
	// TokenState class
	// - base for all the tokenizer states

	public abstract class TokenState : ITokenState
	{

		internal TokenContext context_ { get; set; }  // derived classes store context ref here

		//----< delegate source opening to context's src >---------------

		public bool open(string path)
		{
			return context_.src.open(path);
		}
		//----< pass interface's requirement onto derived states >-------

		public abstract Token getTok();

		//----< derived states don't have to know about other states >---

		static public TokenState nextState(TokenContext context)
		{
			int nextItem = context.src.peek();
			if (nextItem < 0)
				return null;
			char ch = (char)nextItem;

			if (Char.IsWhiteSpace(ch))
				return context.ws_;
			if (Char.IsLetterOrDigit(ch))
				return context.as_;
			if (context.dq_.isDoubleQuote(nextItem))
				return context.dq_;
			if (context.dp_.isDoublePunct(nextItem))
				return context.dp_;
			if (context.cp_.isCppComment())
				return context.cp_;
			if (context.sc_.isSingleSpecial(nextItem))
				return context.sc_;
			if (context.cc_.isSingleLineComment(nextItem))
				return context.cc_;


			if (context.sq_.isSpecialPound(nextItem))
				return context.sq_;
			// Test for strings and comments here since we don't
			// want them classified as punctuators.

			// toker's definition of punctuation is anything that
			// is not whitespace and is not a letter or digit
			// Char.IsPunctuation is not inclusive enough

			return context.ps_;
		}
		//----< has tokenizer reached the end of its source? >-----------

		public bool isDone()
		{
			if (context_.src == null)
				return true;
			return context_.src.end();
		}
	}
	///////////////////////////////////////////////////////////////////
	// Derived State Classes
	/* - WhiteSpaceState          Token with space, tab, and newline chars
     * - AlphaNumState            Token with letters and digits
     * - PunctuationState         Token holding anything not included above
     * ----------------------------------------------------------------
     * - Each state class accepts a reference to the context in its
     *   constructor and saves in its inherited context_ property.
     * - It is only required to provide a getTok() method which
     *   returns a token conforming to its state, e.g., whitespace, ...
     * - getTok() assumes that the TokenSource's first character 
     *   matches its type e.g., whitespace char, ...
     * - The nextState() method ensures that the condition, above, is
     *   satisfied.
     * - The getTok() method promises not to extract characters from
     *   the TokenSource that belong to another state.
     * - These requirements lead us to depend heavily on peeking into
     *   the TokenSource's content.
     */
	///////////////////////////////////////////////////////////////////
	// WhiteSpaceState class
	// - extracts contiguous whitespace chars as a token
	// - will be thrown away by tokenizer

	public class WhiteSpaceState : TokenState
	{
		public WhiteSpaceState(TokenContext context)
		{
			context_ = context;
		}
		//----< manage converting extracted ints to chars >--------------

		bool isWhiteSpace(int i)
		{
			int nextItem = context_.src.peek();
			if (nextItem < 0)
				return false;
			char ch = (char)nextItem;
			return Char.IsWhiteSpace(ch);
		}
		//----< keep extracting until get none-whitespace >--------------

		override public Token getTok()
		{
			Token tok = new Token();
			tok.Append((char)context_.src.next());     // first is WhiteSpace

			while (isWhiteSpace(context_.src.peek()))  // stop when non-WhiteSpace
			{
				tok.Append((char)context_.src.next());
			}
			return tok;
		}
	}
	///////////////////////////////////////////////////////////////////
	// PunctState class
	// - extracts contiguous punctuation chars as a token

	public class PunctState : TokenState
	{
		public PunctState(TokenContext context)
		{
			context_ = context;
		}
		//----< manage converting extracted ints to chars >--------------

		bool isPunctuation(int i)
		{
			int nextItem = context_.src.peek();
			if (nextItem < 0)
				return false;
			char ch = (char)nextItem;
			return (!Char.IsWhiteSpace(ch) && !Char.IsLetterOrDigit(ch));
		}
		//----< keep extracting until get none-punctuator >--------------

		override public Token getTok()
		{
			Token tok = new Token();
			tok.Append((char)context_.src.next());       // first is punctuator

			while (isPunctuation(context_.src.peek()))   // stop when non-punctuator
			{
				tok.Append((char)context_.src.next());
			}
			return tok;
		}


	}
	///////////////////////////////////////////////////////////////////
	// AlphaState class
	// - extracts contiguous letter and digit chars as a token



	public class AlphaState : TokenState
	{
		public AlphaState(TokenContext context)
		{
			context_ = context;
		}
		//----< manage converting extracted ints to chars >--------------

		bool isLetterOrDigit(int i)
		{
			int nextItem = context_.src.peek();
			if (nextItem < 0)
				return false;
			char ch = (char)nextItem;
			return Char.IsLetterOrDigit(ch);
		}
		//----< keep extracting until get none-alpha >-------------------

		override public Token getTok()
		{
			Token tok = new Token();
			tok.Append((char)context_.src.next());          // first is alpha

			while (isLetterOrDigit(context_.src.peek()))    // stop when non-alpha
			{
				tok.Append((char)context_.src.next());
			}
			return tok;
		}
	}
	// Checks for double punctuation characters------------------------->
	public class DoublePunct : TokenState
	{
		public DoublePunct(TokenContext context)
		{
			context_ = context;
		}


		List<string> doublepunctlist = new List<string>() { ">>", "::", "++", "--", "==", "+=", "-=", "*=", "/=", "&&", "||", "<<" };
		public void setdoublepunctuation(string doublepunct)
		{
			doublepunctlist.Add(doublepunct);
		}
		public bool isDoublePunct(int i)
		{
			string temptoken = "";
			bool secondcheck = false;
			int nextvalue = context_.src.peek();

			if (nextvalue < 0)
				return false;
			char ch = (char)nextvalue;
			foreach (string dp in doublepunctlist)
			{
				if (ch.ToString().Equals(dp[0].ToString()))
				{
					secondcheck = true;
					temptoken += ch;
					break;
				}
			}
			if (secondcheck)
			{
				char second = (char)context_.src.peek(1);
				temptoken += second;

				if (doublepunctlist.Contains(temptoken.ToString()))
				{
					context_.src.next();
					context_.src.next();
					context_.settemptoken(temptoken);
					return true;
				}
			}
			return false;
		}

		override public Token getTok()
		{
			StringBuilder sbl = new StringBuilder();
			return sbl.Append(context_.gettemptoken());

		}
	}
	// Checks for Single Special Character----->
	public class SingleSpecial : TokenState
	{
		public SingleSpecial(TokenContext context)
		{
			context_ = context;
		}
		List<string> str1 = new List<string> { "*", ">", "[", "]", "(", ")", "{", "}", ":", "=", "+", "-", "<" };
		public void setsinglespecial(string singles)
		{
			str1.Add(singles);
		}
		public bool isSingleSpecial(int nextvalue)
		{
			nextvalue = context_.src.peek();
			char char1 = (char)nextvalue;
			if (str1.Contains(char1.ToString()))
			{
				return true;
			}
			return false;
		}
		override public Token getTok()
		{
			Token tok = new Token();
			tok.Append((char)context_.src.next());

			while (isSingleSpecial(context_.src.peek()))
			{
				tok.Append((char)context_.src.next());
			}
			return tok;
		}
	}
	// Checks for Single Line Comment------------------>
	public class SingleLineComment : TokenState
	{


		public SingleLineComment(TokenContext context)
		{
			context_ = context;
		}
		//----< manage converting extracted ints to chars >--------------
		public bool isSingleLineComment(int i)
		{
			string temptoken = "";
			char nexttoken;
			bool secondcheck = false;
			//temptoken += (char)i;
			//int nextItem = context_.src.peek();

			//if (nextItem < 0)
			//	return false;
			char ch1 = (char)i;

			if (ch1.ToString().Equals("/"))
			{
				secondcheck = true;
				temptoken += ch1;
			}

			if (secondcheck)
			{
				char second = (char)context_.src.peek(0);

				if (second.ToString().Equals("/"))
				{
					//context_.src.next();

					temptoken += (char)context_.src.next();


					do
					{
						nexttoken = (char)context_.src.peek();
						temptoken += (char)context_.src.next();
					} while (nexttoken.ToString() != "\n");
					context_.settemptoken(temptoken.ToString());
					return true;
				}
			}
			return false;
		}

		override public Token getTok()
		{
			StringBuilder Singlecomm = new StringBuilder();
			return Singlecomm.Append(context_.gettemptoken());
		}
	}

	// Checks for Multi Line Comments------->
	public class CppComment : TokenState
	{
		string temptok = "";
		public CppComment(TokenContext context)
		{
			context_ = context;
		}
		public bool isCppComment()
		{
			char nextItem = (char)context_.src.peek();
			if (nextItem.ToString() == "/")
			{
				temptok += (char)nextItem;
				context_.src.next();
				char nextItem1 = (char)context_.src.peek();
				if (nextItem1.ToString() == "*")
				{
					char tok;
					temptok += (char)nextItem1;
					context_.src.next();
					do
					{
						tok = (char)context_.src.peek();
						temptok += tok.ToString();
						context_.src.next();
					} while (!EndComment(tok));
					context_.settemptoken(temptok.ToString());
					return true;
				}
			}
			return false;
		}
		public bool EndComment(char tok)
		{
			if (tok.ToString() == "*")
			{
				char nextItem1 = (char)context_.src.peek();
				if (nextItem1.ToString() == "/")
				{
					temptok += (char)nextItem1;
					context_.src.next();
					return true;
				}
				return false;

			}
			return false;
		}
		override public Token getTok()
		{
			StringBuilder cppcomment = new StringBuilder();
			cppcomment.Append(temptok);
			return cppcomment;
		}
	}
	// Checks for String State-------------------------->
	public class DoubleQuote : TokenState
	{
		public DoubleQuote(TokenContext context)
		{
			context_ = context;
		}


		public bool isDoubleQuote(int i)
		{
			string temptoken = "";

			char nexttoken;
			int nextItem = context_.src.peek();

			if (nextItem < 0)
				return false;
			char ch = (char)nextItem;

			if (ch.ToString().Equals("\""))
			{
				temptoken += (char)context_.src.next();

				do
				{
					nexttoken = (char)context_.src.peek();
					temptoken += (char)context_.src.next();

				} while (nexttoken.ToString() != "\"");
				context_.settemptoken(temptoken.ToString());
				return true;
			}
			return false;
		}


		public override Token getTok()
		{
			StringBuilder doublequote = new StringBuilder();
			return doublequote.Append(context_.gettemptoken());
		}
	}
	// Checks for Pound Special Character --------------------->
	public class SpecialPound : TokenState
	{

		public SpecialPound(TokenContext context)
		{
			context_ = context;
		}
		//----< manage converting extracted ints to chars >--------------
		public bool isSpecialPound(int i)
		{
			string temptoken = "";
			char nexttoken;
			int nextItem = context_.src.peek();
			char nextval = (char)nextItem;
			if (nextval == '#')
			{
				temptoken += nextval;
				context_.src.next();

				do
				{
					nexttoken = (char)context_.src.peek();
					temptoken += (char)context_.src.next();
				} while (nexttoken.ToString() != "\n");
				context_.settemptoken(temptoken.ToString());
				return true;
			}
			return false;
		}

		override public StringBuilder getTok()
		{
			StringBuilder SpecialPound = new StringBuilder();
			return SpecialPound.Append(context_.gettemptoken());

		}
	}


	///////////////////////////////////////////////////////////////////
	// TokenSourceFile class
	// - extracts integers from token source
	// - Streams often use terminators that can't be represented by
	//   a character, so we collect all elements as ints
	// - keeps track of the line number where a token is found
	// - uses StreamReader which correctly handles byte order mark
	//   characters and alternate text encodings.

	public class TokenSourceFile : ITokenSource
	{
		public int lineCount { get; set; } = 1;
		private System.IO.StreamReader fs_;           // physical source of text
		private List<int> charQ_ = new List<int>();   // enqueing ints but using as chars
		private TokenContext context_;

		public TokenSourceFile(TokenContext context)
		{
			context_ = context;
		}
		//----< attempt to open file with a System.IO.StreamReader >-----

		public bool open(string path)
		{
			try
			{
				fs_ = new System.IO.StreamReader(path, true);
				context_.currentState_ = TokenState.nextState(context_);
			}
			catch (Exception ex)
			{
				Console.Write("\n  {0}\n", ex.Message);
				return false;
			}
			return true;
		}
		//----< close file >---------------------------------------------

		public void close()
		{
			fs_.Close();
		}
		//----< extract the next available integer >---------------------
		/*
		 *  - checks to see if previously enqueued peeked ints are available
		 *  - if not, reads from stream
		 */
		public int next()
		{
			int ch;
			if (charQ_.Count == 0)  // no saved peeked ints
			{
				if (end())
					return -1;
				ch = fs_.Read();
			}
			else                    // has saved peeked ints, so use the first
			{
				ch = charQ_[0];
				charQ_.Remove(ch);
			}
			if ((char)ch == '\n')   // track the number of newlines seen so far
				++lineCount;
			return ch;
		}
		//----< peek n ints into source without extracting them >--------
		/*
		 *  - This is an organizing prinicple that makes tokenizing easier
		 *  - We enqueue because file streams only allow peeking at the first int
		 *    and even that isn't always reliable if an error occurred.
		 *  - When we look for two punctuator tokens, like ==, !=, etc. we want
		 *    to detect their presence without removing them from the stream.
		 *    Doing that is a small part of your work on this project.
		 */
		public int peek(int n = 0)
		{
			if (n < charQ_.Count)  // already peeked, so return
			{
				return charQ_[n];
			}
			else                  // nth int not yet peeked
			{
				for (int i = charQ_.Count; i <= n; ++i)
				{
					if (end())
						return -1;
					charQ_.Add(fs_.Read());  // read and enqueue
				}
				return charQ_[n];   // now return the last peeked
			}
		}
		//----< reached the end of the file stream? >--------------------

		public bool end()
		{
			return fs_.EndOfStream;
		}
	}

#if (TEST_TOKER)

	class DemoToker
	{
		static bool testToker(string path)
		{
			Toker toker = new Toker();

			string fqf = System.IO.Path.GetFullPath(path);
			if (!toker.open(fqf))
			{
				Console.Write("\n can't open {0}\n", fqf);
				return false;
			}
			else
			{
				Console.Write("\n  processing file: {0}", fqf);
			}
			while (!toker.isDone())
			{
				Token tok = toker.getTok();
				Console.Write("\n -- line#{0, 4} : {1}", toker.lineCount(), tok);
			}
			toker.close();
			return true;
		}
		static void Main(string[] args)
		{
			Console.Write("\n  Demonstrate Toker class");
			Console.Write("\n =========================");

			StringBuilder msg = new StringBuilder();
			msg.Append("\n  Some things this demo does not do for CSE681 Project #2:");
			msg.Append("\n  - collect comments as tokens");
			msg.Append("\n  - collect double quoted strings as tokens");
			msg.Append("\n  - collect single quoted strings as tokens");
			msg.Append("\n  - collect specified single characters as tokens");
			msg.Append("\n  - collect specified character pairs as tokens");
			msg.Append("\n  - integrate with a SemiExpression collector");
			msg.Append("\n  - provide the required package structure");
			msg.Append("\n");

			Console.Write(msg);

			testToker("../../../TextFile2.txt");


			Console.Write("\n\n");
			Console.Read();
		}
	}
}

#endif
