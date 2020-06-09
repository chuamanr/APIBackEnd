using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcoOplacementApi.ViewModels
{
    public class IngresoDataExperianViewModels
    {
        public string identification { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string grant_type { get; set; }
        public List<NotificacionExperianViewModels> notifications { get; set; }
    }

    public class NotificacionExperianViewModels
    {
        public int notificationId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}