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
using System.Windows.Forms;

namespace FTPPOSService
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			this.FormClosed += (x, y) =>
			{
				Application.Exit();
			};
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs ee)
		{
			new Thread(new ThreadStart(delegate
			{
				TcpListener server = null;
				try
				{
					// Set the TcpListener on port 13000.
					Int32 port = 13000;
					IPAddress localAddr = IPAddress.Parse("0.0.0.0");

					// TcpListener server = new TcpListener(port);
					server = new TcpListener(localAddr, port);

					// Start listening for client requests.
					server.Start();

					// Buffer for reading data
					Byte[] bytes = new Byte[256];
					String data = null;

					// Enter the listening loop.
					while (true)
					{
						Invoke(new Action(delegate { richTextBox1.AppendText("Waiting for a connection... \r\n"); }));

						// Perform a blocking call to accept requests.
						// You could also use server.AcceptSocket() here.
						Invoke(new Action(delegate
						{
							richTextBox1.AppendText($"Listening For Amount at {localAddr}:{port}... \r\n");
						}));
						TcpClient client = server.AcceptTcpClient();

						data = null;

						// Get a stream object for reading and writing
						NetworkStream stream = client.GetStream();

						int i;

						// Loop to receive all the data sent by the client.
						while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
						{
							// Translate data bytes to a ASCII string.
							data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
							Invoke(new Action(delegate
							{
								richTextBox1.AppendText($"Received: {data} ");
							}));
							// Process the data sent by the client.
							data = data.ToLower();

							/// Initiate device amount
							initiatePOSRequest(data);
							//
							data = "RECEIVED";
							byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
							// Send back a response.
							stream.Write(msg, 0, msg.Length);
							Invoke(new Action(delegate
							{
								richTextBox1.AppendText($"Sent Receival Token: {data} ");
							}));
						}

						// Shutdown and end connection
						client.Close();
					}
				}
				catch (SocketException e)
				{
					Invoke(new Action(delegate
					{
						richTextBox1.AppendText($"SocketException: {e}");
					}));
				}
				finally
				{
					// Stop listening for new clients.
					server.Stop();
				}
			})).Start();
		}



		ManualResetEvent txnFired = new ManualResetEvent(false);
		void initiatePOSRequest(string Data)
		{
			Invoke(new Action(delegate
			{
				richTextBox1.AppendText($"/////////////////////////////\r\n");
				richTextBox1.AppendText($"Device\r\n");
				richTextBox1.AppendText($"/////////////////////////////\r\n");
			}));
			// data members
			string guid = Regex.Match(Data, @"guid=([\d\.]+);").Groups[1].Value;
			string submitBackIP = Regex.Match(Data, @"client_ip=([\d\.]+);").Groups[1].Value;
			string submitBackPort = Regex.Match(Data, @"client_port=([\d\.]+);").Groups[1].Value;
			string amountPurchase = Regex.Match(Data, @"amount_purchase=([\d\.]+);").Groups[1].Value;
			string amountCash = Regex.Match(Data, @"amount_cash=([\d\.]+);").Groups[1].Value;

			Invoke(new Action(delegate
			{
				richTextBox1.AppendText($"guid: {guid} \r\n");
				richTextBox1.AppendText($"submit_ip: {submitBackIP} \r\n");
				richTextBox1.AppendText($"submit_port: {submitBackIP} \r\n");
				richTextBox1.AppendText($"submit purchase amount: {submitBackIP} \r\n");
				richTextBox1.AppendText($"submit cash amount: {submitBackIP} \r\n");
			}));
			/////
			var eft = new EFTClientIP()
			{
				HostName = "127.0.0.1",
				HostPort = 2011,
				UseSSL = false
			};
			// Hook up events
			eft.OnReceipt += (x, y) =>
			{
				Invoke(new Action(delegate
				{
					richTextBox1.AppendText($"On Reciept\r\n");
					richTextBox1.AppendText($"{ y.Response.Type} receipt\r\n");
					richTextBox1.AppendText($"{y.Response.Type} receipt\r\n");
				}));
			};
			eft.OnTransaction += (x, y) =>
			{

				// Handle transaction event 
				var displayText = y.Response.Success ? "successful" : "unsuccessful";
				Invoke(new Action(delegate
				{
					richTextBox1.AppendText($"On Transaction\r\n");
					richTextBox1.AppendText($"Transaction was {displayText}");
				}));
				SendBackToken(submitBackIP, Convert.ToInt32(submitBackPort), $"guid={guid};message={displayText}");
				txnFired.Set();
			};
			eft.OnTerminated += (x, y) =>
			{
				// Handle socket close
				Invoke(new Action(delegate
				{
					richTextBox1.AppendText($"Device Socket closed");
				}));
				txnFired.Reset();
			};
			// Connect
			if (!eft.Connect())
			{
				// Handle failed connection
				Invoke(new Action(delegate
				{
					richTextBox1.AppendText($"error connecting to device \r\n");
				}));
				return;
			}

			// Build transaction request
			var r = new EFTTransactionRequest()
			{
				// TxnType is required
				TxnType = TransactionType.PurchaseCash,
				// Set TxnRef to something unique
				TxnRef = DateTime.Now.ToString("YYMMddHHmmsszzz"),
				// Set AmtCash for cash out, and AmtPurchase for purchase/refund
				AmtPurchase = Convert.ToDecimal(amountPurchase),
				AmtCash = Convert.ToDecimal(amountCash),
				// Set POS or pinpad printer
				//ReceiptPrintMode = ReceiptPrintModeType.POSPrinter,
				// Set application. Used for gift card & 3rd party payment
				Application = TerminalApplication.EFTPOS
			};
			// Send transaction
			if (!eft.DoTransaction(r))
			{
				// Handle failed send
				Invoke(new Action(delegate
				{
					richTextBox1.AppendText("Device Send failed");
				}));
				return;
			}

			txnFired.WaitOne();
			eft.Disconnect();
			eft.Dispose();
		}


		void SendBackToken(string IP, int Port, string message)
		{
			try
			{
				Invoke(new Action(delegate
				{
					richTextBox1.AppendText($"Sending CallBack response");
				}));
				TcpClient tcpClient = new TcpClient();
				tcpClient.Connect(IP, Port);
				if (tcpClient.Connected)
				{
					using (NetworkStream ns = tcpClient.GetStream())
					{
						var bytes = UTF8Encoding.UTF8.GetBytes(message);
						ns.Write(bytes, 0, bytes.Length);
						///                        
					}
				}
			}
			catch (Exception t)
			{
				MessageBox.Show(t.Message);
			}

		}

		private void button2_Click(object sender, EventArgs e)
		{
			var eft = new EFTClientIP()
			{
				HostName = ipTxt.Text,//"192.168.10.26",
				HostPort = Convert.ToInt32(portTxt.Text),//2011,
				UseSSL = false
			};
			// Hook up events
			eft.OnReceipt += (x, y) =>
			{
				Invoke(new Action(delegate
				{
					richTextBox1.AppendText($"On Reciept\r\n");
					richTextBox1.AppendText($"{ y.Response.Type} receipt\r\n");
					richTextBox1.AppendText($"{y.Response.Type} receipt\r\n");
				}));
			};
			eft.OnTransaction += (x, y) =>
			{

				// Handle transaction event 
				var displayText = y.Response.Success ? "successful" : "unsuccessful";
				Invoke(new Action(delegate
				{
					richTextBox1.AppendText($"On Transaction\r\n");
					richTextBox1.AppendText($"Transaction was {displayText}");
				}));
				//SendBackToken(submitBackIP, Convert.ToInt32(submitBackPort), $"guid={guid};message={displayText}");
				txnFired.Set();
			};
			eft.OnTerminated += (x, y) =>
			{
				// Handle socket close

				richTextBox1.AppendText($"Device Socket closed");
				txnFired.Reset();
			};
			//eft.On
			// Connect
			if (!eft.Connect())
			{
				//	// Handle failed connection
				richTextBox1.AppendText($"error connecting to device \r\n");
				return;
			}
			//eft.Connect();
			// Build transaction request
			var r = new EFTTransactionRequest()
			{
				// TxnType is required
				TxnType = TransactionType.PurchaseCash,
				// Set TxnRef to something unique
				TxnRef = DateTime.Now.ToString("YYMMddHHmmsszzz"),
				// Set AmtCash for cash out, and AmtPurchase for purchase/refund
				AmtPurchase = Convert.ToDecimal(1.00),
				AmtCash = Convert.ToDecimal(0.00),
				// Set POS or pinpad printer
				//ReceiptPrintMode = ReceiptPrintModeType.POSPrinter,
				// Set application. Used for gift card & 3rd party payment
				Application = TerminalApplication.EFTPOS
			};
			// Send transaction
			if (!eft.DoTransaction(r))
			{
				// Handle failed send
				Invoke(new Action(delegate
				{
					richTextBox1.AppendText("Device Send failed");
				}));
				return;
			}

			txnFired.WaitOne();
			//eft.Disconnect();
			//eft.Dispose();
		}

		private void button3_Click(object sender, EventArgs e)
		{

		}
	}
}
