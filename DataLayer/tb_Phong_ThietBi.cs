//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_Phong_ThietBi
    {
        public int IDPHONG { get; set; }
        public string IDTB { get; set; }
        public Nullable<int> SOLUONG { get; set; }
    
        public virtual tb_Phong tb_Phong { get; set; }
        public virtual tb_ThietBi tb_ThietBi { get; set; }
    }
}
