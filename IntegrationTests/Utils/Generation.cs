namespace IntegrationTests.Utils;

public static class Generation
{
    private static List<string> _wordList = File.ReadAllLines("usernames.txt").Where(x => x.Length > 4).ToList();
    //private static List<string> _wordList = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)).Where(x => x.Length > 4).ToList();
    public static string CreateRandomUserName(string suffix = "")
    {
        string randomName1 = _wordList.ElementAt(Random.Shared.Next(0, _wordList.Count));
        string randomName2 = _wordList.ElementAt(Random.Shared.Next(0, _wordList.Count));
        string names = $"{randomName1}-{randomName2}-";
        string guidString = Guid.NewGuid().ToString().Substring(0, 4);


        string userName = $"{names}{guidString}";
        return userName;
    }

    public static string GenerateEmail(string userName)
    {
        string email = $"{userName}@bogusemail.com";
        return email;
    }
}
