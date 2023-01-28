using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;

namespace ACPROpenAI.GraphExtensions.CR.DAC
{
    public sealed class ACPRCRSetupExt : PXCacheExtension<CRSetup>
    {
        public static bool IsActive()
        {
            return PXAccess.FeatureInstalled<FeaturesSet.customerModule>();
        }

        #region UsrACPRAllowAutoGenerateEMail
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Automatically Generate Email")]
        public bool? UsrACPRAllowAutoGenerateEMail { get; set; }
        public abstract class usrACPRAllowAutoGenerateEMail : PX.Data.BQL.BqlBool.Field<usrACPRAllowAutoGenerateEMail> { }
        #endregion

        #region UsrACPRAllowAutoSendEMail
        [PXDBBool()]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Automatically Send Generated Email")]
        [PXUIEnabled(typeof(ACPRCRSetupExt.usrACPRAllowAutoGenerateEMail))]
        [PXFormula(typeof(Where<ACPRCRSetupExt.usrACPRAllowAutoGenerateEMail, Equal<boolTrue>>))]
        public bool? UsrACPRAllowAutoSendEMail { get; set; }
        public abstract class usrACPRAllowAutoSendEMail : PX.Data.BQL.BqlBool.Field<usrACPRAllowAutoSendEMail> { }
        #endregion
    }
}
