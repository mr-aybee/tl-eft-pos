using Newtonsoft.Json;
using PCEFTPOS.EFTClient.IPInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPPOSService
{
	public partial class frmSRV : Form
	{
		WebSocketServer webSocket;
		public frmSRV()
		{
			InitializeComponent();
			webSocket = new WebSocketServer();
			// Set up PC-EFTPOS connection and hook up events
			RequestObj requestObj = new RequestObj
			{
				GUID = Guid.NewGuid().ToString(),
				amountCash = 0,
				amountPurchase = 1,
			};
			var requestSample = JsonConvert.SerializeObject(requestObj);
			txtReceipt.AppendText("Request Sample");
			txtReceipt.AppendText(requestSample);
			txtReceipt.AppendText("\r\n");
			this.Shown+=(x,y)=>{
				StartService();
				button_Start.BackColor = Color.Green;
				button_Start.Enabled = false;
			};
		}

		private void button_Test_Click(object sender, EventArgs e)
		{
			RequestObj request = new RequestObj
			{
				GUID = Guid.NewGuid().ToString(),
				amountCash = 0,
				amountPurchase = 0.5
			};
			DoTransaction(request, null);
		}

		private void button_Start_Click(object sender, EventArgs e)
		{
			StartService();
			button_Start.BackColor = Color.Green;
			button_Start.Enabled = false;
		}

		void StartService()
		{
			webSocket.Listen(13000);
			webSocket.ClientConnected += (cX, client) =>
			{
				client.AnyMessageReceived += (xx, yy) =>
				{
					Thread.Sleep(200);
					var message = UTF8Encoding.UTF8.GetString(yy);
					if (!string.IsNullOrEmpty(message))
					{
						message = message.Trim();
						if (message.StartsWith("{") && message.EndsWith("}"))
						{
							RequestObj request = JsonConvert.DeserializeObject<RequestObj>(message);
							DoTransaction(request, client);
						}
					}
				};
			};

		}



		void SendMessageToClients(CallBackMessage Message, WebSocketSession webSocketSession = null)
		{

			var message = JsonConvert.SerializeObject(Message, Formatting.Indented);
			try
			{
				Invoke(new Action(delegate
				{
					txtReceipt.AppendText(message + "\r\n\r\n");
				}));
			}
			catch { }
			Logger.Log(message + "\r\n---------------------\r\n");
			string messageJSON = JsonConvert.SerializeObject(Message);
			if (webSocketSession != null)
			{
				webSocketSession.SendMessage(UTF8Encoding.UTF8.GetBytes(messageJSON), false, false);
			}
		}

		//void SendMessageToClients(string Message, WebSocketSession webSocketSession = null)
		//{

		//	if (webSocketSession != null)
		//	{
		//		webSocketSession.SendMessage(UTF8Encoding.UTF8.GetBytes(Message), false, false);
		//	}
		//}

		TransactionResponseObj GetResponseObj(EFTTransactionResponse rtxnY)
		{

			return new TransactionResponseObj
			{
				AccountType = rtxnY.AccountType,
				Pan = rtxnY.Pan,
				Merchant = rtxnY.Merchant,
				AmtCash = rtxnY.AmtCash,
				DateSettlement = rtxnY.DateSettlement,
				Success = rtxnY.Success,
				AmtPurchase = rtxnY.AmtPurchase,
				AmtTip = rtxnY.AmtTip,
				AuthCode = rtxnY.AuthCode,
				AvailableBalance = rtxnY.AvailableBalance,
				BalanceReceived = rtxnY.BalanceReceived,
				Caid = rtxnY.Caid,
				CardName = rtxnY.CardName,
				CardType = rtxnY.CardType,
				Catid = rtxnY.Catid,
				ClearedFundsBalance = rtxnY.ClearedFundsBalance,
				Date = rtxnY.Date,
				DateExpiry = rtxnY.DateExpiry,
				ResponseCode = rtxnY.ResponseCode,
				ResponseText = rtxnY.ResponseText,
				RRN = rtxnY.RRN,
				Stan = rtxnY.Stan,
				Track2 = rtxnY.Track2,
				TxnFlags = rtxnY.TxnFlags,
				TxnRef = rtxnY.TxnRef,
				TxnType = rtxnY.TxnType
			};
		}

		bool DoTransaction(RequestObj request, WebSocketSession CallBackClient)
		{
			try
			{
				IEFTClientIP _eft = null;

				_eft = new EFTClientIP();
				_eft.HostName = ipTxt.Text;
				_eft.HostPort = int.Parse(portTxt.Text);
				//
				SendMessageToClients(new CallBackMessage(request.GUID, "CONTACTING EFT-CLIENT, PLEASE WAIT..."), CallBackClient);
				if (!_eft.Connect())
				{
					// Handle failed connection					
					SendMessageToClients(new CallBackMessage(request.GUID, "Error connecting to device"), CallBackClient);
					if (CallBackClient is WebSocketSession) CallBackClient.Close();
					return false;
				}
				_eft.OnDisplay += (dX, dY) =>
				{
					var resp = dY.Response;
					SendMessageToClients(new CallBackMessage(request.GUID, string.Join(", ", resp.DisplayText)), CallBackClient);
				};
				_eft.OnReceipt += (rx, rY) =>
				{

					// Build receipt
					if (rY.Response.IsPrePrint)
					{
						SendMessageToClients(new CallBackMessage(request.GUID, $"{Environment.NewLine}{Environment.NewLine}{rY.Response.Type.ToString()} receipt{Environment.NewLine}"), CallBackClient);
					}
					else
					{
						var receipt = new StringBuilder();
						foreach (var l in rY.Response.ReceiptText)
						{
							receipt.AppendLine(l);
						}
						SendMessageToClients(new CallBackMessage(request.GUID, receipt.ToString()), CallBackClient);
						txtReceipt.AppendText(receipt.ToString());
					}
					SendMessageToClients(new CallBackMessage(request.GUID, "Receipt Printed"), CallBackClient);

				};
				_eft.OnTransaction += (txnX, txnY) =>
				{

					var rtxnY = txnY.Response;
					/// Message tO Client
					SendMessageToClients(new CallBackMessage(request.GUID, $"TRANSACTION {(rtxnY.Success ? "OK" : "FAILED")}"), CallBackClient);
					var responseObj = GetResponseObj(rtxnY);
					var respText = JsonConvert.SerializeObject(responseObj);
					SendMessageToClients(new CallBackMessage(request.GUID, respText), CallBackClient);
				};
				_eft.OnTerminated += (trX, trY) =>
				{
					SendMessageToClients(new CallBackMessage(request.GUID, trY.ToString()), CallBackClient);
					if (CallBackClient is WebSocketSession)
						CallBackClient.Close();
				};
				// Build transaction request
				var r = new EFTTransactionRequest();
				// TxnType is required
				// Set ReferenceNumber to something unique
				// Set AmountCash for cash out, and AmountPurchase for purchase/refund
				// Set POS or pinpad printer
				// Set application. Used for gift card & 3rd party payment				
				r.TxnType = TransactionType.PurchaseCash;
				r.TxnRef = DateTime.Now.ToString("YYMMddHHmmssfff");
				r.AmtPurchase = (r.TxnType == TransactionType.CashOut) ? 0 : decimal.Parse(request.amountPurchase.ToString());
				r.AmtCash = (r.TxnType == TransactionType.CashOut) ? decimal.Parse(request.amountCash.ToString()) : 0;
				r.ReceiptAutoPrint = ReceiptPrintModeType.POSPrinter;
				r.Application = TerminalApplication.EFTPOS;
				//// Do Transaction to EFT POS
				if (!_eft.DoTransaction(r))
				{
					SendMessageToClients(new CallBackMessage(request.GUID, "FAILED TO SEND Transaction"), CallBackClient);
					_eft.Dispose();
					return false;
				}
				//
				SendMessageToClients(new CallBackMessage(request.GUID, "Transaction Request Sent"), CallBackClient);
				return true;
			}
			catch (Exception t)
			{
				try
				{
					SendMessageToClients(new CallBackMessage(request.GUID, JsonConvert.SerializeObject(t)), CallBackClient);
				}
				catch { }
				return false;
			}
		}

	}
}