using PCEFTPOS.EFTClient.IPInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPPOSService
{
	public class TransactionResponseObj
	{

		//
		// Summary:
		//     The type of transaction to perform.
		//
		// Value:
		//     Type: PCEFTPOS.EFTClient.IPInterface.TransactionType
		//     The default is PCEFTPOS.EFTClient.IPInterface.TransactionType.PurchaseCash
		public TransactionType TxnType
		{
			get;
			set;
		} = TransactionType.PurchaseCash;



		//
		// Summary:
		//     Two digit merchant code
		//
		// Value:
		//     Type: System.String
		//     The default is "00"
		public string Merchant
		{
			get;
			set;
		} = "00";


		//
		// Summary:
		//     Indicates the card type that was used in the transaction.
		//
		// Value:
		//     Type: System.String
		//
		// Remarks:
		//     PCEFTPOS.EFTClient.IPInterface.EFTTransactionResponse.CardBIN
		public string CardType
		{
			get;
			set;
		} = "";


		//
		// Summary:
		//     Indicates the card type that was used in the transaction.
		//
		// Value:
		//     Type: System.Int32
		//
		// Remarks:
		//     Card BIN –Card Type
		//     0 –Unknown
		//     1 –Debit
		//     2 –Bankcard
		//     3 –Mastercard
		//     4 –Visa
		//     5 –American Express
		//     6 –Diner Club
		//     7 –JCB
		//     8 –Label Card
		//     9 –JCB
		//     11 –JCB
		//     12 –Other
		public int CardName
		{
			get;
			set;
		}



		//
		// Summary:
		//     Used to retrieve the transaction from the batch.
		//
		// Value:
		//     Type: System.String
		//
		// Remarks:
		//     The retrieval reference number is used when performing a tip adjustment transaction.
		public string RRN
		{
			get;
			set;
		} = "";


		public DateTime DateSettlement
		{
			get;
			set;
		} = DateTime.MinValue;




		//
		// Summary:
		//     The cash amount for the transaction.
		//
		// Value:
		//     Type: System.Decimal
		//     The default is 0.
		//
		// Remarks:
		//     This property is mandatory for a PCEFTPOS.EFTClient.IPInterface.TransactionType.CashOut
		//     transaction type.
		public decimal AmtCash
		{
			get;
			set;
		}



		//
		// Summary:
		//     The purchase amount for the transaction.
		//
		// Value:
		//     Type: System.Decimal
		//     The default is 0.
		//
		// Remarks:
		//     This property is mandatory for all but PCEFTPOS.EFTClient.IPInterface.TransactionType.CashOut
		//     transaction types.
		public decimal AmtPurchase
		{
			get;
			set;
		}


		//
		// Summary:
		//     The tip amount for the transaction.
		//
		// Value:
		//     Type: System.Decimal
		//     Echoed from the request.
		public decimal AmtTip
		{
			get;
			set;
		}



		//
		// Summary:
		//     The authorisation number for the transaction.
		//
		// Value:
		//     Type: System.Int32
		//
		// Remarks:
		//     This property is required for a PCEFTPOS.EFTClient.IPInterface.TransactionType.Completion
		//     transaction type.
		public int AuthCode
		{
			get;
			set;
		}



		//
		// Summary:
		//     The reference number to attach to the transaction. This will appear on the receipt.
		//
		// Value:
		//     Type: System.String
		//
		// Remarks:
		//     This property is optional but it usually populated by a unique transaction identifier
		//     that can be used for retrieval.
		public string TxnRef
		{
			get;
			set;
		} = "";




		//
		// Summary:
		//     The card number to use when pan source of POS keyed is used.
		//
		// Value:
		//     Type: System.String
		//
		// Remarks:
		//     Use this property in conjunction with PCEFTPOS.EFTClient.IPInterface.PanSource.
		public string Pan
		{
			get;
			set;
		} = "";


		//
		// Summary:
		//     The expiry date of the card when of POS keyed is used.
		//
		// Value:
		//     Type: System.String
		//     In MMYY format.
		//
		// Remarks:
		//     Use this property in conjunction with PCEFTPOS.EFTClient.IPInterface.PanSource
		//     when passing the card expiry date to PC-EFTPOS.
		public string DateExpiry
		{
			get;
			set;
		} = "";




		//
		// Summary:
		//     The track 2 data on the magnetic stripe of the card.
		//
		// Value:
		//     Type: System.String
		//     This property contains the partial track 2 data from the card used in this transaction.
		public string Track2
		{
			get;
			set;
		} = "";


		//
		// Summary:
		//     The account to use for this transaction.
		//
		// Value:
		//     Type: PCEFTPOS.EFTClient.IPInterface.EFTTransactionResponse.AccountType
		//     Default is PCEFTPOS.EFTClient.IPInterface.AccountType.Default. Use default to
		//     prompt user to enter the account type.
		//
		// Remarks:
		//     Use this property in conjunction with PCEFTPOS.EFTClient.IPInterface.PanSource
		//     when passing the account type to PC-EFTPOS.
		public AccountType AccountType
		{
			get;
			set;
		} = AccountType.Default;




		//
		// Summary:
		//     Flags that indicate how the transaction was processed.
		//
		// Value:
		//     Type: PCEFTPOS.EFTClient.IPInterface.EFTTransactionResponse.TxnFlags
		public TxnFlags TxnFlags
		{

			get;
			set;
		} = new TxnFlags();


		//
		// Summary:
		//     Indicates if an available balance is present in the response.
		//
		// Value:
		//     Type: System.Boolean
		public bool BalanceReceived
		{
			get;
			set;
		}

		//
		// Summary:
		//     Balance available on the processed account.
		//
		// Value:
		//     Type: System.Decimal
		public decimal AvailableBalance
		{
			get;
			set;
		}

		//
		// Summary:
		//     Cleared balance on the processed account.
		//
		// Value:
		//     Type: System.Decimal
		public decimal ClearedFundsBalance
		{
			get;
			set;
		}

		//
		// Summary:
		//     Indicates if the request was successful.
		//
		// Value:
		//     Type: System.Boolean
		public bool Success
		{
			get;
			set;
		}

		//
		// Summary:
		//     The response code of the request.
		//
		// Value:
		//     Type: System.String
		//     A 2 character response code. "00" indicates a successful response.
		public string ResponseCode
		{
			get;
			set;
		} = "";


		//
		// Summary:
		//     The response text for the response code.
		//
		// Value:
		//     Type: System.String
		public string ResponseText
		{
			get;
			set;
		} = "";


		//
		// Summary:
		//     Date and time of the response returned by the bank.
		//
		// Value:
		//     Type: System.DateTime
		public DateTime Date
		{
			get;
			set;
		} = DateTime.MinValue;



		//
		// Summary:
		//     Terminal ID configured in the PIN pad.
		//
		// Value:
		//     Type: System.String
		public string Catid
		{
			get;
			set;
		} = "";



		//
		// Summary:
		//     Merchant ID configured in the PIN pad.
		//
		// Value:
		//     Type: System.String
		public string Caid
		{
			get;
			set;
		} = "";




		//
		// Summary:
		//     System Trace Audit Number
		//
		// Value:
		//     Type: System.Int32
		public int Stan
		{
			get;
			set;
		}


	}

	public class CallBackMessage
	{

		public string GUID { get; set; }
		public string Message { get; set; }

		public CallBackMessage(string gUID, string message)
		{
			GUID = gUID;
			Message = message;
		}

	}
}
