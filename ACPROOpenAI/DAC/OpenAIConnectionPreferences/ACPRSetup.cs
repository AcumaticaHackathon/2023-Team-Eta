using ACPROpenAI.Descriptor.Attributes;
using PX.Data;
using System;

namespace ACPROpenAI.DAC.OpenAIConnectionPreferences
{
    [Serializable]
    [PXCacheName(SetupCacheName)]
    public class ACPRSetup : ACPRBaseEntity, IBqlTable
    {
        private const string SetupCacheName = "Open AI Preferences";

        #region APIKey
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = "API Key")]
        public virtual string APIKey { get; set; }
        public abstract class aPIKey : PX.Data.BQL.BqlString.Field<aPIKey> { }
        #endregion

        #region EndPoint
        [PXDBString(255, IsUnicode = true)]
        [PXUIField(DisplayName = "End Point URL", Enabled = false)]
        public virtual string EndPoint { get; set; }
        public abstract class endPoint : PX.Data.BQL.BqlString.Field<endPoint> { }
        #endregion

        #region Model
        [PXDBString(255, IsUnicode = true)]
        [PXDefault(typeof(ACPRAiModel.textDavinci003), PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [ACPRAiModel.List]
        [PXUIField(DisplayName = "Model")]
        public virtual string Model { get; set; }
        public abstract class model : PX.Data.BQL.BqlString.Field<model> { }
        #endregion

        #region Temperature
        [PXDBDecimal(2, MinValue = 0, MaxValue = 1)]
        [PXDefault(TypeCode.Decimal, "0.5", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Temperature")]
        public virtual decimal? Temperature { get; set; }
        public abstract class temperature : PX.Data.BQL.BqlDecimal.Field<temperature> { }
        #endregion

        #region MaxTokens
        [PXDBInt(MinValue = 0, MaxValue = 4000)]
        [PXDefault(TypeCode.Int32, "150", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Max Tokens")]
        public virtual int? MaxTokens { get; set; }
        public abstract class maxTokens : PX.Data.BQL.BqlInt.Field<maxTokens> { }
        #endregion

        #region TopP
        [PXDBDecimal(2, MinValue = 0, MaxValue = 1)]
        [PXDefault(TypeCode.Decimal, "0", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Top P")]
        public virtual decimal? TopP { get; set; }
        public abstract class topP : PX.Data.BQL.BqlDecimal.Field<topP> { }
        #endregion

        #region FrequencyPenalty
        [PXDBDecimal(2, MinValue = 0, MaxValue = 2)]
        [PXDefault(TypeCode.Decimal, "0", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Frequency Penalty")]
        public virtual decimal? FrequencyPenalty { get; set; }
        public abstract class frequencyPenalty : PX.Data.BQL.BqlDecimal.Field<frequencyPenalty> { }
        #endregion
        
        #region PresencePenalty
        [PXDBDecimal(2, MinValue = 0, MaxValue = 2)]
        [PXDefault(TypeCode.Decimal, "0", PersistingCheck = PXPersistingCheck.NullOrBlank)]
        [PXUIField(DisplayName = "Presence Penalty")]
        public virtual decimal? PresencePenalty { get; set; }
        public abstract class presencePenalty : PX.Data.BQL.BqlDecimal.Field<presencePenalty> { }
        #endregion

        #region NoteID
        [PXNote]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    }
}
