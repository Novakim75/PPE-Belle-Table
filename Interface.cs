using MySql.Data.MySqlClient;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PPE_Vallade
{
    class Interface
    {
        public static int MenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("...................Bienvenue au salon............");
            Console.WriteLine("1 : Ajouter un participant ");
            Console.WriteLine("2 : Rechercher un participant");
            Console.WriteLine("3 : Creér le badge d'un participant");
            Console.WriteLine("4 : Quitter");
            Console.WriteLine("");
            Console.Write("Votre choix : - ");
            try
            {
                String LeChoix = Console.ReadLine();
                return int.Parse(LeChoix);
            }
            catch
            {
                return 0; //Erreur de Saisie
            }
        }
        public static void TraiterChoix(int LeChoix, DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            switch (LeChoix)
            {
                case 0:
                    Console.WriteLine("Les choix possibles sont 1, 2, 3 ou 4. Appuyez sur une touche pour continuer");
                    Console.ReadKey();
                    break;

                case 1:
                    Console.WriteLine("Vous souhaitez ajouter un participant. Appuyez sur une touche pour continuer");
                    Console.ReadKey();
                    AjouterParticipant(DataBaseConnection, TheReader);
                    break;

                case 2:
                    Console.WriteLine("Vous souhaitez rechercher un participant. Appuyez sur une touche pour continuer");
                    Console.ReadKey();

                    RechercherParticipant(DataBaseConnection, TheReader);
                    break;

                case 3:
                    Console.WriteLine("Vous souhaitez générer le badge d'un participant");
                    Console.ReadKey();
                    //FabriquerBadge(1);
                    break;

                case 4:
                    Console.WriteLine("Au revoir.....");
                    break;
            }
        }
        public static void RechercherParticipant(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            String NomParticipant;
            Console.Clear();
            Console.WriteLine(".....................................................");
            Console.Write("...................Nom du participant recherché ?");
            NomParticipant = Console.ReadLine();

            string query = "select id,nom,prenom,email from Participant where nom =?nom;";
            query = Tools.PrepareLigne(query, "?nom", Tools.PrepareChamp(NomParticipant, "Chaine"));

            var cmd = new MySqlCommand(query, DataBaseConnection.Connection);
            List<Participant> LesParticipantsTrouves = new List<Participant>();
            TheReader = cmd.ExecuteReader();//On est arrivé à la fin, il faut recharger le reader
            while (TheReader.Read())
            {
                Participant UnParticipant = new Participant
                {
                    IDParticipant = (int)TheReader["id"],
                    NomParticipant = (string)TheReader["nom"],
                    PrenomParticipant = (string)TheReader["prenom"],
                    MailParticipant = (string)TheReader["email"]

                };
                LesParticipantsTrouves.Add(UnParticipant);
            }
            if (LesParticipantsTrouves.Count > 0)
            {
                Console.WriteLine("--------------------Participants Trouvés------------------");
                foreach (Participant UnParticipant in LesParticipantsTrouves)
                    Console.WriteLine(UnParticipant.IDParticipant.ToString() + ", " + UnParticipant.PrenomParticipant + ", " + UnParticipant.NomParticipant + ", " + UnParticipant.MailParticipant);
            }
            else
                Console.WriteLine("Je n'ai trouvé personne.");
            Console.WriteLine("Appuyer sur une touche pour continuer");
            Console.ReadKey();
            TheReader.Close();

        }

        public static void FabriquerBadge(int TheparticipantID, String UnNom, String UnPrenom) //Fabrication du badge du participant dont on a reçu l'id
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(TheparticipantID.ToString(), QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
            //Console.WriteLine("Le QR en base 64" + qrCodeImageAsBase64);
            StreamWriter monStreamWriter = new StreamWriter(@"BadgeSalon.html");//Necessite using System.IO;
            String strImage = " <img src = \"data:image/png;base64," + qrCodeImageAsBase64 + "\">";
            monStreamWriter.WriteLine("<html>");
            monStreamWriter.WriteLine("<body>");
            String TempText = "< P >" + UnNom + "</ P >";
            monStreamWriter.WriteLine(TempText);
            TempText = "< P >" + UnPrenom + "</ P >";
            monStreamWriter.WriteLine(TempText);
            monStreamWriter.WriteLine(strImage);
            monStreamWriter.WriteLine("</body>");
            monStreamWriter.WriteLine("</html>");



            //Ecriture de l'image base 64 dans un fichier

            // Fermeture du StreamWriter (Très important) 
            monStreamWriter.Close();
            Console.WriteLine("Le QRCode est généré. Appuyer sur une touche pour continuer");
            Console.ReadKey();

        }

        public static void AjouterParticipant(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            Participant UnParticipant = new Participant(); ;
            String NomParticipant, PrenomParticipant, EmailParticipant;
            Console.Clear();
            Console.WriteLine(".....................................................");
            Console.Write("...................Nom du participant ?");
            NomParticipant = Console.ReadLine();
            Console.Write("...................Prenom du participant ?");
            PrenomParticipant = Console.ReadLine();
            Console.Write("...................email du participant ?");
            EmailParticipant = Console.ReadLine();
            Console.WriteLine("...................Voulez-vous enregistrer ce participant (O/N) ?");
            String Reponse = "";
            do
                try
                {
                    Reponse = Console.ReadLine();
                    Reponse = Reponse.ToUpper();//On convertit en majuscule
                    if (Reponse == "O")
                        //Ici on effectue l'enregistrement dans la BDD
                        UnParticipant.Init(NomParticipant, PrenomParticipant, EmailParticipant);
                    UnParticipant.Save(DataBaseConnection, TheReader);
                    Console.WriteLine("Le participant est enregistré");

                    System.Threading.Thread.Sleep(2000);//On patiente deux secondes
                }
                catch
                {
                    Console.WriteLine("Choix incorrect");
                }
            while ((Reponse != "o") && (Reponse != "O") && (Reponse != "n") && (Reponse != "N"));
        }
    }
}
