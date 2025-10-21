SET IDENTITY_INSERT [dbo].[Items] ON
INSERT INTO [dbo].[Items] ([Id], [Description], [Name], [PurchasePrice], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [TypeItemId], [BrandId]) VALUES 
(1, N'Yoga mat for exercises', N'Yoga Mat', 25.00, 10, 5, 20.00, 1, 1),
(2, N'Running shoes', N'Running Shoes', 80.00, 15, 8, 70.00, 2, 2)
SET IDENTITY_INSERT [dbo].[Items] OFF