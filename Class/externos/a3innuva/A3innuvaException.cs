namespace WebApiCore.Class.externos.a3innuva
{
    public class A3innuvaException : Exception
    {
        public readonly string message;

        public A3innuvaException(string message)
        {
            this.message = message;
        }
    }
}
