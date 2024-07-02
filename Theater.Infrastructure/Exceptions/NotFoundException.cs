namespace Theater.Infrastructure.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {

        }

        public NotFoundException(string message)
           : base(message)
        {
        }
        public NotFoundException(string message, int posterId)
            : base(message)
        {
        }
    }
}
