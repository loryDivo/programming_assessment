using System;
using System.IO;

namespace Programming_Assessment
{
    public abstract class Parser <T>
    {
        protected String Path;
        protected String MarkupString { get; set; }
        protected readonly String BaseDirectory = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        public Parser(String iPath)
        {
            this.Path = System.IO.Path.Combine(BaseDirectory, iPath);
        }
        public abstract void LoadFile(String iFileName);
    }
}
