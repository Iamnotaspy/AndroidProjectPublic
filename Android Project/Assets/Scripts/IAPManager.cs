using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : IStoreListener
{
    private static IStoreController _storeController;
    private static IExtensionProvider _storeExtensionProvider;

    public static string kRemoveAds = "removeads";

    public void Start()
    {
        if (_storeController == null)
        {
            //Configure connection to IAP
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if(IsInitialized()) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(kRemoveAds, ProductType.NonConsumable);
        
        //Initialize IAP with the specified listener and configuration
        //Store controller and extension provider are set
        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = _storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log("Purchasing Product");
                _storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("Product is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized");
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (string.Equals(purchaseEvent.purchasedProduct.definition.id, kRemoveAds, StringComparison.Ordinal))
        {
            Debug.Log("Purchasing product");
            
            PlayerPrefs.SetInt("noAdsPurchased", 1);
            AdManager.Instance.NoAdsPurchased = true;
        }
        else
        {
            Debug.Log("Unrecognized product");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product:'{0}', PurchaseFailureReason: {1}",
            product.definition.storeSpecificId, failureReason));
    }

    //Check if the app can connect to unity IAP
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialised: PASS");
        _storeController = controller;
        _storeExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    { 
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error); 
    }

    private bool IsInitialized()
    { 
        // Check if both the Purchasing references are set
        return _storeController != null && _storeExtensionProvider != null; 
    }
}
