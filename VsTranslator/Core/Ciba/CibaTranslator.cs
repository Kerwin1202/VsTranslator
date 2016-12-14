namespace VsTranslator.Core.Ciba
{
    public class CibaTranslator
    {
        private CibaTranslator()
        {
            
        }

        private const string TranslateUrl = "http://dict-co.iciba.com/api/dictionary.php";

        private string _appkey;

        public CibaTranslator(string appkey)
        {
            _appkey = appkey;
            //413485D0866205071585A258E7DFCA90
        }


        public void Translate(string text)
        {
            
        }
    }
}