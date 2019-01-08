using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIA_Equipementv2.Models
{
    public class Session
    {
        public string Identifiant { get; set; }
        public char Consultation { get; set; }
        public char User { get; set; }
        public char Statut { get; set; }
        public int Id { get; set; }


        
        public Session()
        {

        }

    }
}