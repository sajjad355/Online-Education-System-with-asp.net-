//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Online_Learning.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Teacher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Teacher()
        {
            this.CoursesOfStudents = new HashSet<CoursesOfStudent>();
            this.Subjects = new HashSet<Subject>();
        }
    
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Institute { get; set; }
        public string UserName { get; set; }
        public Nullable<double> Salary { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CoursesOfStudent> CoursesOfStudents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
