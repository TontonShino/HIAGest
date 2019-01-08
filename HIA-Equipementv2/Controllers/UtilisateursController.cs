using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HIA_Equipementv2.Models
{

    namespace HIA_Equipementv2.Controllers
    {
        
        public class UtilisateursController : Controller
        {
            public Session UserSession = new Session();
            string right ;
            //identifiant, consultation,statut,statut,user
            // GET: Utilisateurs
            public ActionResult Index()
            {
                 right = CheckRights("user");

                if (right=="ok")
                {
                    RefreshSession();
                    GestUtilisateur db = new GestUtilisateur(UserSession);
                    List<Utilisateur> utilisateurs = new List<Utilisateur>();
                    utilisateurs = db.FetchUsers();
                    return View(utilisateurs);
                }
                else
                {
                    if(right=="noa")
                    {
                        return RedirectToAction("NoAccess", "Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
               

            }

            //Méthode recherche rapide Utilisateurs
            public ActionResult Recherche_rapide(string recherche)
            {
                right = CheckRights("consultation");

                if(right=="ok")
                {
                    RefreshSession();
                    GestUtilisateur db = new GestUtilisateur(UserSession);
                    List<Utilisateur> result = new List<Utilisateur>();
                    result = db.QuickSearchUser(recherche);
                    return View(result);
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }

                }


            }

            //Methode qui renvoi la vue de création
            public ActionResult Create()
            {
                right = CheckRights("user");
                if(right=="ok")
                {
                    return View();
                }
                else
                {
                    if(right=="noa")
                    {
                        return RedirectToAction("NoAccess","Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index","Home");
                    }
                   
                }
                
                
            }
            public ActionResult NoAccess()
            {
               
                    return View();
            }

            //Méthode qui créer un nouvel utilisateur
            public ActionResult CreateNewUser(string password, string prenom, string nom,string adresse_mail, string fonction, char gest_consltation,char gest_user, char gest_statut)
            {
                right = CheckRights("user");
                if (right == "ok")
                {
                    RefreshSession();
                    GestUtilisateur db = new GestUtilisateur(UserSession);
                    TempData["Message"] = db.AddUser(password, prenom, nom, adresse_mail, fonction, gest_consltation, gest_user, gest_statut);
                    return RedirectToAction("Result", "Utilisateurs");
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }

                }

               
            }

            //deconnexion
            public ActionResult Disconnect()
            {
                //Passer la session à null
                Session["identifiant"] = null;
                Session["consultation"] = null;
                Session["statut"] = null;
                Session["user"] = null;
                Session["id"]= null;
                //Renvoyer la vue /Home/Index
                return RedirectToAction("Index", "Home");
            }


            //Edition d'utilisateur
            public ActionResult Edit(int id)
            {
                right = CheckRights("user");
                if (right == "ok")
                {
                    RefreshSession();
                    GestUtilisateur db = new GestUtilisateur(UserSession);
                    Utilisateur utilisateur = db.SelectUser(id);
                    return View(utilisateur);
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            /*
            //update de l'utilisateur
            public ActionResult UpdateUser(int identifiant, string password, string prenom, string nom, string adresse_mail, string fonction,int fk_acces , char gest_consltation, char gest_user, char gest_statut)
            {
                right = CheckRights("user");

                if(right=="ok")
                {
                    return View();
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                    
            }*/

            

            public string CheckRights(string type)
            {
                string ok = "ok", noa = "noa", nc = "noc";

                if (Session["identifiant"] != null) //Si il existe une session
                {
                    RefreshSession();
                   
                    switch(type) // Au cas par cas
                    {

                        case "consultation":
                            if(UserSession.Consultation=='o')
                            {
                                return ok;
                            }
                            else
                            {
                                return noa;
                            }



                            break;
                        case "user":
                            if (UserSession.User == 'o')
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

            //Renvoi vers la view Delete et recupere les infos utilisateurs puis les renvoient vers la méthode deleteUserById
            public ActionResult Delete(int id)
            {
                right = CheckRights("user");

                if(right=="ok")
                {
                    RefreshSession();
                    GestUtilisateur db = new GestUtilisateur(UserSession);
                    Utilisateur todelete = new Utilisateur();
                    todelete = db.SelectUser(id);
                    return View(todelete);

                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
               
            }

            //Méthode qui efface l'utilisateur et sa clé d'accès
            public ActionResult DeleteUtilisateurById(int identifiant, int access)
            {
                right = CheckRights("user");

                if (right == "ok")
                {
                    RefreshSession();
                    GestUtilisateur db = new GestUtilisateur(UserSession);
                    string result = db.DeleteUser(identifiant, access);
                    TempData["Message"] = result;
                    return RedirectToAction("Result", "Utilisateurs");
                }
                else
                {
                                        if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            public ActionResult Result()
            {
                ViewBag.Message = TempData["Message"];
                return View();
            }

            public ActionResult UpdateUser(int identifiant, int fk_acces,string password, string prenom, string nom, string adresse_mail, string fonction, char gest_consltation, char gest_user, char gest_statut)
            {
                right = CheckRights("user");

                if(right=="ok")
                {
                    RefreshSession();
                    GestUtilisateur db = new GestUtilisateur(UserSession);
                    string result = db.UpdateU(identifiant,fk_acces,password,prenom,nom,adresse_mail,fonction,gest_consltation,gest_user,gest_statut);
                    TempData["Message"] = result;
                    return RedirectToAction("Result","Utilisateurs");
                }
                else
                {
                    if(right=="noa")
                    {
                        return RedirectToAction("NoAccess","Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index","Home");
                    }
                }

            }

            public ActionResult ChangeAccess(int id)
            {
                right = CheckRights("user");

                if(right=="")
                {

                }
                return View();

            }

            public ActionResult DetailsUser(int id)
            {
                right = CheckRights("consultation");
                RefreshSession();
                if (right == "ok")
                {
                    GestUtilisateur db = new GestUtilisateur(UserSession);
                    return View(db.SelectUser(id));
                }
                else
                {
                    if (right == "noa")
                    {
                        return RedirectToAction("NoAccess", "Utilisateurs");
                    }
                    else
                    {
                        TempData["Erreur"] = "La session a expiré veuillez vous reconnecter.";
                        return RedirectToAction("Index", "Home");
                    }
                        
                }

            }


            public void RefreshSession()
            {
                UserSession.Id = Convert.ToInt32(Session["id"]);
                UserSession.Identifiant = Convert.ToString(Session["identifiant"]);
                UserSession.User = Convert.ToChar(Session["user"]);
                UserSession.Consultation = Convert.ToChar(Session["consultation"]);
                UserSession.Statut = Convert.ToChar(Session["statut"]);

            }

        }

       


    }
}

