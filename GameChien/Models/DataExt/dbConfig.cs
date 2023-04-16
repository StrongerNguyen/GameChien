using GameChien.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameChien.Models.Ext
{
    public class dbConfig
    {
        public static long GetMinBet()
        {
            using (var db = new GameChienEntities())
            {
                return db.tblConfigs.FirstOrDefault().MinBet;
            }
        }
    }
}