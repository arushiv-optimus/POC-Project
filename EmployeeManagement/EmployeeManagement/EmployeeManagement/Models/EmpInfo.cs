using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagement.Models
{
    /// <summary>
    /// Employee Model
    /// </summary>
    public class EmpInfo
    {
        public int EMP_ID { get; set; }
        public string Emp_Code { get; set; }
        public string Emp_Name { get; set; }
        public string Emp_Email { get; set; }
        public string Emp_Phone { get; set; }
        public string Emp_Address { get; set; }
        public int Emp_Age { get; set; }
        public string Emp_Gender { get; set; }
        public double Emp_salary { get; set; }
        public string Emp_Department { get; set; }
        public string Emp_Designation { get; set; }
    }
}