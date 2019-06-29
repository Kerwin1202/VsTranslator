using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Translate.Settings;

namespace Visual_Studio_2019_Translator.Adornment.Translate
{
    /// <summary>
    /// how to create this ,you can see https://msdn.microsoft.com/en-us/library/cc138567.aspx
    /// </summary>
    [Guid("43820229-bf50-488c-9566-b4d5ca678c6d")]
    public class TranslateClient : ToolWindowPane
    {
        public TranslateClient() : base(null)
        {
            this.Caption = "Translate Client";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new TranslateClientControl(OptionsSettings.Settings);

        }
    }
}
