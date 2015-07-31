namespace Smc
{
    public interface ITokenCollector
    {
        void OpenBrace(int line, int pos);
        void ClosedBrace(int line, int pos);
        void OpenParen(int line, int pos);
        void ClosedParen(int line, int pos);
        void OpenAngle(int line, int pos);
        void ClosedAngle(int line, int pos);
        void Dash(int line, int pos);
        void Colon(int line, int pos);
        void Name(string name, int line, int pos);
        void Error(int line, int pos);
    }
}