//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PracticeProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int OrderId { get; set; }
        public Nullable<int> TruckId { get; set; }
        public System.DateTime OrderStartDate { get; set; }
        public Nullable<System.DateTime> OrderEndDate { get; set; }
        public bool OrderState { get; set; }
        public Nullable<int> OrderPrice { get; set; }
        public string OrderDescription { get; set; }
        public string OrderDepPoint { get; set; }
        public string OrderEndPoint { get; set; }
        public Nullable<double> OrderDistance { get; set; }
        public Nullable<double> CargoWeight { get; set; }
    
        public virtual Truck Truck { get; set; }
    }
}
