using CalculatorDmain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CalculatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CalculatorController : ControllerBase
    {
        ICalculatorDomain _domain;
        public CalculatorController(ICalculatorDomain domain)
        {
            _domain = domain;
        }


        // GET api/Calculator/2+3*5
        [HttpGet("{expression}")]
        public async Task<string> Get(string expression)
        {
            if (IsValidExpression(expression, out string errorMessage))
            {
                try
                {
                    return await _domain.Calculate(expression);
                }
                catch(Exception ex)
                {
                    return "Invalid Expression.";
                }
            }
            else
            {
                return errorMessage;
            }
        }

        private bool IsValidExpression(string expr, out string errormessage)
        {
            bool isValid = false;
            errormessage = string.Empty;
            if (!string.IsNullOrWhiteSpace(expr))
            {
                Regex RgxUrl = new Regex("^[0-9+*-/()., ]+$");
                var matched= RgxUrl.IsMatch(expr) && validateMathExpression(expr);
                if (matched)
                {
                    isValid = true;
                }
                else
                {
                    errormessage = "Invalid Expression.";
                }
            }
            else
            {
                errormessage = "Expression is required.";
            }
            return isValid;
        }

        private bool validateMathExpression(string expression)
        {
            int parenthesesCounter = 0;
            string strTemp;
            string[] splitedExpression = expression.Split('/', '*', '+', '-');

            if (splitedExpression == null || splitedExpression.Length == 0)
            {
                return false;
            }
            else if (splitedExpression.Length == 1)
            {
                return int.TryParse(splitedExpression[0], out _);
            }

            foreach (string str in splitedExpression)
            {
                strTemp = str.Trim();

                if (strTemp.Length == 0)
                {
                    return false;
                }

                for (int i = 0; i < strTemp.Length; i++)
                {
                    if (i < strTemp.Length && strTemp[i] == '(' && strTemp[i + 1] == ')')
                    {
                        return false;
                    }

                    parenthesesCounter = strTemp[i] == '(' ? parenthesesCounter + 1 : parenthesesCounter;
                    parenthesesCounter = strTemp[i] == ')' ? parenthesesCounter - 1 : parenthesesCounter;

                    if (parenthesesCounter < 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
