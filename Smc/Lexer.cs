using System.Text.RegularExpressions;

namespace Smc
{
    public class Lexer
    {
        private readonly ITokenCollector _collector;
        private int _lineNumber;
        private int _position;

        public Lexer(ITokenCollector collector)
        {
            _collector = collector;
        }

        public void Lex(string input)
        {
            _lineNumber = 1;
            var lines = input.Split('\n');
            foreach (var l in lines)
            {
                LexLine(l);
                _lineNumber++;
            }
        }

        private void LexLine(string line)
        {
            for (_position = 0; _position < line.Length;)
                LexToken(line);
        }

        private void LexToken(string line)
        {
            if (!FindToken(line))
            {
                _collector.Error(_lineNumber, _position + 1);
                _position += 1;
            }
        }

        private bool FindToken(string line)
        {
            return 
                FindWhiteSpace(line) || 
                    FindSingleCharacterToken(line) || 
                        FindName(line);
        }

        private static readonly Regex WhitespacePattern = new Regex("^\\s+");

        private bool FindWhiteSpace(string line)
        {
            var match = WhitespacePattern.Match(line.Substring(_position));
            if (match.Success)
            {
                _position += match.Length;
                return true;
            }

            return false;
        }

        private bool FindSingleCharacterToken(string line)
        {
            var c = line.Substring(_position, 1);

            switch (c)
            {
                case "{":
                    _collector.OpenBrace(_lineNumber, _position);
                    break;
                case "}":
                    _collector.ClosedBrace(_lineNumber, _position);
                    break;
                case "(":
                    _collector.OpenParen(_lineNumber, _position);
                    break;
                case ")":
                    _collector.ClosedParen(_lineNumber, _position);
                    break;
                case "<":
                    _collector.OpenAngle(_lineNumber, _position);
                    break;
                case ">":
                    _collector.ClosedAngle(_lineNumber, _position);
                    break;
                case "-":
                    _collector.Dash(_lineNumber, _position);
                    break;
                case ":":
                    _collector.Colon(_lineNumber, _position);
                    break;
                default:
                    return false;
            }

            _position += 1;
            return true;
        }

        private static readonly Regex NamePattern = new Regex("^\\w+");

        private bool FindName(string line)
        {
            var match = NamePattern.Match(line.Substring(_position));
            if (match.Success)
            {
                _collector.Name(match.Value, _lineNumber, _position);
                _position += match.Length;
                return true;
            }

            return false;
        }
    }
}