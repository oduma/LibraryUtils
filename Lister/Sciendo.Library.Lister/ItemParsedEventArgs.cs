using System;

namespace Sciendo.Library.Lister
{
    public class ItemParsedEventArgs: EventArgs
    {
        public string ItemParsedMessage { get; private set; }

        public ItemParsedEventArgs(string itemParsedMessage)
        {
            ItemParsedMessage = itemParsedMessage;
        }
    }
}