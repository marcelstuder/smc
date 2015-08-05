namespace Smc
{
    public interface IBuilder
    {
        void SetName(string name);
        void SyntaxError(int line, int pos);
        void NewHeaderWithName();
        void AddHeaderWithValue();
        void SetStateName();
        void Done();
        void SetSuperState();
        void SetEvent();
        void AddAction();
        void TransitionWithActions();
        void SetNullEvent();
        void SetNextState();
        void SetNullNextState();
        void TransitionWithAction();
        void TransitionNullAction();
        void HeaderError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos);
        void StateSpecError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos);
        void TransitionError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos);
        void TransitionGroupError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos);
        void EndError(Parser.ParserState state, Parser.ParserEvent parserEvent, int line, int pos);
    }
}