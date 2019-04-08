namespace LIE.KnowledgeBaseTypes
{
    public class KnowledgeBaseLoaded
    {
        public KnowledgeBaseLoaded()
        {
            Excludes=new ExcludesLoaded();
            Spliters= new SplitersLoaded();
            Transforms= new TransformsLoaded();
            Rules= new RulesLoaded();
        }
        public ExcludesLoaded Excludes { get; set; }

        public SplitersLoaded Spliters { get; set; }

        public TransformsLoaded Transforms { get; set; }

        public RulesLoaded Rules { get; set; }
    }
}