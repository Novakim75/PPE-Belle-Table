using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace PPE_Salons
{
    public partial class Login : Form
    {
        public int NiveauUtilisateur;
        public int IdUtilisateur;
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection dbCon = new DBConnection();
                dbCon.Server = "ppebelletablecerfal.chaisgxhr4z6.eu-west-3.rds.amazonaws.com";
                dbCon.DatabaseName = "PPE_Thibault";
                dbCon.UserName = "admin";
                dbCon.Password = Crypto.Decrypt("tr9y0URXywxHt1XgTEn4yg==");//Pour éviter d'afficher le mot de passe en clair dans le code
                int Identifiant = -1;
                if (dbCon.IsConnect())
                {
                    String sqlString = "ChercherContact";
                    var cmd = new MySqlCommand(sqlString, dbCon.Connection);
                    cmd.CommandType = CommandType.StoredProcedure; //Il faut System.Data pour cette ligne

                    cmd.Parameters.Add("@NomEntree", MySqlDbType.VarChar);
                    cmd.Parameters["@NomEntree"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@NomEntree"].Value = tblogin.Text;
                    cmd.Parameters.Add("@IdSortie", MySqlDbType.Int32);
                    cmd.Parameters["@IdSortie"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    Identifiant = (int)cmd.Parameters["@IdSortie"].Value;
                    if(Identifiant>0)
                        labelConnect.Text = "Bienvenue";
                    else labelConnect.Text = "Je ne vous connais pas";
                    dbCon.Close();
              

                }
                dbCon.Close();
              
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Erreur");
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            labelConnect.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection dbCon = new DBConnection();
                dbCon.Server = "ppebelletablecerfal.chaisgxhr4z6.eu-west-3.rds.amazonaws.com";
                dbCon.DatabaseName = "PPE_Thibault";
                dbCon.UserName = "admin";
                dbCon.Password = Crypto.Decrypt("tr9y0URXywxHt1XgTEn4yg==");//Pour éviter d'afficher le mot de passe en clair dans le code
                if (dbCon.IsConnect())
                {
                    String sqlString = "AjouterUtilisateur";
                    var cmd = new MySqlCommand(sqlString, dbCon.Connection);
                    cmd.CommandType = CommandType.StoredProcedure; //Il faut System.Data pour cette ligne

                    cmd.Parameters.Add("@Username", MySqlDbType.VarChar);
                    cmd.Parameters["@Username"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@Username"].Value = tblogin.Text;

                    cmd.Parameters.Add("@LePass", MySqlDbType.Text);
                    cmd.Parameters["@LePass"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@LePass"].Value = tbPass.Text;

                    cmd.ExecuteNonQuery();
                  
                    dbCon.Close();


                }
                dbCon.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Erreur");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DBConnection dbCon = new DBConnection();
                dbCon.Server = "ppebelletablecerfal.chaisgxhr4z6.eu-west-3.rds.amazonaws.com";
                dbCon.DatabaseName = "PPE_Thibault";
                dbCon.UserName = "admin";
                dbCon.Password = Crypto.Decrypt("tr9y0URXywxHt1XgTEn4yg==");//Pour éviter d'afficher le mot de passe en clair dans le code
                if (dbCon.IsConnect())
                {
                    String sqlString = "PS_Login_Conn";
                    var cmd = new MySqlCommand(sqlString, dbCon.Connection);
                    cmd.CommandType = CommandType.StoredProcedure; //Il faut System.Data pour cette ligne

                    cmd.Parameters.Add("@NomEntree", MySqlDbType.VarChar);
                    cmd.Parameters["@NomEntree"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@NomEntree"].Value = tblogin.Text;
                    cmd.Parameters.Add("@LePass", MySqlDbType.Text);
                    cmd.Parameters["@LePass"].Direction = ParameterDirection.Input;
                   
                    cmd.Parameters["@LePass"].Value = SHA.petitsha(tbPass.Text);

                    cmd.Parameters.Add("@IdSortie", MySqlDbType.Int32);
                    cmd.Parameters["@IdSortie"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@LvlSortie", MySqlDbType.Int32);
                    cmd.Parameters["@LvlSortie"].Direction = ParameterDirection.Output;

                    
                    cmd.ExecuteNonQuery();
                    IdUtilisateur = (int)cmd.Parameters["@IdSortie"].Value;
                    if (IdUtilisateur > 0)
                    {
                       NiveauUtilisateur= (int)cmd.Parameters["@LvlSortie"].Value;
                        labelResponse.Text = "Bienvenue";
                    this.DialogResult = DialogResult.OK;//Modale est validée par OK
                    }
                    else labelResponse.Text = "Je ne vous connais pas";
                    dbCon.Close();


                }
                dbCon.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;//Modale est Annulée OK 
        }
    }
    }
