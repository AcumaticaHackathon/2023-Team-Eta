using PX.Common;

namespace ACPROpenAI.Descriptor
{
    [PXLocalizable]
    public class ACPRConstants
    {
        [PXLocalizable]
        public class ACPREndPoint
        {
            public const string Completions = "https://api.openai.com/v1/completions";
            public const string Models = "https://api.openai.com/v1/models";
            public const string Edits = "https://api.openai.com/v1/edits";
        }

        #region Subscribers
        public const string AIEmailNotification = "AI Email Notification";
        public const string AIEmailType = "AIET";
        public const string AIActionName = "aCPRCreateAIEmail";
        #endregion
    }
}
