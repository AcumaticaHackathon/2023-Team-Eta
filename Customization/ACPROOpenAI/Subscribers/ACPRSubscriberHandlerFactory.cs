using ACPROpenAI.Descriptor;
using PX.BusinessProcess.DAC;
using PX.BusinessProcess.Event;
using PX.BusinessProcess.Subscribers.ActionHandlers;
using PX.BusinessProcess.Subscribers.Factories;
using PX.BusinessProcess.UI;
using PX.Data;
using PX.SM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACPROpenAI.Subscribers
{
    //TODO Clarify this part of the implementation.
    //IMPORTANT!!! This is an experimental implementation of a custom event processor. It should be clarified with the product owner

    //public class ACPRSubscriberHandlerFactory : IBPSubscriberActionHandlerFactoryWithCreateAction
    //{
    //    //A string identifier of the subscriber type that is exactly four characters long
    //    public string Type => ACPRConstants.AIEmailType;

    //    //A string label of the subscriber type
    //    public string TypeName => ACPRConstants.AIEmailNotification;

    //    //A string identifier of the action that creates a subscriber of the custom type
    //    public string CreateActionName => ACPRConstants.AIActionName;

    //    //A string label of the button that creates a subscriber of the custom type
    //    public string CreateActionLabel => ACPRConstants.AIEmailNotification;

    //    //The method that creates a subscriber with the specified ID
    //    public IEventAction CreateActionHandler(Guid handlerId, bool stopOnError, IEventDefinitionsProvider eventDefinitionsProvider)
    //    {
    //        var graph = PXGraph.CreateInstance<PXGraph>();
    //        Notification notification = PXSelect<
    //            Notification,
    //            Where<Notification.noteID,
    //          Equal<Required<Notification.noteID>>>>
    //            .Select(graph, handlerId)
    //            .AsEnumerable()
    //            .SingleOrDefault();

    //        return new ACPREventAction(handlerId, notification);
    //    }

    //    //The method that retrieves the list of subscribers of the custom type
    //    public IEnumerable<BPHandler> GetHandlers(PXGraph graph)
    //    {
    //        return PXSelect<
    //            Notification, 
    //            Where<Notification.screenID,
    //          Equal<Current<BPEvent.screenID>>,
    //                Or<Current<BPEvent.screenID>, IsNull>>>
    //            .Select(graph)
    //            .FirstTableItems
    //            .Where(c => c != null)
    //            .Select(c => new BPHandler
    //            {
    //                Id = c.NoteID,
    //                Name = c.Name,
    //                Type = ACPRConstants.AIEmailNotification
    //            });
    //    }

    //    //The method that performs redirection to the subscriber
    //    public void RedirectToHandler(Guid? handlerId)
    //    {
    //        var notificationMaint = PXGraph.CreateInstance<SMNotificationMaint>();
    //        notificationMaint.Message.Current = notificationMaint.Notifications.Search<Notification.noteID>(handlerId);

    //        PXRedirectHelper.TryRedirect(notificationMaint, PXRedirectHelper.WindowMode.New);
    //    }

    //    //The delegate for the action that creates a subscriber of the custom type
    //    public Tuple<PXButtonDelegate, PXEventSubscriberAttribute[]> getCreateActionDelegate(BusinessProcessEventMaint maintGraph)
    //    {
    //        PXButtonDelegate handler = (PXAdapter adapter) =>
    //        {
    //            if (maintGraph.Events?.Current?.ScreenID == null)
    //                return adapter.Get();

    //            var graph = PXGraph.CreateInstance<SMNotificationMaint>();
    //            var cache = graph.Caches<Notification>();
    //            var notification = (Notification)cache.CreateInstance();
    //            var row = cache.InitNewRow(notification);
    //            row.ScreenID = maintGraph.Events.Current.ScreenID;
    //            cache.Insert(row);

    //            var subscriber = new BPEventSubscriber();
    //            var subscriberRow = maintGraph.Subscribers.Cache.InitNewRow(subscriber);
    //            subscriberRow.Type = Type;
    //            subscriberRow.HandlerID = row.NoteID;
    //            graph.Caches[typeof(BPEventSubscriber)].Insert(subscriberRow);

    //            PXRedirectHelper.TryRedirect(graph, PXRedirectHelper.WindowMode.NewWindow);
    //            return adapter.Get();
    //        };

    //        return Tuple.Create(handler, new PXEventSubscriberAttribute[] 
    //        {
    //            new PXButtonAttribute 
    //            {
    //                  OnClosingPopup = PXSpecialButtonType.Refresh
    //            }
    //        });
    //    }
    //}
}
