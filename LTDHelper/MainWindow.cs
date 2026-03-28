using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Geode.Extension;
using Geode.Habbo.Packages;
using Geode.Network;
using Microsoft.VisualBasic.CompilerServices;

namespace LTDHelper;

[DesignerGenerated]
public partial class MainWindow : Window, IComponentConnector
{
	private enum BotState { Idle, WaitingForFurniName, WaitingForMaxPrice, WaitingForAmount, Running }

	public int CurrentLanguageInt;

	[CompilerGenerated]
	[AccessedThroughProperty("Extension")]
	private GeodeExtension _Extension;

	[CompilerGenerated]
	[AccessedThroughProperty("ConsoleBot")]
	private ConsoleBot _ConsoleBot;

	private BotState State = BotState.Idle;
	private string SearchFurniName = string.Empty;
	private int MaxPrice = 0;
	private int TargetAmount = 0; // 0 = unlimited
	private int TotalBought = 0;
	private bool DebugEnabled = true;
	private int _noOfferScanCount = 0;
	private CancellationTokenSource _cts;

	public virtual GeodeExtension Extension
	{
		[CompilerGenerated]
		get => _Extension;
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			Action<DataInterceptedEventArgs> intercept = Extension_OnDataInterceptEvent;
			Action<string> critErr = Extension_OnCriticalErrorEvent;
			if (_Extension != null)
			{
				_Extension.OnDataInterceptEvent -= intercept;
				_Extension.OnCriticalErrorEvent -= critErr;
			}
			_Extension = value;
			if (_Extension != null)
			{
				_Extension.OnDataInterceptEvent += intercept;
				_Extension.OnCriticalErrorEvent += critErr;
			}
		}
	}

	public virtual ConsoleBot ConsoleBot
	{
		[CompilerGenerated]
		get => _ConsoleBot;
		[MethodImpl(MethodImplOptions.Synchronized)]
		[CompilerGenerated]
		set
		{
			Action<string> botLoaded = ConsoleBot_OnBotLoaded;
			Action<string> msgReceived = ConsoleBot_OnMessageReceived;
			if (_ConsoleBot != null)
			{
				_ConsoleBot.OnBotLoaded -= botLoaded;
				_ConsoleBot.OnMessageReceived -= msgReceived;
			}
			_ConsoleBot = value;
			if (_ConsoleBot != null)
			{
				_ConsoleBot.OnBotLoaded += botLoaded;
				_ConsoleBot.OnMessageReceived += msgReceived;
			}
		}
	}

	public MainWindow()
	{
		base.Loaded += MainWindow_Loaded;
		InitializeComponent();
	}

	private void MainWindow_Loaded(object sender, RoutedEventArgs e)
	{
		try
		{
			base.Visibility = Visibility.Hidden;
			string culture = CultureInfo.CurrentCulture.Name.ToLower();
			if (culture.StartsWith("es")) CurrentLanguageInt = 1;
			else if (culture.StartsWith("pt")) CurrentLanguageInt = 2;
			else CurrentLanguageInt = 0;
			Extension = new GeodeExtension("LTDHelper", "Marketplace auto-buyer.", "Lilith");
			Extension.Start();
			ConsoleBot = new ConsoleBot(Extension, "LTDHelper");
			ConsoleBot.ShowBot();
		}
		catch (Exception ex)
		{
			base.Visibility = Visibility.Visible;
			base.ShowInTaskbar = true;
			MessageBox.Show("Startup error:\n" + ex, "LTDHelper", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	public void BotWelcome()
	{
		ConsoleBot.BotSendMessage(AppTranslator.WelcomeMessage[CurrentLanguageInt]);
		ConsoleBot.BotSendMessage(AppTranslator.CommandsMessage[CurrentLanguageInt]);
	}

	// ── Search loop ────────────────────────────────────────────────────────

	private void StartSearchLoop()
	{
		_cts = new CancellationTokenSource();
		State = BotState.Running;
		_ = SearchLoop(_cts.Token);
	}

	private void StopSearchLoop()
	{
		_cts?.Cancel();
		_cts = null;
		_noOfferScanCount = 0;
		State = BotState.Idle;
	}

	private async Task SearchLoop(CancellationToken ct)
	{
		try
		{
			while (!ct.IsCancellationRequested)
			{
				if (TargetAmount > 0 && TotalBought >= TargetAmount)
				{
					ConsoleBot.BotSendMessage(AppTranslator.TargetReached[CurrentLanguageInt]);
					StopSearchLoop();
					return;
				}
				await SearchAndBuyAsync(ct);
				try { await Task.Delay(new Random().Next(2000, 4000), ct); }
				catch (OperationCanceledException) { return; }
			}
		}
		catch (OperationCanceledException) { }
		catch (Exception)
		{
			ConsoleBot.BotSendMessage(AppTranslator.UnexpectedError[CurrentLanguageInt]);
			StopSearchLoop();
		}
	}

	private async Task SearchAndBuyAsync(CancellationToken ct)
	{
		try
		{
			// Packet validated from logs:
			// {out:GetMarketplaceOffers}{i:-1}{i:-1}{s:"query"}{i:1}
			Extension.SendToServerAsync(Extension.Out.GetMarketplaceOffers, -1, -1, SearchFurniName, 1);

			DataInterceptedEventArgs result = await Extension.WaitForPacketAsync(Extension.In.MarketPlaceOffers, 5000);
			if (result == null || ct.IsCancellationRequested) return;

			(int bestOfferId, int bestPrice) = ParseBestOffer(result.Packet);
			if (bestOfferId == -1)
			{
				_noOfferScanCount++;
				if (DebugEnabled && _noOfferScanCount % 5 == 0)
				{
					ConsoleBot.BotSendMessage(AppTranslator.NoOfferAtPrice[CurrentLanguageInt]);
				}
				return;
			}
			_noOfferScanCount = 0;

			ConsoleBot.BotSendMessage(string.Format(AppTranslator.OfferFound[CurrentLanguageInt], bestPrice));
			(bool bought, int resultCode) = await TryBuyOfferAsync(bestOfferId, bestPrice);
			if (bought)
			{
				TotalBought++;
				ConsoleBot.BotSendMessage(string.Format(AppTranslator.PurchaseOK[CurrentLanguageInt], bestPrice, TotalBought));
			}
			else
			{
				if (resultCode == -1)
				{
					ConsoleBot.BotSendMessage(AppTranslator.PurchaseFailed[CurrentLanguageInt]);
				}
				else
				{
					ConsoleBot.BotSendMessage(string.Format(AppTranslator.PurchaseRejected[CurrentLanguageInt], resultCode));
				}
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex)
		{
			if (DebugEnabled)
			{
				ConsoleBot.BotSendMessage(string.Format(AppTranslator.DebugError[CurrentLanguageInt], ex.Message));
			}
		}
	}

	private async Task<(bool bought, int resultCode)> TryBuyOfferAsync(int offerId, int price)
	{
		// Attempt 1: format seen in your logs.
		Extension.SendToServerAsync(Extension.Out.BuyMarketplaceOffer, offerId);
		DataInterceptedEventArgs buyResult = await Extension.WaitForPacketAsync(Extension.In.MarketplaceBuyOfferResult, 2000);
		if (buyResult != null)
		{
			int resultCode = ReadBuyResultCode(buyResult.Packet);
			return (resultCode == 1, resultCode);
		}

		// Attempt 2: some builds require (offerId, price).
		Extension.SendToServerAsync(Extension.Out.BuyMarketplaceOffer, offerId, price);
		buyResult = await Extension.WaitForPacketAsync(Extension.In.MarketplaceBuyOfferResult, 2000);
		if (buyResult != null)
		{
			int resultCode = ReadBuyResultCode(buyResult.Packet);
			return (resultCode == 1, resultCode);
		}

		return (false, -1);
	}

	private int ReadBuyResultCode(dynamic packet)
	{
		try
		{
			return packet.ReadInteger();
		}
		catch
		{
			return -1;
		}
	}

	// Parses {in:MarketPlaceOffers} and returns the cheapest offer <= MaxPrice.
	// Observed format:
	// {i:offersCount} then N offers with:
	// {i:offerId}{i:furniId}{i:furniType}{i:spriteId}{i:stuffData}
	// {i:extraInt}{s:extraData}{i:status}{i:avgPrice}{i:offerCount}
	// {i:timeLeftMinutes}{i:price}{i:unknown}
	private (int offerId, int price) ParseBestOffer(dynamic packet)
	{
		int bestOfferId = -1;
		int bestPrice = int.MaxValue;
		try
		{
			int offersCount = packet.ReadInteger();
			for (int i = 0; i < offersCount; i++)
			{
				int offerId = packet.ReadInteger();
				packet.ReadInteger(); // furniId
				packet.ReadInteger(); // furniType
				packet.ReadInteger(); // spriteId
				packet.ReadInteger(); // stuffData
				packet.ReadInteger(); // extraInt
				packet.ReadString();  // extraData
				int status = packet.ReadInteger();
				packet.ReadInteger(); // avgPrice
				packet.ReadInteger(); // offerCount
				packet.ReadInteger(); // timeLeftMinutes
				int price = packet.ReadInteger();
				packet.ReadInteger(); // unknown
				if (status == 1 && price <= MaxPrice && price < bestPrice)
				{
					bestPrice = price;
					bestOfferId = offerId;
				}
			}
		}
		catch
		{
			// Parsing error — packet structure may differ; check PACKET NOTE above.
		}
		return (bestOfferId, bestPrice == int.MaxValue ? 0 : bestPrice);
	}

	// ── Command handling ───────────────────────────────────────────────────

	private void ConsoleBot_OnBotLoaded(string e)
	{
		BotWelcome();
	}

	private void ConsoleBot_OnMessageReceived(string e)
	{
		string input = e.Trim();
		string lower = input.ToLower();

		if (lower.StartsWith("/debug"))
		{
			string[] parts = lower.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 2 && parts[1] == "on")
			{
				DebugEnabled = true;
				ConsoleBot.BotSendMessage(AppTranslator.DebugEnabled[CurrentLanguageInt]);
			}
			else if (parts.Length == 2 && parts[1] == "off")
			{
				DebugEnabled = false;
				ConsoleBot.BotSendMessage(AppTranslator.DebugDisabled[CurrentLanguageInt]);
			}
			else
			{
				ConsoleBot.BotSendMessage(AppTranslator.DebugUsage[CurrentLanguageInt]);
			}
			return;
		}

		// Wizard input states
		if (State == BotState.WaitingForFurniName)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				ConsoleBot.BotSendMessage(AppTranslator.AskFurniName[CurrentLanguageInt]);
				return;
			}
			SearchFurniName = input;
			State = BotState.WaitingForMaxPrice;
			ConsoleBot.BotSendMessage(AppTranslator.AskMaxPrice[CurrentLanguageInt]);
			return;
		}

		if (State == BotState.WaitingForMaxPrice)
		{
			if (int.TryParse(input, out int price) && price > 0)
			{
				MaxPrice = price;
				State = BotState.WaitingForAmount;
				ConsoleBot.BotSendMessage(AppTranslator.AskAmount[CurrentLanguageInt]);
			}
			else
			{
				ConsoleBot.BotSendMessage(AppTranslator.InvalidPrice[CurrentLanguageInt]);
			}
			return;
		}

		if (State == BotState.WaitingForAmount)
		{
			if (int.TryParse(input, out int amount) && amount >= 0 && amount <= 100)
			{
				TargetAmount = amount;
				TotalBought = 0;
				string target = TargetAmount == 0 ? "∞" : TargetAmount.ToString();
				ConsoleBot.BotSendMessage(string.Format(AppTranslator.SearchStarted[CurrentLanguageInt], SearchFurniName, MaxPrice, target));
				StartSearchLoop();
			}
			else
			{
				ConsoleBot.BotSendMessage(AppTranslator.InvalidAmount[CurrentLanguageInt]);
			}
			return;
		}

		// Commands
		switch (lower)
		{
			case "/iniciar":
			case "/start":
			case "/começar":
				if (State == BotState.Idle)
				{
					State = BotState.WaitingForFurniName;
					ConsoleBot.BotSendMessage(AppTranslator.AskFurniName[CurrentLanguageInt]);
				}
				else
				{
					ConsoleBot.BotSendMessage(AppTranslator.AlreadyRunning[CurrentLanguageInt]);
				}
				break;

			case "/detener":
			case "/stop":
			case "/parar":
				if (State == BotState.Running)
				{
					StopSearchLoop();
					ConsoleBot.BotSendMessage(AppTranslator.StoppedMessage[CurrentLanguageInt]);
				}
				else
				{
					ConsoleBot.BotSendMessage(AppTranslator.NotRunning[CurrentLanguageInt]);
				}
				break;

			case "/estado":
			case "/status":
				if (State == BotState.Running)
					ConsoleBot.BotSendMessage(string.Format(AppTranslator.StatusRunning[CurrentLanguageInt], SearchFurniName, MaxPrice, TotalBought));
				else
					ConsoleBot.BotSendMessage(AppTranslator.StatusIdle[CurrentLanguageInt]);
				break;

			case "/salir":
			case "/exit":
			case "/sair":
				ConsoleBot.CustomExitCommand = e;
				break;

			default:
				ConsoleBot.BotSendMessage(AppTranslator.CommandsMessage[CurrentLanguageInt]);
				break;
		}
	}

	// ── Packet interception ───────────────────────────────────────────────

	private void Extension_OnDataInterceptEvent(DataInterceptedEventArgs e)
	{
		if (Extension.In.NotEnoughBalance.Match(e) && State == BotState.Running)
		{
			StopSearchLoop();
			ConsoleBot.BotSendMessage(AppTranslator.NotEnoughBalance[CurrentLanguageInt]);
		}
	}

	private void Extension_OnCriticalErrorEvent(string e)
	{
		base.Visibility = Visibility.Visible;
		base.ShowInTaskbar = true;
		Activate();
		MessageBox.Show(e + ".", "Critical error", MessageBoxButton.OK, MessageBoxImage.Error);
		Environment.Exit(0);
	}
}
