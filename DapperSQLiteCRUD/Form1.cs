using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DapperSQLiteCRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnCreateDatabaseTable_Click(object sender, EventArgs e)
        {
            using (IDbConnection db = GetConnection())
            {
                db.Execute("Create Table Students(ID int, Name varchar(50))");
                MessageBox.Show("Database e table created");
            }
        }
        public IDbConnection GetConnection()
            {
                return new SQLiteConnection(@"Data Source=D:\StudentDB.db; Version=3; New=true");
            }
        public class Student
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(!File.Exists(@"D:\\StudentDB.db"))
            {
                LoadData();
            }
            LoadData();
        }

        private void LoadData()
        {
            using (IDbConnection db = GetConnection())
            {
                var list = db.Query<Student>("select * from Students").ToList();
                if (list.Count() > 0)
                {
                    dataGridView1.DataSource = list;
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            using (IDbConnection db = GetConnection())
            {
                int id = Convert.ToInt32(textBox1.Text);
                string name = textBox2.Text;

              int n =  db.Execute("insert into students(id, name) values(@ID, @Name)", new { ID = id, Name = name });
                if (n > 0)
                {
                    MessageBox.Show("inserted");
                    LoadData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = (int) dataGridView1.CurrentRow.Cells[0].Value;
            using (IDbConnection db = GetConnection())
            {
               int n = db.Execute("delete from students where id=@ID", new { ID = id });
                if (n > 0)
                {
                    MessageBox.Show("deleted");
                    LoadData();
                }

            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = (int)dataGridView1.CurrentRow.Cells[0].Value;
            string name = dataGridView1.CurrentRow.Cells[1].Value.ToString();

            using (IDbConnection db = GetConnection())
            {
                int n = db.Execute("update students set name=@Name where id=@ID", new { Name=name, ID = id });
                if (n > 0)
                {
                    MessageBox.Show("updated");
                    LoadData();
                }

            }
        }
    }
}
