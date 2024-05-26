using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeNet.Pages
{
    public class IndexModel(ILogger<IndexModel> logger) : PageModel
    {
        ILogger<IndexModel> _logger = logger;

        public void OnGet()
        {
            ViewData["Sender"] = "FromIndex";
        }
    }
}
