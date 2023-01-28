using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;

namespace ACPROpenAI.GraphExtensions.CR.DAC
{
    public sealed class ACPRCRCaseExt : PXCacheExtension<CRCase>
    {
        public static bool IsActive()
        {
            return PXAccess.FeatureInstalled<FeaturesSet.customerModule>();
        }

        #region UsrACPRAIAnswer
        [PXDBString(4000, IsUnicode = true)]
        [PXUIField(DisplayName = "AI Answer")]
        public string UsrACPRAIAnswer { get; set; }
        public abstract class usrACPRAIAnswer : PX.Data.BQL.BqlString.Field<usrACPRAIAnswer> { }
        #endregion
    }
}
