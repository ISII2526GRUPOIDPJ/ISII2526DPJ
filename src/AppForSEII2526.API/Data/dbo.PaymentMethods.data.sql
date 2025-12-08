SET IDENTITY_INSERT [dbo].[PaymentMethods] ON;

INSERT INTO [dbo].[PaymentMethods] ([Id], [UserId], [Discriminator], [CreditCardNumber], [ExpirationDate], [TelephoneNumber], [Email]) VALUES 
(1, N'1df6eba9-fe88-45e1-9368-5ab1b2f6e905', N'CreditCard', 123456789, '2025-12-31', NULL, NULL),
(2, N'1df6eba9-fe88-45e1-9368-5ab1b2f6e905', N'PayPal', NULL, NULL, NULL, N'paypal@email.com'),
(3, N'1df6eba9-fe88-45e1-9368-5ab1b2f6e905', N'Bizum', NULL, NULL, 666777888, NULL);

SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF;
