using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPPOSService
{
	[Serializable]
	public class RequestObj
	{
		public string GUID { get; set; }
		public double amountPurchase { get; set; }
		public double amountCash { get; set; }

	}
}
