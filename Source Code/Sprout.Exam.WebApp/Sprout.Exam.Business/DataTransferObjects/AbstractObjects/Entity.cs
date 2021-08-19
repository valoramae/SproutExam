using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects.AbstractObjects
{
    public abstract class Entity
    {
        public bool isSuccess { get; set; }
        public List<string> MessageList { get; set; }
    }
}
