using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PlanDTOs;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PurchaseController_test
{
    public class CreatePurchase_test : AppForSEII25264SqliteUT
    {
        public class TestPaymentMethod : PaymentMethod { }

        public CreatePurchase_test() {
            ApplicationUser user = new ApplicationUser(1, "John", "Doe");

            var paymentMethod = new TestPaymentMethod()
            {
                Id = 1,
                User = user
            };

            var purchase = new List<Purchase>() {
                new Purchase("Madrid", "Spain", DateTime.Parse("2024-01-10"), "Gym equipment", "Main Street 123", 150, paymentMethod)
            };

            _context.AddRange(user);
            _context.AddRange(purchase);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase_BadRequest()
        {
            var allTests = new List<object[]>
            {
                //City missing
                new object[] {
                    new CreatePurchaseDTO(
                        null,
                        "Spain",
                        "Main Street 123",
                        DateTime.Parse("2024-01-10"),
                        "Description",
                        0m,
                        4,
                        new List<PurchaseItemsDTO> {new PurchaseItemsDTO("Yoga Mat", "Nike", 10, 25m)},
                        new TestPaymentMethod() {Id = 1, User = new ApplicationUser(1, "John", "Doe")}
                    ),
                    "A street must be introduced."
                },

                //Country missing
                new object[] {
                    new CreatePurchaseDTO(
                        "Madrid",
                        null,
                        "Main Street 123",
                        DateTime.Parse("2024-01-10"),
                        "Description",
                        0m,
                        4,
                        new List<PurchaseItemsDTO> {new PurchaseItemsDTO("Yoga Mat", "Nike", 10, 25m)},
                        new TestPaymentMethod() {Id = 1, User = new ApplicationUser(1, "John", "Doe")}
                    ),
                    "A street must be introduced."
                },

                //Street missing
                new object[] {
                    new CreatePurchaseDTO(
                        "Madrid",
                        "Spain",
                        null,
                        DateTime.Parse("2024-01-10"),
                        "Description",
                        0m,
                        4,
                        new List<PurchaseItemsDTO> {new PurchaseItemsDTO("Yoga Mat", "Nike", 10, 25m)},
                        new TestPaymentMethod() {Id = 1, User = new ApplicationUser(1, "John", "Doe")}
                    ),
                    "A street must be introduced."
                },

                //Invalid payment method
                new object[] {
                    new CreatePurchaseDTO(
                        "Madrid",
                        "Spain",
                        null,
                        DateTime.Parse("2024-01-10"),
                        "Description",
                        0m,
                        4,
                        new List<PurchaseItemsDTO> {new PurchaseItemsDTO("Yoga Mat", "Nike", 10, 25m)},
                        new TestPaymentMethod() {Id = -1, User = new ApplicationUser(1, "John", "Doe")}
                    ),
                    "Selected payment method not found."
                },

                //No quantity available
                new object[] {
                    new CreatePurchaseDTO(
                        "Madrid",
                        "Spain",
                        null,
                        DateTime.Parse("2024-01-10"),
                        "Description",
                        0m,
                        new Item().QuantityAvailableForPurchase+1,
                        new List<PurchaseItemsDTO> {new PurchaseItemsDTO("Yoga Mat", "Nike", 10, 25m)},
                        new TestPaymentMethod() {Id = -1, User = new ApplicationUser(1, "John", "Doe")}
                    ),
                    "There are not that many items available."
                }
        };

            return allTests;
        }
    }
}
