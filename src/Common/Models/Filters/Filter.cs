namespace Common.Models.Filters
{
    public abstract class Filter
    {
        public abstract Employee[] Execute(Employee[] employees);
    }
}