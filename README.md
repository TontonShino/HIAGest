# Introduction

Ce projet est basé sur un contexte hospitalier qui concerne la gestion/inventaire s'apuyant sur une base de donnée MYSQL.
Développé sous C# ASP.NET.

Il a été livré dans le cadre de l'examen du BTS SIO option SLAM 2018

#Fonctionnalités/Modules

Gestion d'équipements:
- ajout
- retrait
- modification/Update
- suppresion
- gestion des statuts/état 

Gestion des utilisateurs:
- ajout
- retrait
- modification update
- suppresion
- Gestion des accès (consulation)

Authentification:
- Connexion
- Vérification des accès avant actions.
- Affichage des données utilsateur authentifié
- Système de Session - Cookies

Système de colorisation:
- En fonction du statut des équipements: affichage fond rouge/vert/orange
- En fonction des accès utilisateurs : affichage rouge/vert


1.	Notes de procédures de déploiement (Windows Server 2012 R2):

Notes de déploiement

Avant la connexion à distance des machines vérifier que les machines (clientes-serveur) ont bien l’accès à distance activé.

Ajouter le rôle IIS : http://syskb.com/comment-installer-un-serveur-iis-8-sur-windows-server-2012/ 

Installer le Web Platform Installer : https://www.microsoft.com/web/downloads/platform.aspx 
https://host4asp.net/what-is-web-deploy/ 

Créer des utilisateurs locaux (Web deploy)

À noter que le déploiement sur Windows Server 2012 R2 comporte de nombreux bugs il est conseillé de déployer l'application sur la version de Windows Server 2016.


2.	Pré-requis:

- Base de donnée: MySql minimum 5.7.2 - Elle devra posseder la dernière version de la base de donnée hia

- .NET framework 4.6.1
- Visual Studio Express/2015/2017 (Conseillé)
- Boostrap 4.1.1
