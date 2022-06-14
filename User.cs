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
    public partial class User : Form
    {
        public User()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ComboValue MaComboValue = (ComboValue)comboLevel.SelectedItem;
            int LevelUser = int.Parse(MaComboValue.Value);

            try
            {
                DBConnection dbCon = new DBConnection();
                dbCon.Server = "127.0.0.1";
                dbCon.DatabaseName = "ppesalons";
                dbCon.UserName = "root";
                dbCon.Password = Crypto.Decrypt("MGgAtv/61oXwMgJN47ilHg==");//Pour éviter d'afficher le mot de passe en clair dans le code
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
                    cmd.Parameters["@LePass"].Value = SHA.petitsha(tbPass.Text);

                    cmd.Parameters.Add("@Niveau", MySqlDbType.Int32);
                    cmd.Parameters["@Niveau"].Direction = ParameterDirection.Input;
                    cmd.Parameters["@Niveau"].Value = LevelUser;
                    

                    cmd.ExecuteNonQuery();

                    dbCon.Close();
                    MessageBox.Show("Utilisateur ajouté");


                }
                dbCon.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Erreur");
            }
        }

        private void User_Load(object sender, EventArgs e)
        {
            var ComboLevelSource = new List<ComboValue>();
            ComboLevelSource.Add(new ComboValue() { Name = "Admin", Value = "1"});
            ComboLevelSource.Add(new ComboValue() { Name = "Utilisateur", Value = "2" });
            comboLevel.DataSource = ComboLevelSource;
            comboLevel.DisplayMember = "Name";
            comboLevel.ValueMember = "Value";
            
        }

        private void comboLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Index = comboLevel.SelectedIndex;
            if (Index > 0)///On a selectionné quelque chose
            {
                ComboValue MaComboValue = (ComboValue)comboLevel.SelectedItem;
            }
        }
    }
}
