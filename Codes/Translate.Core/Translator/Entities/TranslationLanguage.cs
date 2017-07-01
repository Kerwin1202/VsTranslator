namespace Translate.Core.Translator.Entities
{
	public class TranslationLanguage
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public TranslationLanguage(string code, string name)
		{
			Code = code;
			Name = name;
		}

	    public override string ToString()
	    {
	        return Name;
	    }
	}
}
