using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.IN;
using PX.Objects.SO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACPROpenAI.GraphExtensions.CR.DAC.Projection
{
    [Serializable]
    [PXCacheName(OrderListProj)]
    [PXProjection(typeof(Select<SOOrder, Where<SOOrder.customerID, Equal<CurrentValue<CRCase.customerID>>>>), Persistent = false)] //
    public class ACPROrderListProj : IBqlTable
    {
        private const string OrderListProj = "Sales Order List";

        #region Selected
        [PXBool]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region OrderType
        [PXDBString(2, IsKey = true, IsFixed = true, BqlField = typeof(SOOrder.orderType))]
        [PXSelector(typeof(Search5<
            SOOrderType.orderType,
            InnerJoin<SOOrderTypeOperation,
                On2<SOOrderTypeOperation.FK.OrderType,
                And<SOOrderTypeOperation.operation, Equal<SOOrderType.defaultOperation>>>,
            LeftJoin<SOSetupApproval,
                On<SOOrderType.orderType, Equal<SOSetupApproval.orderType>>>>,
            Aggregate<
                GroupBy<SOOrderType.orderType>>>))]
        [PXRestrictor(typeof(Where<SOOrderTypeOperation.iNDocType, NotEqual<INTranType.transfer>, Or<FeatureInstalled<FeaturesSet.warehouse>>>), "'{0}' cannot be found in the system.", new Type[]
        {
            typeof(SOOrderType.orderType)
        })]
        [PXRestrictor(typeof(Where<SOOrderType.requireAllocation, NotEqual<True>, Or<AllocationAllowed>>), "'{0}' cannot be found in the system.", new Type[]
        {
            typeof(SOOrderType.orderType)
        })]
        [PXRestrictor(typeof(Where<SOOrderType.active, Equal<True>>), null, new Type[]
        {

        })]
        [PXUIField(DisplayName = "Order Type", Enabled = false, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string OrderType { get; set; }
        public abstract class orderType : PX.Data.BQL.BqlString.Field<orderType> { }
        #endregion

        #region OrderNbr
        [PXDBString(15, IsKey = true, IsUnicode = true, BqlField = typeof(SOOrder.orderNbr))]
        [PXUIField(DisplayName = "Order Nbr.", Enabled = false, Visibility = PXUIVisibility.SelectorVisible)]
        public virtual string OrderNbr { get; set; }
        public abstract class orderNbr : PX.Data.BQL.BqlString.Field<orderNbr> { }
        #endregion

        #region Status
        [PXDBString(1, IsFixed = true, BqlField = typeof(SOOrder.status))]
        [PXUIField(DisplayName = "Status", Visibility = PXUIVisibility.SelectorVisible, Enabled = false)]
        [SOOrderStatus.List]
        public virtual string Status { get; set; }
        public abstract class status : PX.Data.BQL.BqlString.Field<status> { }
        #endregion

        #region OrderDate
        [PXDBDate(BqlField = typeof(SOOrder.status))]
        [PXDefault(typeof(AccessInfo.businessDate))]
        [PXUIField(DisplayName = "Date")]
        public virtual DateTime? OrderDate { get; set; }
        public abstract class orderDate : PX.Data.BQL.BqlDateTime.Field<orderDate> { }
        #endregion
    }
}
