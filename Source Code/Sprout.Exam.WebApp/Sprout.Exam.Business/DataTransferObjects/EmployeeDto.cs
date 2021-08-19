using Sprout.Exam.Business.DataTransferObjects.AbstractObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class EmployeeDto : Entity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Birthdate { get; set; }
        public string Tin { get; set; }
        public int TypeId { get; set; }
        public bool isDeleted { get; set; }
    }
}
