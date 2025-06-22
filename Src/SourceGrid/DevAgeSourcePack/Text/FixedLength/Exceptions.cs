using System;

namespace DevAge.Text.FixedLength
{
	[Serializable]
	public class InvalidFieldLengthException : DevAgeException
	{
		public InvalidFieldLengthException(int length):
			base("Invalid field length " + length.ToString() + " must be a positive number.")
		{
		}
	}

	[Serializable]
	public class ValueNotValidLengthException : DevAgeException
	{
		public ValueNotValidLengthException(string value, int expectedLength):
			base("Value " + value + " not valid, length must be " + expectedLength.ToString())
		{
		}
	}

	[Serializable]
	public class ValueNotSupportedException : DevAgeException
	{
		public ValueNotSupportedException(string value, Type type):
			base("Value " + value + " not supported, type is " + type.Name)
		{
		}
	}

	[Serializable]
	public class TypeNotSupportedException : DevAgeException
	{
		public TypeNotSupportedException(Type type):
			base("Type " + type.ToString() + " not supported")
		{
		}
	}

	[Serializable]
	public class RegExException : DevAgeException
	{
		public RegExException(string group):
			base("Regular expression group " + group + " not valid")
		{
		}
	}

	[Serializable]
	public class FieldParseException : DevAgeException
	{
		public FieldParseException(string name, string valToParse, Exception innerException):
			base("Failed to parse field " + name + " '" + valToParse + "' - " + innerException.Message, innerException)
		{
		}
	}

	[Serializable]
	public class FieldStringConvertException : DevAgeException
	{
		public FieldStringConvertException(string name, object value, Exception innerException):
			base("Failed to convert to string field " + name + " '" + FieldStringConvertException.ObjectToStringForError(value) + "' - " + innerException.Message, innerException)
		{
		}

		/// <summary>
		/// Returns a string used for error description for a specified object. Usually used when printing the object for the error message when there is a conversion error.
		/// </summary>
		/// <param name="val"></param>
		private static string ObjectToStringForError(object val)
		{
			try
			{
				if (val == null)
					return "<null>";
				else
					return val.ToString();
			}
			catch(Exception)
			{
				return "<object>";
			}
		}
	}


	[Serializable]
	public class FieldNotDefinedException : DevAgeException
	{
		public FieldNotDefinedException(int fieldIndex):
			base("Field " + fieldIndex.ToString() + " not defined.")
		{
		}
	}

	[Serializable]
	public class FailedPropertySetFieldException : DevAgeException
	{
		public FailedPropertySetFieldException(string field, Exception innerException):
			base("Failed to set property for field " + field + " - " + innerException.Message, innerException)
		{
		}
	}

	[Serializable]
	public class FailedPropertyGetFieldException : DevAgeException
	{
		public FailedPropertyGetFieldException(string field, Exception innerException):
			base("Failed to get property for field " + field + " - " + innerException.Message, innerException)
		{
		}
	}
}
