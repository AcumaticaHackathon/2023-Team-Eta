using ACPROpenAI.DAC.OpenAIConnectionPreferences;
using ACPROpenAI.Descriptor;
using ACPROpenAI.Graph;
using Customization;
using PX.Data;
using System;
using System.Linq;

namespace ACPROpenAI.Plugin
{
    public class ACPRPlugin : CustomizationPlugin
    {
        public override void OnPublished()
        {

        }

        public override void UpdateDatabase()
        {
            WriteLog(ACPRMessages.PluginStart);

            #region OpenAIConnectionPreferences Defaulting
            OpenAIConnectionPreferencesDefaulting();
            #endregion

            WriteLog(ACPRMessages.PluginEnd);
        }

        private void OpenAIConnectionPreferencesDefaulting()
        {
            try
            {
                var setupMaint = PXGraph.CreateInstance<ACPRSetupMaint>();
                var setup = PXSelect<ACPRSetup>.Select(setupMaint).RowCast<ACPRSetup>();
                if (!setup.Any())
                {
                    WriteLog(ACPRMessages.PluginCreateConnectionPref);
                    setupMaint.Setup.Insert(new ACPRSetup()
                    {
                        APIKey = string.Empty,
                        EndPoint = ACPRConstants.ACPREndPoint.Completions
                    });
                    setupMaint.Save.PressButton();
                    WriteLog(ACPRMessages.PluginCreateConnectionPrefSuccess);
                }
            }
            catch (Exception ex)
            {
                WriteLog(string.Format(ACPRMessages.PluginCreateConnectionPrefError, ex.Message));
            }
        }
    }
}
