export class OrderEntity {
    ResultId: number;
    Category: number;
    OrderNumber: number;
    OrderCoins: number;
    AlcoholAmount: number;

    constructor(resultId: number, category: number, orderNumber: number, orderCoins: number, alcoholAmount: number = 0) {
        this.ResultId = resultId;
        this.Category = category;
        this.OrderNumber = orderNumber;
        this.OrderCoins = orderCoins;
        this.AlcoholAmount = alcoholAmount;
    }
}