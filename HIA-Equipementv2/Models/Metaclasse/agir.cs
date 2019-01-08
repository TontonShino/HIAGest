using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace HIA_Equipementv2.Models.Metaclasse
{
    public class Agir
    {
        [DisplayName("N° Action")]
        public int IdAgir { get; set; }
        [DisplayName("Action")]
        public string Action { get; set; }
        [DisplayName("Ref Equipement")]
        public int IdEquipement { get; set; }
        [DisplayName("Ref Utilisateur")]
        public int IdUtilisateur { get; set; }
        [DisplayName("Nom équipement")]
        public string Nom { get; set; }
        [DisplayName("N° de Série")]
        public string Sn { get; set; }
        [DisplayName("Heure/Date action")]
        public DateTime Horaire { get; set; }

        public Agir()
        {

        }

    }
}