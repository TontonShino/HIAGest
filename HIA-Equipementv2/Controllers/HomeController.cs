using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace HIA_Equipementv2.Models
{
    namespace HIA_Equipementv2.Controllers
    {
        public class HomeController : Controller
        {
            Session UserSession = new Session();
            // GET: Home
            public ActionResult Index()
            {
                if(TempData["erreur"] != null)
                {
                    ViewBag.erreur= TempData["erreur"].ToString();
                }
                if(Session["identifiant"]!=null)
                {
                    return RedirectToAction("Index", "Equipements");
                }
                return View();
            }

            public ActionResult Connect(string id, string mdp)
            {
                Connexion log = new Connexion();
                Session userData = new Session();
                userData = log.connect(id, mdp);

                if(userData.Identifiant!=null)
                {
                    Session["identifiant"] = userData.Identifiant;//adresse mail
                    Session["consultation"] = userData.Consultation;
                    Session["statut"] = userData.Statut;
                    Session["user"] = userData.User;
                    Session["id"] = userData.Id; //id primary key
                    return RedirectToAction("Index", "Equipements");
                }
                else
                {
                    TempData["erreur"] = "Identifiant ou mot de passe incorrect, veuillez réessayer"; 
                    return RedirectToAction("Index", "Home");// Dans ce cas on renvoi la page d'accueil 
                }

                
                             
                
                
            }

            public ActionResult Infos()
            {
                if(Session["identifiant"]!=null)
                {
                    RefreshSession();
                    GestUtilisateur db = new GestUtilisateur(UserSession);
                    Utilisateur user = new Utilisateur();
                    user = db.getUserInfo(Convert.ToInt32(Session["id"]));

                    return View(user);

                }
                else
                {
                    TempData["erreur"] = "Vous n'êtes pas connecté ou identifié";
                    return RedirectToAction("Index","Home");
                }
                
            }

            public ActionResult Error()
            {
                
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
        }
    }
}