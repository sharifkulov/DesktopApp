using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Npgsql;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Users newUsr;
        private string connectstr = String.Format("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1357924680");
        private string sql;
        private NpgsqlCommand cmd; 
        private NpgsqlConnection conn;
        private DataTable dt;
        private int rowIndex = -1;
        public MainWindow()
        {
            InitializeComponent();
            conn = new NpgsqlConnection(connectstr);
            Select();
        }

        private void Select()
        {
           try
           {
                conn.Open();
                sql = @"select * from select_users()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                /*NpgsqlDataReader reader = cmd.ExecuteReader();
                List<object[]> data = new List<object[]>();
                while (reader.Read())
                {
                    var values = new object[reader.FieldCount];
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        values[i] = reader[i];
                    }

                    data.Add(values);
                }*/
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                dgvData.ItemsSource = dt.DefaultView;
            }
           
           catch (Exception ex)
           {
                MessageBox.Show("Error: " + ex.Message);
           }
        }

        private void insertButton_Click(object sender, RoutedEventArgs e)
        {
            rowIndex = -1;
            NameBox.Text = SNameBox.Text = AgeBox.Text = null;
            NameBox.IsEnabled = SNameBox.IsEnabled = AgeBox.IsEnabled = true;
        }

        private void Cell_Click(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.GetIndex() >= 0)
            {
                rowIndex = e.Row.GetIndex();
                NameBox.Text = dt.Rows[rowIndex][1].ToString();
                SNameBox.Text = dt.Rows[rowIndex][2].ToString();
                AgeBox.Text = dt.Rows[rowIndex][3].ToString();
            }
            Select();
        }

        private void deletButton_Click(object sender, RoutedEventArgs e)
        {
            if (rowIndex < 0)
            {
                MessageBox.Show("Выберите пользлователя чтобы удалить");
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from delete_usr(:_id)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id", int.Parse(dt.Rows[rowIndex][0].ToString()));
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Удаление прошло успешно");
                    rowIndex = -1;
                    conn.Close();
                }
                Select();

            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Deleted fail. Error: " + ex.Message);

            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            NameBox.IsEnabled = SNameBox.IsEnabled = AgeBox.IsEnabled = false;
            int result = 0;
            if (rowIndex < 0)
            {
                try
                {
                    conn.Open();
                    sql = @"select * from insert(:firstname, :lastname, :age)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("firstname", NameBox.Text);
                    cmd.Parameters.AddWithValue("lastname", SNameBox.Text);
                    cmd.Parameters.AddWithValue("age", int.Parse(AgeBox.Text));
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("Добавление прошло успешно");
                        Select();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка добавления");
                    }
                }
                catch(Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Ошибка добавления. Error: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    conn.Open();
                    sql = @"select * from update_users(:_id, :_firstname, :_lastname, :_age)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id", int.Parse(dt.Rows[rowIndex][0].ToString()));
                    cmd.Parameters.AddWithValue("_firstname", NameBox.Text);
                    cmd.Parameters.AddWithValue("_lastname", SNameBox.Text);
                    cmd.Parameters.AddWithValue("_age", int.Parse(AgeBox.Text));
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result == 1)
                    {
                        MessageBox.Show("Обновление прошло успешно");
                        Select();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка обновления");
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Ошибка обновления. Error: " + ex.Message);
                }

            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            if (rowIndex <= 0)
            {
                MessageBox.Show("Выберите пользователя");
            }
            else
            {
               NameBox.IsEnabled = SNameBox.IsEnabled = AgeBox.IsEnabled = true;
            } 
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SearchBox.Text.Length == 0)
            {
                Select();
            }
            else
            {
                try
                {
                    dt.Clear();
                    conn.Open();
                    sql = @"select * from select_search(:text_search)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("text_search", SearchBox.Text + ":*");
                    dt.Load(cmd.ExecuteReader());
                    conn.Close();
                    dgvData.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Ошибка поиска. Error: " + ex.Message);
                }


            }
        }
            
    }

}
 