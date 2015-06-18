using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrescriptionValidator.Models
{
    public enum ThreatType : int
    {
        Warning = 0,
        Danger = 1,
        Lethal = 2
    }
}