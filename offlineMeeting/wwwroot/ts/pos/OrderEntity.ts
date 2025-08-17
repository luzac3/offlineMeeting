export class OrderEntity {
    PurchaseId: string;
    OrderNumber: number;
    OrderCoins: number;
    AlcoholAmount: number;

    constructor(purchaseId: string, orderNumber: number, orderCoins: number, alcoholAmount: number = 0) {
        this.PurchaseId = purchaseId;
        this.OrderNumber = orderNumber;
        this.OrderCoins = orderCoins;
        this.AlcoholAmount = alcoholAmount;
    }
}