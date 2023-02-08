using ACPROpenAI.Descriptor;
using ACPROpenAI.GraphExtensions.CR.DAC;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;

namespace ACPROpenAI.GraphExtensions.CR
{
    public class ACPRCRSetupMaintExt : PXGraphExtension<CRSetupMaint>
    {
        public static bool IsActive()
        {
            return PXAccess.FeatureInstalled<FeaturesSet.customerModule>();
        }

        #region Event Handlers
        protected virtual void _(Events.RowSelected<CRSetup> args)
        {
            if (args.Row is CRSetup row)
            {
                var rowExt = PXCache<CRSetup>.GetExtension<ACPRCRSetupExt>(row);
                if (rowExt.UsrACPRAllowAutoGenerateEMail == true && rowExt.UsrACPRAllowAutoSendEMail == true)
                {
                    args.Cache.RaiseExceptionHandling<ACPRCRSetupExt.usrACPRAllowAutoSendEMail>(row, rowExt.UsrACPRAllowAutoSendEMail, new PXSetPropertyException(ACPRMessages.Limitations, PXErrorLevel.RowWarning));
                }
            }
        }
        #endregion
    }
}
