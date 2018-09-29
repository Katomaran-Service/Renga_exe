using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace SearchTextBox
{
    public class SearchEventArgs : RoutedEventArgs
    {
        private string m_keyword = "";

        public string Keyword
        {
            get { return m_keyword; }
            set { m_keyword = value; }
        }
        private List<string> m_sections = new List<string>();

        public List<string> Sections
        {
            get { return m_sections; }
            set { m_sections = value; }
        }
        public SearchEventArgs() : base()
        {

        }
        public SearchEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {

        }
    }
    public class SearchTextBox
    {

    }
}
