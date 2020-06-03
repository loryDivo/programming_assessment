﻿using System;
using System.IO;

namespace Programming_Assessment
{
    public abstract class Parser <T>
    {
        protected String path;
        protected String markupString;
        protected String baseDirectory = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        public Parser(String path)
        {
            this.path = Path.Combine(baseDirectory, path);
        }
        public abstract void LoadFile();
    }
}
