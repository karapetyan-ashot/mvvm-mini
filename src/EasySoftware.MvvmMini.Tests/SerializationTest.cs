using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasySoftware.MvvmMini.Tests
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void ModelSerializationTest()
        {
            var errors = new Dictionary<string, IEnumerable<string>>
            {
                { "Name", new List<string> { "Name error 2", "Name error 2" } }
            };
            Person person = new Person { Name = "gugush", Errors = errors };
            string json = JsonSerializer.Serialize(person);
            
            var model = JsonSerializer.Deserialize<PersonModel>(json);
            
            Assert.IsNotNull(model);
            Assert.AreEqual(person.Name, model.Name);
            Assert.IsTrue(model.HasErrors);

            model.AddError("Name", "Name error 3");

            Assert.AreEqual(1, model.Errors.Count);
            Assert.AreEqual(3, model.Errors["Name"].Count());

        }
    }

    public class PersonModel: ModelBase
    {
        public string Name { get; set; }
    }

    public class Person
    {
        public string Name { get; set; }
        public IReadOnlyDictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
