using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace HIA_Equipementv2.Models
{
    public class ConnectionSettings
    {
        
        public String Cs { get; set; } = @"server=localhost;userid=aforp;password=system;database=hia;port=3306;SslMode=none";//Paramètre de connexion
        public ConnectionSettings()
        {

        }

    }
}