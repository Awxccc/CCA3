using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;


public class EconomyMgr : MonoBehaviour
{
    [SerializeField] TMP_Text t_money, t_catalog, t_inventory;
    [SerializeField] TMP_Dropdown dd_catalog; //dropdown for catalog items
    List<CatalogItem> catItems; // to store catalog items
    public void GetVirtualCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
        r =>{
            int coins = r.VirtualCurrency["CN"];
            t_money.text = "Coins:" + coins;
        }, OnError);
    }
    void OnError(PlayFabError e)//Function to handle failure
    {
        t_money.text="Error:"+e.GenerateErrorReport();
    }

    public void GetCatalog()
    {
        var catReq = new GetCatalogItemsRequest { CatalogVersion = "FruitsCatalog" };
        PlayFabClientAPI.GetCatalogItems(catReq,
        catRes =>
        {
            catItems = catRes.Catalog;
            t_catalog.text = "Catalog Items\n";
            t_catalog.text += "-------------\n";
            List<string> itemNames = new List<string>(); //for dropdown
            foreach(CatalogItem i in catItems)
            {
                t_catalog.text += (i.DisplayName + ":" + i.VirtualCurrencyPrices["CN"] + "\n");
                itemNames.Add(i.DisplayName + "[" + i.VirtualCurrencyPrices["CN"]+"]");
            }
            dd_catalog.ClearOptions();//Clear previous options
            dd_catalog.AddOptions(itemNames);//add new options based on itemNames
            dd_catalog.Show(); //expand the dropdown
        }, OnError);
    }

    public void BuyItem()
    {
        var buyReq = new PurchaseItemRequest
        {
            CatalogVersion = "FruitsCatalog",
            ItemId = "FC001",
            VirtualCurrency = "CN",
            Price = 12
        };
        PlayFabClientAPI.PurchaseItem(buyReq,
            r =>
            {
                t_money.text = "Bought";
                GetPlayerInventory();
            }, OnError
        );
    }
    public void BuyDropDownItem()
    {
        int index = dd_catalog.value;//selected index 0,1,2
        CatalogItem selectedItem = catItems[index];
        var buyReq = new PurchaseItemRequest
        {
            CatalogVersion = "FruitsCatalog",
            ItemId = selectedItem.ItemId,//retrieve ItemId from selected catalog item
            VirtualCurrency = "CN",
            Price = (int)selectedItem.VirtualCurrencyPrices["CN"] //retrieve price from selected catalog item

        };
        PlayFabClientAPI.PurchaseItem(buyReq,
            result =>
            {
                t_money.text = "Bought " + selectedItem.DisplayName;
                GetPlayerInventory();
                GetVirtualCurrency();
            }, OnError
            );
    }
    public void GetPlayerInventory()
    {
        var UserInvReq = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(UserInvReq,
            r =>
            {
                List<ItemInstance> myInventoryList = r.Inventory;
                t_inventory.text = "My Inventory\n";
                foreach(ItemInstance ii in myInventoryList)
                {
                    t_inventory.text += (ii.DisplayName + ":" + ii.ItemId + ":" + ii.ItemInstanceId + "\n");
                }
            },OnError
            );
    }

    private void Start()
    {
        GetVirtualCurrency();
        GetCatalog();
        GetPlayerInventory();
    }

    public void OnBack()
    {
        SceneManager.LoadScene("NextScn");
    }
}
