CREATE NONCLUSTERED INDEX [IX_LocaleStringResource] ON Localization.[LocaleStringResource] ([ResourceName] ASC,  [LanguageId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_PriceDatesEtc] ON [Catalog].[Product]  ([Price] ASC, [AvailableStartDateTimeUtc] ASC, [AvailableEndDateTimeUtc] ASC, [Published] ASC, [Deleted] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Country_DisplayOrder] ON Directory.[Country] ([DisplayOrder] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Currency_DisplayOrder] ON Directory.[Currency] ( [DisplayOrder] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Log_CreatedOnUtc] ON Logging.[Log] ([CreatedOnUtc] DESC)
GO

CREATE NONCLUSTERED INDEX [IX_User_Email] ON Users.[User] ([Email] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_User_Username] ON Users.[User] ([Username] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_User_UserGuid] ON Users.[User] ([UserGuid] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_User_SystemName] ON Users.[User] ([SystemName] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_User_CreatedOnUtc] ON Users.[User] ([CreatedOnUtc] DESC)
GO

CREATE NONCLUSTERED INDEX [IX_GenericAttribute_EntityId_and_KeyGroup] ON Common.[GenericAttribute] ([EntityId] ASC, [KeyGroup] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_QueuedEmail_CreatedOnUtc] ON [Messages].[QueuedEmail] ([CreatedOnUtc] DESC)
GO

CREATE NONCLUSTERED INDEX [IX_Order_CreatedOnUtc] ON [Orders].[Order] ([CreatedOnUtc] DESC)
GO

CREATE NONCLUSTERED INDEX [IX_Language_DisplayOrder] ON Localization.[Language] ([DisplayOrder] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_NewsletterSubscription_Email_StoreId] ON [Messages].[NewsLetterSubscription] ([Email] ASC, [StoreId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_ShoppingCartItem_ShoppingCartTypeId_UserId] ON Orders.[ShoppingCartItem] ([ShoppingCartTypeId] ASC, [UserId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_RelatedProduct_ProductId1] ON [Catalog].[RelatedProduct] ([ProductId1] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_ProductAttributeValue_ProductAttributeMappingId_DisplayOrder] ON [Catalog].[ProductAttributeValue] ([ProductAttributeMappingId] ASC, [DisplayOrder] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_ProductAttribute_Mapping_ProductId_DisplayOrder] ON [Catalog].[Product_ProductAttribute_Mapping] ([ProductId] ASC, [DisplayOrder] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Manufacturer_DisplayOrder] ON [Catalog].[Manufacturer] ([DisplayOrder] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Category_DisplayOrder] ON [Catalog].[Category] ([DisplayOrder] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Category_ParentCategoryId] ON [Catalog].[Category] ([ParentCategoryId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Forums_Group_DisplayOrder] ON Forums.ForumGroup ([DisplayOrder] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Forums_Forum_DisplayOrder] ON Forums.[Forum] ([DisplayOrder] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Forums_Subscription_ForumId] ON Forums.ForumSubscription ([ForumId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Forums_Subscription_TopicId] ON Forums.ForumSubscription ([TopicId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_Deleted_and_Published] ON [Catalog].[Product] ([Published] ASC, [Deleted] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_Published] ON [Catalog].[Product] ([Published] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_ShowOnHomepage] ON [Catalog].[Product] ([ShowOnHomePage] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_ParentGroupedProductId] ON [Catalog].[Product] ([ParentGroupedProductId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_VisibleIndividually] ON [Catalog].[Product] ([VisibleIndividually] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_PCM_Product_and_Category] ON [Catalog].[Product_Category_Mapping] ([CategoryId] ASC, [ProductId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_PCM_ProductId_Extended] ON [Catalog].[Product_Category_Mapping] ([ProductId] ASC, [IsFeaturedProduct] ASC) INCLUDE ([CategoryId])
GO

CREATE NONCLUSTERED INDEX [IX_PMM_Product_and_Manufacturer] ON [Catalog].[Product_Manufacturer_Mapping] ([ManufacturerId] ASC, [ProductId] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_PMM_ProductId_Extended] ON [Catalog].[Product_Manufacturer_Mapping] ([ProductId] ASC, [IsFeaturedProduct] ASC) INCLUDE ([ManufacturerId])
GO

CREATE NONCLUSTERED INDEX [IX_PSAM_AllowFiltering] ON [Catalog].[Product_SpecificationAttribute_Mapping] ([AllowFiltering] ASC) INCLUDE ([ProductId],[SpecificationAttributeOptionId])
GO

CREATE NONCLUSTERED INDEX [IX_PSAM_SpecificationAttributeOptionId_AllowFiltering] ON [Catalog].[Product_SpecificationAttribute_Mapping] ([SpecificationAttributeOptionId] ASC, [AllowFiltering] ASC) INCLUDE ([ProductId])
GO

CREATE NONCLUSTERED INDEX [IX_ProductTag_Name] ON [Catalog].[ProductTag] ([Name] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_ActivityLog_CreatedOnUtc] ON Logging.[ActivityLog] ([CreatedOnUtc] DESC)
GO

CREATE NONCLUSTERED INDEX [IX_UrlRecord_Slug] ON Seo.[UrlRecord] ([Slug] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_UrlRecord_Custom_1] ON Seo.[UrlRecord] ([EntityId] ASC, [EntityName] ASC, [LanguageId] ASC, [IsActive] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_AclRecord_EntityId_EntityName] ON [AclRecord] ([EntityId] ASC, [EntityName] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_StoreMapping_EntityId_EntityName] ON [StoreMapping] ([EntityId] ASC, [EntityName] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Category_LimitedToStores] ON [Catalog].[Category] ([LimitedToStores] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Manufacturer_LimitedToStores] ON [Catalog].[Manufacturer] ([LimitedToStores] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_LimitedToStores] ON [Catalog].[Product] ([LimitedToStores] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Category_SubjectToAcl] ON [Catalog].[Category] ([SubjectToAcl] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Manufacturer_SubjectToAcl] ON [Catalog].[Manufacturer] ([SubjectToAcl] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_SubjectToAcl] ON [Catalog].[Product] ([SubjectToAcl] ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_Category_Mapping_IsFeaturedProduct] ON [Catalog].[Product_Category_Mapping] (IsFeaturedProduct ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_Manufacturer_Mapping_IsFeaturedProduct] ON [Catalog].[Product_Manufacturer_Mapping] (IsFeaturedProduct ASC)
GO

CREATE NONCLUSTERED INDEX [IX_User_UserRole_Mapping_User_Id] ON Users.[User_UserRole_Mapping] (User_Id ASC)
GO

CREATE NONCLUSTERED INDEX [IX_Product_Delete_Id] ON [Catalog].[Product] (Deleted ASC, Id ASC)
GO

CREATE NONCLUSTERED INDEX [IX_GetLowStockProducts] ON [Catalog].[Product] (Deleted ASC, VendorId ASC, ProductTypeId ASC, ManageInventoryMethodId ASC, MinStockQuantity ASC, UseMultipleWarehouses ASC)
GO

CREATE NONCLUSTERED INDEX [IX_QueuedEmail_SentOnUtc_DontSendBeforeDateUtc_Extended] ON [Messages].[QueuedEmail] ([SentOnUtc], [DontSendBeforeDateUtc]) INCLUDE ([SentTries])
GO

CREATE NONCLUSTERED INDEX [IX_Product_VisibleIndividually_Published_Deleted_Extended] ON [Catalog].[Product] ([VisibleIndividually],[Published],[Deleted]) INCLUDE ([Id],[AvailableStartDateTimeUtc],[AvailableEndDateTimeUtc])
GO

CREATE NONCLUSTERED INDEX [IX_Category_Deleted_Extended] ON [Catalog].[Category] ([Deleted]) INCLUDE ([Id],[Name],[SubjectToAcl],[LimitedToStores],[Published])
GO