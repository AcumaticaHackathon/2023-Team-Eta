using ACPROpenAI.Descriptor.Views;
using ACPROpenAI.GraphExtensions.CR.DAC;
using ACPROpenAI.GraphExtensions.CR.DAC.Projection;
using ACPROpenAI.Helpers.Extensions;
using ACPROpenAI.Models;
using ACPROpenAI.Services.Abstractions;
using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.SO;
using PX.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;
using PX.Data.Licensing;
using Contact = PX.Objects.CR.Contact;
using System;

namespace ACPROpenAI.GraphExtensions.CR
{
    public class ACPRCRCaseMaintExt : PXGraphExtension<CRCaseMaint>
    {
        public static bool IsActive()
        {
            return PXAccess.FeatureInstalled<FeaturesSet.customerModule>();
        }

        #region Properties
        private ACPRCRSetupExt aCPRCRSetupExt;
        #endregion

        #region DataViews
        [CRReference(typeof(CRCase.customerID), typeof(CRCase.contactID))]
        public ACPRCRActivityListView<CRCase> ACPRCRActivity;
        #endregion

        #region Dependency Injection
        [InjectDependency]
        public IACPROpenAIRestService _openAIRestService { get; set; }
        #endregion

        #region Init
        public override void Initialize()
        {
            base.Initialize();
            if (Base.Setup.Current != null)
            {
                aCPRCRSetupExt = PXCache<CRSetup>.GetExtension<ACPRCRSetupExt>(Base.Setup.Current);
            }
        }
        #endregion

        #region Events
        public virtual void __(Events.RowSelected<CRCase> e)
        {
            if(e.Row != null)
            {
                CRCase row = e.Row as CRCase;
                if(row != null)
                {
                    ACPRGenerateAIResponse.SetEnabled(row.Status == "N" || row.Status == "O");
                }
            }
        }
        #endregion

        #region Methods
        private static void GenerateAIResponse(CRCase crCase)
        {
            CRCaseMaint graph = PXGraph.CreateInstance<CRCaseMaint>();
            ACPRCRCaseMaintExt graphExt = graph.GetExtension<ACPRCRCaseMaintExt>();

            graph.Case.Update(crCase);

            if (!string.IsNullOrEmpty(crCase.Description))
            {
                ResponseModel requestResult = graphExt._openAIRestService.RequestToAI(graph.Case.Current?.Description.ToPlainText());
                if (requestResult.Choices != null)
                {
                    ACPRCRCaseExt caseExt = PXCache<CRCase>.GetExtension<ACPRCRCaseExt>(graph.Case.Current);
                    caseExt.UsrACPRAIAnswer = requestResult.Choices[0].Text;

                    crCase.IsActive = true;
                    crCase.Resolution = "CR";
                    crCase.ResolutionDate = null;
                    crCase.Status = "P";
                    crCase.StatusDate = DateTime.Now;

                    _ = graph.Case.Update(crCase);

                    graph.Actions.PressSave();
                }
            }
        }
        #endregion

        #region Actions
        public PXAction<CRCase> ACPRGenerateAIResponse;
        [PXUIField(DisplayName = "Generate AI Response", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update, Visible = true)]
        [PXButton(CommitChanges = true, Connotation = ActionConnotation.None, DisplayOnMainToolbar = true)]
        public virtual IEnumerable aCPRGenerateAIResponse(PXAdapter adapter)
        {
            CRCase cRCase = Base.Case.Current;
            if(cRCase != null)
            {
                PXLongOperation.StartOperation(this.Base, () => GenerateAIResponse(cRCase));
            }
            return adapter.Get();
        }
        #endregion
    }
}