namespace Global.Exception.Handling.Exceptions
{
    [Serializable]
    public class BadRequestException(string message) : System.Exception(message)
    {
    }
}
