namespace Mediator
{
    public class Mediator
    {
//        Employee[] employees = new List<Employee>
//        {
//            RandomDataGenerator.GetNewRandomEmployee(),
//            RandomDataGenerator.GetNewRandomEmployee(),
//            RandomDataGenerator.GetNewRandomEmployee(),
//            RandomDataGenerator.GetNewRandomEmployee(),
//            RandomDataGenerator.GetNewRandomEmployee(),
//            RandomDataGenerator.GetNewRandomEmployee(),
//            RandomDataGenerator.GetNewRandomEmployee(),
//        }.ToArray();
//
//        Console.WriteLine($"Before {employees.Length}"); 
//        var dataMessage = new GetDataMessage
//        {
//            Filters = new List<Filter<Employee>>
//            {
//                new WhereFilter {Predicate = employee => employee.Age < 40},
//                new WhereFilter {Predicate = employee => employee.Age == 10}
//            }
//        };
//        var wireProtocol = new DefaultWireProtocol();
//        var stream = new MemoryStream();
//        wireProtocol.WriteMessage(new DefaultSerializer(stream), dataMessage);
//        var getDataMessage =(GetDataMessage) wireProtocol.ReadMessage(new DefaultDeserializer(new MemoryStream(stream.ToArray())));
//
//        getDataMessage.Filters.ForEach(filter => employees = filter.Execute(employees));
//
//        Console.WriteLine($"After {employees.Length}");
    }
}