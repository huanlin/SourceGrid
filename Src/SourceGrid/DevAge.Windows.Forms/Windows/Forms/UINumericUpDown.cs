#region Copyright

//------------------------------------------------------------------------ 
// Copyright (C) Siemens AG 2017    
//------------------------------------------------------------------------ 
// Project           : UIGrid
// Author            : Sandhra.Prakash@siemens.com
// In Charge for Code: Sandhra.Prakash@siemens.com
//------------------------------------------------------------------------ 

#endregion Copyright

using System.ComponentModel;

namespace DevAge.Windows.Forms
{
    public class UINumericUpDown : System.Windows.Forms.NumericUpDown
    {
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool ReadOnly
        {
            get { return base.ReadOnly; }
            set
            {
                base.ReadOnly = value;
                base.Controls[0].Enabled = !value;
            }
        }
    }
}