namespace PolizaWebAPI.Domain.Exceptions;
[Serializable]
public class UnsupportedPolizaException : Exception
{
    public UnsupportedPolizaException(string message)
        : base(message)
    {
    }
}