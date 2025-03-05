using K9.DataAccessLayer.Models;
using System.Collections.Generic;

namespace K9.WebApplication.ViewModels
{
    public class ProtocolsViewModel
    {
        public List<Protocol> Recommended { get; set; }
        public List<Protocol> Protocols { get; set; }
    }
}