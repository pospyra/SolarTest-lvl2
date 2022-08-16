namespace тестсолар
{
    public class Person
    {
        public int Id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public DateTime nearestBirthday { get; set; }

        public override string ToString()
        {
            return $"{name} {date.ToString("d")}";
        }
    }
}
