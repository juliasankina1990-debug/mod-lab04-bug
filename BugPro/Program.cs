using System;
using Stateless;


namespace BugPro
{
    public class Bug
    {
        // заглушка
        public Bug() { }
        public void Analyze() { }
        public void Defer() { }
        public void WontFix() { }
        public void SeparateSolution() { }
        public void Duplicate() { }
        public void OtherProduct() { }
        public void NeedInfo() { }
        public void CannotReproduce() { }
        public void StartFix() { }
        public void Verify() { }
        public void Yes() { }
        public void No() { }
        public void Reopen() { }
        public string GetState() => "NewDefect";
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bug workflow simulator (stub)");
            var bug = new Bug();
            Console.WriteLine($"Current state: {bug.GetState()}");
        }
    }
}
