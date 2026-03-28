namespace LTDHelper;

public class AppTranslator
{
	// [0]=EN  [1]=ES  [2]=PT

	public static string[] WelcomeMessage = new string[3]
	{
		"Marketplace Auto-Buyer ready!",
		"Auto-comprador del Mercadillo listo!",
		"Auto-comprador do Mercado pronto!"
	};

	public static string[] CommandsMessage = new string[3]
	{
		"Commands: /start, /stop, /status, /debug on|off, /exit",
		"Comandos: /iniciar, /detener, /estado, /debug on|off, /salir",
		"Comandos: /começar, /parar, /estado, /debug on|off, /sair"
	};

	public static string[] AskFurniName = new string[3]
	{
		"Enter the furni name to search in the marketplace:",
		"Ingresa el nombre del furni a buscar en el Mercadillo:",
		"Insira o nome do furni para buscar no Mercado:"
	};

	public static string[] AskMaxPrice = new string[3]
	{
		"Enter the maximum price (coins) you want to pay:",
		"Ingresa el precio máximo (monedas) que deseas pagar:",
		"Insira o preço máximo (moedas) que deseja pagar:"
	};

	// 0 = unlimited
	public static string[] AskAmount = new string[3]
	{
		"How many units to buy? (1-100, enter 0 for unlimited):",
		"Cuántas unidades comprar? (1-100, ingresa 0 para ilimitado):",
		"Quantas unidades comprar? (1-100, insira 0 para ilimitado):"
	};

	public static string[] InvalidPrice = new string[3]
	{
		"Invalid price. Enter a positive number.",
		"Precio inválido. Ingresa un número positivo.",
		"Preço inválido. Insira um número positivo."
	};

	public static string[] InvalidAmount = new string[3]
	{
		"Invalid amount. Enter a number between 0 and 100.",
		"Cantidad inválida. Ingresa un número entre 0 y 100.",
		"Quantidade inválida. Insira um número entre 0 e 100."
	};

	// {0}=furniName  {1}=maxPrice  {2}=targetAmount ("∞" when 0)
	public static string[] SearchStarted = new string[3]
	{
		"Searching for '{0}' at {1} coins or less. Target: {2}. Use /stop to cancel.",
		"Buscando '{0}' a {1} monedas o menos. Objetivo: {2}. Usa /detener para cancelar.",
		"Buscando '{0}' por {1} moedas ou menos. Alvo: {2}. Use /parar para cancelar."
	};

	// {0}=price
	public static string[] OfferFound = new string[3]
	{
		"Offer found at {0} coins! Buying...",
		"Oferta encontrada a {0} monedas! Comprando...",
		"Oferta encontrada por {0} moedas! Comprando..."
	};

	// {0}=price  {1}=totalBought
	public static string[] PurchaseOK = new string[3]
	{
		"Purchased at {0} coins! (Total bought: {1})",
		"Comprado a {0} monedas! (Total comprado: {1})",
		"Comprado por {0} moedas! (Total comprado: {1})"
	};

	public static string[] PurchaseFailed = new string[3]
	{
		"Purchase failed (offer may have been taken). Retrying...",
		"Compra fallida (la oferta pudo haber sido tomada). Reintentando...",
		"Compra falhou (a oferta pode ter sido tomada). Tentando novamente..."
	};

	public static string[] NoOfferAtPrice = new string[3]
	{
		"No offers found at or below your max price yet.",
		"Aun no hay ofertas al precio maximo o menor.",
		"Ainda nao ha ofertas no preco maximo ou menor."
	};

	// {0}=code
	public static string[] PurchaseRejected = new string[3]
	{
		"Purchase rejected by server (code: {0}). Retrying...",
		"Compra rechazada por el servidor (codigo: {0}). Reintentando...",
		"Compra rejeitada pelo servidor (codigo: {0}). Tentando novamente..."
	};

	// {0}=message
	public static string[] DebugError = new string[3]
	{
		"Debug: {0}",
		"Debug: {0}",
		"Debug: {0}"
	};

	public static string[] DebugEnabled = new string[3]
	{
		"Debug mode enabled.",
		"Modo debug activado.",
		"Modo debug ativado."
	};

	public static string[] DebugDisabled = new string[3]
	{
		"Debug mode disabled.",
		"Modo debug desactivado.",
		"Modo debug desativado."
	};

	public static string[] DebugUsage = new string[3]
	{
		"Usage: /debug on or /debug off",
		"Uso: /debug on o /debug off",
		"Uso: /debug on ou /debug off"
	};

	public static string[] StoppedMessage = new string[3]
	{
		"Search stopped.",
		"Búsqueda detenida.",
		"Busca parada."
	};

	public static string[] AlreadyRunning = new string[3]
	{
		"Search is already running. Use /stop to cancel.",
		"La búsqueda ya está en curso. Usa /detener para cancelar.",
		"A busca já está em andamento. Use /parar para cancelar."
	};

	public static string[] NotRunning = new string[3]
	{
		"No search is currently running.",
		"No hay ninguna búsqueda en curso.",
		"Nenhuma busca está em andamento."
	};

	public static string[] TargetReached = new string[3]
	{
		"Target reached! All requested units have been purchased.",
		"Objetivo alcanzado! Se compraron todas las unidades solicitadas.",
		"Alvo atingido! Todas as unidades solicitadas foram compradas."
	};

	public static string[] StatusIdle = new string[3]
	{
		"Status: Idle. Use /start to configure a search.",
		"Estado: Inactivo. Usa /iniciar para configurar una búsqueda.",
		"Estado: Inativo. Use /começar para configurar uma busca."
	};

	// {0}=furniName  {1}=maxPrice  {2}=totalBought
	public static string[] StatusRunning = new string[3]
	{
		"Status: Running | Furni: {0} | Max price: {1} coins | Bought: {2}",
		"Estado: Activo | Furni: {0} | Precio máximo: {1} monedas | Comprado: {2}",
		"Estado: Ativo | Furni: {0} | Preço máximo: {1} moedas | Comprado: {2}"
	};

	public static string[] NotEnoughBalance = new string[3]
	{
		"Not enough balance! Search stopped.",
		"Balance insuficiente! Búsqueda detenida.",
		"Saldo insuficiente! Busca parada."
	};

	public static string[] UnexpectedError = new string[3]
	{
		"Unexpected error. Search stopped.",
		"Error inesperado. Búsqueda detenida.",
		"Erro inesperado. Busca parada."
	};
}
