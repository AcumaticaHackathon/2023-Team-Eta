using PX.Common;

namespace ACPROpenAI.Descriptor
{
    [PXLocalizable]
    public class ACPRMessages
    {
        #region Plugin
        public const string PluginStart = "INFO:......Acupower OpenAI Plugin start working...";
        public const string PluginEnd = "INFO:......Acupower OpenAI Plugin work completed.";

        public const string PluginCreateConnectionPref = "INFO:......Creating Connection preferences...";
        public const string PluginCreateConnectionPrefSuccess = "SUCCESS:...Connection Preferences created successfully!";
        public const string PluginCreateConnectionPrefError = "ERROR:.....{0}";
        #endregion

        #region StatusCode
        public const string StatusCodeError = "Error: Received a {0} status code. Content: {1}";
        public const string RemoteServerError = "The remote server returned an error. For more details open trace.";
        #endregion

        #region TestRequest
        public const string TestRequestText = "Answer to the Ultimate Question of Life, the Universe, and Everything";
        #endregion

        #region Warnings
        public const string Limitations = "May occasionally generate incorrect information";
        #endregion
    }
}
