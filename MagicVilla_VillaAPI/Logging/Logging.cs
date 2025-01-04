namespace MagicVilla_VillaAPI.Logging
{
    // a class that implements the ILogging interface
    public class Logging : ILogging
    {
        public void Log(string message, string type)
        {
            // implement the method
            if(type == "error")
            {
                Console.WriteLine("Error: " + message);
            }
            else
            {
                Console.WriteLine("Info: " + message);
            }
        }
    }
}
