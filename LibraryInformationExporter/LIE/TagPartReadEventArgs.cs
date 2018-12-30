namespace LIE
{
    public class TagPartReadEventArgs
    {
        public string Name { get; private set; }

        public TagPartReadEventArgs(string name)
        {
            Name = name;
        }
    }
}