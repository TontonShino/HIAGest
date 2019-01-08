using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;  //Permet de lui donner une annotation

namespace HIA_Equipementv2.Models
{
    public class Utilisateur
    {
        public Utilisateur()
        {

        }
        [DisplayName("Identifiant interne")]
        public int identifiant { get; set; }
        [DisplayName("Mot de passe")]
        public string password { get; set; }
        [DisplayName("Prénom")]
        public string prenom { get; set; }
        [DisplayName("Nom")]
        public string nom { get; set; }
        [DisplayName("Adresse mail")]
        public string adresse_mail { get; set; }
        [DisplayName("Fonction")]
        public string fonction { get; set; }

        [DisplayName("Clé d'accès")]
        public Nullable<int> fk_acces { get; set; }
        [DisplayName("Accès de consultation")]
        public char gest_consltation { get; set; }
        [DisplayName("Gestion des utilisateurs")]
        public char gest_user { get; set; }
        [DisplayName("Accès Gestion statut")]
        public char gest_statut { get; set; }
    }
}