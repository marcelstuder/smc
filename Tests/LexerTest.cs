using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smc;

namespace Tests
{
    [TestClass]
    public class LexerTest : ITokenCollector
    {
        private string _tokens = "";
        private Lexer _lexer;
        private bool _firstToken = true;

        [TestInitialize]
        public void SetUp()
        {
            _lexer = new Lexer(this);
        }

        [TestMethod]
        public void FindsOpenBrace()
        {
            AssertLexResult("{", "OB");
        }

        [TestMethod]
        public void FindsClosedBrace()
        {
            AssertLexResult("}", "CB");
        }

        [TestMethod]
        public void FindsOpenParen()
        {
            AssertLexResult("(", "OP");
        }

        [TestMethod]
        public void FindsClosedParen()
        {
            AssertLexResult(")", "CP");
        }

        [TestMethod]
        public void FindsOpenAngle()
        {
            AssertLexResult("<", "OA");
        }

        [TestMethod]
        public void FindsClosedAngle()
        {
            AssertLexResult(">", "CA");
        }

        [TestMethod]
        public void FindsDash()
        {
            AssertLexResult("-", "D");
        }

        [TestMethod]
        public void FindsColon()
        {
            AssertLexResult(":", "C");
        }

        [TestMethod]
        public void FindsSimpleName()
        {
            AssertLexResult("blah", "#blah#");
        }

        [TestMethod]
        public void FindsComplexName()
        {
            AssertLexResult("blah_666", "#blah_666#");
        }

        [TestMethod]
        public void Error()
        {
            AssertLexResult(".", "E1/1");
        }

        [TestMethod]
        public void NothingButWhiteSpace()
        {
            AssertLexResult(" ", "");
        }

        [TestMethod]
        public void WhiteSpaceBefore()
        {
            AssertLexResult("     \t\n  -", "D");
        }

        [TestMethod]
        public void SimpleSequence()
        {
            AssertLexResult("{}", "OB,CB");   
        }

        [TestMethod]
        public void ComplexSequence()
        {
            AssertLexResult("FSM:fsm{this}", "#FSM#,C,#fsm#,OB,#this#,CB");
        }

        [TestMethod]
        public void AllTokens()
        {
            AssertLexResult("{}()<>-: name .", "OB,CB,OP,CP,OA,CA,D,C,#name#,E1/15");
        }

        [TestMethod]
        public void MultipleLines()
        {
            AssertLexResult("FSM:fsm.\n{bob-.}", "#FSM#,C,#fsm#,E1/8,OB,#bob#,D,E2/6,CB");
        }

        private void AddToken(string token)
        {
            if (!_firstToken)
                _tokens += ",";
            _tokens += token;
            _firstToken = false;
        }

        private void AssertLexResult(string input, string expected)
        {
            _lexer.Lex(input);
            Assert.AreEqual(expected, _tokens);
        }

        public void OpenBrace(int line, int pos)
        {
            AddToken("OB");
        }

        public void ClosedBrace(int line, int pos)
        {
            AddToken("CB");
        }

        public void OpenParen(int line, int pos)
        {
            AddToken("OP");
        }

        public void ClosedParen(int line, int pos)
        {
            AddToken("CP");
        }

        public void OpenAngle(int line, int pos)
        {
            AddToken("OA");
        }

        public void ClosedAngle(int line, int pos)
        {
            AddToken("CA");
        }

        public void Dash(int line, int pos)
        {
            AddToken("D");
        }

        public void Colon(int line, int pos)
        {
            AddToken("C");
        }

        public void Name(string name, int line, int pos)
        {
            AddToken("#" + name + "#");
        }

        public void Error(int line, int pos)
        {
            AddToken("E" + line + "/" + pos);
        }
    }
}