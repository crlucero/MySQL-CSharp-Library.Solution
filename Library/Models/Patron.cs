using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public class Patron
    {
        private int _id;
        private string _name;

        public Patron(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO patrons (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Patron Find(int id)
         {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patrons WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int PatronId = 0;
            string PatronName = "";

            while(rdr.Read())
            {
                PatronId = rdr.GetInt32(0);
                PatronName = rdr.GetString(1);
            }

            Patron newPatron = new Patron(PatronName, PatronId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newPatron;
        }

        public static Patron FindByName(string name)
         {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patrons WHERE Name = (@searchName);";
            MySqlParameter searchName = new MySqlParameter();
            searchName.ParameterName = "@searchName";
            searchName.Value = name;
            cmd.Parameters.Add(searchName);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int PatronId = 0;
            string PatronName = "";

            while(rdr.Read())
            {
                PatronId = rdr.GetInt32(0);
                PatronName = rdr.GetString(1);
            }

            Patron newPatron = new Patron(PatronName, PatronId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newPatron;
        }

        public static List<Patron> GetAll()
            {
            List<Patron> allPatrons = new List<Patron> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM patrons;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);

                Patron newPatron = new Patron(name, id);
                allPatrons.Add(newPatron);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allPatrons;
            }

        public void AddBook(Book newBook)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO patrons_books (patron_id, book_id) VALUES (@patronsId, @bookId);";

            MySqlParameter patrons_id = new MySqlParameter();
            patrons_id.ParameterName = "@patronsId";
            patrons_id.Value = _id;
            cmd.Parameters.Add(patrons_id);

            MySqlParameter book_id = new MySqlParameter();
            book_id.ParameterName = "@bookId";
            book_id.Value = newBook.GetId();
            cmd.Parameters.Add(book_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Book> GetBooks()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT books.* FROM patrons
                JOIN patrons_books ON (patrons.id = patrons_books.patron_id)
                JOIN books ON (patrons_books.book_id = books.id)
                WHERE patrons.id = @patronId;";
            MySqlParameter patronIdParameter = new MySqlParameter();
            patronIdParameter.ParameterName = "@patronId";
            patronIdParameter.Value = _id;
            cmd.Parameters.Add(patronIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Book> Books = new List<Book>{};
            while(rdr.Read())
            {
            int BookId = rdr.GetInt32(0);
            string BookName = rdr.GetString(1);
            int BookCopies = rdr.GetInt32(2);
            string BookAuthor = rdr.GetString(3);
            Book newBook = new Book(BookName, BookCopies, BookAuthor, BookId);
            Books.Add(newBook);
            }
            conn.Close();
            if (conn != null)
            {
            conn.Dispose();
            }
            return Books;
            }
    }
}