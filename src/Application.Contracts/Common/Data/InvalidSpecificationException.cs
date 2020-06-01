using System;

namespace SocialMediaLists.Application.Contracts.Common.Data
{
    public class InvalidSpecificationException : Exception
    {
        public InvalidSpecificationException(string message)
            : base(message)
        {
        }
    }
}