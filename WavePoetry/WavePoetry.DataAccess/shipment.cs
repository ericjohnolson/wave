//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WavePoetry.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class shipment
    {
        public int id { get; set; }
        public int title_id { get; set; }
        public int contact_id { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public Nullable<System.DateTime> date_sent { get; set; }
        public int quantity { get; set; }
        public bool should_followup { get; set; }
        public int createdby { get; set; }
        public System.DateTime createdat { get; set; }
        public int updatedby { get; set; }
        public System.DateTime updatedat { get; set; }
        public Nullable<int> award_id { get; set; }

        public virtual title shipment_title { get; set; }
        public virtual contact shipment_contact { get; set; }
        public virtual user shipment_user { get; set; }
    }
}
