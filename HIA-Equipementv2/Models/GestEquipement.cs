using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HIA_Equipementv2.Models.Metaclasse;
using MySql.Data.MySqlClient;

namespace HIA_Equipementv2.Models
{
    public class GestEquipement
    {
        //Attributs
        public ConnectionSettings conSett = new ConnectionSettings();
        public MySqlConnection Conn { get; set; }
        List<Equipement> LstEquipements { get; set; }
        List<Utilisateur> LstUtilisateurs { get; set; }
        MySqlCommand cmd;
        
        MySqlDataReader Collecte { get; set; }

        private Session ActiveSession { get; set; }
        
        //Contrustreur par défaut qui prend en paramètre les infos de l'utilisateur qui seront utilisé pour enregistrer une action
        public GestEquipement(Session userActive)
        {
            Conn = new MySqlConnection(conSett.Cs);//On initialise une connexion avec les paramètre ci-dessus (cs)
            Conn.Open();
            ActiveSession = userActive;
        }
        //Destructeur par défaut
        ~GestEquipement()
        {
            Conn.Close();
        }
        //Recupération de la liste des equipements auprès de la bdd
        public void FetchEquipements()
        {
            
            string requete = "SELECT * FROM Equipement, Statut WHERE Statut.idStatut=Equipement.idEquipement ";
            cmd = new MySqlCommand(requete, Conn);
            Collecte = cmd.ExecuteReader();//on instancie un objet qui vas collecter les donnée
            //lstquipements
            while (Collecte.Read())//boucle de lecture
            {
                //à chaque boucle on 
                LstEquipements.Add(new Equipement
                {
                    idEquipement = Convert.ToInt32(Collecte["idEquipement"]),
                    nom = Collecte["nom"].ToString(),
                    description = Collecte["description"].ToString(),
                    serial_number = Collecte["serial_number"].ToString(),
                    type = Collecte["type"].ToString(),
                    commentaire = Collecte["commentaire"].ToString(),
                    statut = Collecte["statut"].ToString(),
                    time = Convert.ToDateTime(Collecte["date"])


                    
                });
            }
            
        }
        //Méthode qui ferme la connexion à la bdd
        public void Close()
        {
            Conn.Close();
        }
        //recherche equipement par id
        public Equipement FindEquipementById(int id)
        {
            
            string requete = "SELECT * FROM Equipement,Statut WHERE idEquipement="+id+" AND Equipement.fk_statut=Statut.idStatut;";
            cmd = new MySqlCommand(requete, Conn);
            Collecte = cmd.ExecuteReader();//on instancie un objet qui vas collecter les donnée
            Equipement resarch = new Equipement();
            while (Collecte.Read())
            {
                resarch.idEquipement = Convert.ToInt32(Collecte["idEquipement"]);
                resarch.nom = Collecte["nom"].ToString();
                resarch.description = Collecte["description"].ToString();
                resarch.serial_number = Collecte["serial_number"].ToString();
                resarch.type = Collecte["type"].ToString();
                resarch.commentaire = Collecte["commentaire"].ToString();
                resarch.statut = Collecte["statut"].ToString();
                resarch.fk_statut = Convert.ToInt32(Collecte["fk_statut"]);
                resarch.time = Convert.ToDateTime(Collecte["date"]);
            }
            
            return resarch;
        }
        //Recupère auprès de la bdd l'id le plus haut+1 afin d'effectuer le prochain enregistrement du statut
        public int NextidStatut()
        {
            
            
            string req = "SELECT MAX(idStatut) as idStatut FROM statut;";
            int nextid = new int();
            cmd = new MySqlCommand(req,Conn);

            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                nextid = Convert.ToInt32(reader["idStatut"]);

            }
            nextid = nextid + 1;
            reader.Close();
            return nextid = nextid + 1;

        }
        //Mise à jour des données d'un équipement
        public string UpdateEquipement(int id,string nom,string description,string sn,string type)
        {
            
            string requete = $"UPDATE Equipement SET nom=\"{nom}\",description=\"{description}\", serial_number=\"{sn}\", type=\"{type}\" WHERE idEquipement={id};";
            string result = QueryEquipement(requete, Conn);

            if (result == "ok")
            {
                string action = "User:" + ActiveSession.Identifiant + " Description:" + description + " nom" + nom +" sn:"+sn +" type:"+type;
                RecordAction(id,action);

            }
            
            return result;   
        }
        // méthode de requete equipement (insert/update/delete) permet de faire insertion , mise à jour et suppresion
        public string QueryEquipement(string req, MySqlConnection conn)
        { 
            try
            {
                cmd = new MySqlCommand(req, conn);
                cmd.ExecuteNonQuery();
            }
            catch(MySqlException ex)
            {
                
                return ex.Message;
            }
            
            return "ok";
        }
        //Recherche à partir d'un mot clé    
        public List<Equipement> QuickSearchE(string recherche)
        {
            
            string requete = "SELECT * FROM Equipement,Statut WHERE Equipement.fk_statut=statut.idStatut AND (nom LIKE \"%" + recherche + "%\" OR type LIKE \"%" + recherche + "%\");";//On prépare la requete
            string req = string.Format("");//à finaliser !!!
            cmd = new MySqlCommand(requete, Conn);
            Collecte = cmd.ExecuteReader();
            LstEquipements = new List<Equipement>();
            while(Collecte.Read())
            {
                LstEquipements.Add(new Equipement
                {
                    idEquipement = Convert.ToInt32(Collecte["idEquipement"]),
                    nom = Collecte["nom"].ToString(),
                    description = Collecte["description"].ToString(),
                    serial_number = Collecte["serial_number"].ToString(),
                    type = Collecte["type"].ToString(),
                    fk_statut=Convert.ToInt32(Collecte["fk_statut"]),
                    commentaire = Collecte["commentaire"].ToString(),
                    statut = Collecte["statut"].ToString(),
                    time = Convert.ToDateTime(Collecte["date"])
                });
            }

            return LstEquipements;
        }
        //création d'équipements
        public string CreateEquipement(string p_nom, string p_description, string p_type, string p_sn)
        {
            
            int idstatut = NextidStatut();
            string reqStatus = "INSERT INTO statut(idStatut) VALUES("+idstatut+")";
           
            string requete = "INSERT INTO Equipement(nom,description,type,serial_number,fk_statut) VALUES(\"" + p_nom + "\", \"" + p_description + "\", \"" + p_type + "\", \"" + p_sn + "\","+idstatut+") ;";//On prépare la requete

            string result = QueryEquipement(reqStatus, Conn);
            if (result == "ok") { 
            result = QueryEquipement(requete, Conn);
                return result;
            }
            else
            {
                return result;
            }

            
        }
        //Méthode d'effacement d'un équipement ainsi que son statut rattaché à celui-ci
        public string DeleteEquipement(int id,int idStatut)
        {
            
            string requete = "DELETE FROM Equipement WHERE idEquipement="+id+";";
            string reqStatut = "DELETE FROM statut WHERE idStatut="+idStatut;
            string result = QueryEquipement(requete, Conn);
            if(result=="ok")
            {
                result = QueryEquipement(reqStatut, Conn);
                return result;
            }
            else
            {
                return result;
            }

            
            
        }
        //Méthode Mise à jour d'un statut/état d'un équipement avec un commentaire
        public string UpdateStatut(int fk_statut, string statut, string commentaire,int IdEquipement)
        {
            
            
            string req = $"UPDATE statut SET statut=\"{statut}\", commentaire=\"{commentaire}\" WHERE idStatut={fk_statut} ;";
            string result = QueryEquipement(req, Conn);
            if (result=="ok")
            {
                string modif = "User:"+ActiveSession.Identifiant +" Statut:"+statut+" commentaire:"+commentaire;
                RecordAction(IdEquipement,modif);
            }
            return result;

        }
        //Enregistrement D'une action effectué par un utilisateur sur un équipement
        public void RecordAction(int idEquipement, string modif){
            
            String req = $"INSERT INTO agir (action,fk_equipement,fk_utilisateur) VALUES(\"{modif}\",{idEquipement},{ActiveSession.Id});";
            string result = QueryEquipement(req, Conn);
        }
        //Historique d'un équipement (changement de statut et commentaires)
        public List<Agir> ListAgirEquipement(int idEquipement)
        {
            List<Agir> actions = new List<Agir>();
            string req= "SELECT * FROM lstActions WHERE idEquipement="+idEquipement;

            cmd = new MySqlCommand(req, Conn);
            Collecte = cmd.ExecuteReader();

            while (Collecte.Read())
            {
                actions.Add(new Agir
                    {

                        IdAgir = Convert.ToInt32(Collecte["idAGIR"]),
                        Action = Convert.ToString(Collecte["action"]),
                        IdEquipement = Convert.ToInt32(Collecte["fk_equipement"]),
                        IdUtilisateur = Convert.ToInt32(Collecte["fk_utilisateur"]),
                        Nom = Convert.ToString(Collecte["nom"]),
                        Sn = Convert.ToString(Collecte["serial_number"]),
                        Horaire = Convert.ToDateTime(Collecte["horaire"])

                    });
            }

            return actions;


        }

    }
}