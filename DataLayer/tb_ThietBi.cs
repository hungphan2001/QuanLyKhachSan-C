
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
    
public partial class tb_ThietBi
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public tb_ThietBi()
    {

        this.tb_Phong_ThietBi = new HashSet<tb_Phong_ThietBi>();

    }


    public string IDTB { get; set; }

    public string TENTHIETBI { get; set; }

    public Nullable<double> DONGIA { get; set; }

    public Nullable<bool> DISABLED { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<tb_Phong_ThietBi> tb_Phong_ThietBi { get; set; }

}

}
