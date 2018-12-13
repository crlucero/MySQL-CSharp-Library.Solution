using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Library;

namespace Library.Models
{
    public class Book
    {
        private int _id;
        private string _name;
        private int _copies;
        private string _author;

        public Book(string name, int copies, string author, int id = 0)
        {
            _name = name;
            _copies = copies;
            _author = author;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetCopies()
        {
            return _copies;
        }

        public void SetCopies(int newCopies)
        {
            _copies = newCopies;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetAuthor()
        {
            return _author;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO books (name, copies, author) VALUES (@name, @copies, @author);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            MySqlParameter copies = new MySqlParameter();
            copies.ParameterName = "@copies";
            copies.Value = this._copies;
            cmd.Parameters.Add(copies);

            MySqlParameter author = new MySqlParameter();
            author.ParameterName = "@author";
            author.Value = this._author;
            cmd.Parameters.Add(author);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Book> GetAll()
            {
            List<Book> allBooks = new List<Book> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                int copies = rdr.GetInt32(2);
                string author = rdr.GetString(3);

                Book newBook = new Book(name, copies, author, id);
                allBooks.Add(newBook);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allBooks;
            }

        public static Book Find(int id)
         {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int BookId = 0;
            string BookName = "";
            int BookCopies = 0;
            string BookAuthor = "";

            while(rdr.Read())
            {
                BookId = rdr.GetInt32(0);
                BookName = rdr.GetString(1);
                BookCopies = rdr.GetInt32(2);
                BookAuthor =rdr.GetString(3);
            }

            Book newBook = new Book(BookName, BookCopies, BookAuthor, BookId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newBook;
        }

        public static List<Book> FindAuthor(string author)
         {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books WHERE author = (@searchauthor);";
            MySqlParameter searchauthor = new MySqlParameter();
            searchauthor.ParameterName = "@searchauthor";
            searchauthor.Value = author;
            cmd.Parameters.Add(searchauthor);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int BookId = 0;
            string BookName = "";
            int BookCopies = 0;
            string BookAuthor = "";

            while(rdr.Read())
            {
                BookId = rdr.GetInt32(0);
                BookName = rdr.GetString(1);
                BookCopies = rdr.GetInt32(2);
                BookAuthor =rdr.GetString(3);
            }

            List<Book> foundBooks = new List<Book> ();
            Book newBook = new Book(BookName, BookCopies, BookAuthor, BookId);
            foundBooks.Add(newBook);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundBooks;
        }

         public static Book FindTitle(string name)
         {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM books WHERE name = (@searchname);";
            MySqlParameter searchname = new MySqlParameter();
            searchname.ParameterName = "@searchname";
            searchname.Value = name;
            cmd.Parameters.Add(searchname);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int BookId = 0;
            string BookName = "";
            int BookCopies = 0;
            string BookAuthor = "";

            while(rdr.Read())
            {
                BookId = rdr.GetInt32(0);
                BookName = rdr.GetString(1);
                BookCopies = rdr.GetInt32(2);
                BookAuthor =rdr.GetString(3);
            }

            Book foundBook = new Book(BookName, BookCopies, BookAuthor, BookId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return foundBook;
        }

        public void AddPatron(Patron newPatron)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO patrons_books (book_id, patron_id) VALUES (@booksId, @patronId);";
            
            MySqlParameter books_id = new MySqlParameter();
            books_id.ParameterName = "@booksId";
            books_id.Value = _id;
            cmd.Parameters.Add(books_id);

            MySqlParameter patron_id = new MySqlParameter();
            patron_id.ParameterName = "@patronId";
            patron_id.Value = newPatron.GetId();
            cmd.Parameters.Add(patron_id);
        
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }

        public List<Patron> GetPatrons()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT patrons.* FROM books
                JOIN patrons_books ON (books.id = patrons_books.book_id)
                JOIN patrons ON (patrons_books.patron_id = patrons.id)
                WHERE books.id = @booksId;";
            MySqlParameter booksIdParameter = new MySqlParameter();
            booksIdParameter.ParameterName = "@booksId";
            booksIdParameter.Value = _id;
            cmd.Parameters.Add(booksIdParameter);
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Patron> Patrons = new List<Patron>{};
            while(rdr.Read())
            {
            int PatronId = rdr.GetInt32(0);
            string PatronName = rdr.GetString(1);
            Patron newPatron = new Patron(PatronName, PatronId);
            Patrons.Add(newPatron);
            }
            conn.Close();
            if (conn != null)
            {
            conn.Dispose();
            }
            return Patrons;
        }


        public void EditCopies (int copies)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE books SET copies = @newCopyAmount WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter copy = new MySqlParameter();
            copy.ParameterName = "@newCopyAmount";
            copy.Value = copies;
            cmd.Parameters.Add(copy);

            cmd.ExecuteNonQuery();
            _copies = copies;

            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
        }

        public void DeletePatron(int PatronId)
            {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM patrons_books WHERE (patron_id, book_id) = (@patronId, @bookId);";

            MySqlParameter PatronParameter = new MySqlParameter();
            PatronParameter.ParameterName = "@patronId";
            PatronParameter.Value = PatronId;
            cmd.Parameters.Add(PatronParameter);

            MySqlParameter bookParameter = new MySqlParameter();
            bookParameter.ParameterName = "@bookId";
            bookParameter.Value = this.GetId();
            cmd.Parameters.Add(bookParameter);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }
        

    }
}
