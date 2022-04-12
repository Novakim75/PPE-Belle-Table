using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPE_Vallade
{
    class Participant
    {
        public string NomParticipant { get; set; }
        public string PrenomParticipant { get; set; }
        public string Ville { get; set; }
        public int IDParticipant { get; set; }
        public string MailParticipant { get; set; }

        public void Init(string LeNom, string LePrenom, string LeMail)
        {
            NomParticipant = LeNom;
            PrenomParticipant = LePrenom;
            MailParticipant = LeMail;
        }
        public void Save(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            if (IDParticipant > 0)
                UpdateParticipant(DataBaseConnection, TheReader);
            else
                AddParticipant(DataBaseConnection, TheReader);
        }
        private int GiveNewID(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            int NewCode_c = 0;
            try
            {
                String sqlString = "SELECT MAX(code_c) FROM client;";
                var cmd = new MySqlCommand(sqlString, DataBaseConnection.Connection);
                TheReader = cmd.ExecuteReader();

                while (TheReader.Read())
                { NewCode_c = TheReader.GetInt32(0); }
                TheReader.Close();
                NewCode_c++;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.Write("Erreur N° " + ex.Number + " : " + ex.Message);
            }
            return NewCode_c;
        }
        private void AddParticipant(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            try
            {
                IDParticipant = GiveNewID(DataBaseConnection, TheReader);
                String sqlString = "INSERT INTO client(code_c,nom,adresse,ville) VALUES(?code_c,?nom,?adresse,?ville)";
                sqlString = Tools.PrepareLigne(sqlString, "?code_c", Tools.PrepareChamp(IDParticipant.ToString(), "Nombre"));
                sqlString = Tools.PrepareLigne(sqlString, "?nom", Tools.PrepareChamp(NomParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?prenom", Tools.PrepareChamp(PrenomParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?Mail", Tools.PrepareChamp(MailParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?ville", Tools.PrepareChamp(Ville, "Chaine"));


                var cmd = new MySqlCommand(sqlString, DataBaseConnection.Connection);
                cmd.ExecuteNonQuery();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.Write("Erreur N° " + ex.Number + " : " + ex.Message);
            }

        }

        private void UpdateParticipant(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            try
            {
                String sqlString = "UPDATE client SET nom = ?nom,ville = ?ville,adresse = ?adresse WHERE code_c = ?code_c";
                sqlString = Tools.PrepareLigne(sqlString, "?code_c", Tools.PrepareChamp(IDParticipant.ToString(), "Nombre"));
                sqlString = Tools.PrepareLigne(sqlString, "?nom", Tools.PrepareChamp(NomParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?prenom", Tools.PrepareChamp(PrenomParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?mail", Tools.PrepareChamp(MailParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?ville", Tools.PrepareChamp(Ville, "Chaine"));


                var cmd = new MySqlCommand(sqlString, DataBaseConnection.Connection);
                cmd.ExecuteNonQuery();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.Write("Erreur N° " + ex.Number + " : " + ex.Message);
            }

        }
        public void Delete(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            try
            {
                String sqlString = "DELETE FROM client WHERE code_c = ?code_c";
                sqlString = Tools.PrepareLigne(sqlString, "?code_c", Tools.PrepareChamp(IDParticipant.ToString(), "Nombre"));
                var cmd = new MySqlCommand(sqlString, DataBaseConnection.Connection);
                cmd.ExecuteNonQuery();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.Write("Erreur N° " + ex.Number + " : " + ex.Message);
            }

        }
    }
}
