using ACPROpenAI.Descriptor.Views;
using ACPROpenAI.GraphExtensions.CR.DAC;
using ACPROpenAI.Helpers.Extensions;
using ACPROpenAI.Models;
using ACPROpenAI.Services.Abstractions;
using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.SO;
using PX.Objects.IN;
using PX.Web.UI;
using System.Collections;
using System;
using ACPROpenAI.GraphExtensions.CR.DAC.Projection;
using System.Collections.Generic;
using System.Linq;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Objects.AR;
using System.Text;

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

        public PXSelect<ACPROrderListProj> ACPROrderListProj;
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
                var selectedRows = graphExt.ACPROrderListProj.Select().RowCast<ACPROrderListProj>().
                    OrderByDescending(a => a.OrderDate).Take(10).ToList();
                var items = new List<InventoryItem>();

                var ordersForRequest = new List<SOOrder>();
                var details = new List<SOLine>();

                foreach (var row in selectedRows)
                {
                    PXSelect<
                        SOLine,
                        Where<SOLine.orderType, Equal<Required<SOLine.orderType>>,
                            And<SOLine.orderNbr, Equal<Required<SOLine.orderNbr>>>>>
                        .Select(graph, row.OrderType, row.OrderNbr).RowCast<SOLine>().ToList().ForEach(_ =>
                        {
                            var inventoryItem = PXSelect<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(graph, _.InventoryID);
                            items.Add(inventoryItem);

                            details.Add(_);
                        });

                    var ord = SelectFrom<SOOrder>.Where<SOOrder.orderNbr.IsEqual<@P.AsString>.And<SOOrder.orderType.IsEqual<@P.AsString>>>.View.Select(graph,
                                  row.OrderNbr, row.OrderType);

                    ordersForRequest.Add(ord);

                }
                var customer = SelectFrom<Customer>.Where<Customer.bAccountID.IsEqual<@P.AsInt>>.View
                    .Select(graph, graph.Case.Current.ContactID).FirstOrDefault();

                var sb = new StringBuilder();

                details.ForEach(a =>
                {
                    string lineDetails = $" Branch: {a.BranchID}, inventoryid: \"{a.InventoryID}\", LineDescription: \"{a.TranDesc}\", Quantity: {a.Qty}, " +
                                         $"OrderType: {a.OrderType}, OrderNbr: {a.OrderNbr} ";
                    sb.Append(lineDetails);
                });

                ordersForRequest.ForEach(_ =>
                {
                    sb.Append("Order: " + _.OrderNbr + " status: " + _.Status + " date: " + _.OrderDate + " Description: " + _.OrderDesc);
                });

                if (graph.Case.Current.ContactID != null)
                {
                    var contact = PXSelect<Contact, Where<Contact.contactID, Equal<Required<Contact.contactID>>>>.Select(graph, graph.Case.Current.ContactID).FirstTableItems.FirstOrDefault();
                    sb.Append("Contact: " + contact.FirstName + " " + contact.LastName + "\n");
                }
                string orders = string.Empty;
                items.ForEach(_ =>
                {
                    orders += _.InventoryCD + " " + _.Descr + ", ";
                });
                sb.Append($"Customer Orderer: {orders}\n");
                sb.Append($"Description: {graph.Case.Current.Description.ToPlainText()}\n");

                ResponseModel requestResult = graphExt._openAIRestService.RequestToAI(sb.ToString());
                if (requestResult.Choices != null)
                {
                    ACPRCRCaseExt caseExt = PXCache<CRCase>.GetExtension<ACPRCRCaseExt>(graph.Case.Current);
                    caseExt.UsrACPRAIAnswer = requestResult.Choices[0].Text;

                    ACPRCRActivityListView<CRCase> aCPRCRActivity = new ACPRCRActivityListView<CRCase>(graph);
                    aCPRCRActivity.MailBody = requestResult.Choices[0].Text;
                    aCPRCRActivity.DefaultSubject = graph.Activities.DefaultSubject;
                    aCPRCRActivity.GetNewEmailAddress = graph.Activities.GetNewEmailAddress;
                    var result = aCPRCRActivity.CreateNewEmailActivity();
                    result.Item2.Send.PressButton();

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