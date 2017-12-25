// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="CouchbaseSample">
//   Couchbase Sample
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CouchbaseSample
{
    using System;
    using System.Collections.Generic;
    using Couchbase;
    using Couchbase.Authentication;
    using Couchbase.Configuration.Client;

    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary> The main. </summary>
        /// <param name="args"> The args. </param>
        public static void Main(string[] args)
        {
            var cluster = new Cluster(new ClientConfiguration
            {
                Servers = new List<Uri> { new Uri("http://localhost:8091") }
            });

            var authenticator = new PasswordAuthenticator("CB_Test", "123456");
            cluster.Authenticate(authenticator);

            using (var bucket = cluster.OpenBucket("CB_Test"))
            {
                var document = new Document<List<Person>>
                {
                    Id = "PersonList",
                    Content = new List<Person>()
                    {
                        new Person
                        {
                            DateOfBirth = new DateTime(1990,1,1),Gender = "Male",Name = "Cony",Surname = "Mony"
                        },
                        new Person
                        {
                            DateOfBirth = new DateTime(1990,2,2),Gender = "Female",Name = "Helga",Surname = "Melga"
                        },
                        new Person
                        {
                            DateOfBirth = new DateTime(1990,3,3),Gender = "Male",Name = "Hans",Surname = "Mans"
                        },
                        new Person
                        {
                            DateOfBirth = new DateTime(1990,4,4),Gender = "Female",Name = "Olga",Surname = "Molga"
                        }
                    }
                };

                var upsert = bucket.Upsert(document);
                if (upsert.Success)
                {
                    var get = bucket.GetDocument<List<Person>>(document.Id);
                    document = get.Document;

                    foreach (var person in document.Content)
                    {
                        Console.WriteLine("{0} {1} | {2} | {3}", person.Name, person.Surname, person.Gender, person.DateOfBirth);
                    }
                }

                Console.Read();
            }
        }

        /// <summary> The person. </summary>
        public class Person
        {
            /// <summary> Gets or sets the name. </summary>
            public string Name { get; set; }

            /// <summary> Gets or sets the surname. </summary>
            public string Surname { get; set; }

            /// <summary> Gets or sets the gender. </summary>
            public string Gender { get; set; }

            /// <summary> Gets or sets the date of birth. </summary>
            public DateTime DateOfBirth { get; set; }
        }
    }
}
