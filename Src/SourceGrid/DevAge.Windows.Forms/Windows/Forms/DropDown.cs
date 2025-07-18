#region Copyright

/*SourceGrid LICENSE (MIT style)

Copyright (c) 2005 - 2012 http://sourcegrid.codeplex.com/, Davide Icardi, Darius Damalakas

Permission is hereby granted, free of charge, to any person obtaining 
a copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the 
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included 
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
DEALINGS IN THE SOFTWARE. */

//------------------------------------------------------------------------ 
// Copyright (C) Siemens AG 2016    
//------------------------------------------------------------------------ 
// Project           : UIGrid
// Author            : Sandhra.Prakash@siemens.com
// In Charge for Code: Sandhra.Prakash@siemens.com
//------------------------------------------------------------------------ 

/*Changes :
 * 1. CalcLocation() : Sandhra.prakash@siemens.com :The minimum width of panel should be the size 
                of the cell  but if the control width is more than size of cell the dropdown
                width should be that of control's
 * 2. CloseDropDown, OnDropDownClosed, DropDownClosed, DropDownClosedEventArgs: To notify the user if the dropdown was closed due to a
 * (pressing Escape key)cancel action or by changing focus or pressing Enter key which corresponds to an accept action.
*/
#endregion Copyright

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DevAge.Windows.Forms
{
	/// <summary>
	/// Summary description for DropDownCustom.
	/// </summary>
	public class DropDown : System.Windows.Forms.Form
	{
		private Point StartLocation = new Point(0,0);
		private System.Windows.Forms.Panel panelContainer;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DropDown()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

        /// <summary>
        /// Constructor to create a dropdown form used to display the innerControl specified.
        /// It is responsability of the caller to dispose the innerControl.
        /// </summary>
        /// <param name="innerControl"></param>
        /// <param name="parentControl"></param>
        /// <param name="owner"></param>
		public DropDown(Control innerControl, Control parentControl, System.Windows.Forms.Form owner):this()
		{
			Owner = owner;
			m_InnerControl = innerControl;
			m_ParentControl = parentControl;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelContainer = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panelContainer
			// 
			this.panelContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelContainer.Location = new System.Drawing.Point(0, 0);
			this.panelContainer.Name = "panelContainer";
			this.panelContainer.Size = new System.Drawing.Size(84, 48);
			this.panelContainer.TabIndex = 0;
			// 
			// ctlDropDownCustom
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(84, 48);
			this.ControlBox = false;
			this.Controls.Add(this.panelContainer);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ctlDropDownCustom";
			this.ShowInTaskbar = false;
			this.Text = "ctlDropDownCustom";
            this.Visible = false;
            this.StartPosition = FormStartPosition.Manual;
			this.Deactivate += new System.EventHandler(this.DropDown_Deactivate);
            this.Layout += new LayoutEventHandler(DropDown_Layout);
			this.ResumeLayout(false);

		}
		#endregion

		private Control m_ParentControl = null;
		private Control m_InnerControl = null;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control ParentControl
		{
			get{return m_ParentControl;}
			set{m_ParentControl = value;}
		}

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Control InnerControl
		{
			get{return m_InnerControl;}
			set{m_InnerControl = value;}
		}

        void DropDown_Layout(object sender, LayoutEventArgs e)
        {
            SuspendLayout();
            CalcLocation();
            ResumeLayout(false);
        }

		private void CalcLocation()
		{
			if (m_InnerControl != null && m_ParentControl != null)
			{
				//m_InnerControl.Width = Math.Max(m_ParentControl.Width,m_InnerControl.Width);
                //m_InnerControl.Width = Math.Max(m_ParentControl.Width, m_InnerControl.Width);
                //m_InnerControl.Location = new Point(0, 0);

                //Sandhra.prakash@siemens.com :The minimum width of panel should be the size 
                //of the cell  but if the control width is more than size of cell the dropdown
                //width should be that of control's
			    const int offset = 2;
                Size = new Size(Math.Max(m_ParentControl.Width,m_InnerControl.Width), 
                    Math.Max(m_ParentControl.Height,m_InnerControl.Height) + offset);
				//Size = m_InnerControl.Size;
			}

			Rectangle parentRectangle = m_ParentControl.RectangleToScreen(m_ParentControl.ClientRectangle);

			// Determine which screen we're on and how big it is.
			Screen DisplayedOnScreen = Screen.FromPoint(new Point(parentRectangle.X, parentRectangle.Bottom));
			int MinScreenXPos = DisplayedOnScreen.Bounds.X;
			//int MinScreenYPos = DisplayedOnScreen.Bounds.Y;
			int MaxScreenXPos = DisplayedOnScreen.Bounds.X + DisplayedOnScreen.Bounds.Width;
			int MaxScreenYPos = DisplayedOnScreen.Bounds.Y + DisplayedOnScreen.Bounds.Height;

			int DropdownWidth  = Width; //CalcWidth();
			int DropdownHeight = Height; //CalcHeight();

			// Will we bump into the right edge of the window when we first display the control?
			if((parentRectangle.X + DropdownWidth) <= MaxScreenXPos )
			{
				if( parentRectangle.X < MinScreenXPos )
					StartLocation.X = MinScreenXPos;
				else
					StartLocation.X = parentRectangle.X;
			}
			else
			{
				//DrawLeftToRight = false;

				// Make sure we aren't overhanging the left side of the screen.
				if( Screen.FromPoint(new Point((parentRectangle.X + parentRectangle.Width), 0)) == DisplayedOnScreen )
					StartLocation.X = parentRectangle.Right-DropdownWidth;
				else
					StartLocation.X = MaxScreenXPos - DropdownWidth;
			}

			// And now check the bottom of the screen.
			if( (parentRectangle.Bottom + DropdownHeight) <= MaxScreenYPos )
				StartLocation.Y = parentRectangle.Bottom;
			else
			{
				//DrawTopToBottom = false;
				StartLocation.Y = parentRectangle.Y-DropdownHeight;
			}

			this.Location = StartLocation;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ( (m_Flags & DropDownFlags.CloseOnEscape) == DropDownFlags.CloseOnEscape)
			{
				if (keyData == Keys.Escape)
				{
					DialogResult = DialogResult.Cancel;
					CloseDropDown();
					//return true; altrimenti alcuni controlli che gestiscono i tasti non funzionano bene (ad esempio i controlli UITypeEditor)
				}
			}

			if ( (m_Flags & DropDownFlags.CloseOnEnter) == DropDownFlags.CloseOnEnter)
			{
				if (keyData == Keys.Enter)
				{
					DialogResult = DialogResult.OK;
					CloseDropDown();
					//return true; altrimenti alcuni controlli che gestiscono i tasti non funzionano bene (ad esempio i controlli UITypeEditor)
				}
			}

			return base.ProcessCmdKey(ref msg,keyData);
		}

		private DropDownFlags m_Flags = DropDownFlags.CloseOnEnter | DropDownFlags.CloseOnEscape;
        
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DropDownFlags DropDownFlags
		{
			get{return m_Flags;}
			set{m_Flags = value;}
		}

		private void DropDown_Deactivate(object sender, System.EventArgs e)
		{
			CloseDropDown();
		}

		private bool m_bShowed = false;
		public void ShowDropDown()
		{
			if (m_bShowed)
				return;
			m_bShowed = true;

			if (InnerControl == null)
				throw new Exception("InnerControl is null");
			if (ParentControl == null)
				throw new Exception("ParentControl is null");
			if (Owner == null)
				throw new Exception("Owner is null");

			OnDropDownOpen(EventArgs.Empty);
		}

		public void CloseDropDown()
		{
			if (m_bShowed == false)
				return;
            // sandhra.prakash@siemens.com: To notify the user if the dropdown was closed due to a
            // (pressing Escape key)cancel action or by changing focus or pressing Enter key which corresponds to an accept action.
            DropDownClosedEventArgs dropDownClosedEventArgs = new DropDownClosedEventArgs(DialogResult == DialogResult.Cancel);
            OnDropDownClosed(dropDownClosedEventArgs);

			m_bShowed = false;
		}

		protected virtual void OnDropDownOpen(EventArgs e)
		{
            if (DropDownOpen != null)
                DropDownOpen(this, e);

            panelContainer.Controls.Add(m_InnerControl);

            try
			{
                CalcLocation();

				Show();

                //This code simulate a ShowDialog. ShowDialog cannot be used because I need to receive the deactivate event to close the window.
                // This is not the best solution anyway because the parent for is deactivated and the user experience it is not the best.
				while(m_bShowed)
				{
					Application.DoEvents();
					System.Threading.Thread.Sleep(1); //To prevent the CPU to work on 100%
				}
			}
			finally
			{
				//I must remove the control because UITypeEditor doesn't support dispose and the Controls collection is automatically disposed
				panelContainer.Controls.Remove(m_InnerControl);
			}
		}

        
        /// <summary>
        /// sandhra.prakash@siemens.com: To notify the user if the dropdown was closed due to a
        /// (pressing Escape key)cancel action or by changing focus or pressing Enter key which corresponds to an accept action.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDropDownClosed(DropDownClosedEventArgs e)
		{
			Owner.Activate();

			Hide();

			if (DropDownClosed != null)
				DropDownClosed(this, e);
		}

		public event EventHandler DropDownOpen;

	    /// <summary>
	    /// sandhra.prakash@siemens.com: To notify the user if the dropdown was closed due to a
	    /// (pressing Escape key)cancel action or by changing focus or pressing Enter key which corresponds to an accept action.
	    /// </summary>
	    public event EventHandler<DropDownClosedEventArgs> DropDownClosed;
	}

	[Flags]
	public enum DropDownFlags
	{
		None = 0,
		/// <summary>
		/// Close the DropDown whe the user press the escape key, return DialogResult.Cancel
		/// </summary>
		CloseOnEscape = 1,
		/// <summary>
		/// Close the DropDown whe the user press the enter key, return DialogResult.OK
		/// </summary>
		CloseOnEnter = 2
	}

    /// <summary>
    /// To notify the user if the dropdown was closed due to a
    /// (pressing Escape key)cancel action or by changing focus or pressing Enter key which corresponds to an accept action.
    /// </summary>
    public class DropDownClosedEventArgs : EventArgs
    {
        public DropDownClosedEventArgs(bool cancelled)
        {
            Cancelled = cancelled;
        }

        public bool Cancelled { get; private set; }

    }
}
