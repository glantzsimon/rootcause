using K9.WebApplication.Enums;
using System;

namespace K9.WebApplication.Models
{
    public class RetrieveLastModel
    {
        public ESection Section { get; set; }
        public DateTime? StoredOn { get; set; }
        public string Value { get; set; }
    }
}