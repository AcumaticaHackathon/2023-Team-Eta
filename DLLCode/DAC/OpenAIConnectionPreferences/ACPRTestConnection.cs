using PX.Data;
using System;

namespace ACPROpenAI.DAC.OpenAIConnectionPreferences
{
    [Serializable]
    [PXCacheName(TestConnectionChacheName)]
    public class ACPRTestConnection : IBqlTable
    {
        private const string TestConnectionChacheName = "Test Connection";

        #region Prompt
        [PXString(255, IsUnicode = true)]
        [PXUIField(DisplayName = "Prompt")]
        public virtual string Prompt { get; set; }
        public abstract class prompt : IBqlField { }
        #endregion

        #region Answer
        [PXString(4000, IsUnicode = true)]
        [PXUIField(DisplayName = "Answer", Enabled = false)]
        public virtual string Answer { get; set; }
        public abstract class answer : IBqlField { }
        #endregion
    }
}
