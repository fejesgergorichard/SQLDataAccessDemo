using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace FormUI
{
    public class DataAccess
    {
        public List<Person> GetPeople(string lastName)
        {
            // get a sqlconnection via the given connectionstring
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("SampleDB")))
            {
                // SQL injection is very dangerous so we should avoid that
                //var output = connection.Query<Person>($"SELECT * FROM People WHERE LastName = '{lastName}'").ToList();

                // In order to avoid SQL Injection we use a stored procedure called People_GetByLastName and pass it a parameter
                // via a dynamic class (anonymus class)
                // dynamic class syntax:    new { PropertyName = propertyValue }
                var output = connection.Query<Person>("dbo.People_GetByLastName @LastName", new { LastName = lastName }).ToList();
                return output;
            }
        }

        public void InsertPerson(string firstName, string lastName, string emailAddress, string phoneNumber)
        {
            Person person = new Person
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                PhoneNumber = phoneNumber
            };

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(Helper.CnnVal("SampleDB")))
            {
                connection.Execute("dbo.People_Insert @FirstName, @LastName, @EmailAddress, @PhoneNumber", person);
            }
        }
    }
}
