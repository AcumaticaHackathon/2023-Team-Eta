using PX.Data;
using PX.Objects.CR;
using System;

namespace ACPROpenAI.Descriptor.Views
{
    public class ACPRCRActivityListView<TPrimaryView> : CRActivityList<TPrimaryView> where TPrimaryView : class, IBqlTable, new()
    {
        public string MailBody { get; set; }

        public ACPRCRActivityListView(PXGraph graph) : base(graph) { }

        protected override string GenerateMailBody()
        {
            return MailBody;
        }

        /// <summary>
        /// classId must be set to 4 and type must be null in order to execute the email creation logic
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="type"></param>
        public (Guid?, CREmailActivityMaint) CreateNewEmailActivity(int classId = 4, string type = null)
        {
            var graphInstance = base.CreateNewActivity(classId, type);
            var cacheCopy = PXCache<CRSMEmail>.CreateCopy((CRSMEmail)_Graph.Caches[typeof(CRSMEmail)].CreateInstance());

            CREmailActivityMaint emailActivityMaint = (CREmailActivityMaint)graphInstance;

            graphInstance.Persist();
            return (cacheCopy.NoteID, emailActivityMaint);
        }
    }
}
