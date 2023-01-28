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

        //TODO Clarify this part of the implementation.
        //IMPORTANT!!! This is an experimental implementation of a custom action that allows creating response buy action
        //It should be clarified with the product owner

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

        #region Override
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            if (Base.Case.Current?.Description != null)
            {
                var requestResult = _openAIRestService.RequestToAI(Base.Case.Current?.Description.ToPlainText());
                if (requestResult.Choices != null)
                {
                    var caseExt = PXCache<CRCase>.GetExtension<ACPRCRCaseExt>(Base.Case.Current);
                    caseExt.UsrACPRAIAnswer = requestResult.Choices[0].Text;
                }
            }

            baseMethod();
        }
        #endregion

        #region Actions
        //TODO Clarify this part of the implementation.
        //IMPORTANT!!! This is an experimental implementation of a custom action that allows creating response buy action
        //It should be clarified with the product owner

        //public PXAction<CRCase> ACPRCreateAIEmail;
        //[PXUIField(DisplayName = "Create AI Email", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update, Visible = false)]
        //[PXButton(CommitChanges = true, Connotation = ActionConnotation.None, DisplayOnMainToolbar = true)]
        //public virtual IEnumerable aCPRCreateAIEmail(PXAdapter adapter)
        //{
        //    if (Base.Case.Current != null)
        //    {
        //        var restService = _openAIRestService;
        //        var currentCase = Base.Case.Current;
        //        var activity = ACPRCRActivity;
        //        var baseActivity = Base.Activities;

        //        if (aCPRCRSetupExt.UsrACPRAllowAutoGenerateEMail == true)
        //        {
        //            CreateEmail(restService, currentCase, activity, baseActivity, aCPRCRSetupExt);
        //        }
        //    }

        //    return adapter.Get();
        //}

        public PXAction<CRCase> ACPRGenerateAIResponse;
        [PXUIField(DisplayName = "Generate AI Response", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update, Visible = true)]
        [PXButton(CommitChanges = true, Connotation = ActionConnotation.None, DisplayOnMainToolbar = true)]
        public virtual IEnumerable aCPRGenerateAIResponse(PXAdapter adapter)
        {
            if (ACPROrderListProj.AskExt() == WebDialogResult.OK)
            {
                var selectedRows = ACPROrderListProj.Select().RowCast<ACPROrderListProj>().Where(_=>_.Selected == true).ToList();
                var items= new List<InventoryItem>();

                var ordersForRequest = new List<SOOrder>();
                var details = new List<SOLine>();

                foreach (var row in selectedRows)
                {
                    PXSelect<
                        SOLine,
                        Where<SOLine.orderType, Equal<Required<SOLine.orderType>>,
                            And<SOLine.orderNbr, Equal<Required<SOLine.orderNbr>>>>>
                        .Select(Base, row.OrderType, row.OrderNbr).RowCast<SOLine>().ToList().ForEach(_ =>
                        {
                            var inventoryItem = PXSelect<InventoryItem, Where<InventoryItem.inventoryID, Equal<Required<InventoryItem.inventoryID>>>>.Select(Base, _.InventoryID);
                            items.Add(inventoryItem);

                            details.Add(_);
                        });

                    var ord = SelectFrom<SOOrder>.Where<SOOrder.orderNbr.IsEqual<@P.AsString>.And<SOOrder.orderType.IsEqual<@P.AsString>>>.View.Select(Base,
                                  row.OrderNbr, row.OrderType);

                    ordersForRequest.Add(ord);

                }

                var customer = SelectFrom<Customer>.Where<Customer.bAccountID.IsEqual<@P.AsInt>>.View
                    .Select(Base, Base.Case.Current.ContactID).FirstOrDefault();

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
                } );

                if (Base.Case.Current.ContactID != null)
                {
                    var contact = PXSelect<Contact, Where<Contact.contactID, Equal<Required<Contact.contactID>>>>.Select(Base, Base.Case.Current.ContactID).FirstTableItems.FirstOrDefault();
                    sb.Append("Contact: " + contact.FirstName + " " + contact.LastName +"\n");
                }
                string orders = string.Empty;
                items.ForEach(_ =>
                {
                    orders += _.InventoryCD + " " + _.Descr + ", ";
                });
                sb.Append($"Customer Orderer: {orders}\n");
                sb.Append($"Description: {Base.Case.Current.Description.ToPlainText()}\n");
                var requestResult = _openAIRestService.RequestToAI(sb.ToString());
                if (requestResult.Choices[0] != null)
                {
                    ACPRCRActivity.MailBody = requestResult.Choices[0].Text;
                    ACPRCRActivity.DefaultSubject = Base.Activities.DefaultSubject;
                    ACPRCRActivity.GetNewEmailAddress = Base.Activities.GetNewEmailAddress;
                    var result = ACPRCRActivity.CreateNewEmailActivity();
                    result.Item2.Send.PressButton();
                }
            }

            return adapter.Get();
        }
        #endregion

        //#region Service Methods
        //protected virtual void CreateEmail(IACPROpenAIRestService restService, CRCase currentCase, ACPRCRActivityListView<CRCase> activity, CRActivityList<CRCase> baseActivity, ACPRCRSetupExt aCPRCRSetupExt)
        //{
        //    PXLongOperation.StartOperation(Base.UID, () =>
        //    {
        //        var requestResult = restService.RequestToAI(currentCase.Description.ToPlainText());
        //        if (requestResult.Choices[0] != null)
        //        {
        //            CreateEmail(activity, baseActivity, requestResult, aCPRCRSetupExt);
        //        }
        //    });
        //}
        //private static void CreateEmail(ACPRCRActivityListView<CRCase> activity, CRActivityList<CRCase> baseActivity, ResponseModel requestResult, ACPRCRSetupExt aCPRCRSetupExt)
        //{
        //    activity.MailBody = requestResult.Choices[0].Text;
        //    activity.DefaultSubject = baseActivity.DefaultSubject;
        //    activity.GetNewEmailAddress = baseActivity.GetNewEmailAddress;
        //    var result = activity.CreateNewEmailActivity();

        //    if (aCPRCRSetupExt.UsrACPRAllowAutoSendEMail == true)
        //    {
        //        result.Item2.Send.PressButton();
        //    }
        //}
        //#endregion
    }
}
