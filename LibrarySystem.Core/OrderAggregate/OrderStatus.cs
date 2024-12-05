using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem.Core.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending ,
        [EnumMember(Value = "Payment Recived")]
        PaymentRecived ,
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed
    }
}
