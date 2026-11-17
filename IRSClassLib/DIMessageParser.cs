using System;
using System.Collections.Generic;
using System.Text;

using System.Configuration;
using System.Collections;

namespace IRSClassLib
{
	/// <summary>
	/// Summary description for DIMessageParser.
	/// </summary>
	public class DIMessageParser : System.Object, IDisposable
	{
		protected Array m_aMessages;
//		protected Array m_aPassbookEntries;
		protected string m_DIMessage;
		protected PassbookEntryCollection m_PassbookEntries;

		private DIMessageParser()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public DIMessageParser(string Message)
		{
			m_DIMessage = Message;
			this.Parse();
		}

		protected virtual void Parse()
		{
			int iPos = 0;
			ArrayList aTemp;
			ArrayList aTempPassbookEntries;
			PassbookEntryCollection pecTemp;
			

			pecTemp = new PassbookEntryCollection();

			string sTemp = "";

			string strDelimiter = System.Configuration.ConfigurationSettings.AppSettings["Delimiter"];

			if( m_DIMessage.Length < 2)
			{
				return;
			}
//			else if(m_DIMessage.Substring(0, 2) != strDelimiter)
//			{
//				return;
//			}
			try
			{

				sTemp = m_DIMessage;
				//
				iPos = sTemp.IndexOf(strDelimiter);
				if (iPos > 0) 
				{
					sTemp = m_DIMessage.Substring(iPos, m_DIMessage.Length - iPos);  
				}
				else if (iPos == -1)	//No '5C' was found
				{
					return;
				}
				//

				//sTemp = m_DIMessage;
				aTemp = new ArrayList();
				aTempPassbookEntries = new ArrayList();

				sTemp = sTemp.Replace('\n',  ' ');

				while(sTemp.Length > 0)
				{
					sTemp = sTemp.Substring(2, sTemp.Length - 2); //Strip of the first "5C"
					iPos = sTemp.IndexOf(strDelimiter);
					
					if(iPos > 0)
					{
						if(sTemp.Substring(0, 3).Equals("069") || sTemp.Substring(0, 3).Equals("068"))
						{
							pecTemp.Add(new PassbookEntry(sTemp.Substring(4, iPos - 6).Trim(), sTemp.Substring(0, 3).Equals("068")? 0 : 1));
							//							aTempPassbookEntries.Add(new PassbookEntry(sTemp.Substring(4, iPos - 6).Trim()));
							
						}
						else
						{
							aTemp.Add(sTemp.Substring(14, iPos - 16).Trim());
						}
						
						//sTemp = sTemp.Substring(iPos, sTemp.Length - (iPos) + 1);
						sTemp = sTemp.Substring(iPos);
					}
					else
					{
						aTemp.Add(sTemp.Substring(14));
						sTemp = string.Empty;
					}
				}
				m_aMessages = aTemp.ToArray();
				//				m_aPassbookEntries = aTempPassbookEntries.ToArray();
				m_PassbookEntries = pecTemp;

				

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				aTemp = null;
				aTempPassbookEntries = null;
				sTemp = string.Empty;
				pecTemp = null;
				//m_aMessages = null;
			}
		}

		public Array Items
		{
			get
			{
				return m_aMessages;
			}
		}

		public PassbookEntryCollection PassbookEntries
		{
			get
			{
				return m_PassbookEntries;
			}
		}

		public void Dispose() 
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		protected virtual void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				m_aMessages = null;
				// Free other state (managed objects).
			}
			// Free your own state (unmanaged objects).
			// Set large fields to null.
		}

		~DIMessageParser()
		{
			Dispose(false);
		}
		//		[System.Runtime.CompilerServices.IndexerName("Messages")]
		//		public string this[int index]
		//		{
		//			get
		//			{
		//				return m_aMessages.GetValue(index).ToString();
		//			}
		//			set
		//			{
		//			}
		//		}

	}

	public class PassbookEntryCollection : IDisposable
	{
		ArrayList m_Entries;

		internal PassbookEntryCollection()
		{
			m_Entries = new ArrayList();
		}

		internal void Add(PassbookEntry pe)
		{
			m_Entries.Add(pe);
			
		}

		public PassbookEntry GetPassbookEntry(int index)
		{
			return (PassbookEntry)m_Entries[index];
		}

		public int PassBookEntryCount
		{
			get
			{
				return m_Entries.Count;
			}
		}

		public void Dispose() 
		{
			Dispose(true);
			GC.SuppressFinalize(this); 
		}

		protected virtual void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				m_Entries = null;
				// Free other state (managed objects).
			}
			// Free your own state (unmanaged objects).
			// Set large fields to null.
		}

		~PassbookEntryCollection()
		{
			Dispose(false);
		}


	}

	public class PassbookEntry 
	{
		private string m_PassbookMessageLine;
		private int m_TranType; //0 - Debit
								//1 - Credit

		private PassbookEntry()
		{

		}

		internal PassbookEntry(string Message, int TranType)
		{
			m_PassbookMessageLine = Message;
			m_TranType = TranType;
		}

		public string GetEntryDetail(string DetailName)
		{
			try
			{
				switch(DetailName)
				{
					case "SequenceNumber" : 
						return m_PassbookMessageLine.Substring(0, 4);
					case "UnpostedAmount" : 
						return m_PassbookMessageLine.Substring(4, 12);
					case "PassBookBalance" :
						return m_PassbookMessageLine.Substring(16, 12);
					case "TransactionDate" :
						return m_PassbookMessageLine.Substring(28, 6);
					case "BranchCode" :
						//Console.WriteLine(m_PassbookMessageLine.Substring(34,3));
						//return m_PassbookMessageLine.Substring(4, 3);   //original code comment by jas 04/05/2005
						return m_PassbookMessageLine.Substring(34, 3);	//jas 04/05/2005
					case "TransactionCode" :
						return m_PassbookMessageLine.Substring(37);
					case "TransactionType" :
						return (m_TranType == 0)? "Debit" : "Credit";
					case "TransPneumonic" :	//jas 04/05/2005
						return m_PassbookMessageLine.Substring(37, 6);	//jas 04/05/2005
					default: 
						return string.Empty;
				}
			}
			catch
			{
				return string.Empty;
			}
			
		}
		
		
		//		public string SequenceNumber
		//		{
		//			get
		//			{
		//				return m_PassbookMessageLine.Substring(0, 4);
		//			}
		//		}
		//
		//		public string UnpostedAmount
		//		{
		//			get
		//			{
		//				return m_PassbookMessageLine.Substring(4, 12);
		//			}
		//		}
		//		
		//		public string PassBookBalance
		//		{
		//			get
		//			{
		//				return m_PassbookMessageLine.Substring(16, 12);
		//			}
		//		}
		//
		//		public string TransactionDate
		//		{
		//			get
		//			{
		//				return m_PassbookMessageLine.Substring(28, 6);
		//			}
		//		}
		//
		//		public string BranchCode
		//		{
		//			get
		//			{
		//				return m_PassbookMessageLine.Substring(34, 3);
		//			}
		//		}
		//
		//		public string TransactionCode
		//		{
		//			get
		//			{
		//				return m_PassbookMessageLine.Substring(37);
		//			}
		//		}
	}
}

