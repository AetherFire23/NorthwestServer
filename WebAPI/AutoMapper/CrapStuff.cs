namespace WebAPI.AutoMapper
{
    public class SourceMappa
    {
        public string Name { get; set; } = string.Empty;
        public int Same { get; set; }
    }

    public class DestMappa
    {
        public int Same { get; set; }
        public float SomeValue { get; set; }
    }
}
