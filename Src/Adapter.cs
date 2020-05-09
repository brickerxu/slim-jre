using System;
using System.Windows.Controls;

namespace slim_jre
{
    public class Adapter
    {
        public delegate void DAppendText(string text);

        public static DAppendText dAppendText;

        
    }
}