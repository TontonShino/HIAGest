using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace HIA_Equipementv2.Models
{
    public class GestUtilisateur
    {
        //Attributs
        public ConnectionSettings settings = new ConnectionSettings(); //On récupère les paramètres de connexion
        public MySqlConnection Conn { get; set; }

        List<Utilisateur> LstUtilisateurs { get; set; }
        MySqlCommand cmd;

        MySqlDataReader Collecte { get; set; }

        private Session ActiveSession { get; set; } 

        public GestUtilisateur(Session session)
        {
            Conn = new MySqlConnection(settings.Cs);
            ActiveSession = session;
            Conn.Open();
        }
        ~GestUtilisateur()
        {
            Conn.Close();
        }

        //Méthode pour rechercher tout les utilisateurs "enregsitrés" qui possède une clé d'accès
        public List<Utilisateur> FetchUsers()
        {

            string requete = "SELECT * FROM Utilisateur,Acces WHERE Utilisateur.fk_acces = Acces.idAcess;";
            cmd = new MySqlCommand(requete, Conn);
            Collecte = cmd.ExecuteReader();
            LstUtilisateurs = new List<Utilisateur>();

            while (Collecte.Read())
            {
                LstUtilisateurs.Add(new Utilisateur
                {
                    identifiant = Convert.ToInt32(Collecte["identifiant"]),
                    password = Collecte["password"].ToString(),
                    prenom = Collecte["prenom"].ToString(),
                    nom = Collecte["nom"].ToString(),
                    adresse_mail = Collecte["adresse_mail"].ToString(),
                    fonction = Collecte["fonction"].ToString(),
                    fk_acces = Convert.ToInt32(Collecte["fk_acces"]),
                    gest_consltation = Convert.ToChar(Collecte["gest_consultation"]),
                    gest_statut = Convert.ToChar(Collecte["gest_statut"]),
                    gest_user = Convert.ToChar(Collecte["gest_user"])


                });
            }

            return LstUtilisateurs;
        }

        //Recherche rapide d'un utilisateur
        public List<Utilisateur> QuickSearchUser(string param)
        {

            //string requete = "SELECT * FROM Utilisateur,Acces WHERE Utilisateur.fk_acces = Acces.idAcess AND (nom LIKE \"%" + param + "%\" OR prenom LIKE \"%" + param + "%\" OR adresse_mail LIKE \"%" + param + "%\") ;";
            string requete = string.Format("SELECT * FROM user_access WHERE nom LIKE '%{0}%' OR '%{0}%' OR prenom LIKE '%{0}%' OR adresse_mail LIKE '%{0}%'",param);
            cmd = new MySqlCommand(requete, Conn);
            Collecte = cmd.ExecuteReader();
            LstUtilisateurs = new List<Utilisateur>();

            while (Collecte.Read())
            {
                LstUtilisateurs.Add(new Utilisateur
                {
                    identifiant = Convert.ToInt32(Collecte["identifiant"]),
                    password = Collecte["password"].ToString(),
                    prenom = Collecte["prenom"].ToString(),
                    nom = Collecte["nom"].ToString(),
                    adresse_mail = Collecte["adresse_mail"].ToString(),
                    fonction = Collecte["fonction"].ToString(),
                    fk_acces = Convert.ToInt32(Collecte["fk_acces"]),
                    gest_consltation = Convert.ToChar(Collecte["gest_consultation"]),
                    gest_statut = Convert.ToChar(Collecte["gest_statut"]),
                    gest_user = Convert.ToChar(Collecte["gest_user"])


                });
            }

            return LstUtilisateurs;
        }

        //Méthode pour récuperer le prochain id (utilisateurs)
        public int NextIdUser()
        {

            string req = "SELECT identifiant FROM utilisateur ORDER BY identifiant DESC LIMIT 1;";
            int id = new int();
            cmd = new MySqlCommand(req, Conn);
            Collecte = cmd.ExecuteReader();
            while (Collecte.Read())
            {
                id = Convert.ToInt32(Collecte["identifiant"]);
            }
            Collecte.Close();
            return id + 1;
        }

        //Ajout d'utilisateur
        public string AddUser(string password, string prenom, string nom, string adresse_mail, string fonction, char gest_consltation, char gest_user, char gest_statut)
        {

            int identifiant = NextIdUser();
            int idAccess = NextAccessID();
            string reqUser = "INSERT INTO Utilisateur(identifiant,password,prenom,nom,adresse_mail,fonction, fk_acces) VALUES(" + identifiant + ",\"" + password + "\",\"" + prenom + "\",\"" + nom + "\",\"" + adresse_mail + "\",\"" + fonction + "\"," + idAccess + ")";// préparation de la requete permettant d'insérer un utilisateur
            //string recupId = $"SELECT * FROM Utilisateur WHERE adresse_mail=\"@adresse_mail\""; //récupéreration de l'id de l'utilisateur enregistré
            string reqAcces = "INSERT INTO Acces(idAcess, gest_consultation,gest_statut,gest_user) VALUES(\"" + idAccess + "\",\'" + gest_consltation + "\',\'" + gest_statut + "\',\'" + gest_user + "\')";

            string result;
            result = ReqUser(reqAcces, Conn);
            if(result=="ok")
            {
                result = ReqUser(reqUser, Conn);
                return result;
            }
            else
            {
                return result;
            }
            


            return result;
        }

        //Mérhode requetes Utilisateurs
        public string ReqUser(string req, MySqlConnection conn)
        {
            try
            {
                cmd = new MySqlCommand(req, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {

                return ex.Message;
            }

            return "ok";
        }

        //Méthode qui prend un ID (identifiant) en entré et qui renvoi un objet Utilisateur
        public Utilisateur SelectUser(int id)
        {
            List<Utilisateur> Utilisateurs = new List<Utilisateur>();
            Utilisateur selectedUser;
            Utilisateurs = FetchUsers();

            for (int i = 0; i < LstUtilisateurs.Count; i++)
            {
                if (LstUtilisateurs[i].identifiant == id)
                {
                    return selectedUser = LstUtilisateurs[i];
                }
            }
            return null;

        }

        //Méthode qui se connecte à la bdd puis calcul le prochain id à insérer
        public int NextAccessID()
        {
            string req = "SELECT MAX(idAcess) as idAcess FROM Acces;";
            int id = new int();
            cmd = new MySqlCommand(req, Conn);
            Collecte = cmd.ExecuteReader();
            while (Collecte.Read())
            {
                id = Convert.ToInt32(Collecte["idAcess"]);
            }
            Collecte.Close();
            return id + 1;

        }

        //Methode d'effacement utilisateur
        public string DeleteUser(int id,int fkaccess)
        {
            string req = "DELETE FROM Utilisateur WHERE identifiant=" + id + ";";
            string req2 = "DELETE FROM Acces WHERE idAcess=" + fkaccess + ";";
            string result = ReqUser(req, Conn);

            if(result=="ok")
            {
                result = ReqUser(req2, Conn);
                return result;
            }
            else
            {
                return result;
            }
            
            
        }

        //Mise à jour utilisateur
        public string UpdateU(int identifiant, int fk_acces, string password, string prenom, string nom, string adresse_mail, string fonction, char gest_consltation, char gest_user, char gest_statut)
        {
            String req = String.Format("UPDATE utilisateur SET nom='{0}', prenom='{1}' ,password='{2}' , adresse_mail='{3}' WHERE identifiant={4}",nom,prenom,password,adresse_mail,identifiant);
            String reqStatut = String.Format("UPDATE acces SET gest_consultation='{0}' ,gest_user='{1}' , gest_statut='{2}' WHERE idAcess={3}",gest_consltation,gest_user,gest_statut,fk_acces);
            string result;

            result = ReqUser(req, Conn);

            if(result=="ok")
            {
                result = ReqUser(reqStatut, Conn);
                return result;
            }
            else
            {
                return result;
            }
           
        }
        //Méthode qui renvoi les infos de l'utilisateur
        public Utilisateur getUserInfo(int id)
        {
            Utilisateur user = new Utilisateur();
            user = SelectUser(id);

            return user;
        }








    }
}