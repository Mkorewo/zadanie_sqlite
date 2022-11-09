using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace sqlite
{
    internal class DatabaseManager
    {
        private SqliteConnection connection;

        public DatabaseManager()
        {
            var path = @"Data source="+Application.StartupPath + "\\database.db";
            connection = new SqliteConnection(path);
            connection.Open();

            createTbale();
        }

        private void createTbale()
        {
            var command = new SqliteCommand("CREATE TABLE IF NOT EXISTS people (name STRING, surname STRING, age INT)",connection);

            command.ExecuteScalar();
        }
        public void AddPerson(Person person)
        {
            var command = new SqliteCommand("INSERT INTO people (name,surname,age) VALUES (@name,@surname,@age)", connection);

            command.Parameters.AddWithValue("name",person.Name);
            command.Parameters.AddWithValue("surname", person.Surname);
            command.Parameters.AddWithValue("age", person.Age);

            command.ExecuteScalar();

        }
        public void DeletePerson()
        {
            var command = new SqliteCommand("DELETE FROM people WHERE ROWID = (SELECT MAX(ROWID) FROM people);", connection);

            command.ExecuteScalar();
        }
        public List<Person> GetPeople()
        {
            var people = new List<Person>();

            var command = new SqliteCommand("SELECT name,surname,age FROM people", connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                people.Add(new Person
                {
                    Name = reader.GetString(0),
                    Surname = reader.GetString(1),
                    Age = reader.GetInt32(2)

                });

            }

            return people;
        }
        public List<Person> GetPeople(string querry)
        {
            var people = new List<Person>();

            var command = new SqliteCommand($"SELECT name,surname,age FROM people WHERE surname = '{querry}'", connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                people.Add(new Person
                {
                    Name = reader.GetString(0),
                    Surname = reader.GetString(1),
                    Age = reader.GetInt32(2)

                });

            }

            return people;
        }

    }
}
