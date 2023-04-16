using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models
{
    public class Logging
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}