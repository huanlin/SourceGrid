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
// Copyright (C) Siemens AG 2017    
//------------------------------------------------------------------------ 
// Project           : UIGrid
// Author            : Sandhra.Prakash@siemens.com
// In Charge for Code: Sandhra.Prakash@siemens.com
//------------------------------------------------------------------------ 

/*Changes :
 * 1. sandhra.prakash@siemens.com: Readonly property added to decide whether to go into edit mode in readonly state. enhancement : selectable readonly text
 * 2. InternalEndEdit, InternalStartEdit: Made it protected internal, so as to override it in UIGrid.enhancement : checkbox cell with icon
 * 3. chinnari.prasad@siemens.com : Method SetCellValue is changed to restric the user to edit the data if the cell is readonly. enhancement : Enable clipboard modes
*/

#endregion Copyright

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SourceGrid.Cells.Editors
{
	/// <summary>
	/// Represents the base class of a DataModel. This DataModel support conversion but doesn't provide any user interface editor.
	/// </summary>
    [System.ComponentModel.ToolboxItem(false)]
    public class EditorBase : DevAge.ComponentModel.Validator.ValidatorTypeConverter
	{
		#region Constructor
		/// <summary>
		/// Construct a Model. Based on the Type specified the constructor populate StringEditor property
		/// </summary>
		/// <param name="p_Type">The type of this model</param>
		public EditorBase(Type p_Type):base(p_Type)
		{
		}
		#endregion
		
		#region Edit coordinates
		//Queste variabili vengono usato durante le fasi di editing
		private CellContext mEditCellContext = CellContext.Empty;

		/// <summary>
		/// Cell in editing, if null no cell is in editing state
		/// </summary>
		public CellContext EditCellContext
		{
			get{return mEditCellContext;}
		}

		/// <summary>
		/// Cell in editing, if null no cell is in editing state
		/// </summary>
		public Cells.ICellVirtual EditCell
		{
			get{return mEditCellContext.Cell;}
		}
		/// <summary>
		/// Cell in editing, if Empty no cell is in editing state
		/// </summary>
		public Position EditPosition
		{
			get{return mEditCellContext.Position;}
		}

		/// <summary>
		/// Set the current editing cell, for an editor only one cell can be in editing state
		/// </summary>
		/// <param name="cellContext"></param>
		protected void SetEditCell(CellContext cellContext)
		{
			mEditCellContext = cellContext;
		}
		#endregion

		#region ErrorString
		/// <summary>
		/// Error rappresentation
		/// </summary>
		private string m_ErrorString = "#ERROR!";

		/// <summary>
		/// Returns true if the string passed is equal to the error string rappresentation
		/// </summary>
		/// <param name="p_str"></param>
		/// <returns></returns>
		public bool IsErrorString(string p_str)
		{
			if (p_str == ErrorString)
				return true;
			else
				return false;
		}
        /// <summary>
        /// String used when error occurred
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ErrorString
		{
			get{return m_ErrorString;}
			set{m_ErrorString = value;}
		}
		#endregion

		#region Editable settings
		private bool m_bEnableEdit = true;
        /// <summary>
        /// Enable or disable the cell editor (if disable no visual edit is allowed)
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableEdit
		{
			get{return m_bEnableEdit;}
			set{m_bEnableEdit = value;}
		}

		private EditableMode m_EditableMode = EditableMode.Default;
        /// <summary>
        /// Mode to edit the cell.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EditableMode EditableMode
		{
			get{return m_EditableMode;}
			set{m_EditableMode = value;}
		}

		private bool m_bEnableCellDrawOnEdit = true;

        /// <summary>
        /// Indicates if the draw of the cell when in editing mode is enabled.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool EnableCellDrawOnEdit
		{
			get{return m_bEnableCellDrawOnEdit;}
			set{m_bEnableCellDrawOnEdit = value;}
		}

        private bool mUseCellViewProperties = true;

        /// <summary>
        /// Gets or sets if the editor must assign to the editor control the default view properties: ForeColor, BackColor, Font. This can be disabled if you want to manually assign these properties to the control.
        /// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool UseCellViewProperties
        {
            get { return mUseCellViewProperties; }
            set { mUseCellViewProperties = value; }
        }

        /// <summary>
        /// Gets or sets whether the editor should open in readonly state or not.
        /// sandhra.prakash@siemens.com: enhancement : selectable readonly text
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ReadOnly { get; set; }

		#endregion

		#region StartEdit/EndEdit/IsEditing/ApplyEdit/GetEditedValue
		/// <summary>
		/// Indicates if the current editor is in editing state
		/// </summary>
		public bool IsEditing
		{
			get{return (EditCell != null);}
		}

		/// <summary>
		/// Start editing the cell passed. Do not call this method for start editing a cell, you must use CellContext.StartEdit.
        /// sandhra.prakash@siemens.com: Made it protected internal, so as to override it in UIGrid.
		/// </summary>
		/// <param name="cellContext">Cell to start edit</param>
		protected internal virtual void InternalStartEdit(CellContext cellContext)
		{
			if (cellContext.Cell == null)
				throw new ArgumentNullException("cellContext.Cell");
			if (cellContext.Grid == null)
				throw new ArgumentNullException("cellContext.Grid");
			if (cellContext.Grid.Selection.ActivePosition != cellContext.Position)
				throw new SourceGridException("Cell must have the focus");

			//no edit supported for this editor
		}


		/// <summary>
		/// Apply edited value
		/// </summary>
		/// <returns></returns>
		public virtual bool ApplyEdit()
		{
			return true;
		}
		/// <summary>
		/// Cancel the edit action. Do not call this method directly, use the CellContext.EndEdit instead.
		/// sandhra.prakash@siemens.com: Made it protected internal, so as to override it in UIGrid.
		/// </summary>
		/// <param name="cancel">True to cancel the editing and return to normal mode, false to call automatically ApplyEdit and terminate editing</param>
		/// <returns>Returns true if the cell terminate the editing mode</returns>
		protected internal virtual bool InternalEndEdit(bool cancel)
		{
			return true;
		}


		/// <summary>
		/// Returns the new value edited with the custom control
		/// </summary>
		/// <returns></returns>
		public virtual object GetEditedValue()
		{
			throw new SourceGridException("No valid cell editor found");
		}
	
		#endregion

		#region ClearCell/SetCellValue
		/// <summary>
		/// Clear the value of the cell using the default value
		/// </summary>
		/// <param name="cellContext"></param>
		public virtual void ClearCell(CellContext cellContext)
		{
			SetCellValue(cellContext, DefaultValue);
		}

		/// <summary>
		/// Change the value of the cell applying the rule of the current editor. Is recommend to use this method to simulate a edit operation and to validate the cell value using the current model.
		/// Doesn't call the StartEdit and EndEdit but change directly the cell value. Use the CellContext.Start edit to begin an edit operation.
		/// </summary>
		/// <param name="cellContext">Cell to change the value</param>
		/// <param name="p_NewValue"></param>
		/// <returns>returns true if the value passed is valid and has been applied to the cell</returns>
		public virtual bool SetCellValue(CellContext cellContext, object p_NewValue)
		{
		    if (cellContext.Cell == null)
                throw new SourceGridException("Invalid CellContext, cell is null");

            //chinnari.prasad@siemens.com : to restrict the user to edit the data if the cell is readonly
            if (EnableEdit)
            {
                if (ReadOnly)
                {
                    //chinnari.prasad@siemens.com: return true in case its readonly. Else cell wont be able to end edit mode.
                    return true;
                }
                ValidatingCellEventArgs l_cancelEvent = new ValidatingCellEventArgs(cellContext, p_NewValue);
                OnValidating(l_cancelEvent);
                
                //check if cancel == true 
                if (l_cancelEvent.Cancel == false)
                {
                    object l_PrevValue = cellContext.Cell.Model.ValueModel.GetValue(cellContext);
                    try
                    {
                        cellContext.Cell.Model.ValueModel.SetValue(cellContext, ObjectToValue(l_cancelEvent.NewValue));
                        OnValidated(new CellContextEventArgs(cellContext));
                    }
                    catch (Exception err)
                    {
                        OnEditException(new ExceptionEventArgs(err));
                        cellContext.Cell.Model.ValueModel.SetValue(cellContext, l_PrevValue);
                        l_cancelEvent.Cancel = true;//di fatto ?fallita la validazione del dato
                    }
                }

                return (l_cancelEvent.Cancel == false);
            }
            else
                return false;
		}

	    #endregion

		#region Validating

        //TODO: Remove OnValidated and OnValidating methods. For a better user experience (focus, validating, ...) must be used the validating event of the Control. The cell simply check if the value is valid but cannot handle correctly the user interface.

		/// <summary>
		/// Functions used when the validating operation is finished
		/// </summary>
		/// <param name="e"></param>
        protected void OnValidated(CellContextEventArgs e)
		{
			if (m_Validated!=null)
				m_Validated(this,e);
		}
		/// <summary>
		/// Validating the value of the cell.
		/// </summary>
		/// <param name="e"></param>
		protected void OnValidating(ValidatingCellEventArgs e)
		{
			if (m_Validating!=null)
				m_Validating(this,e);
		}

		private event ValidatingCellEventHandler m_Validating;
		private event CellContextEventHandler m_Validated;
		/// <summary>
		/// Validating event
		/// </summary>
        [Obsolete("You should use the Control.Validating event")]
        public event ValidatingCellEventHandler Validating
		{
			add{m_Validating += value;}
			remove{m_Validating -= value;}
		}
		/// <summary>
		/// Validated event
		/// </summary>
        [Obsolete("You should use the Control.Validated event")]
        public event CellContextEventHandler Validated
		{
			add{m_Validated += value;}
			remove{m_Validated -= value;}
		}

		/// <summary>
		/// Event fired when an exception is throw in the Validated event or in an editing method
		/// </summary>
		public event ExceptionEventHandler EditException;

		/// <summary>
		/// Event fired when an exception is throw in the Validated event or in an editing method
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnEditException(ExceptionEventArgs e)
		{
#if DEBUG
			System.Diagnostics.Debug.WriteLine("Exception on editing cell: " + e.Exception.ToString());
#endif

			if (EditException!=null)
				EditException(this, e);
		}
		#endregion

		#region Send Keys
		/// <summary>
		/// Used to send some keyboard keys to the active editor. It is only valid when there is an active edit operations.
		/// </summary>
		/// <param name="key"></param>
		public virtual void SendCharToEditor(char key)
		{
			//No Action
		}
		#endregion

        #region Minimum Size
        /// <summary>
        /// Calculate the minimum required size for the specified editor cell.
        /// </summary>
        /// <param name="cellContext"></param>
        /// <returns></returns>
        public virtual System.Drawing.Size GetMinimumSize(CellContext cellContext)
        {
            return System.Drawing.Size.Empty;
        }
        #endregion

    }
}
