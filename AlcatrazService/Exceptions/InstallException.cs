﻿namespace AlcatrazService.Exceptions
{
    public class InstallException : Exception
    {
        public InstallException() { }
        public InstallException(string message) : base(message) { }
        public InstallException(string message, Exception inner) : base(message, inner) { }
    }
}
