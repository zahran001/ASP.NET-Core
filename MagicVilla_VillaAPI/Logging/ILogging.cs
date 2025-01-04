namespace MagicVilla_VillaAPI.Logging
{
    // for logging - we create an interface first
    public interface ILogging
    {
        // add one method
        public void Log(string message, string type);
    }
}
