using ConsultCnpj.Models;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Diagnostics;

namespace ConsultCnpj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "ConsultarCnpj");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult ObterCertidaoPrimeiroSite(string cnpj)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("user-agent= SombraBranca(Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddUserProfilePreference("download.default_directory", @"C:\Downloads");
            chromeOptions.AddUserProfilePreference("download.prompt_for_download", false);
            chromeOptions.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
            try
            {
                using (IWebDriver driver = new ChromeDriver(chromeOptions))
                {
                    // Interação com o primeiro site
                    ObterCertidaoPrimeiroSite(driver, cnpj);

                    // Após obter a certidão, você pode redirecionar para uma view que a exiba ou fazer algo mais com ela
                    return View(); // Por exemplo, redireciona para a view "ResultadoCertidao"
                }
            }
            catch (Exception e)
            {
                // Em caso de erro, você pode redirecionar para uma página de erro ou retornar uma mensagem de erro para a view
                return Content("Erro: " + e.Message);
            }
        }

        private void ObterCertidaoPrimeiroSite(IWebDriver driver, string cnpj)
        {
            driver.Navigate().GoToUrl("https://certidoes.cgu.gov.br");

            var checkbox = driver.FindElement(By.CssSelector("input[type='checkbox'][value='8']"));
            checkbox.Click();

            var inputCnpj = driver.FindElement(By.Id("cpfCnpj"));
            inputCnpj.SendKeys(cnpj);

            var submit = driver.FindElement(By.Id("consultar"));
            submit.Click();

            Thread.Sleep(6000);

            var botaoEmitirCertidao = driver.FindElement(By.CssSelector(".btn.btn.btn-labeled.btn-primary.botoes-alinhados.btn-secondary"));
            botaoEmitirCertidao.Click();
            Thread.Sleep(4000);
        }
    }
}

