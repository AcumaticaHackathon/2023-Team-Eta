using ACPROpenAI.DAC.OpenAIConnectionPreferences;
using ACPROpenAI.Descriptor;
using ACPROpenAI.Services.Abstractions;
using PX.Common;
using PX.Data;
using PX.Data.WorkflowAPI;
using System.Collections;

namespace ACPROpenAI.Graph
{
    public class ACPRSetupMaint : PXGraph<ACPRSetupMaint>
    {
        #region DataView
        public PXSelect<ACPRSetup> Setup;

        public PXFilter<ACPRTestConnection> TestView;
        #endregion

        #region Dependency Injection
        [InjectDependency]
        public IACPROpenAIRestService _openAIRestService { get; set; }
        #endregion

        #region Actions
        public PXSave<ACPRSetup> Save;
        public PXCancel<ACPRSetup> Cancel;

        public PXAction<ACPRSetup> TestConnection;
        [PXUIField(DisplayName = "Test Connection", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        [PXButton(CommitChanges = true, Connotation = ActionConnotation.None, DisplayOnMainToolbar = true)]
        public virtual IEnumerable testConnection(PXAdapter adapter)
        {
            var requestResult = _openAIRestService.RequestToAI(ACPRMessages.TestRequestText);
            if (requestResult.Choices != null)
            {
                TestView.Current.Prompt = ACPRMessages.TestRequestText;
                TestView.Current.Answer = $"[{PXTimeZoneInfo.Now}]: " + requestResult.Choices[0].Text.TrimStart('\n', '\n');
                TestView.AskExt();
            }
            
            return adapter.Get();
        }

        public PXAction<ACPRTestConnection> SendRequest;
        [PXUIField(DisplayName = "Send", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        [PXButton(CommitChanges = true)]
        public virtual IEnumerable sendRequest(PXAdapter adapter)
        {
            var currentTestView = TestView.Current;
            if (currentTestView?.Prompt != null)
            {
                var requestResult = _openAIRestService.RequestToAI(currentTestView?.Prompt);
                TestView.Current.Answer += "\n";
                TestView.Current.Answer += $"[{PXTimeZoneInfo.Now}]: " + requestResult.Choices[0].Text.TrimStart('\n', '\n');
            }

            return adapter.Get();
        }
        #endregion
    }
}
