using ACPROpenAI.GraphExtensions.CR;
using PX.BusinessProcess.Subscribers.ActionHandlers;
using PX.Data.BusinessProcess;
using PX.Data;
using PX.Objects.CR;
using PX.PushNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PX.SM;

namespace ACPROpenAI.Subscribers
{
    //TODO Clarify this part of the implementation.
    //IMPORTANT!!! This is an experimental implementation of a custom event processor. It should be clarified with the product owner

    //public class ACPREventAction : IEventAction
    //{
    //    private const string CaseCD = "Case_CaseCD";

    //    //The GUID that identifies a subscriber
    //    public Guid Id { get; set; }

    //    //The name of the subscriber of the custom type
    //    public string Name { get; protected set; }

    //    //The notification template
    //    private readonly Notification _notificationTemplate;

    //    #region Ctor
    //    public ACPREventAction(Guid id, Notification notification)
    //    {
    //        Id = id;
    //        Name = notification.Name;
    //        _notificationTemplate = notification;
    //    }
    //    #endregion

    //    public void Process(MatchedRow[] eventRows, CancellationToken cancellation)
    //    {
    //        var caseMaint = PXGraph.CreateInstance<CRCaseMaint>();
    //        var parameters = @eventRows.Select(r => Tuple.Create<IDictionary<string, object>, IDictionary<string, object>>(r.NewRow?.ToDictionary(c => c.Key.FieldName, c => c.Value),
    //            r.OldRow?.ToDictionary(c => c.Key.FieldName, c => (c.Value as ValueWithInternal)?.ExternalValue ?? c.Value))).ToArray();

    //        var keyValuePair = parameters[0].Item1.Where(_ => _.Key == CaseCD).FirstOrDefault();

    //        string value = keyValuePair.Value.ToString();
    //        caseMaint.Case.Current = caseMaint.Case.Search<CRCase.caseCD>(value);
    //        caseMaint.Case.UpdateCurrent();
    //        var caseMaintExt = caseMaint.GetExtension<ACPRCRCaseMaintExt>();

    //        caseMaintExt.ACPRCreateAIEmail.PressButton();
    //    }
    //}
}
