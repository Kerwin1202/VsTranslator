namespace VsTranslator.Core
{
    public interface ITranslator
    {
        string Name { get; }

        string Description { get;}

        string Translate(string text,string from,string to);
    }
}