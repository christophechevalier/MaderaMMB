using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Test_envoi_msg
{
    class DoubleTextBox : TextBox
    {
        static DoubleTextBox()
        {
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            e.Handled = !DoubleCharChecker(e.Text);
            base.OnTextInput(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            e.Handled = (e.Key == Key.Space);
            base.OnPreviewKeyDown(e);
        }

        private bool DoubleCharChecker(string str)
        {
            foreach (char c in str)
            {
                if (c.Equals('-'))
                    return true;

                else if (c.Equals('.'))
                    return true;

                else if (Char.IsNumber(c))
                    return true;
            }
            return false;
        }
    }
}
