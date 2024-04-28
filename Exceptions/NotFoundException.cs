namespace Global.Exception.Handling.Exceptions
{
    [Serializable]
    public class NotFoundException(string message) : System.Exception(message)
    {
    }
}
