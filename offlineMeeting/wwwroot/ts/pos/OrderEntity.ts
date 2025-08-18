export class OrderEntity {
    ResultId: number;
    OrderNumber: number;
    OrderCoins: number;
    AlcoholAmount: number;

    constructor(resultId: number, orderNumber: number, orderCoins: number, alcoholAmount: number = 0) {
        this.ResultId = resultId;
        this.OrderNumber = orderNumber;
        this.OrderCoins = orderCoins;
        this.AlcoholAmount = alcoholAmount;
    }
}