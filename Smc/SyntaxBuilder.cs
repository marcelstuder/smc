namespace Smc
{
    public class SyntaxBuilder : IBuilder
    {
        public void SetName(string name)
        {
            throw new System.NotImplementedException();
        }

        public void SyntaxError(int line, int pos)
        {
            throw new System.NotImplementedException();
        }

        public void NewHeaderWithName()
        {
            throw new System.NotImplementedException();
        }

        public void AddHeaderWithValue()
        {
            throw new System.NotImplementedException();
        }

        public void SetStateName()
        {
            throw new System.NotImplementedException();
        }

        public void Done()
        {
            throw new System.NotImplementedException();
        }

        public void SetSuperState()
        {
            throw new System.NotImplementedException();
        }

        public void SetEvent()
        {
            throw new System.NotImplementedException();
        }

        public void AddAction()
        {
            throw new System.NotImplementedException();
        }

        public void TransitionWithActions()
        {
            throw new System.NotImplementedException();
        }

        public void SetNullEvent()
        {
            throw new System.NotImplementedException();
        }

        public void SetNextState()
        {
            throw new System.NotImplementedException();
        }

        public void SetNullNextState()
        {
            throw new System.NotImplementedException();
        }

        public void TransitionWithAction()
        {
            throw new System.NotImplementedException();
        }

        public void TransitionNullAction()
        {
            throw new System.NotImplementedException();
        }

        public void HeaderError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos)
        {
            throw new System.NotImplementedException();
        }

        public void StateSpecError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos)
        {
            throw new System.NotImplementedException();
        }

        public void TransitionError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos)
        {
            throw new System.NotImplementedException();
        }

        public void TransitionGroupError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos)
        {
            throw new System.NotImplementedException();
        }

        public void EndError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos)
        {
            throw new System.NotImplementedException();
        }
    }
}