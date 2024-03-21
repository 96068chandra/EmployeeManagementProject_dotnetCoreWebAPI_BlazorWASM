using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CivilId { get; set; }
        public string? FileNumber { get; set; }
        public string? FullName { get; set; }
        public string? MyProperty { get; set; }
        public string? JobName { get; set; }
        public string? Addressname { get; set; }
        public string? TelephoneNumver { get; set; }
        public string? Photo { get; set; }
        public string? Other { get; set; }
        //Many-one
        public GeneralDepartment? GeneralDepartment { get; set; }
        public int GeneralDepartmentId { get; set; }

        public Department? Department{ get; set; }
        public int DepartmentId { get; set; }

        public Branch? Branch { get; set; }
        public int BranchId {  get; set; }

        public Town? town { get; set; }
        public int? townId { get;set; }


    }
}
