namespace GZipTest
{
    internal class Program
    {
        //  C:\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\MachineName\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\?\C:\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\?\UNC\MachineName\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\127.0.0.1\Work\Node.js Design Patterns - Second Edition.pdf
        //  \\?\UNC\127.0.0.1\Work\Node.js Design Patterns - Second Edition.pdf


        private static void Main(string[] args)
        {
            var test = Files.File.OpenRead(@"\\?\C:\Work\Node.js Design Patterns - Second Edition.pdf", 4096, false);

        }
    }
}