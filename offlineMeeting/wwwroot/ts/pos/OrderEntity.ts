export class OrderEntity {
    EventId: number;
    ResultId: number;
    OrderNumber: number;
    OrderCoins: number;
    AlcoholAmount: number;

    constructor(eventId: number, resultId: number, orderNumber: number, orderCoins: number, alcoholAmount: number = 0) {
        this.EventId = eventId;
        this.ResultId = resultId;
        this.OrderNumber = orderNumber;
        this.OrderCoins = orderCoins;
        this.AlcoholAmount = alcoholAmount;
    }
}