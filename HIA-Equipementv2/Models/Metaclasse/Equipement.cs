using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;  //Permet de lui donner une annotation

namespace HIA_Equipementv2.Models
{
    public class Equipement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Equipement()
        {
            
        }
        [DisplayName("ID interne")]
        public int idEquipement { get; set; }
        [DisplayName("N/S")]
        public string serial_number { get; set; }
        [DisplayName("Nom")]
        public string nom { get; set; }
        [DisplayName("Description:")]
        public string description { get; set; }
        [DisplayName("Type")]
        public string type { get; set; }
        [DisplayName("Reférence statut")]
        public Nullable<int> fk_statut { get; set; }
        [DisplayName("Reférence TAG")]
        public Nullable<int> fk_tag { get; set; }
        [DisplayName("Commentaires")]
        public string commentaire { get; set; }
        [DisplayName("Statut")]
        public string statut { get; set; }
        [DisplayName("Date/Heure")]
        public DateTime time { get; set; }

    }
}