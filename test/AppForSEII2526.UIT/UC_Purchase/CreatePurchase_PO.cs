using AppForSEII2526.API.DTOs.PurchaseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class CreatePurchase_PO : PageObject {
        By inputCity = By.Id("City");
        By inputCountry= By.Id("Country");
        By inputStreet = By.Id("Street");
        By inputDescription = By.Id("Description");
        By selectPaymentMethod = By.Id("PaymentMethod");
        By inputPaymentMethod = By.Id("PMdescription");

        By buttonModifyItems = By.Id("ModifyItems");
        By buttonConfirmPurchase = By.Id("ConfirmPurchase");

        By purchasedItems = By.Id("PurchasedItems");

        By errorShownBy = By.Id("CreatePurchaseErrors");

        By dialogOkButton = By.Id("Button_DialogOK");

        By createPurchaseErrorsBy = By.Id("CreatePurchaseErrors");


        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {
        }

        public void FillPurchaseForm(string city, string country, string street, string description, int paymentMethod, string paymentMethodDescription) {

            _driver.FindElement(inputCity).Clear();
            _driver.FindElement(inputCity).SendKeys(city);

            _driver.FindElement(inputCountry).Clear();
            _driver.FindElement(inputCountry).SendKeys(country);

            _driver.FindElement(inputStreet).Clear();
            _driver.FindElement(inputStreet).SendKeys(street);

            _driver.FindElement(inputDescription).Clear();
            if(!string.IsNullOrEmpty(description)) {
                _driver.FindElement(inputDescription).SendKeys(description);
            }

            if(paymentMethod > 0) {
                var selectElement = new SelectElement(_driver.FindElement(selectPaymentMethod));
                selectElement.SelectByIndex(paymentMethod);
            }

            _driver.FindElement(inputPaymentMethod).Clear();
            _driver.FindElement(inputPaymentMethod).SendKeys(paymentMethodDescription);
        }

        public void ClickModifyItems() {
            _driver.FindElement(buttonModifyItems).Click();
        }

        public void ClickConfirmPurchase() {
            var element = _driver.FindElement(buttonConfirmPurchase);

            Actions actions = new Actions(_driver);
            actions.MoveToElement(element).Perform();

            element.Click();
        }

        public bool CheckMessageError(string errorMessage) {
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        public void ClickDialogOk() {
            WaitForBeingClickable(dialogOkButton);
            _driver.FindElement(dialogOkButton).Click();
        }

        public bool CheckPurchaseMessage(string errorMessage) {
            try {
                IWebElement errorElement = _driver.FindElement(createPurchaseErrorsBy);
                _output.WriteLine($"Stock error: {errorElement.Text}");
                return errorElement.Text.Contains(errorMessage);
            } catch (NoSuchElementException) {
                return false;
            }
        }

        public bool CheckSuccessfulPurchase() {
            try {
                WaitForBeingVisible(purchasedItems);
                return true;
            } catch (Exception ex) {
                return false;
            }
        }
    }
}
