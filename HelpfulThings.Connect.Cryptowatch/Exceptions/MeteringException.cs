﻿using System;

namespace HelpfulThings.Connect.Cryptowatch.Exceptions
{
    public class MeteringException : Exception
    {
        public MeteringException() : base("You are out of request capacity. Please wait for the capacity to reset.") { }
    }
}
