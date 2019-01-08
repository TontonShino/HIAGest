using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using HIA_Equipementv2.Models.Metaclasse;


namespace HIA_Equipementv2.Models
{
    namespace HIA_Equipementv2.Controllers
    {
        
        public class EquipementsController : Controller
        {
            Session UserSession = new Session();
            string right; //variable des droits
            //Renvoi la vue /Equipements/Create création d'équipements dans la base 
            public ActionResult Create()
            {
                right = CheckRights("statut");
                if(right=="ok")
                { 
                return View();
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            // Renvoi la vue /Equipements/Index 
            public ActionResult Index()
            {
                right = CheckRights("consultation");

                if(right=="ok")
                {
                    return View();
                }
                else
                {
                    if(right=="noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        //On renvoi une erreur veuillez vous connecter
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                
 
            }

            //Renvoi la vue en chargeant les informations de l'équipement recherchéd
            public ActionResult Delete(int id)
            {
                right = CheckRights("statut");

                if(right=="ok")
                {
                    
                    GestEquipement db = new GestEquipement(UserSession);
                    Equipement equipement = new Equipement();
                    equipement = db.FindEquipementById(id);
                    return View(equipement);
                }
                else
                {
                    if(right=="noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }


            }

            //Méthode Recherche rapide
            public ActionResult Recherche_rapide(string recherche)
            {
                right = CheckRights("consultation");

                if(right=="ok")
                {
                    GestEquipement db = new GestEquipement(UserSession);//Init de la connexion à la bdd
                    List<Equipement> Result = new List<Equipement>(); // On initialise les objets dans une liste d'objet "Equipements"
                    Result = db.QuickSearchE(recherche);//on fait une recherche dans la bdd

                    return View(Result);//on renvoi la vue + le résultat
                }
                else
                {
                    if(right=="noa")
                    {
                        return RedirectToAction("NoAccess","Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }

            }

            //Méthode de création de nouvel équipement
            public ActionResult CreateNewEquipement(string p_nom,string p_description,string p_type,string p_sn)
            {
                right = CheckRights("statut");

                if(right=="ok")
                {
                    GestEquipement db = new GestEquipement(UserSession);//Init de la connexion à la bdd
                    ViewBag.Statut = db.CreateEquipement(p_nom, p_description, p_type, p_sn);//on stocke l'eventuelle erreur dans le viewBag
                    return RedirectToAction("Result", "Equipements", new { result = ViewBag.Statut });
                }
                else
                {
                    if(right=="noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                
                
            }

            //Méthode qui de modification d'attribut d'équipements
            public ActionResult Edit(int id)
            {
                right = CheckRights("statut");

                if(right=="ok")
                { 
                GestEquipement db = new GestEquipement(UserSession);//Init de la connexion à la bdd
                Equipement equipement = new Equipement(); //Init d'un objet equipement
                equipement = db.FindEquipementById(id);//on recherche l'équipement par l'id et on stocke le résultat dans l'objet equipement (ci-dessus)
                return View(equipement);//On renvoi la vue de modification avec les données de l'équipement
                }
                else
                {
                    if(right=="noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            //Méthode de mise à jour d'équipement
            public ActionResult UpdateEquip(int idEquipement,string nom,string description,string serial_number,string type)
            {
                right = CheckRights("statut");

                if(right=="ok")
                {
                    GestEquipement db = new GestEquipement(UserSession);//Init de la connexion à la bdd
                    ViewBag.resultat = db.UpdateEquipement(idEquipement, nom, description, serial_number, type);//on met à jour l'équipement dans la bdd
                                                                                                                //rajouter message d'information pour confirmer que tout s'est bien passé ou non
                    return RedirectToAction("Result", "Equipements", new { result = ViewBag.resultat });//Renvoi vers l'index
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }

            }

            //Méthode d'effacement d'équipement dans la base de donnée
            public ActionResult DeleteEquipementById(int idEquipement, int fk_statut)
            {
                right = CheckRights("statut");

                if(right=="ok")
                {
                    GestEquipement db = new GestEquipement(UserSession);//Init de la connexion à la bdd
                    ViewBag.Statut = db.DeleteEquipement(idEquipement, fk_statut);//Passages des paramètres à la méthode qui efface l'équipement de la base et renvoi un string "statut" (erreur ou pas)
                    return RedirectToAction("Result", "Equipements", new { result = ViewBag.Statut });
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                
            }

            //Déconnexion de l'utilisateur
            public ActionResult Disconnect()
            {
                //Passer la session à null
                //Passer la session à null
                Session["identifiant"] = null;
                Session["consultation"] = null;
                Session["statut"] = null;
                Session["user"] = null;
                Session["id"] = null;

                //Renvoyer la vue /Home/Index
                return RedirectToAction("Index", "Home");

            }

            //Changement de statut (Vue)
            public ActionResult ChangeStatus(int id)
            {

                right = CheckRights("statut");

                if(right=="ok")
                { 
                GestEquipement db = new GestEquipement(UserSession);
                Equipement equipement = new Equipement();
                equipement = db.FindEquipementById(id);
                   
                return View(equipement);
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            //update de l'equipement
            public ActionResult UpdateStatus(int fk_statut,string statut, string commentaire,int idEquipement)
            {
                right = CheckRights("statut");

                if(right=="ok")
                {
                    GestEquipement db = new GestEquipement(UserSession);
                    string resultUp;
                    resultUp = db.UpdateStatut(fk_statut, statut, commentaire, idEquipement);

                    return RedirectToAction("Result", "Equipements", new { result = resultUp });
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }

            }

            //Vue résultat d'une manipulation
            public ActionResult Result(string result)
            {
                ViewBag.Result = result;
                return View();
            }


            public void RefreshSession()
            {
                UserSession.Id = Convert.ToInt32(Session["id"]);
                UserSession.Identifiant = Convert.ToString(Session["identifiant"]);
                UserSession.User = Convert.ToChar(Session["user"]);
                UserSession.Consultation = Convert.ToChar(Session["consultation"]);
                UserSession.Statut = Convert.ToChar(Session["statut"]);

            }
            //Méthode qui renvoi vers les infos de l'utilisateur
            public ActionResult Historique(int id)
            {
                right = CheckRights("consultation");

                if(right=="ok")
                {
                    List<Agir> actions = new List<Agir>();
                    GestEquipement db = new GestEquipement(UserSession);

                    actions = db.ListAgirEquipement(id);
                    return View(actions);
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Equipements");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }


            }
            public ActionResult NoAccess()
            {
                return View();
            }
            public string CheckRights(string type)
            {
                string ok = "ok", noa = "noa", nc = "noc";

                if (Session["identifiant"] != null) //Si il existe une session
                {
                    RefreshSession();

                    switch (type) // Au cas par cas
                    {

                        case "consultation":
                            if (UserSession.Consultation == 'o')
                            {
                                return ok;
                            }
                            else
                            {
                                return noa;
                            }



                            break;
                        case "statut":
                            if (UserSession.Statut == 'o')
                            {
                                return ok;
                            }
                            else
                            {
                                return noa;
                            }
                            break;
                        default:
                            return "erreur inconnue";
                            break;

                    }

                }
                else // Si il l'en existe pas
                {
                    return nc;
                }

                
            }
        }

    }
}