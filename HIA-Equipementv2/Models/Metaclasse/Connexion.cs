using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace HIA_Equipementv2.Models
{
    public class Connexion
    {
        MySqlConnection conn;
        MySqlDataReader reader;
        MySqlCommand cmd;
        List<int> identifiant = new List<int>();
        List<string> mail = new List<string>();
        List<string> pswd = new List<string>();
        List<char> cons = new List<char>();
        List<char> statut = new List<char>();
        List<char> user = new List<char>();
        List<int> Id = new List<int>();
        ConnectionSettings ConnectionSettings = new ConnectionSettings();
        Session userSession { get; set; }
        public Connexion()
        {
            conn = new MySqlConnection(ConnectionSettings.Cs);
            conn.Open();
        }
        public Session connect(string id, string mdp)
        {
            userSession = new Session();
            string req = "select * from user_access";
            cmd = new MySqlCommand(req, conn);
            reader = cmd.ExecuteReader();

            //Boucle de lecture
            while (reader.Read())//tant qu'il a des colonnes à lire on parcourt l'objet
            {
                //On ajoute l'élément pointé à notre liste
                identifiant.Add(Convert.ToInt32(reader["identifiant"])); //colonne identiant dans la base de donnée
                mail.Add(Convert.ToString(reader["adresse_mail"])); //colonne adresse_mail dans la base de donnée
                pswd.Add(Convert.ToString(reader["password"]));//colonne password dans la base de données
                cons.Add(Convert.ToChar(reader["gest_consultation"]));
                statut.Add(Convert.ToChar(reader["gest_statut"]));
                user.Add(Convert.ToChar(reader["gest_user"]));
                
                
            }
            int taille = mail.Count;//on compte le nombre d'élément stocké pour pouvoir les parcourrir


            for (int i = 0; i < taille; i++)
            {
                if (mail[i] == id && pswd[i] == mdp)//Si identifiant et mot de passe OK
                {
                    //L'objet Session gardera ces paramètres tout au long de la session et seront disponible quel que soit le contrôleur
                    userSession.Identifiant = mail[i];
                    userSession.Consultation = cons[i];
                    userSession.Statut = statut[i];
                    userSession.User = user[i];
                    userSession.Id = identifiant[i];

                    
                }

            }//
            return userSession;
        }

        
    }
}