using System;
using MySql.Data.MySqlClient;


namespace PPE_Vallade
{
    class Program
    {
        static void Main(string[] args)
        {

            //Connnexion à la BDD
             DBConnection dbCon = new DBConnection();
            dbCon.Server = "127.0.0.1";
            dbCon.DatabaseName = "ppe";
            dbCon.UserName = "root";
           dbCon.Password = Crypto.Decrypt("r7bmN7pqFWFZMyztVQ8FCA==");
            if (dbCon.IsConnect())
            {
                //matricule nom
                string query = "SELECT code_c, nom, prenom, mail, ville, telephone FROM participant";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                //Menu principal
                int leChoix;
                reader.Close();
                do
                {
                    leChoix = Interface.MenuPrincipal();
                    Interface.TraiterChoix(leChoix, dbCon, reader);
                } while (leChoix != 4);
                Console.ReadLine();
            }
        }

    }
}
