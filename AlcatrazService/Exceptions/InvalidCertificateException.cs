namespace AlcatrazService.Exceptions
{
    public class InvalidCertificateException : Exception
    {
        public InvalidCertificateException() { }
        public InvalidCertificateException(string message) : base(message) { }
        public InvalidCertificateException(string message, Exception inner) : base(message, inner) { }
    }
}
