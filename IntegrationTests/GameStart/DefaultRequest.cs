namespace IntegrationTests.GameStart;

public static class DefaultRequest
{
    public static RegisterRequest DefaultLocalPlayerRequest = new RegisterRequest()
    {
        Email = "test@gmail.com",
        Password = "password",
        UserName = "username",
    };
}
